using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using WebCustomerApp.Data;
using WebCustomerApp.Models;
using WebCustomerApp.Services;

namespace WebCustomerApp.Repositories
{
	public class UnitOfWork : IDisposable, IUnitOfWork
	{
		public ApplicationDbContext context;
		public UserManager<ApplicationUser> UserRepository { get; }	
		public SignInManager<ApplicationUser> SignInRepository { get; }
		public IEmailSender EmailSender { get; }
		public IPhoneRepository PhoneRepository { get; }
		public IMessageRepository MessageRepository { get; }
		public IMessageRecipientRepository MessageRecipientRepository { get; }

		public UnitOfWork(ApplicationDbContext context,
				UserManager<ApplicationUser> userRepository, 
				SignInManager<ApplicationUser> signInRepository,
				IEmailSender emailSender,
				IPhoneRepository phoneRepository, 
				IMessageRepository messageRepository,
				IMessageRecipientRepository messageRecipientRepository)
		{
			EmailSender = emailSender;
			UserRepository = userRepository;
			SignInRepository = signInRepository;
			PhoneRepository = phoneRepository;
			MessageRepository = messageRepository;
			MessageRecipientRepository = messageRecipientRepository;
			this.context = context;
		}

		public void Save()
		{
			context.SaveChanges();
		}

		private bool disposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					context.Dispose();
				}
			}
			disposed = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}