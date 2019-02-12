using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Interfaces
{
	interface IUnitOfWork
	{
		void Save();
		void Dispose();
	}
}
