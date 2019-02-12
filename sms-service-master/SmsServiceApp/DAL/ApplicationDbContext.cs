using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebCustomerApp.Models;

namespace WebCustomerApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
		public DbSet<Message> Messages		{ get; set; }
		public DbSet<Phone> Phones			{ get; set; }
		public DbSet<AdditInfo> AdditInfo	{ get; set; }
		public DbSet<MessegeRecipient> MessegesRecipients { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
		{
			Database.EnsureCreated();
		}

		protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
