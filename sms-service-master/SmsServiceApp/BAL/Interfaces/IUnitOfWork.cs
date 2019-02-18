using Microsoft.AspNetCore.Identity;
using WebCustomerApp.Models;

namespace BAL.Interfaces
{
	public interface IUnitOfWork
	{
		void Save();
		void Dispose();
		IEmailSender EmailSender { get; }
		IPhoneRepository PhoneRepository { get; }
		IMessageRepository MessageRepository { get; }
		IAdditInfoRepository AdditInfoRepository { get; }
		IMessageRecipientRepository MessageRecipientRepository { get; }
		UserManager<ApplicationUser> UserRepository	{ get; }
		SignInManager<ApplicationUser> SignInRepository { get; }
	}
}
