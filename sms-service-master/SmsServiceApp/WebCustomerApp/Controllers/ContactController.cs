using System.Collections.Generic;
using System.Threading.Tasks;
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

		public async Task<IActionResult> ContactList()
		{
			string userID = _unitOfWork.UserRepository.GetUserId(User);
			List<Phone> contactList = await _unitOfWork.PhoneRepository.GetPhonesByUserIdAsync(userID);
			ViewBag.ContactList = contactList;
			return View();
		}
	}
}
