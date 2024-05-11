// Decompiled with JetBrains decompiler
// Type: Baconit.SettingPages.BaconSync.SyncStatus
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Baconit.Database;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

#nullable disable
namespace Baconit.SettingPages.BaconSync
{
  public class SyncStatus : PhoneApplicationPage
  {
    private bool firstOpen = true;
    internal Grid LayoutRoot;
    internal StackPanel TitlePanel;
    internal TextBlock ApplicationTitle;
    internal TextBlock PageTitle;
    internal StackPanel ContentPanel;
    internal TextBlock AccountName;
    internal TextBlock DeviceName;
    internal TextBlock SyncStatusText;
    internal TextBlock LastSync;
    internal Button SetPassCode;
    internal Button SignOut;
    private bool _contentLoaded;

    public SyncStatus() => this.InitializeComponent();

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      if (this.firstOpen)
      {
        this.firstOpen = false;
        if (this.NavigationContext.QueryString.ContainsKey("RemoveBack") && this.NavigationService.CanGoBack)
          this.NavigationService.RemoveBackEntry();
      }
      this.SyncStatusText.Text = App.DataManager.SettingsMan.BaconSyncEnabled ? "Last Status: " + App.DataManager.SettingsMan.BaconSyncStatus : "Last Status: Disabled";
      DateTime dateTime = BaconitStore.DoubleToDateTime(App.DataManager.BaconitStore.LastUpdatedTime("BaconSync"));
      if (dateTime.Year == 1989)
      {
        this.LastSync.Text = "Last Time: Never";
      }
      else
      {
        CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
        dateTime.ToString(currentCulture.DateTimeFormat.ShortDatePattern.ToString());
        this.LastSync.Text = "Last Time: " + dateTime.ToString(currentCulture.DateTimeFormat.ShortDatePattern.ToString() + " " + currentCulture.DateTimeFormat.ShortTimePattern.ToString());
      }
      this.AccountName.Text = "Account Name: " + App.DataManager.SettingsMan.BaconSyncAccountName;
      this.DeviceName.Text = "Device Name: " + App.DataManager.SettingsMan.BaconSyncDeviceName;
      if (!App.DataManager.SettingsMan.BaconSyncStatus.Equals("Failed - Wrong Pass Code"))
        return;
      int num;
      this.Dispatcher.BeginInvoke((Action) (() => num = (int) MessageBox.Show("Your pass code is not longer valid, please either set a new pass code or sign out and back into BaconSync.", "Pass Code Error", MessageBoxButton.OK)));
      this.SyncStatusText.Foreground = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, (byte) 50, (byte) 50));
    }

    private void SignOut_Click(object sender, RoutedEventArgs e)
    {
      if (MessageBox.Show("Signing out of BaconSync will stop all synchronization until signed back in.", "Sign out", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
        return;
      App.DataManager.BaconSyncObj.SignOutOfBaconSync();
      this.NavigationService.Navigate(new Uri("/SettingPages/BaconSync/BaconSyncLanding.xaml?RemoveBack=true", UriKind.Relative));
    }

    private void Extensions_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        new WebBrowserTask()
        {
          Uri = new Uri("http://www.quinndamerell.com/Baconit/BaconSync/", UriKind.Absolute)
        }.Show();
      }
      catch
      {
      }
    }

    private void SetPassCode_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.NavigationService.Navigate(new Uri("/SettingPages/BaconSync/SetNewPassCode.xaml", UriKind.Relative));
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
      Application.LoadComponent((object) this, new Uri("/Baconit;component/SettingPages/BaconSync/SyncStatus.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TitlePanel = (StackPanel) this.FindName("TitlePanel");
      this.ApplicationTitle = (TextBlock) this.FindName("ApplicationTitle");
      this.PageTitle = (TextBlock) this.FindName("PageTitle");
      this.ContentPanel = (StackPanel) this.FindName("ContentPanel");
      this.AccountName = (TextBlock) this.FindName("AccountName");
      this.DeviceName = (TextBlock) this.FindName("DeviceName");
      this.SyncStatusText = (TextBlock) this.FindName("SyncStatusText");
      this.LastSync = (TextBlock) this.FindName("LastSync");
      this.SetPassCode = (Button) this.FindName("SetPassCode");
      this.SignOut = (Button) this.FindName("SignOut");
    }
  }
}
