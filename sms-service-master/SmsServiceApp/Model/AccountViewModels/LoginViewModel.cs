﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebCustomerApp.Models.AccountViewModels
{
    public class LoginViewModel
    {
		[Required]
		[Display(Name = "Login")]
		public string Login { get; set; }

		[Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
