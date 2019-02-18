using BAL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using WebCustomerApp.Data;
using WebCustomerApp.Models;

namespace BAL.Repositories
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
			return context.Phones.Join(context.MessegesRecipients,
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
					.GroupBy(p => p.num)
					.Select(t => new Phone
					{
						Number = t.Key,
						//AdditInfo = t.Select(r => r.recInfo.Select(inf => new AdditInfo
						//{
						//	Key = inf.key,
						//	Value = inf.value
						//}).ToList()


						AdditInfo = t.Distinct().Select(inf => new AdditInfo
						{
							Key = inf.recInfo.FirstOrDefault().key,
							Value = inf.recInfo.FirstOrDefault().value
						}).ToList()

					}).ToList();

		}
	}
}
