﻿using DAL.Interfaces;
using System.Collections.Generic;
using WebCustomerApp.Data;
using WebCustomerApp.Models;

namespace WebCustomerApp.Repositories
{
	public class MessageRepository : GenericRepository<Message>, IMessageRepository
	{
		public MessageRepository(ApplicationDbContext context) : base(context)
		{
		}

		public void CreateMessage(Message message)
		{
			context.Messages.Add(message);
			context.SaveChanges();
		}

	}
}
