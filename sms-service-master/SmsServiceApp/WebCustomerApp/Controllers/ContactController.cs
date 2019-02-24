using System.Collections.Generic;
using System.Threading.Tasks;
using BAL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebCustomerApp.Models;
using WebCustomerApp.Models.ContactViewModels;

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
			List<Phone> contactList = await _unitOfWork.PhoneRepository.GetPhonesByUserId(userID);
			ViewBag.ContactList = contactList;
			return View();
		}

		public void AddContactInfo(AdditInfoModel info)
		{
			Phone ph = _unitOfWork.PhoneRepository.FindByPhone(info.Phone);
			AdditInfo newInfo = new AdditInfo() { PhoneId = ph.Id, Key = info.Key, Value = info.Value };
			_unitOfWork.AdditInfoRepository.Add(newInfo);
			_unitOfWork.Save();
		}
	}
}
