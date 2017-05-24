using System;
using System.Collections.Generic;

using Xamarin.Forms;
using OxyPlot.Xamarin.Forms;
using OxyPlot;
using OxyPlot.Series;

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
				plotSeries.Slices.Add(new PieSlice(credit.Currency, credit.Amount) { IsExploded = false });
			}

			plot.Series.Add(plotSeries);

			PlotView view = this.FindByName<PlotView>("graph");
			view.Model = plot;
            plot.InvalidatePlot(true);
        }

		public PlotModel Plot
		{
			get { return plot; }
			set { plot = value; }
		}
    }
}
