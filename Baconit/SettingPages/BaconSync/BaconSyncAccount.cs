// Decompiled with JetBrains decompiler
// Type: Baconit.SettingPages.BaconSync.BaconSyncAccount
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using BaconitData.Database;
using Microsoft.Phone.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

#nullable disable
namespace Baconit.SettingPages.BaconSync
{
  public class BaconSyncAccount : PhoneApplicationPage
  {
    internal Grid LayoutRoot;
    internal StackPanel TitlePanel;
    internal TextBlock ApplicationTitle;
    internal TextBlock PageTitle;
    internal StackPanel ContentPanel;
    internal ListPicker accountPicker;
    internal Button NextPage;
    private bool _contentLoaded;

    public BaconSyncAccount() => this.InitializeComponent();

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      if (App.DataManager.SettingsMan.BaconSyncEnabled)
      {
        this.LayoutRoot.Visibility = Visibility.Collapsed;
        this.Dispatcher.BeginInvoke((Action) (() =>
        {
          if (!this.NavigationService.CanGoBack)
            return;
          this.NavigationService.GoBack();
        }));
      }
      else
        this.LayoutRoot.Visibility = Visibility.Visible;
      string str = "";
      List<RedditAccount> userAccounts = App.DataManager.SettingsMan.UserAccounts;
      List<string> stringList = new List<string>();
      foreach (RedditAccount redditAccount in userAccounts)
      {
        stringList.Add(redditAccount.UserName);
        if (redditAccount.UserName.Equals(App.DataManager.SettingsMan.BaconSyncAccountName))
          str = redditAccount.UserName;
      }
      this.accountPicker.ItemsSource = (IEnumerable) stringList;
      if (!str.Equals(""))
        this.accountPicker.SelectedItem = (object) str;
      if (stringList.Count != 0)
      {
        this.NextPage.IsEnabled = true;
        this.accountPicker.IsEnabled = true;
      }
      else
      {
        this.NextPage.IsEnabled = false;
        this.accountPicker.IsEnabled = false;
      }
    }

    private void AddAccount_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        this.NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
      }
      catch
      {
      }
    }

    private void Next_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        App.DataManager.SettingsMan.BaconSyncAccountName = ((string) this.accountPicker.SelectedItem).ToLower();
        this.NavigationService.Navigate(new Uri("/SettingPages/BaconSync/BaconSyncAccountInfo.xaml", UriKind.Relative));
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
      Application.LoadComponent((object) this, new Uri("/Baconit;component/SettingPages/BaconSync/BaconSyncAccount.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TitlePanel = (StackPanel) this.FindName("TitlePanel");
      this.ApplicationTitle = (TextBlock) this.FindName("ApplicationTitle");
      this.PageTitle = (TextBlock) this.FindName("PageTitle");
      this.ContentPanel = (StackPanel) this.FindName("ContentPanel");
      this.accountPicker = (ListPicker) this.FindName("accountPicker");
      this.NextPage = (Button) this.FindName("NextPage");
    }
  }
}
