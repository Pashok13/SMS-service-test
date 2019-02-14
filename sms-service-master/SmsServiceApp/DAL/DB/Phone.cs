using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebCustomerApp.Models
{
	public class Phone
	{
		[Key]
		public int Id			{ get; set; }
		public string Number	{ get; set; }

		public ICollection<MessageRecipient> MessageRecipient	{ get; set; }
		public ICollection<AdditInfo> AdditInfo					{ get; set; }

		public Phone()
		{
			MessageRecipient = new List<MessageRecipient>();
			AdditInfo = new List<AdditInfo>();
		}
	}
}