using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebCustomerApp.Models.MessageViewModels
{
	public class MessageListModel
	{
		public string MessageText { get; set; }
		public List<string> RecepientPhones { get; set; }

		public MessageListModel(string text, List<string> recepientPhones)
		{
			MessageText = text;
			RecepientPhones = recepientPhones;
		}

	}
}
