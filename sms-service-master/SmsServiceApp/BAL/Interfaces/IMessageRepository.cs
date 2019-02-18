using System.Collections.Generic;
using WebCustomerApp.Models;

namespace BAL.Interfaces
{
	public interface IMessageRepository : IRepository<Message>
	{
		void CreateMessage(Message message);
		List<Message> GetMessagesByUserId(string userId);
	}
}
