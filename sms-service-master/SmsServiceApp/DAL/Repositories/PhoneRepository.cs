using DAL.Interfaces;
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
			context.Phone.Add(record);
			context.SaveChanges();
		}

		public Phone FindByPhone(string number)
		{
			Phone record = context.Phone.FirstOrDefault(p => p.Number == number);
			return record;
		}
	}
}
