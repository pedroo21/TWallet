using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TWallet.Modals
{
	public partial class AddCurrency : ContentPage
	{
        Picker picker;
        List<Currency> currencies;

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

        async void InitializeData() {
            currencies = await App.Database.GetItemsAsync();
            currencies.Add(new Currency() 
            {
                CurrencyDescription = "EUR",
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
                    double exchangeRateToEur = 1 / currency.CurrencyValue;
                    double ammountToAdd = ammount * exchangeRateToEur;

                    Account account = await App.Database.GetCredits();
                    account.CreditsDb += ammountToAdd;

                    await App.Database.UpdateCredits(account);
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

					Account account = await App.Database.GetCredits();
                    account.CreditsDb -= ammountToRemove;

					await App.Database.UpdateCredits(account);
				}
			}

			await Navigation.PopModalAsync();
		}
	}
}