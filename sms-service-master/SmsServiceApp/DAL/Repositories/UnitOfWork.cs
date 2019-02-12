using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using WebCustomerApp.Data;
using WebCustomerApp.Models;

namespace WebCustomerApp.Repositories
{
	public class UnitOfWork : IDisposable, IUnitOfWork
	{
		private readonly ApplicationDbContext context;
		private UserManager<ApplicationUser> userRepository;
		private PhoneRepository phoneRepository;
		private MessageRepository messageRepository;

		public UnitOfWork(ApplicationDbContext context)
		{
			this.context = context;
		}

		public UserManager<ApplicationUser> UserRepository
		{
			get
			{
				if (userRepository == null)
				{
					userRepository = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context), context);
				}
				return userRepository;
			}
		}

		public PhoneRepository PhoneRepository
		{
			get
			{
				if (phoneRepository == null)
				{
					phoneRepository = new PhoneRepository(context);
				}
				return phoneRepository;
			}
		}

		public MessageRepository MessageRepository
		{
			get
			{
				if (messageRepository == null)
				{
					messageRepository = new MessageRepository(context);
				}
				return messageRepository;
			}
		}

		public void Save()
		{
			//UserRepository.SaveChanges();
			//PhoneRepository.SaveChanges();
			//MessageRepository.SaveChanges();
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