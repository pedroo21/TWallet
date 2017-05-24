using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TWallet.Models;
using Xamarin.Forms;

namespace TWallet.Views
{
	public partial class Settings : ContentPage
	{
        Account account;
        Picker picker;

		public Settings()
		{
			InitializeComponent();
            InitializeLayout();
            InitializeData();
		}

		void InitializeLayout()
		{
			picker = this.FindByName<Picker>("currency_picker");
		}

		void InitializeData()
		{
            account = App.Database.GetAccount();
            CurrencyManager.Init(account.RootCurrency);
			List<Currency> currencies = CurrencyManager.GetCurrencies();
			picker.ItemsSource = currencies;
		}

        async void OnSave(object sender, EventArgs args)
        {
            Currency currency = (Currency)picker.SelectedItem;

            if (currency != null)
            {
                account.RootCurrency = currency.CurrencyKey;
                App.Database.InsertOrReplaceAccount(account);
                await App.Database.SaveCurrenciesToDatabase(account.RootCurrency);
            }
        }
	}
}
