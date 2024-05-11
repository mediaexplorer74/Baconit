// Decompiled with JetBrains decompiler
// Type: Baconit.SettingPages.BaconSync.BaconSyncLanding
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Microsoft.Phone.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

#nullable disable
namespace Baconit.SettingPages.BaconSync
{
  public class BaconSyncLanding : PhoneApplicationPage
  {
    private bool firstOpen = true;
    internal Grid LayoutRoot;
    internal StackPanel TitlePanel;
    internal TextBlock ApplicationTitle;
    internal TextBlock PageTitle;
    internal StackPanel ContentPanel;
    private bool _contentLoaded;

    public BaconSyncLanding()
    {
      this.InitializeComponent();
      App.DataManager.BaconitAnalytics.LogEvent("Settings - BaconSync Landing");
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      if (App.DataManager.SettingsMan.BaconSyncEnabled && e.NavigationMode == NavigationMode.Back)
      {
        this.LayoutRoot.Visibility = Visibility.Collapsed;
        this.Dispatcher.BeginInvoke((Action) (() =>
        {
          if (!this.NavigationService.CanGoBack)
            return;
          this.NavigationService.GoBack();
        }));
      }
      else if (App.DataManager.SettingsMan.BaconSyncEnabled)
      {
        this.LayoutRoot.Visibility = Visibility.Collapsed;
        this.NavigationService.Navigate(new Uri("/SettingPages/BaconSync/SyncStatus.xaml?RemoveBack=true", UriKind.Relative));
      }
      else
      {
        this.LayoutRoot.Visibility = Visibility.Visible;
        App.DataManager.SettingsMan.BaconSyncPushURL = "";
      }
      if (!this.firstOpen)
        return;
      this.firstOpen = false;
      if (!this.NavigationContext.QueryString.ContainsKey("RemoveBack") || !this.NavigationService.CanGoBack)
        return;
      this.NavigationService.RemoveBackEntry();
    }

    private void GetStarted_Click(object sender, RoutedEventArgs e)
    {
      this.NavigationService.Navigate(new Uri("/SettingPages/BaconSync/BaconSyncAccount.xaml", UriKind.Relative));
    }

    private void MoreInfo_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.NavigationService.Navigate(new Uri("/SettingPages/BaconSync/BaconSyncInfo.xaml", UriKind.Relative));
      }
      catch
      {
      }
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/SettingPages/BaconSync/BaconSyncLanding.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TitlePanel = (StackPanel) this.FindName("TitlePanel");
      this.ApplicationTitle = (TextBlock) this.FindName("ApplicationTitle");
      this.PageTitle = (TextBlock) this.FindName("PageTitle");
      this.ContentPanel = (StackPanel) this.FindName("ContentPanel");
    }
  }
}
