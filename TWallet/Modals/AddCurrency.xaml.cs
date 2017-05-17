using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace TWallet.Modals
{
	public partial class AddCurrency : ContentPage
	{
		public AddCurrency()
		{
			InitializeComponent();
			InitializeLayout();
		}

		void InitializeLayout()
		{
			Picker picker = this.FindByName<Picker>("currency_picker");
		}

		async void OnDismissButtonClicked(object sender, EventArgs args)
		{
			await Navigation.PopModalAsync();
		}

		//void OnAddTapped(object sender, System.EventArgs e)
		//{
		//    if (Double.TryParse(this.entry_add.Text, out double currencyToAdd))
		//    {
		//        account.CreditsDb += currencyToAdd;
		//        App.Database.UpdateCredits(account);
		//        this.currencies.Clear();
		//        GetCurrenciesFromDatabase();
		//    }
		//}

		//void OnRemoveTapped(object sender, System.EventArgs e)
		//{
		//    if (Double.TryParse(this.entry_remove.Text, out double currencyToRemove))
		//    {
		//        account.CreditsDb -= currencyToRemove;
		//        App.Database.UpdateCredits(account);
		//        this.currencies.Clear();
		//        GetCurrenciesFromDatabase();
		//    }
		//}
	}
}