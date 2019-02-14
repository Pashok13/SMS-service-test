using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebCustomerApp.Models.MessageViewModels
{
	public class CreateViewModel
	{
		//[Required]
		//[Display(Name = "Recepient phone")]
		//[RegularExpression(@"^\+[0-9]{12}$", ErrorMessage = "Invalid phone(s) number")]
		public List<string> RecepientPhones { get; set; }

		//[Required]
		//[Display(Name = "Text")]
		public string MessageText { get; set; }

	}
}
