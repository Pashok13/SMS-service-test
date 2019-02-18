using BAL.Interfaces;
using WebCustomerApp.Data;
using WebCustomerApp.Models;

namespace BAL.Repositories
{
	public class AdditInfoRepository : GenericRepository<AdditInfo>, IAdditInfoRepository
	{
		public AdditInfoRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}
