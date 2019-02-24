using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebCustomerApp.Models.ContactViewModels
{
	public class AdditInfoModel
	{
		[Required]
		public string Phone { get; set; }
		[Required]
		public string Key { get; set; }
		[Required]
		public string Value { get; set; }
	}
}
