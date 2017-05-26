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
using TWallet.Util;

using Xamarin.Forms;
using System.Net.Http;

namespace TWallet.Views
{
	public partial class Currencies : ContentPage
	{
        private ListView list;
        private ObservableCollection<Credit> credits = new ObservableCollection<Credit>();
        private Account account;

        private double cashDisplay;

		public Currencies()
		{
			InitializeComponent();
			InitializeLayout();

            this.list = this.FindByName<ListView>("currency_list");
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
            CurrencyManager.ClearCurrencies();
            this.credits.Clear();
            PrepareCurrencies();
        }

        // Initialize the listing
        async void PrepareCurrencies()
        {
            account = App.Database.GetAccount();
			if (account.Credits.Count == 0)
			{
				list.IsVisible = false;
				Label total = this.FindByName<Label>("cash");
				total.Text = "Empty";
			}
            else {
                list.IsVisible = true;
				Label empty = this.FindByName<Label>("cash_empty");
				empty.IsVisible = false;
            }
            try
            {
                await App.Database.SaveCurrenciesToDatabase(account.RootCurrency);
            }
            catch(HttpRequestException) { }
            GetCurrenciesFromDatabase();

            if(CurrencyManager.GetCurrencies().Count <= 0)
            {
                return; // no connection
            }
        }

        // Double tap gesture on the wallet total
		async void OnTapGesture(object sender, EventArgs e)
		{
            Currency cur = CurrencyManager.MoveNext(account.RootCurrency);
            if (cur != null)
            {
                account.RootCurrency = cur.CurrencyKey;
                App.Database.InsertOrReplaceAccount(account);
                try
                {
                    await App.Database.SaveCurrenciesToDatabase(account.RootCurrency);
                }
                catch(HttpRequestException) { }
            }
            OnAppearing();
		}

        // Get all cached currencies from the database
		void GetCurrenciesFromDatabase()
		{
            bool dbSetup = CurrencyManager.Init(account.RootCurrency);
            if (!dbSetup)
            {
                // launch activity saying internet is required
                return;
            }
			foreach (var item in account.Credits)
			{
                if (!this.credits.Contains(item))
                {
                    this.credits.Add(item);
                }
			}
            this.currency_list.ItemsSource = this.credits;
            this.currency_list.HasUnevenRows = true;
			CalculateRates();
		}

		/* Helper */

		void CalculateRates()
		{
            double total = 0;
            foreach (var credit in account.Credits) 
            {
                Currency currency = credit.GetCurrency();
                if (currency != null)
                {
                    double exchangeRate = 1 / currency.CurrencyValue;
                    total += credit.Amount * exchangeRate;
                }
            }
            this.cashDisplay = total;
            this.cash.Text = cashDisplay.ToString("0.00 ") + account.RootCurrency;
		}

		string ConvertTo(double amount, string toType)
		{
            Currency currency;
            if ((currency = CurrencyManager.GetCurrency(toType)) != null)
            {
                double rate = CurrencyManager.GetCurrency(toType).CurrencyValue;
                double result = amount * rate;
                return result.ToString("0.00 ") + currency.CurrencySymbol;
            }
            return "";
		}
	}
}
