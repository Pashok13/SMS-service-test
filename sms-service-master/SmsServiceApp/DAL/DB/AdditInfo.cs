﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebCustomerApp.Models
{
	public class AdditInfo
	{
		[Key]
		public int Id			{ get; set; }
		public int PhoneId		{ get; set; }
		public string Key		{ get; set; }
		public string Value		{ get; set; }

		[ForeignKey("PhoneId")]
		public Phone Phone { get; set; }
	}
}