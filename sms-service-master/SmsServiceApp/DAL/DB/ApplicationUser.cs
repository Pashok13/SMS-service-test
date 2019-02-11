using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WebCustomerApp.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
		public ICollection<Message> Messages { get; set; }

		public ApplicationUser()
		{
			Messages = new List<Message>();
		}
	}
}
