using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebCustomerApp.Models;

namespace DAL.Interfaces
{
	public interface IMessageRepository : IRepository<Message>
	{
		void CreateMessage(Message message);
		List<Message> GetMessagesByUserId(string userId);
	}
}
