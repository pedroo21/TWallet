using Xamarin.Forms;
using TWallet.Views;

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

			var currencyPage = new NavigationPage(new Currencies());
			currencyPage.BarBackgroundColor = navColor;
			currencyPage.BarTextColor = navText;
			currencyPage.Icon = "icon_money.png";
			currencyPage.Title = "Wallet";

			var settingsPage = new NavigationPage(new Settings());
			settingsPage.BarBackgroundColor = navColor;
			settingsPage.BarTextColor = navText;
			settingsPage.Icon = "icon_settings.png";
			settingsPage.Title = "Settings";

			Children.Add(currencyPage);
			Children.Add(settingsPage);
		}
	}
}
