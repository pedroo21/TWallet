using System;
using System.Collections.Generic;

namespace TWallet.Models
{
	public class Root
	{
		public string @base { get; set; }
		public DateTime date { get; set; }
		public Dictionary<string, double> rates { get; set; }
	}
}
