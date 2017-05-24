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
            picker.ItemsSource = currencies;
        }

		async void OnTopUpClicked(object sender, EventArgs args)
		{
            Currency currency = (Currency) picker.SelectedItem;

            if (currency != null)
            {
				if (Double.TryParse(this.amount.Text, out double amount))
				{
                    //double exchangeRateToEur = 1 / currency.CurrencyValue;
                    //double ammountToAdd = ammount * exchangeRateToEur;

                    Account account = App.Database.GetAccount();
                    account.AddCredits(currency.CurrencyKey, amount);

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
                if (Double.TryParse(this.amount.Text, out double amount))
				{
					Account account = App.Database.GetAccount();
                    account.RemoveCredits(currency.CurrencyKey, amount);
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