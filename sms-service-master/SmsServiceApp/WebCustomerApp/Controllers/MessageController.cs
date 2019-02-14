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

		public MessageController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult NewMessage()
		{
			return View();
		}

		[HttpPost]
		public IActionResult NewMessage(CreateViewModel model)
		{
			if (ModelState.IsValid)
			{
				Message message = new Message() { TextMessage = model.MessageText, UserId = _unitOfWork.UserRepository.GetUserId(User) };
				_unitOfWork.MessageRepository.Add(message);

				foreach (var phone in model.RecepientPhones)
				{
					Phone currentPhone;
					currentPhone = _unitOfWork.PhoneRepository.FindByPhone(phone);

					if (currentPhone == null)
					{
						currentPhone = new Phone() { Number = phone };
						_unitOfWork.PhoneRepository.Add(currentPhone);
					}

					_unitOfWork.MessageRecipientRepository.CreateСontactRecord(message.MessageId, currentPhone.Id);		
				}
			
				_unitOfWork.Save();
				return View("SuccessSend");
			}
			else
			{
				return View(model);
			}
		}

		public IActionResult SuccessSend()
		{
			return View();
		}
	}
}
