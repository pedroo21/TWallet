﻿using System;
using System.Collections.Generic;
using SQLite;

using TWallet.Models;

namespace TWallet
{
	public class Account
	{
		[PrimaryKey]
		public int Id { get; set; }

        public string RootCurrency { get; set; }

        [Ignore]
        public List<Credit> Credits { get; set; }

		public Account()
        {
        }

		public Account(string rootCurrency)
        {
            this.RootCurrency = rootCurrency;
            this.Credits = new List<Credit>();
        }

        public void AddCredits(string currencyKey, double amount) {
            int index = Credits.FindIndex(x => x.Currency == currencyKey);
            if(index != -1)
            {
                Credits[index].Amount += amount;
            }
            else 
            {
                Currency currency = CurrencyManager.GetCurrency(currencyKey);
                if (currency != null)
                {
                    Credits.Add(new Credit(currency, amount));
                }
            }
        }

        public void RemoveCredits(string currencyKey, double amount) {
            int index = Credits.FindIndex(x => x.Currency == currencyKey);
            if(Credits[index].Amount - amount <= 0) {
                Credits.RemoveAll(x => x.Currency == currencyKey);
            }
            else {
                Credits[index].Amount -= amount;
            }
        }
	}
}
