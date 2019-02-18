using WebCustomerApp.Models;

namespace BAL.Interfaces
{
	public interface IMessageRecipientRepository : IRepository<MessageRecipient>
	{
		void CreateСontactRecord(int messageId, int recepientId);
	}
}
