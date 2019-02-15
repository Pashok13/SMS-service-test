using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;

namespace DAL.Interfaces
{
	public interface IMessageRecipientRepository : IRepository<MessageRecipient>
	{
		void CreateСontactRecord(int messageId, int recepientId);
	}
}
