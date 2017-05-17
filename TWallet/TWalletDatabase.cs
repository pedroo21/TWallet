using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SQLite;
using TWallet.API;
using TWallet.Models;

namespace TWallet
{
	public class TWalletDatabase
	{
		readonly SQLiteAsyncConnection database;

		public TWalletDatabase(string dbPath)
		{
			database = new SQLiteAsyncConnection(dbPath);
			database.CreateTableAsync<Currency>().Wait();
			database.CreateTableAsync<Account>().Wait();
		}

		public Task<Account> GetCredits()
		{
			return database.Table<Account>().FirstOrDefaultAsync();
		}

		public Task<int> CreateCredits(Account account)
		{
			return database.InsertAsync(account);
		}

		public Task<int> UpdateCredits(Account account)
		{
			return database.UpdateAsync(account);
		}

		public Task<List<Currency>> GetItemsAsync()
		{
			return database.Table<Currency>().ToListAsync();
		}

		public Task<int> SaveItemAsync(Currency currency)
		{
			return database.InsertOrReplaceAsync(currency);
		}

		public Task DeleteAllItems()
		{
			return database.QueryAsync<Currency>("delete from Currency");
		}

		public async Task SaveCurrenciesToDatabase()
		{
			HttpClientHandler handler = new HttpClientHandler();
			HttpResponseMessage response = await APIHandler.Get("http://api.fixer.io/latest", handler);
			if (response.IsSuccessStatusCode)
			{
				Root rootCurrency = JsonConvert.DeserializeObject<Root>(await response.Content.ReadAsStringAsync());
				foreach (var item in rootCurrency.rates)
				{
					Currency curr = new Currency(item.Key, item.Value);
					await App.Database.SaveItemAsync(curr);
					curr = null;
				}
			}
		}
	}
}