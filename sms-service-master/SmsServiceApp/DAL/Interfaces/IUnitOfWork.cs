using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;
using WebCustomerApp.Repositories;

namespace DAL.Interfaces
{
	public interface IUnitOfWork
	{
		void Save();
		void Dispose();
		IPhoneRepository PhoneRepository				{ get; }
		IMessageRepository MessageRepository			{ get; }
		UserManager<ApplicationUser> UserRepository		{ get; }
		SignInManager<ApplicationUser> SignInRepository { get; }
	}
}
