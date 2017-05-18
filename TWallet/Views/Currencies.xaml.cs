using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;

using Newtonsoft.Json;

using TWallet.API;
using TWallet.Models;
using TWallet.Modals;

using Xamarin.Forms;

namespace TWallet.Views
{
	public partial class Currencies : ContentPage
	{
		string[] prices = new string[5];
		static int pos;
		Account account;
		ObservableCollection<Currency> currencies = new ObservableCollection<Currency>();
		Root rootCurrency;

		public Currencies()
		{
			InitializeComponent();
			InitializeLayout();

			pos = 0;
		}

		void InitializeLayout()
		{
			ToolbarItems.Add(new ToolbarItem("add", "icon_add.png", async () =>
			{
				var addCurrencyPage = new AddCurrency();
				await Navigation.PushModalAsync(addCurrencyPage);
			}));
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.currencies.Clear();
            prepareCurrencies();
        }

        async void prepareCurrencies()
        {
            await GetCreditsFromDatabase();
			if (IsInternetAvailable())
			{
				SetDatabaseWithCurrencies();
			}
			else
			{
				GetCurrenciesFromDatabase();
			}
        }

        async Task GetCreditsFromDatabase()
        {
            account = await App.Database.GetCredits();
            if (account == null) 
            {
                await App.Database.CreateCredits(new Account(){ CreditsDb = 100 });
                account = await App.Database.GetCredits();
            }
        }

		async void SetDatabaseWithCurrencies()
		{
			await App.Database.SaveCurrenciesToDatabase();
			GetCurrenciesFromDatabase();
		}

		void OnTapGesture(object sender, EventArgs e)
		{
			if (pos > prices.Length - 1) { pos = 0; }
			this.cash.Text = prices[pos];
			pos++;
		}

		async void GetCurrenciesFromDatabase()
		{
			List<Currency> currenciesFromDatabase = await App.Database.GetItemsAsync();
			rootCurrency = new Root()
			{
				rates = new Dictionary<string, double>()
			};
			foreach (var item in currenciesFromDatabase)
			{
				this.currencies.Add(new Currency(item.CurrencyDescription, item.CurrencyValue, account.CreditsDb));
				this.rootCurrency.rates.Add(item.CurrencyDescription, item.CurrencyValue);
			}
			SetCurrencyListItems();
			Populate();
		}

		void SetCurrencyListItems()
		{
			this.currency_list.ItemsSource = this.currencies;
		}

		/* Helper */

		void Populate()
		{
			prices[0] = account.CreditsDb.ToString("0.00") + " €";
			prices[1] = ConvertTo(account.CreditsDb, CurrencyEnum.USD);
			prices[2] = ConvertTo(account.CreditsDb, CurrencyEnum.GBP);
			prices[3] = ConvertTo(account.CreditsDb, CurrencyEnum.JPY);
			prices[4] = ConvertTo(account.CreditsDb, CurrencyEnum.CAD);
			this.cash.Text = prices[pos];
		}

		string ConvertTo(double currency, string toType)
		{
            if (rootCurrency.rates.ContainsKey(toType))
            {
                double rate = rootCurrency.rates[toType];
                double result = currency * rate;
                return result.ToString("0.00") + " " + toType;
            }
            return "";
		}

		bool IsInternetAvailable()
		{
			string CheckUrl = "http://google.com";

			try
			{
				HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl);

				iNetRequest.Timeout = 5000;

				WebResponse iNetResponse = iNetRequest.GetResponse();

				iNetResponse.Close();

				return true;

			}
			catch (WebException)
			{
                return false;
			}
		}
	}
}
