﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.NetworkInformation;

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
            GetCreditsFromDatabase();

            if (IsInternetAvailable())
            {
            	SetDatabaseWithCurrencies();
            }
            else
            {
            	GetCurrenciesFromDatabase();
            }

        }

        async void GetCreditsFromDatabase()
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

			prices[0] = account.CreditsDb.ToString() + " €";
			prices[1] = ConvertTo(account.CreditsDb, CurrencyEnum.USD);
			prices[2] = ConvertTo(account.CreditsDb, CurrencyEnum.GBP);
			prices[3] = ConvertTo(account.CreditsDb, CurrencyEnum.JPY);
			prices[4] = ConvertTo(account.CreditsDb, CurrencyEnum.CAD);
			this.cash.Text = prices[pos];
		}

		string ConvertTo(double currency, string toType)
		{
			double rate = rootCurrency.rates[toType];
			double result = currency * rate;
			return result + " " + toType;
		}

		bool IsInternetAvailable()
		{
			try
			{
				Ping ping = new Ping();
				string host = "google.pt";
				byte[] buffer = new byte[32];
				int timeout = 500;

				PingOptions opts = new PingOptions();
				PingReply reply = ping.Send(host, timeout, buffer, opts);

				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
	}
}