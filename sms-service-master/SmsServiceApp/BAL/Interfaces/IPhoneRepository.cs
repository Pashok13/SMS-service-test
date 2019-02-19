using System.Collections.Generic;
using System.Threading.Tasks;
using WebCustomerApp.Models;

namespace BAL.Interfaces
{
	public interface IPhoneRepository : IRepository<Phone>
	{
		void CreateByPhone(string number);
		Phone FindByPhone(string number);
		Task<List<Phone>> GetPhonesByUserIdAsync(string userId);
	}
}
