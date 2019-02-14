using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCustomerApp.Models
{

	public class Message
	{
        [Key] 
        public int MessageId		{ get; set; }
		public string UserId			{ get; set; }
		public string TextMessage   { get; set; }
		public DateTime CreateDate { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime Period { get; set; }

		[ForeignKey("UserId")]
		public ApplicationUser User			{ get; set; }

		public ICollection<MessageRecipient> MessageRecipient { get; set; }

		public Message()
		{
			MessageRecipient = new List<MessageRecipient>();
		}
	}
}
