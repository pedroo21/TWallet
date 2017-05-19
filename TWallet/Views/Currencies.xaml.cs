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
            account = await App.Database.GetAccount();
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
            if (IsInternetAvailable())
            {
                await App.Database.SaveCurrenciesToDatabase(account.RootCurrency);
            }
            GetCurrenciesFromDatabase();
        }

        // Double tap gesture on the wallet total
		async void OnTapGesture(object sender, EventArgs e)
		{
            Currency cur = CurrencyManager.MoveNext(account.RootCurrency);
            if (cur != null)
            {
                account.RootCurrency = cur.CurrencyKey;
                await App.Database.InsertOrReplaceAccount(account);
            }
            OnAppearing();
		}

        // Get all cached currencies from the database
		async void GetCurrenciesFromDatabase()
		{
            await CurrencyManager.Init();
			foreach (var item in account.Credits)
			{
				this.credits.Add(item);
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

		string ConvertTo(double currency, string toType)
		{
            if (CurrencyManager.GetCurrency(toType) != null)
            {
                double rate = CurrencyManager.GetCurrency(toType).CurrencyValue;
                double result = currency * rate;
                return result.ToString("0.00 ") + toType;
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
