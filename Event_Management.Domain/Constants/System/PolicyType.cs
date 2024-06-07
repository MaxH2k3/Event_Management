using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.Constants.System
{
	public class PolicyType
	{
		// All user login into system
		public const string User = "User";
		// Just admin system
		public const string Admin = "Admin";
		// Just guest
		public const string Guest = "Guest";
	}
}
