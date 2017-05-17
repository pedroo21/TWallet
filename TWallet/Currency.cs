using System;
using SQLite;
using TWallet.Models;

namespace TWallet
{

	public class Currency
	{
		[PrimaryKey]
		public string CurrencyDescription { get; set; }
		public double CurrencyValue { get; set; }
		public string CurrencyConverted { get; set; }
		public string Image { get; set; }

		public Currency() { }

		public Currency(string currDesc, double currValue)
		{
			this.CurrencyValue = currValue;
			this.CurrencyDescription = currDesc;
		}

		public Currency(string currDesc, double currValue, double credits)
		{
			this.CurrencyValue = currValue;
			this.CurrencyDescription = currDesc;
			double wallet = credits;
			this.CurrencyConverted = (wallet * currValue).ToString();
			SetImage();
		}

		private void SetImage()
		{
			if (this.CurrencyDescription.Equals("TRY"))
			{
				this.Image = this.CurrencyDescription.ToUpper() + "Y.png";
			}
			else
			{
				this.Image = this.CurrencyDescription.ToUpper() + ".png";
			}
		}
	}
}
