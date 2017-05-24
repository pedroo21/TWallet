using Xamarin.Forms;
using TWallet.Views;
using TWallet.Models;

namespace TWallet
{
	public partial class TWalletPage : TabbedPage
	{
		public TWalletPage()
		{
			InitializeComponent();
			InitializeLayout();
		}

		void InitializeLayout()
		{
			Color navColor = Color.FromHex("#007ef6");
			Color navText = Color.White;

			var graphsPage = new NavigationPage(new Graphs());
			graphsPage.BarBackgroundColor = navColor;
			graphsPage.BarTextColor = navText;
			graphsPage.Icon = "icon_metrics.png";
			graphsPage.Title = "Metrics";

			var currencyPage = new NavigationPage(new Currencies());
			currencyPage.BarBackgroundColor = navColor;
			currencyPage.BarTextColor = navText;
			currencyPage.Icon = "icon_money.png";
			currencyPage.Title = "Wallet";

			var ratesPage = new NavigationPage(new Rates());
			ratesPage.BarBackgroundColor = navColor;
			ratesPage.BarTextColor = navText;
			ratesPage.Icon = "icon_rates.png";
			ratesPage.Title = "Rates";

			var settingsPage = new NavigationPage(new Settings());
			settingsPage.BarBackgroundColor = navColor;
			settingsPage.BarTextColor = navText;
			settingsPage.Icon = "icon_settings.png";
			settingsPage.Title = "Settings";

            Children.Add(currencyPage);
			Children.Add(graphsPage);
            Children.Add(ratesPage);
            Children.Add(settingsPage);
		}
	}
}
