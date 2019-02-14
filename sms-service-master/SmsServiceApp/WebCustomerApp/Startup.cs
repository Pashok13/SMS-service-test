using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebCustomerApp.Data;
using WebCustomerApp.Models;
using WebCustomerApp.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebCustomerApp.Repositories;
using DAL.Interfaces;
using DAL.Repositories;

namespace WebCustomerApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>
			(opts =>
			{
				opts.Password.RequiredLength = 5;
				opts.Password.RequireNonAlphanumeric = false;
				opts.Password.RequireDigit = false;
				opts.User.RequireUniqueEmail = true;
			}
			)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
			services.AddTransient<IUnitOfWork, UnitOfWork>();
			services.AddTransient<IMessageRepository, MessageRepository>();
			services.AddTransient<IPhoneRepository, PhoneRepository>();
			services.AddTransient<IMessageRecipientRepository, MessageRecipientRepository>();
			services.AddTransient<IRepository<Phone>, GenericRepository<Phone>>();
			services.AddTransient<IRepository<Message>, GenericRepository<Message>>();
			services.AddTransient<IRepository<MessageRecipient>, GenericRepository<MessageRecipient>>();

			//services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");});

			services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
