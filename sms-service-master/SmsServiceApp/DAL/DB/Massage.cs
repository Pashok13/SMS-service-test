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
		public string UserId		{ get; set; }
		public string TextMessage   { get; set; }
		public DateTime CreateDate	{ get; set; }
		public DateTime SendDate	{ get; set; }

		[ForeignKey("UserId")]
		public ApplicationUser User	{ get; set; }

		public ICollection<MessageRecipient> MessageRecipient { get; set; }

		public Message()
		{
			CreateDate = DateTime.Now;
			MessageRecipient = new List<MessageRecipient>();
		}
	}
}
