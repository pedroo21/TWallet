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
		readonly SQLiteAsyncConnection database;

		public TWalletDatabase(string dbPath)
		{
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Currency>();
            database.CreateTableAsync<Account>();
            database.CreateTableAsync<Credit>();
		}

		public async Task<Account> GetAccount()
		{
            Account account = await database.Table<Account>().FirstOrDefaultAsync();
            if(account == null) {
                account = new Account(CurrencyEnum.EUR);
                account.Id = await App.Database.InsertOrReplaceAccount(account);
            }
            account.Credits = await GetCredits();
            return account;
		}

        public async Task<int> InsertOrReplaceAccount(Account account)
		{
			return await database.InsertOrReplaceAsync(account);
		}

        public async Task<List<Credit>> GetCredits()
        {
            return await database.Table<Credit>().ToListAsync();
        }

        public async Task UpdateCredits(List<Credit> credits)
		{
            foreach (var credit in credits)
            {
                await database.InsertOrReplaceAsync(credit);
            }
		}

		public async Task<List<Currency>> GetCurrencies()
		{
            return await database.Table<Currency>().ToListAsync();
		}

		public async Task<int> SaveCurrencies(Currency currency)
		{
			return await database.InsertOrReplaceAsync(currency);
		}

		public async Task<List<Currency>> DeleteAllCurrencies()
		{
            return await database.QueryAsync<Currency>("delete from Currency");
		}

		public async Task SaveCurrenciesToDatabase(string currency)
		{
			HttpClientHandler handler = new HttpClientHandler();
			HttpResponseMessage response = await APIHandler.Get("http://api.fixer.io/latest?base=" + currency, handler);
			if (response.IsSuccessStatusCode)
			{
				Root rootCurrency = JsonConvert.DeserializeObject<Root>(await response.Content.ReadAsStringAsync());
				foreach (var item in rootCurrency.rates)
				{
                    Currency curr = new Currency(item.Key, item.Value);
					await App.Database.SaveCurrencies(curr);
					curr = null;
				}
			}
		}
	}
}