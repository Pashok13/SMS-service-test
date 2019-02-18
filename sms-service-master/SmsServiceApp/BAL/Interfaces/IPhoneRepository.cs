using System.Collections.Generic;
using WebCustomerApp.Models;

namespace BAL.Interfaces
{
	public interface IPhoneRepository : IRepository<Phone>
	{
		void CreateByPhone(string number);
		Phone FindByPhone(string number);
		List<Phone> GetPhonesByUserId(string userId);
	}
}
