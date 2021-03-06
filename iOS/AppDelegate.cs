﻿using System;
using System.Collections.Generic;
using System.Linq;

using OxyPlot;
using Foundation;
using UIKit;

namespace TWallet.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            OxyPlot.Xamarin.Forms.Platform.iOS.PlotViewRenderer.Init();

            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, true);

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
