// Decompiled with JetBrains decompiler
// Type: Baconit.SettingPages.LiveTiles
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Baconit.Database;
using BaconitData.Libs;
using Microsoft.Phone.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Windows.Phone.System.UserProfile;
using Windows.System;

#nullable disable
namespace Baconit.SettingPages
{
  public class LiveTiles : PhoneApplicationPage
  {
    private bool ValuesSet;
    private bool UpdateLockOnLeave;
    private bool UpdaterWarn = true;
    internal Grid LayoutRoot;
    internal StackPanel TitlePanel;
    internal TextBlock SETTINGS;
    internal TextBlock PageTitle;
    internal Grid ContentPanel;
    internal ToggleSwitch LockScreenEnable;
    internal ListPicker LockScreenSubreddit;
    internal ListPicker LockScreenUpdate;
    internal ToggleSwitch OnlyUpdateOnWifiLockScreen;
    internal ListPicker MainLiveTileBacktround;
    internal ToggleSwitch LiveTileShowUnreadCount;
    internal ToggleSwitch ShowLinkKarma;
    internal ToggleSwitch OnlyUpdateOnWifiTiles;
    private bool _contentLoaded;

    public LiveTiles()
    {
      this.InitializeComponent();
      TransitionService.SetNavigationInTransition((UIElement) this, App.DelayedFeatherInTransition);
      TransitionService.SetNavigationOutTransition((UIElement) this, App.FeatherOutTransition);
      App.DataManager.BaconitAnalytics.LogPage("Settings - Live Tiles");
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        this.LiveTileShowUnreadCount.IsChecked = new bool?(App.DataManager.SettingsMan.MainLiveTileShowUnread);
        this.ShowLinkKarma.IsChecked = new bool?(App.DataManager.SettingsMan.MainLiveTileShowKarma);
        this.MainLiveTileBacktround.ItemsSource = (IEnumerable) new List<LiveTileBackgroundItem>()
        {
          new LiveTileBackgroundItem()
          {
            Title = "baconit",
            Path = "/Images/LiveTiles/BaconDispenser_m.png"
          },
          new LiveTileBackgroundItem()
          {
            Title = "snoo",
            Path = "/Images/LiveTiles/RedditAlieanNoBack_m.png"
          },
          new LiveTileBackgroundItem()
          {
            Title = "snoo outline",
            Path = "/Images/LiveTiles/RedditAlieanNoFill_m.png"
          },
          new LiveTileBackgroundItem()
          {
            Title = "snoo face",
            Path = "/Images/LiveTiles/Face_m.png"
          },
          new LiveTileBackgroundItem()
          {
            Title = "inbox message",
            Path = "/Images/LiveTiles/MessageBackground_m.png"
          },
          new LiveTileBackgroundItem()
          {
            Title = "snoo",
            Path = "/Images/LiveTiles/RedditAliean_m.png"
          },
          new LiveTileBackgroundItem()
          {
            Title = "Old School",
            Path = "/Images/LiveTiles/OldBack_m.png"
          }
        };
        this.MainLiveTileBacktround.SelectedIndex = App.DataManager.SettingsMan.MainTileBackground;
        List<SubReddit> subSortedReddits = App.DataManager.SubredditDataManager.GetSubSortedReddits();
        List<string> stringList = new List<string>();
        stringList.Add("windows phone lock screens");
        int num1 = 0;
        int num2 = 1;
        foreach (SubReddit subReddit in subSortedReddits)
        {
          stringList.Add(subReddit.DisplayName.ToLower());
          if (subReddit.URL.Equals(App.DataManager.SettingsMan.LockScreenSubredditURL))
            num1 = num2;
          ++num2;
        }
        this.LockScreenSubreddit.ItemsSource = (IEnumerable) stringList;
        this.LockScreenSubreddit.SelectedIndex = num1;
        App.DataManager.SettingsMan.UseLockScreenImage = LockScreenManager.IsProvidedByCurrentApplication;
        this.LockScreenEnable.IsChecked = new bool?(App.DataManager.SettingsMan.UseLockScreenImage);
        this.OnlyUpdateOnWifiLockScreen.IsChecked = new bool?(App.DataManager.SettingsMan.OnlyUpdateWifiLockScreenImages);
        this.OnlyUpdateOnWifiTiles.IsChecked = new bool?(App.DataManager.SettingsMan.OnlyUpdateWifiTilesImages);
        this.LockScreenUpdate.ItemsSource = (IEnumerable) new List<string>()
        {
          "30 minutes",
          "2 hours",
          "5 hours",
          "12 hours",
          "1 day"
        };
        switch (App.DataManager.SettingsMan.LockScreenUpdateTime)
        {
          case 25:
            this.LockScreenUpdate.SelectedIndex = 0;
            break;
          case 115:
            this.LockScreenUpdate.SelectedIndex = 1;
            break;
          case 495:
            this.LockScreenUpdate.SelectedIndex = 2;
            break;
          case 715:
            this.LockScreenUpdate.SelectedIndex = 3;
            break;
          case 1435:
            this.LockScreenUpdate.SelectedIndex = 4;
            break;
          default:
            this.LockScreenUpdate.SelectedIndex = 2;
            break;
        }
        this.ValuesSet = true;
      }));
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      base.OnNavigatedFrom(e);
      if (e.Uri.OriginalString.Contains("Microsoft.Phone.Controls.Toolkit"))
        return;
      ThreadPool.QueueUserWorkItem((WaitCallback) (obj => App.DataManager.UpdateApplicationTile()));
      if (!this.UpdateLockOnLeave)
        return;
      this.UpdateLockOnLeave = false;
      App.DataManager.BaconitStore.UpdateLastUpdatedTime("LockScreenLastUpdateInternetMS", true);
      ThreadPool.QueueUserWorkItem((WaitCallback) (obj => App.DataManager.SpecialImageManager.UpdateLockSreenWallpapersList(true)));
    }

    private void LiveTileShowUnreadCount_Click(object sender, RoutedEventArgs e)
    {
      if (!this.ValuesSet || this.LiveTileShowUnreadCount == null)
        return;
      App.DataManager.SettingsMan.MainLiveTileShowUnread = this.LiveTileShowUnreadCount.IsChecked.Value;
      new Thread((ThreadStart) (() => App.DataManager.UpdateApplicationTile())).Start();
    }

    private void ShowLinkKarma_Click(object sender, RoutedEventArgs e)
    {
      if (!this.ValuesSet || this.ShowLinkKarma == null)
        return;
      App.DataManager.SettingsMan.MainLiveTileShowKarma = this.ShowLinkKarma.IsChecked.Value;
      new Thread((ThreadStart) (() => App.DataManager.UpdateApplicationTile())).Start();
    }

    private void MainLiveTileBacktround_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!this.ValuesSet || this.MainLiveTileBacktround == null || this.MainLiveTileBacktround.SelectedIndex < 0 || this.MainLiveTileBacktround.SelectedIndex > DataManager.LiveTileBackgrounds.Length)
        return;
      App.DataManager.SettingsMan.MainTileBackground = this.MainLiveTileBacktround.SelectedIndex;
      App.DataManager.BaconitStore.UpdateLastUpdatedTime("MainTilethumb", true);
      new Thread((ThreadStart) (() => App.DataManager.UpdateApplicationTile())).Start();
    }

    private void LockScreenSubreddit_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!this.ValuesSet)
        return;
      List<SubReddit> subSortedReddits = App.DataManager.SubredditDataManager.GetSubSortedReddits();
      if (this.LockScreenSubreddit.SelectedItem.Equals((object) "windows phone lock screens"))
      {
        App.DataManager.SettingsMan.LockScreenSubredditURL = "/r/WPLockscreens";
      }
      else
      {
        foreach (SubReddit subReddit in subSortedReddits)
        {
          if (subReddit.DisplayName.ToLower().Equals(this.LockScreenSubreddit.SelectedItem))
            App.DataManager.SettingsMan.LockScreenSubredditURL = subReddit.URL;
        }
      }
      ThreadPool.QueueUserWorkItem((WaitCallback) (obj => App.DataManager.SpecialImageManager.UpdateLockSreenWallpapersList(true)));
      this.CheckForBackgroundUpdater();
      this.UpdateLockOnLeave = true;
    }

    private void LockScreenUpdate_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!this.ValuesSet)
        return;
      switch (this.LockScreenUpdate.SelectedIndex)
      {
        case 0:
          App.DataManager.SettingsMan.LockScreenUpdateTime = 25;
          break;
        case 1:
          App.DataManager.SettingsMan.LockScreenUpdateTime = 115;
          break;
        case 2:
          App.DataManager.SettingsMan.LockScreenUpdateTime = 495;
          break;
        case 3:
          App.DataManager.SettingsMan.LockScreenUpdateTime = 715;
          break;
        case 4:
          App.DataManager.SettingsMan.LockScreenUpdateTime = 1435;
          break;
      }
      this.CheckForBackgroundUpdater();
    }

    private void LockScreenEnable_Click(object sender, RoutedEventArgs e)
    {
      if (!this.ValuesSet)
        return;
      App.DataManager.SettingsMan.UseLockScreenImage = this.LockScreenEnable.IsChecked.Value;
      if (!this.LockScreenEnable.IsChecked.Value)
        return;
      this.AskForLockScreen(true);
      this.UpdateLockOnLeave = true;
      this.CheckForBackgroundUpdater();
    }

    private async void AskForLockScreen(bool overRide)
    {
      LockScreenRequestResult screenRequestResult = await LockScreenManager.RequestAccessAsync();
      if (LockScreenManager.IsProvidedByCurrentApplication)
      {
        try
        {
          LockScreen.SetImageUri(new Uri("ms-appx:///Images/LockScreenDefault.png", UriKind.Absolute));
        }
        catch
        {
        }
        try
        {
          LockScreen.SetImageUri(new Uri("ms-appx:///Images/LockScreenDefault.png", UriKind.Absolute));
        }
        catch
        {
        }
      }
      else
        this.LockScreenEnable.IsChecked = new bool?(false);
    }

    private async void LockSreen_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        int num = await Launcher.LaunchUriAsync(new Uri("ms-settings-lock:")) ? 1 : 0;
      }
      catch
      {
      }
    }

    private void CheckForBackgroundUpdater()
    {
      if (App.DataManager.SettingsMan.BackgroundAgentEnabled == 1 || !this.UpdaterWarn)
        return;
      this.UpdaterWarn = false;
      App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("", false, false, "Updater Needed", "Without the background updater enabled your lock screen will not rotate images until the app is opened."));
    }

    private void OnlyUpdateOnWifiLockScreen_Click_1(object sender, RoutedEventArgs e)
    {
      if (!this.ValuesSet || this.OnlyUpdateOnWifiLockScreen == null)
        return;
      App.DataManager.SettingsMan.OnlyUpdateWifiLockScreenImages = this.OnlyUpdateOnWifiLockScreen.IsChecked.Value;
      this.CheckForBackgroundUpdater();
      ThreadPool.QueueUserWorkItem((WaitCallback) (obj => App.DataManager.SpecialImageManager.UpdateLockSreenWallpapersList(true)));
    }

    private void OnlyUpdateOnWifiTiles_Click_1(object sender, RoutedEventArgs e)
    {
      if (!this.ValuesSet || this.OnlyUpdateOnWifiTiles == null)
        return;
      App.DataManager.SettingsMan.OnlyUpdateWifiTilesImages = this.OnlyUpdateOnWifiTiles.IsChecked.Value;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/SettingPages/LiveTiles.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TitlePanel = (StackPanel) this.FindName("TitlePanel");
      this.SETTINGS = (TextBlock) this.FindName("SETTINGS");
      this.PageTitle = (TextBlock) this.FindName("PageTitle");
      this.ContentPanel = (Grid) this.FindName("ContentPanel");
      this.LockScreenEnable = (ToggleSwitch) this.FindName("LockScreenEnable");
      this.LockScreenSubreddit = (ListPicker) this.FindName("LockScreenSubreddit");
      this.LockScreenUpdate = (ListPicker) this.FindName("LockScreenUpdate");
      this.OnlyUpdateOnWifiLockScreen = (ToggleSwitch) this.FindName("OnlyUpdateOnWifiLockScreen");
      this.MainLiveTileBacktround = (ListPicker) this.FindName("MainLiveTileBacktround");
      this.LiveTileShowUnreadCount = (ToggleSwitch) this.FindName("LiveTileShowUnreadCount");
      this.ShowLinkKarma = (ToggleSwitch) this.FindName("ShowLinkKarma");
      this.OnlyUpdateOnWifiTiles = (ToggleSwitch) this.FindName("OnlyUpdateOnWifiTiles");
    }
  }
}
