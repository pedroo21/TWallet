using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TWallet.Models;
using Xamarin.Forms;

namespace TWallet.Views
{
	public partial class Settings : ContentPage
	{
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

		async void InitializeData()
		{
            await CurrencyManager.Init();
			List<Currency> currencies = CurrencyManager.GetCurrencies();
			picker.ItemsSource = currencies;
		}

        async void OnSave(object sender, EventArgs args)
        {
            Currency currency = (Currency)picker.SelectedItem;

            if (currency != null)
            {
                Account account = await App.Database.GetAccount();
                account.RootCurrency = currency.CurrencyKey;
                await App.Database.InsertOrReplaceAccount(account);
            }
        }
	}
}
