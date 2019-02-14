using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebCustomerApp.Models;
using WebCustomerApp.Models.MessageViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Controllers
{
	public class MessageController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILogger _logger;

		public MessageController(IUnitOfWork unitOfWork, ILogger<MessageController> logger)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
		}

		public IActionResult NewMessage(string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			return View();
		}

		[HttpPost]
		public IActionResult NewMessage(CreateViewModel model, string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			if (ModelState.IsValid)
			{
				Message message = new Message() { TextMessage = model.MessageText, UserId = _unitOfWork.UserRepository.GetUserId(User) };
				
				foreach (var phone in model.RecepientPhones)
				{
					if (_unitOfWork.PhoneRepository.FindByPhone(phone) == null)
						_unitOfWork.PhoneRepository.CreateByPhone(phone);
				}

				_unitOfWork.MessageRepository.Add(message);
				_unitOfWork.Save();
				return RedirectToAction(nameof(SuccessSend));
			}
			else
			{
				_logger.LogWarning("Invalid phone(s) number");
				return View(model);
			}
		}

		public IActionResult SuccessSend()
		{
			return View("~/Views/Message/SuccessSend.cshtml");
		}
	}
}
