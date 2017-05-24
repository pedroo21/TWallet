using System;
using System.Collections.Generic;

using TWallet.Models;
using Xamarin.Forms;

namespace TWallet.Views
{
    public partial class Rates : ContentPage
    {
        private Account account;

        public Rates()
        {
            InitializeComponent();

            this.cash.Text = "Pick a currency";
        }

        protected override void OnAppearing() {
            account = App.Database.GetAccount();
            CurrencyManager.Init(account.RootCurrency);
			List<Currency> currency = CurrencyManager.GetCurrencies();
			this.currency_picker.ItemsSource = currency;
        }

        public void OnIndexChanged(object sender, EventArgs e) {
            OnChangeAmount(sender, null);
        }

        public void OnChangeAmount(object sender, TextChangedEventArgs e)
        {
            if (Int32.TryParse(this.amount.Text, System.Globalization.NumberStyles.AllowDecimalPoint,
                               System.Globalization.CultureInfo.InvariantCulture, out int number))
            {
                if (this.currency_picker.SelectedItem != null)
                {
                    if (this.currency_picker.SelectedItem.GetType() == typeof(Currency))
                    {
                        Currency currency = (Currency)this.currency_picker.SelectedItem;
                        this.cash.Text = ConvertFrom(number, currency);
                    }
                }
            }
        }

		string ConvertFrom(double currency, Currency type)
		{
			if (type != null)
			{
                Currency root = CurrencyManager.GetCurrency(account.RootCurrency);
				double exchangeRate = 1 / type.CurrencyValue;
				double total = currency * exchangeRate;
				return total.ToString("0.00 ") + root.CurrencySymbol;
			}
			return "";
		}
    }
}
