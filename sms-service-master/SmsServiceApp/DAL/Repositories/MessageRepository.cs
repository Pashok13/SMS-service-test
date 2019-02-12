using DAL.Interfaces;
using WebCustomerApp.Data;
using WebCustomerApp.Models;

namespace WebCustomerApp.Repositories
{
	public class MessageRepository : GenericRepository<Message>, IMessageRepository
	{
		public MessageRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}
