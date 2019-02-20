using BAL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCustomerApp.Data;
using WebCustomerApp.Models;

namespace BAL.Repositories
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

		public async Task<List<Message>> GetMessagesByUserId(string userId)
		{
			Task<List<Message>> task = Task.Run(() =>
			{
				return context.Messages.Where(mes => mes.UserId == userId)
					.Select(m => new
					{
						text = m.TextMessage,
						dateCreate = m.CreateDate,
						dateSend = m.SendDate,
						rec = m.MessageRecipient.Select(mesres => new
						{
							ph = mesres.Phone.Number,
							recInfo = mesres.Phone.AdditInfo.Select(info => new
							{
								key = info.Key,
								value = info.Value
							})
						})
					})
					.OrderByDescending(m => m.dateSend)
					.AsEnumerable()
					.Select(tabl => new Message
					{
						TextMessage = tabl.text,
						CreateDate = tabl.dateCreate,
						SendDate = tabl.dateSend,
						MessageRecipient = tabl.rec.Select(r => new MessageRecipient
						{
							Phone = new Phone()
							{
								Number = r.ph,
								AdditInfo = r.recInfo.Select(inf => new AdditInfo
								{
									Key = inf.key,
									Value = inf.value
								}).ToList()
							}
						}).ToList()
					}).ToList();
			});

			return await task;
		}
	}
}
