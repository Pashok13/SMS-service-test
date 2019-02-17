using DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using WebCustomerApp.Data;
using WebCustomerApp.Models;

namespace WebCustomerApp.Repositories
{
	public class PhoneRepository : GenericRepository<Phone>, IPhoneRepository
	{
		public PhoneRepository(ApplicationDbContext context) : base(context)
		{
		}

		public void CreateByPhone(string number)
		{
			Phone record = new Phone() { Number = number };
			context.Phones.Add(record);
			context.SaveChanges();
		}

		public Phone FindByPhone(string number)
		{
			Phone record = context.Phones.FirstOrDefault(p => p.Number == number);
			return record;
		}

		public List<Phone> GetPhonesByUserId(string userId)
		{
			List<Phone> phones = context.Phones.Join(context.MessegesRecipients,
					p => p.Id,
					mr => mr.RecepientId,
					(p, mr) => new
					{
						mes = mr.Message,
						num = p.Number,
						recInfo = p.AdditInfo.Select(info => new
						{
							key = info.Key,
							value = info.Value
						})
					}).Where(m => m.mes.UserId == userId)
					.OrderByDescending(m => m.mes.CreateDate)
					.AsEnumerable()
					.Select(tab => new Phone
					{
						Number = tab.num,
						AdditInfo = tab.recInfo.Select(inf => new AdditInfo
						{
							Key = inf.key,
							Value = inf.value
						}).ToList()
					}).ToList();

			return phones;
		}
	}
}
