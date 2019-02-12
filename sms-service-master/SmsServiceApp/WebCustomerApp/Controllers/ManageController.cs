using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DAL.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebCustomerApp.Models;
using WebCustomerApp.Models.ManageViewModels;
using WebCustomerApp.Services;

namespace WebCustomerApp.Controllers
{
	[Authorize]
    [Route("[controller]/[action]")]
    public class ManageController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly UrlEncoder _urlEncoder;
		private readonly IUnitOfWork _unitOfWork;

        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        private const string RecoveryCodesKey = nameof(RecoveryCodesKey);

        public ManageController(
		  IUnitOfWork unitOfWork,
		  IEmailSender emailSender,
          ILogger<ManageController> logger,
          UrlEncoder urlEncoder)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _logger = logger;
            _urlEncoder = urlEncoder;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_unitOfWork.UserRepository.GetUserId(User)}'.");
            }

            var model = new IndexViewModel
            {
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsEmailConfirmed = user.EmailConfirmed,
                StatusMessage = StatusMessage
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _unitOfWork.UserRepository.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_unitOfWork.UserRepository.GetUserId(User)}'.");
            }

            var email = user.Email;
            if (model.Email != email)
            {
                var setEmailResult = await _unitOfWork.UserRepository.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting email for user with ID '{user.Id}'.");
                }
            }

            var phoneNumber = user.PhoneNumber;
            if (model.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _unitOfWork.UserRepository.SetPhoneNumberAsync(user, model.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    throw new ApplicationException($"Unexpected error occurred setting phone number for user with ID '{user.Id}'.");
                }
            }

            StatusMessage = "Your profile has been updated";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendVerificationEmail(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _unitOfWork.UserRepository.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_unitOfWork.UserRepository.GetUserId(User)}'.");
            }

            var code = await _unitOfWork.UserRepository.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.EmailConfirmationLink(user.Id, code, Request.Scheme);
            var email = user.Email;
            await _emailSender.SendEmailConfirmationAsync(email, callbackUrl);

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_unitOfWork.UserRepository.GetUserId(User)}'.");
            }

            var hasPassword = await _unitOfWork.UserRepository.HasPasswordAsync(user);
            if (!hasPassword)
            {
                return RedirectToAction(nameof(SetPassword));
            }

            var model = new ChangePasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _unitOfWork.UserRepository.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_unitOfWork.UserRepository.GetUserId(User)}'.");
            }

            var changePasswordResult = await _unitOfWork.UserRepository.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                AddErrors(changePasswordResult);
                return View(model);
            }

            await _unitOfWork.SignInRepository.SignInAsync(user, isPersistent: false);
            _logger.LogInformation("User changed their password successfully.");
            StatusMessage = "Your password has been changed.";

            return RedirectToAction(nameof(ChangePassword));
        }

        [HttpGet]
        public async Task<IActionResult> SetPassword()
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_unitOfWork.UserRepository.GetUserId(User)}'.");
            }

            var hasPassword = await _unitOfWork.UserRepository.HasPasswordAsync(user);

            if (hasPassword)
            {
                return RedirectToAction(nameof(ChangePassword));
            }

            var model = new SetPasswordViewModel { StatusMessage = StatusMessage };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _unitOfWork.UserRepository.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_unitOfWork.UserRepository.GetUserId(User)}'.");
            }

            var addPasswordResult = await _unitOfWork.UserRepository.AddPasswordAsync(user, model.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                AddErrors(addPasswordResult);
                return View(model);
            }

            await _unitOfWork.SignInRepository.SignInAsync(user, isPersistent: false);
            StatusMessage = "Your password has been set.";

            return RedirectToAction(nameof(SetPassword));
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLogins()
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_unitOfWork.UserRepository.GetUserId(User)}'.");
            }

            var model = new ExternalLoginsViewModel { CurrentLogins = await _unitOfWork.UserRepository.GetLoginsAsync(user) };
            model.OtherLogins = (await _unitOfWork.SignInRepository.GetExternalAuthenticationSchemesAsync())
                .Where(auth => model.CurrentLogins.All(ul => auth.Name != ul.LoginProvider))
                .ToList();
            model.ShowRemoveButton = await _unitOfWork.UserRepository.HasPasswordAsync(user) || model.CurrentLogins.Count > 1;
            model.StatusMessage = StatusMessage;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LinkLogin(string provider)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // Request a redirect to the external login provider to link a login for the current user
            var redirectUrl = Url.Action(nameof(LinkLoginCallback));
            var properties = _unitOfWork.SignInRepository.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _unitOfWork.UserRepository.GetUserId(User));
            return new ChallengeResult(provider, properties);
        }

        [HttpGet]
        public async Task<IActionResult> LinkLoginCallback()
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_unitOfWork.UserRepository.GetUserId(User)}'.");
            }

            var info = await _unitOfWork.SignInRepository.GetExternalLoginInfoAsync(user.Id);
            if (info == null)
            {
                throw new ApplicationException($"Unexpected error occurred loading external login info for user with ID '{user.Id}'.");
            }

            var result = await _unitOfWork.UserRepository.AddLoginAsync(user, info);
            if (!result.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occurred adding external login for user with ID '{user.Id}'.");
            }

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            StatusMessage = "The external login was added.";
            return RedirectToAction(nameof(ExternalLogins));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveLogin(RemoveLoginViewModel model)
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_unitOfWork.UserRepository.GetUserId(User)}'.");
            }

            var result = await _unitOfWork.UserRepository.RemoveLoginAsync(user, model.LoginProvider, model.ProviderKey);
            if (!result.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occurred removing external login for user with ID '{user.Id}'.");
            }

            await _unitOfWork.SignInRepository.SignInAsync(user, isPersistent: false);
            StatusMessage = "The external login was removed.";
            return RedirectToAction(nameof(ExternalLogins));
        }

        [HttpGet]
        public async Task<IActionResult> TwoFactorAuthentication()
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_unitOfWork.UserRepository.GetUserId(User)}'.");
            }

            var model = new TwoFactorAuthenticationViewModel
            {
                HasAuthenticator = await _unitOfWork.UserRepository.GetAuthenticatorKeyAsync(user) != null,
                Is2faEnabled = user.TwoFactorEnabled,
                RecoveryCodesLeft = await _unitOfWork.UserRepository.CountRecoveryCodesAsync(user),
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Disable2faWarning()
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_unitOfWork.UserRepository.GetUserId(User)}'.");
            }

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
            }

            return View(nameof(Disable2fa));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Disable2fa()
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_unitOfWork.UserRepository.GetUserId(User)}'.");
            }

            var disable2faResult = await _unitOfWork.UserRepository.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
            {
                throw new ApplicationException($"Unexpected error occured disabling 2FA for user with ID '{user.Id}'.");
            }

            _logger.LogInformation("User with ID {UserId} has disabled 2fa.", user.Id);
            return RedirectToAction(nameof(TwoFactorAuthentication));
        }

        [HttpGet]
        public async Task<IActionResult> EnableAuthenticator()
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_unitOfWork.UserRepository.GetUserId(User)}'.");
            }

            var model = new EnableAuthenticatorViewModel();
            await LoadSharedKeyAndQrCodeUriAsync(user, model);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableAuthenticator(EnableAuthenticatorViewModel model)
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_unitOfWork.UserRepository.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadSharedKeyAndQrCodeUriAsync(user, model);
                return View(model);
            }

            // Strip spaces and hypens
            var verificationCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await _unitOfWork.UserRepository.VerifyTwoFactorTokenAsync(
                user, _unitOfWork.UserRepository.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2faTokenValid)
            {
                ModelState.AddModelError("Code", "Verification code is invalid.");
                await LoadSharedKeyAndQrCodeUriAsync(user, model);
                return View(model);
            }

            await _unitOfWork.UserRepository.SetTwoFactorEnabledAsync(user, true);
            _logger.LogInformation("User with ID {UserId} has enabled 2FA with an authenticator app.", user.Id);
            var recoveryCodes = await _unitOfWork.UserRepository.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            TempData[RecoveryCodesKey] = recoveryCodes.ToArray();

            return RedirectToAction(nameof(ShowRecoveryCodes));
        }

        [HttpGet]
        public IActionResult ShowRecoveryCodes()
        {
            var recoveryCodes = (string[])TempData[RecoveryCodesKey];
            if (recoveryCodes == null)
            {
                return RedirectToAction(nameof(TwoFactorAuthentication));
            }

            var model = new ShowRecoveryCodesViewModel { RecoveryCodes = recoveryCodes };
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetAuthenticatorWarning()
        {
            return View(nameof(ResetAuthenticator));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetAuthenticator()
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_unitOfWork.UserRepository.GetUserId(User)}'.");
            }

            await _unitOfWork.UserRepository.SetTwoFactorEnabledAsync(user, false);
            await _unitOfWork.UserRepository.ResetAuthenticatorKeyAsync(user);
            _logger.LogInformation("User with id '{UserId}' has reset their authentication app key.", user.Id);

            return RedirectToAction(nameof(EnableAuthenticator));
        }

        [HttpGet]
        public async Task<IActionResult> GenerateRecoveryCodesWarning()
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_unitOfWork.UserRepository.GetUserId(User)}'.");
            }

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' because they do not have 2FA enabled.");
            }

            return View(nameof(GenerateRecoveryCodes));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GenerateRecoveryCodes()
        {
            var user = await _unitOfWork.UserRepository.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{_unitOfWork.UserRepository.GetUserId(User)}'.");
            }

            if (!user.TwoFactorEnabled)
            {
                throw new ApplicationException($"Cannot generate recovery codes for user with ID '{user.Id}' as they do not have 2FA enabled.");
            }

            var recoveryCodes = await _unitOfWork.UserRepository.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            _logger.LogInformation("User with ID {UserId} has generated new 2FA recovery codes.", user.Id);

            var model = new ShowRecoveryCodesViewModel { RecoveryCodes = recoveryCodes.ToArray() };

            return View(nameof(ShowRecoveryCodes), model);
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenticatorUriFormat,
                _urlEncoder.Encode("WebCustomerApp"),
                _urlEncoder.Encode(email),
                unformattedKey);
        }

        private async Task LoadSharedKeyAndQrCodeUriAsync(ApplicationUser user, EnableAuthenticatorViewModel model)
        {
            var unformattedKey = await _unitOfWork.UserRepository.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _unitOfWork.UserRepository.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _unitOfWork.UserRepository.GetAuthenticatorKeyAsync(user);
            }

            model.SharedKey = FormatKey(unformattedKey);
            model.AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey);
        }

        #endregion
    }
}
