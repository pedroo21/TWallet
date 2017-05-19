using System;
using System.Collections.Generic;

using Xamarin.Forms;

using TWallet.Models;

namespace TWallet.Modals
{
	public partial class AddCurrency : ContentPage
	{
        Picker picker;

		public AddCurrency()
		{
			InitializeComponent();
			InitializeLayout();
            InitializeData();
		}

		void InitializeLayout()
		{
			picker = this.FindByName<Picker>("currency_picker");
		}

        void InitializeData() {
            List<Currency> currencies = CurrencyManager.GetCurrencies();
            currencies.Add(new Currency() 
            {
                CurrencyKey = "EUR",
                CurrencyValue = 1.0000000
            });
            picker.ItemsSource = currencies;
        }

		async void OnTopUpClicked(object sender, EventArgs args)
		{
            Currency currency = (Currency) picker.SelectedItem;

            if (currency != null)
            {
				if (Double.TryParse(this.ammount.Text, out double ammount))
				{
                    //double exchangeRateToEur = 1 / currency.CurrencyValue;
                    //double ammountToAdd = ammount * exchangeRateToEur;

                    Account account = await App.Database.GetAccount();
                    account.AddCredits(currency.CurrencyKey, ammount);

                    App.Database.UpdateCredits(account.Credits);
				}
            }

            await Navigation.PopModalAsync();
		}

		async void OnTopDownClicked(object sender, EventArgs args)
		{
			Currency currency = (Currency)picker.SelectedItem;

			if (currency != null)
			{
				if (Double.TryParse(this.ammount.Text, out double ammount))
				{
					double exchangeRateToEur = 1 / currency.CurrencyValue;
					double ammountToRemove = ammount * exchangeRateToEur;

					Account account = await App.Database.GetAccount();
                    //account.CreditsDb -= ammountToRemove;

					App.Database.UpdateCredits(account.Credits);
				}
			}

			await Navigation.PopModalAsync();
		}

        async void OnDismiss(object sender, EventArgs args)
        {
            await Navigation.PopModalAsync();
        }
	}
}