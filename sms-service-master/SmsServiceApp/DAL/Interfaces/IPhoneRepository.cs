﻿using System;
using System.Collections.Generic;
using System.Text;
using WebCustomerApp.Models;

namespace DAL.Interfaces
{
	interface IPhoneRepository : IRepository<Phone>
	{
		void CreateByPhone(string number);
		Phone FindByPhone(string number);
	}
}
