using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebCustomerApp.Models;
using WebCustomerApp.Models.MessageViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Controllers
{
	public class MessageController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public MessageController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult NewMessage(string returnUrl = null)
		{
			ViewData["ReturnUrl"] = returnUrl;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> NewMessage(CreateViewModel model, string returnUrl = null)
		{
			if (ModelState.IsValid)
			{
				Message message = new Message() { TextMessage = model.MessageText, UserId = _unitOfWork.UserRepository.GetUserId(User) };
				Phone phone = new Phone() { Number = model.RecepientPhone };
				_unitOfWork.MessageRepository.Add(message);
				_unitOfWork.PhoneRepository.Add(phone);
				_unitOfWork.Save();
				return RedirectToAction("Index", "Home");
			}

			return View(model);

		}


	}
}
