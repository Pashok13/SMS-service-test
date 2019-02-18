using BAL.Interfaces;
using WebCustomerApp.Models;
using WebCustomerApp.Data;

namespace BAL.Repositories
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
