﻿using Baconit.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Baconit.Panels.SettingsPanels
{
    public sealed partial class AboutSettings : UserControl, IPanel
    {
        IPanelHost m_host;

        public AboutSettings()
        {
            this.InitializeComponent();
        }

        public void PanelSetup(IPanelHost host, Dictionary<string, object> arguments)
        {
            m_host = host;
        }

        public void OnNavigatingFrom()
        {
            // Ignore
        }

        public void OnPanelPulledToTop(Dictionary<string, object> arguments)
        {
            // Ignore
        }

        public void OnNavigatingTo()
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;
            ui_buildString.Text = $"Build: {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";

            App.BaconMan.TelemetryMan.ReportEvent(this, "AboutOpened");
        }

        private async void RateAndReview_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=" + CurrentApp.AppId));
            App.BaconMan.TelemetryMan.ReportEvent(this, "RateAndReviewTapped");
        }

        private void Facebook_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OpenGlobalPresenter("http://facebook.com/Baconit");
            App.BaconMan.TelemetryMan.ReportEvent(this, "FacebookOpened");
        }

        private void Website_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OpenGlobalPresenter("http://baconit.quinndamerell.com/");
            App.BaconMan.TelemetryMan.ReportEvent(this, "WebsiteOpened");
        }

        private void Twitter_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OpenGlobalPresenter("http://twitter.com/BaconitWP");
            App.BaconMan.TelemetryMan.ReportEvent(this, "TwitterOpened");
        }

        private void ShowSource_Tapped(object sender, TappedRoutedEventArgs e)
        {
            OpenGlobalPresenter("http://github.com/QuinnDamerell/Baconit");
            App.BaconMan.TelemetryMan.ReportEvent(this, "SourceOpened");
        }

        private void OpenBaconitSub_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //#todo make this open in app when we support it
            OpenGlobalPresenter("http://reddit.com/r/Baconit");
            App.BaconMan.TelemetryMan.ReportEvent(this, "SubredditOpened");
        }

        private void Logo_Tapped(object sender, TappedRoutedEventArgs e)
        {
            m_host.Navigate(typeof(DeveloperSettings), "DeveloperSettings");
        }

        private void OpenGlobalPresenter(string url)
        {
            App.BaconMan.ShowGlobalContent(url);
        }
    }
}
