using System;
using SQLite;

using TWallet.Models;

namespace TWallet.Models
{
    public class Credit
    {
        [PrimaryKey]
        public string Currency { get; set; }

        public double Amount { get; set; }


        public Credit()
        {
        }

        public Credit(Currency currency, double amount) 
        {
            this.Currency = currency.CurrencyKey;
            this.Amount = amount;
        }

		private string currencyImg;
		public string CurrencyImg
		{
			get => CurrencyManager.GetCurrency(Currency).Image;
			set { currencyImg = value; }
		}

        public Currency GetCurrency()
        {
            return CurrencyManager.GetCurrency(Currency);
        }
    }
}
