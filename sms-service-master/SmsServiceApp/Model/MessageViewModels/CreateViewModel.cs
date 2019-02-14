using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebCustomerApp.Models.MessageViewModels
{
	public class CreateViewModel
	{
		[Required]
		public List<string> RecepientPhones { get; set; }

		[Required]
		public string MessageText { get; set; }

	}
}
