using System;
using SQLite;
using TWallet.Models;
using TWallet.Util;

namespace TWallet
{

	public class Currency
	{
        [PrimaryKey]
		public string CurrencyKey { get; set; }
		public double CurrencyValue { get; set; }
        public string CurrencySymbol { get; set; }
		public string Image { get; set; }

		public Currency() { }

		public Currency(string currDesc, double currValue)
		{
			this.CurrencyValue = currValue;
			this.CurrencyKey = currDesc;
            try
            {
                CurrencySymbolEnum symbol = (CurrencySymbolEnum)Enum.Parse(typeof(CurrencySymbolEnum), currDesc);
                this.CurrencySymbol = symbol.ToString();
            }
            catch(Exception) 
            {
                this.CurrencySymbol = currDesc;
            }
            SetImage();
		}

		private void SetImage()
		{
			if (this.CurrencyKey.Equals("TRY"))
			{
				this.Image = this.CurrencyKey.ToUpper() + "Y.png";
			}
			else
			{
				this.Image = this.CurrencyKey.ToUpper() + ".png";
			}
		}
	}
}
