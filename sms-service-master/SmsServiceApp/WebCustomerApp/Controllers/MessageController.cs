using System.Collections.Generic;
using BAL.Interfaces;
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

		public IActionResult NewMessage()
		{
			return View();
		}

		[HttpPost]
		public IActionResult NewMessage(CreateViewModel model)
		{
			if (ModelState.IsValid)
			{
				Message message = new Message() { TextMessage = model.MessageText, SendDate = model.DateOfSend,
						UserId = _unitOfWork.UserRepository.GetUserId(User) };
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

		public IActionResult MessageList()
		{
			string userID = _unitOfWork.UserRepository.GetUserId(User);
			List<Message> messageList = _unitOfWork.MessageRepository.GetMessagesByUserId(userID);
			ViewBag.MessagesList = messageList;
			return View();
		}

		public IActionResult SuccessSend()
		{
			return View();
		}
	}
}
