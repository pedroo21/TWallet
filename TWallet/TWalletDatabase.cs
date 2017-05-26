using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SQLite;
using TWallet.API;
using TWallet.Models;
using TWallet.Util;

namespace TWallet
{
	public class TWalletDatabase
	{
		readonly SQLiteConnection database;

		public TWalletDatabase(string dbPath)
		{
            database = new SQLiteConnection(dbPath);
            database.CreateTable<Currency>();
            database.CreateTable<Account>();
            database.CreateTable<Credit>();
		}

		public Account GetAccount()
		{
            Account account = database.Table<Account>().FirstOrDefault();
            if(account == null) {
                account = new Account(CurrencyEnum.EUR);
                account.Id = App.Database.InsertOrReplaceAccount(account);
            }
            account.Credits = GetCredits();
            return account;
		}

        public int InsertOrReplaceAccount(Account account)
		{
			return database.InsertOrReplace(account);
		}

        public List<Credit> GetCredits()
        {
            return database.Query<Credit>("select * from credit");
        }

        public void UpdateCredits(List<Credit> credits)
		{
            database.Query<Credit>("delete from credit");
            foreach (var credit in credits)
            {
                database.Insert(credit);
            }
		}

		public List<Currency> GetCurrencies()
		{
            return database.Query<Currency>("select * from currency");
		}

		public int SaveCurrencies(Currency currency)
		{
			return database.InsertOrReplace(currency);
		}
       
		public List<Currency> DeleteAllCurrencies()
		{
            return database.Query<Currency>("delete from Currency");
		}

		public async Task SaveCurrenciesToDatabase(string currency)
		{
            HttpClientHandler handler = new HttpClientHandler();
            HttpResponseMessage response = await APIHandler.Get("http://api.fixer.io/latest?base=" + currency, handler);
            if (response.IsSuccessStatusCode)
            {
                DeleteAllCurrencies();

                Root rootCurrency = JsonConvert.DeserializeObject<Root>(await response.Content.ReadAsStringAsync());
                foreach (var item in rootCurrency.rates)
                {
                    Currency curr = new Currency(item.Key, item.Value);
                    Console.WriteLine("Saving " + item.Key + " " + item.Value);
                    App.Database.SaveCurrencies(curr);
                    curr = null;
                }
                CurrencyManager.Init(currency);
            }
		}
	}
}