using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;
using WebCustomerApp.Repositories;
using WebCustomerApp.Services;

namespace DAL.Interfaces
{
	public interface IUnitOfWork
	{
		void Save();
		void Dispose();
		IEmailSender EmailSender { get; }
		IPhoneRepository PhoneRepository { get; }
		IMessageRepository MessageRepository { get; }
		IMessageRecipientRepository MessageRecipientRepository { get; }
		UserManager<ApplicationUser> UserRepository	{ get; }
		SignInManager<ApplicationUser> SignInRepository { get; }
	}
}
