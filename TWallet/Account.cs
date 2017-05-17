using System;
using SQLite;

namespace TWallet
{
	public class Account
	{
		[PrimaryKey]
		public int Id { get; set; }
		public double CreditsDb { get; set; }
	}
}
