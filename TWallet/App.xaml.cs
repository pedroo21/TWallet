using Xamarin.Forms;

namespace TWallet
{
	public partial class App : Application
	{
		static TWalletDatabase database = new TWalletDatabase(
            DependencyService.Get<IFileHelper>().GetLocalFilePath("TWalletSQLite.db3"));

		public App()
		{
			InitializeComponent();

			MainPage = new TWalletPage();
		}

		public static TWalletDatabase Database
		{
			get
			{
				if (database == null)
				{
					database = new TWalletDatabase(
                        DependencyService.Get<IFileHelper>().GetLocalFilePath("TWalletSQLite.db3"));
				}
				return database;
			}
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
