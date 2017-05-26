using System;
using System.Collections.Generic;

using Xamarin.Forms;
using OxyPlot.Xamarin.Forms;
using OxyPlot;
using OxyPlot.Series;
using TWallet.Models;

namespace TWallet.Views
{
    public partial class Graphs : ContentPage
    {
        private Account account;
        private PlotModel plot;

        public Graphs()
        {
            InitializeComponent();

			plot = new PlotModel { Title = "Currency Graph" };
        }

        protected override void OnAppearing()
        {
			account = App.Database.GetAccount();

			dynamic plotSeries = new PieSeries
			{
				StrokeThickness = 1.0,
				InsideLabelPosition = 0.8,
				AngleSpan = 360,
				StartAngle = 0
			};

			foreach (var credit in account.Credits)
			{
                plotSeries.Slices.Add(new PieSlice(credit.Currency, ConvertTo(credit.Amount, credit.Currency)) 
                { 
                    IsExploded = false 
                });
			}

			plot.Series.Add(plotSeries);

			PlotView view = this.FindByName<PlotView>("graph");
			view.Model = plot;
            plot.InvalidatePlot(true);
        }

		double ConvertTo(double amount, string toType)
		{
			Currency currency;
			if ((currency = CurrencyManager.GetCurrency(toType)) != null)
			{
				double rate = CurrencyManager.GetCurrency(toType).CurrencyValue;
				double result = amount * rate;
                return result;
			}
			return 0;
		}

		public PlotModel Plot
		{
			get { return plot; }
			set { plot = value; }
		}
    }
}
