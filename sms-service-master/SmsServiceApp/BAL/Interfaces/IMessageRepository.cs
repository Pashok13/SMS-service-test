using System.Collections.Generic;
using System.Threading.Tasks;
using WebCustomerApp.Models;

namespace BAL.Interfaces
{
	public interface IMessageRepository : IRepository<Message>
	{
		void CreateMessage(Message message);
		Task<List<Message>> GetMessagesByUserId(string userId);
	}
}
