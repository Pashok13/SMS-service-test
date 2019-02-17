using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebCustomerApp.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
