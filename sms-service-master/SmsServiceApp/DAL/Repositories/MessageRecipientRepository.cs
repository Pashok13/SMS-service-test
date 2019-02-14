using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Repositories;
using WebCustomerApp.Models;
using WebCustomerApp.Data;

namespace DAL.Repositories
{
	public class MessageRecipientRepository : GenericRepository<MessageRecipient>, IMessageRecipientRepository
	{
		public MessageRecipientRepository(ApplicationDbContext context) : base(context)
		{
		}

		public void CreateСontactRecord(int messageId, int recepientId)
		{
			MessageRecipient record = new MessageRecipient() { MessageId = messageId, RecepientId = recepientId};
			context.MessegesRecipients.Add(record);
			context.SaveChanges();
		}

	}
}
