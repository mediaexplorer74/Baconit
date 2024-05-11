// Decompiled with JetBrains decompiler
// Type: Baconit.SettingPages.General
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Baconit.Database;
using BaconitData.Libs;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

#nullable disable
namespace Baconit.SettingPages
{
  public class General : PhoneApplicationPage
  {
    private bool ValuesSet;
    private bool AdultTold;
    private int oldSortValue;
    internal Grid LayoutRoot;
    internal StackPanel TitlePanel;
    internal TextBlock ApplicationTitle;
    internal TextBlock PageTitle;
    internal Grid ContentPanel;
    internal StackPanel stackPanel1;
    internal ListPicker accountUpdatePicker;
    internal ListPicker LandingBackground;
    internal ListPicker LandingTopStories;
    internal ListPicker SortSubRedditsBy;
    internal ListPicker NumberRecent;
    internal ToggleSwitch ResetOrentationLock;
    internal ToggleSwitch ResetAppOnHome;
    internal ToggleSwitch ShowSystemBar;
    internal ToggleSwitch AdultFilter;
    internal Button ClearCache;
    internal Button sendCacheReport;
    private bool _contentLoaded;

    public General()
    {
      this.InitializeComponent();
      TransitionService.SetNavigationInTransition((UIElement) this, App.FeatherInTransition);
      TransitionService.SetNavigationOutTransition((UIElement) this, App.FeatherOutTransition);
      App.DataManager.BaconitAnalytics.LogPage("Settings - General");
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      if (this.State.ContainsKey(nameof (General)))
        return;
      this.State[nameof (General)] = (object) true;
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        switch ((int) App.DataManager.SettingsMan.AccountUpdateTime)
        {
          case 0:
            this.accountUpdatePicker.SelectedIndex = 0;
            break;
          case 120000:
            this.accountUpdatePicker.SelectedIndex = 1;
            break;
          case 300000:
            this.accountUpdatePicker.SelectedIndex = 2;
            break;
          case 600000:
            this.accountUpdatePicker.SelectedIndex = 3;
            break;
          case 900000:
            this.accountUpdatePicker.SelectedIndex = 4;
            break;
          case 1200000:
            this.accountUpdatePicker.SelectedIndex = 5;
            break;
          case 1800000:
            this.accountUpdatePicker.SelectedIndex = 6;
            break;
          case 3600000:
            this.accountUpdatePicker.SelectedIndex = 7;
            break;
          case 7200000:
            this.accountUpdatePicker.SelectedIndex = 8;
            break;
          case 18000000:
            this.accountUpdatePicker.SelectedIndex = 9;
            break;
          case 86400000:
            this.accountUpdatePicker.SelectedIndex = 10;
            break;
        }
        this.NumberRecent.SelectedIndex = App.DataManager.SettingsMan.NumberOfRecentTiles;
        switch (App.DataManager.SettingsMan.SortSubRedditsBy)
        {
          case 0:
            this.SortSubRedditsBy.SelectedIndex = 0;
            break;
          case 1:
            this.SortSubRedditsBy.SelectedIndex = 1;
            break;
          default:
            this.SortSubRedditsBy.SelectedIndex = 0;
            break;
        }
        this.oldSortValue = App.DataManager.SettingsMan.SortSubRedditsBy;
        this.AdultFilter.IsChecked = new bool?(App.DataManager.SettingsMan.AdultFilter);
        this.ShowSystemBar.IsChecked = new bool?(App.DataManager.SettingsMan.ShowSystemBar);
        this.ResetOrentationLock.IsChecked = new bool?(App.DataManager.SettingsMan.ResetOrenationOnStart);
        this.ResetAppOnHome.IsChecked = new bool?(App.DataManager.SettingsMan.FreshStartFromLiveTile);
        List<SubReddit> subSortedReddits = App.DataManager.SubredditDataManager.GetSubSortedReddits();
        List<string> stringList1 = new List<string>();
        List<string> stringList2 = new List<string>();
        int num1 = 0;
        int num2 = 0;
        int num3 = 1;
        foreach (SubReddit subReddit in subSortedReddits)
        {
          stringList1.Add(subReddit.DisplayName.ToLower());
          stringList2.Add(subReddit.DisplayName.ToLower());
          if (subReddit.URL.Equals(App.DataManager.SettingsMan.LandingWallpaper))
            num1 = num3;
          if (subReddit.URL.Equals(App.DataManager.SettingsMan.LandingStoryProvider))
            num2 = num3;
          ++num3;
        }
        stringList2.Insert(0, "most viewed subreddit");
        this.LandingTopStories.ItemsSource = (IEnumerable) stringList2;
        this.LandingTopStories.SelectedIndex = num2;
        stringList1.Insert(0, "earth images");
        this.LandingBackground.ItemsSource = (IEnumerable) stringList1;
        this.LandingBackground.SelectedIndex = num1;
        this.ValuesSet = true;
      }));
    }

    private void accountUpdatePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!this.ValuesSet || this.accountUpdatePicker == null || this.accountUpdatePicker.SelectedIndex < 0)
        return;
      switch (this.accountUpdatePicker.SelectedIndex)
      {
        case 0:
          App.DataManager.SettingsMan.AccountUpdateTime = 0.0;
          break;
        case 1:
          App.DataManager.SettingsMan.AccountUpdateTime = 120000.0;
          break;
        case 2:
          App.DataManager.SettingsMan.AccountUpdateTime = 300000.0;
          break;
        case 3:
          App.DataManager.SettingsMan.AccountUpdateTime = 600000.0;
          break;
        case 4:
          App.DataManager.SettingsMan.AccountUpdateTime = 900000.0;
          break;
        case 5:
          App.DataManager.SettingsMan.AccountUpdateTime = 1200000.0;
          break;
        case 6:
          App.DataManager.SettingsMan.AccountUpdateTime = 1800000.0;
          break;
        case 7:
          App.DataManager.SettingsMan.AccountUpdateTime = 3600000.0;
          break;
        case 8:
          App.DataManager.SettingsMan.AccountUpdateTime = 7200000.0;
          break;
        case 9:
          App.DataManager.SettingsMan.AccountUpdateTime = 18000000.0;
          break;
        case 10:
          App.DataManager.SettingsMan.AccountUpdateTime = 86400000.0;
          break;
      }
    }

    private void NumberRecent_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!this.ValuesSet || this.NumberRecent == null || this.NumberRecent.SelectedIndex < 0)
        return;
      App.DataManager.SettingsMan.NumberOfRecentTiles = this.NumberRecent.SelectedIndex;
      App.DataManager.SettingsMan.CheckMainLandingTileCount();
    }

    private void ShowSystemBar_Click(object sender, RoutedEventArgs e)
    {
      if (!this.ValuesSet || this.ShowSystemBar == null)
        return;
      App.DataManager.SettingsMan.ShowSystemBar = this.ShowSystemBar.IsChecked.Value;
    }

    private void AdultFilter_Click(object sender, RoutedEventArgs e)
    {
      if (!this.ValuesSet || this.AdultFilter == null)
        return;
      App.DataManager.SettingsMan.AdultFilter = this.AdultFilter.IsChecked.Value;
      if (this.AdultTold)
        return;
      this.AdultTold = true;
      int num = (int) MessageBox.Show("The results of this setting will not be noticed until Baconit’s content is refreshed.", "Adult Content Filter", MessageBoxButton.OK);
    }

    private void ResetOrentationLock_Click(object sender, RoutedEventArgs e)
    {
      if (!this.ValuesSet || this.ShowSystemBar == null)
        return;
      App.DataManager.SettingsMan.ResetOrenationOnStart = this.ResetOrentationLock.IsChecked.Value;
    }

    private void SortSubRedditsBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!this.ValuesSet || this.SortSubRedditsBy == null || this.SortSubRedditsBy.SelectedIndex < 0)
        return;
      switch (this.SortSubRedditsBy.SelectedIndex)
      {
        case 0:
          App.DataManager.SettingsMan.SortSubRedditsBy = 0;
          break;
        case 1:
          App.DataManager.SettingsMan.SortSubRedditsBy = 1;
          break;
      }
      if (this.SortSubRedditsBy.SelectedIndex != this.oldSortValue)
        new Thread((ThreadStart) (() => App.DataManager.SubredditDataManager.ReSortSubreddits())).Start();
      this.oldSortValue = this.SortSubRedditsBy.SelectedIndex;
    }

    private void ResetAppOnHome_Click_1(object sender, RoutedEventArgs e)
    {
      if (!this.ValuesSet || this.ResetAppOnHome == null)
        return;
      App.DataManager.SettingsMan.FreshStartFromLiveTile = this.ResetAppOnHome.IsChecked.Value;
    }

    private void LandingBackground_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!this.ValuesSet)
        return;
      List<SubReddit> subSortedReddits = App.DataManager.SubredditDataManager.GetSubSortedReddits();
      if (this.LandingBackground.SelectedItem.Equals((object) "earth images"))
      {
        App.DataManager.SettingsMan.LandingWallpaper = "/r/earthporn/";
      }
      else
      {
        foreach (SubReddit subReddit in subSortedReddits)
        {
          if (subReddit.DisplayName.ToLower().Equals(this.LandingBackground.SelectedItem))
            App.DataManager.SettingsMan.LandingWallpaper = subReddit.URL;
        }
      }
      ThreadPool.QueueUserWorkItem((WaitCallback) (obj => App.DataManager.MainLandingImageMan.CheckForUpdate(true)));
    }

    private void LandingTopStories_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!this.ValuesSet)
        return;
      List<SubReddit> subSortedReddits = App.DataManager.SubredditDataManager.GetSubSortedReddits();
      if (this.LandingTopStories.SelectedItem.Equals((object) "most viewed subreddit"))
      {
        App.DataManager.SettingsMan.LandingStoryProvider = "mostViewed";
      }
      else
      {
        foreach (SubReddit subReddit in subSortedReddits)
        {
          if (subReddit.DisplayName.ToLower().Equals(this.LandingTopStories.SelectedItem))
            App.DataManager.SettingsMan.LandingStoryProvider = subReddit.URL;
        }
      }
    }

    private void ClearCache_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.ClearCache.IsEnabled = false;
      ThreadPool.QueueUserWorkItem((WaitCallback) (obj =>
      {
        App.DataManager.CacheMan.DeleteCache();
        this.Dispatcher.BeginInvoke((Action) (() =>
        {
          App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("Cache cleared", true, false, "", ""));
          this.ClearCache.IsEnabled = true;
        }));
      }));
    }

    private void SendCacheReport_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.sendCacheReport.IsEnabled = false;
      ThreadPool.QueueUserWorkItem((WaitCallback) (obj =>
      {
        App.DataManager.CacheMan.GetCacheReport();
        this.Dispatcher.BeginInvoke((Action) (() =>
        {
          new EmailComposeTask()
          {
            To = "quinnd@outlook.com",
            Subject = ("Baconit Cache Report - " + (object) new Random().Next(9999999)),
            Body = ("\n\rCache Report:\n" + App.DataManager.CacheMan.GetCacheReport())
          }.Show();
          this.sendCacheReport.IsEnabled = true;
        }));
      }));
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/SettingPages/General.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TitlePanel = (StackPanel) this.FindName("TitlePanel");
      this.ApplicationTitle = (TextBlock) this.FindName("ApplicationTitle");
      this.PageTitle = (TextBlock) this.FindName("PageTitle");
      this.ContentPanel = (Grid) this.FindName("ContentPanel");
      this.stackPanel1 = (StackPanel) this.FindName("stackPanel1");
      this.accountUpdatePicker = (ListPicker) this.FindName("accountUpdatePicker");
      this.LandingBackground = (ListPicker) this.FindName("LandingBackground");
      this.LandingTopStories = (ListPicker) this.FindName("LandingTopStories");
      this.SortSubRedditsBy = (ListPicker) this.FindName("SortSubRedditsBy");
      this.NumberRecent = (ListPicker) this.FindName("NumberRecent");
      this.ResetOrentationLock = (ToggleSwitch) this.FindName("ResetOrentationLock");
      this.ResetAppOnHome = (ToggleSwitch) this.FindName("ResetAppOnHome");
      this.ShowSystemBar = (ToggleSwitch) this.FindName("ShowSystemBar");
      this.AdultFilter = (ToggleSwitch) this.FindName("AdultFilter");
      this.ClearCache = (Button) this.FindName("ClearCache");
      this.sendCacheReport = (Button) this.FindName("sendCacheReport");
    }
  }
}
