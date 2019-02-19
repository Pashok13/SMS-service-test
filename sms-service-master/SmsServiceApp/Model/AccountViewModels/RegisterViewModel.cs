using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebCustomerApp.Models.AccountViewModels
{
    public class RegisterViewModel
    {

		//[EmailAddress]	
		[Required]
		[RegularExpression(@"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))"
					+ @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$", ErrorMessage = "Invalid e-mail adress")]
		[Display(Name = "Email")]
        public string Email { get; set; }

		//[Phone]
		[Display(Name = "Phone number")]
		[RegularExpression(@"^\+[0-9]{12}$", ErrorMessage = "Invalid phone number")]
		public string PhoneNumber { get; set; }

		[Required]
		[Display(Name = "Login")]
		public string Login { get; set; }

		[Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
