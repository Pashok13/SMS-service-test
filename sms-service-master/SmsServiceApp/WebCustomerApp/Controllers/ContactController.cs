using System.Collections.Generic;
using BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebCustomerApp.Models;

namespace WebApp.Controllers
{
	public class ContactController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public ContactController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult ContactList()
		{
			string userID = _unitOfWork.UserRepository.GetUserId(User);
			List<Phone> contactList = _unitOfWork.PhoneRepository.GetPhonesByUserId(userID);
			ViewBag.ContactList = contactList;
			return View();
		}
	}
}
