// Decompiled with JetBrains decompiler
// Type: Baconit.MainLandingViewModel
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Baconit.Database;
using Baconit.Libs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Baconit
{
  public class MainLandingViewModel : INotifyPropertyChanged
  {
    public bool LoadingSubredditsFromWeb;
    public bool LoadingAccountFromWeb;
    public bool UpdateDecisionMade;
    private DateTime LastLoadTime = new DateTime(1989, 4, 19, 0, 0, 0);

    public static ObservableCollection<ItemViewModel> Reddits { get; private set; }

    public static ObservableCollection<MainLandingTile> RecentTiles { get; private set; }

    public static List<SubReddit> subReddits { get; set; }

    public MainLandingViewModel()
    {
      MainLandingViewModel.Reddits = new ObservableCollection<ItemViewModel>();
      MainLandingViewModel.RecentTiles = new ObservableCollection<MainLandingTile>();
      MainLandingViewModel.subReddits = App.DataManager.SubredditDataManager.GetSubSortedReddits();
      MainLandingViewModel.ShowSubreddits();
      this.LoadAccountInfo();
      this.UpdateRecentTiles();
      App.DataManager.SubredditDataManager.SubRedditsUpdated += new SubredditDataManager.SubredditUpdatedEventHandler(this.SubRedditsUpdated);
      App.DataManager.MessageInboxUpdated += new DataManager.MessageInboxUpdatedEventHandler(this.MessagesUpdated);
      App.DataManager.AccountChanged += new DataManager.AccountChangedEvent(this.AccountUpdated);
    }

    public void LoadData()
    {
      if (DateTime.Now.Subtract(this.LastLoadTime).TotalMinutes <= 10.0)
        return;
      this.LastLoadTime = DateTime.Now;
      ThreadPool.QueueUserWorkItem((WaitCallback) (obj => this.LoadDataThread()));
    }

    public void LoadDataThread()
    {
      if (App.DataManager.BaconitStore.LastUpdatedTime("!Main_Subreddit") < BaconitStore.currentTime() - App.DataManager.SettingsMan.AccountUpdateTime)
      {
        this.LoadingSubredditsFromWeb = true;
        App.DataManager.SubredditDataManager.UpdateSubReddits();
      }
      else
        this.LoadingSubredditsFromWeb = false;
      if (App.DataManager.BaconitStore.LastUpdatedTime("!Main_Account") < BaconitStore.currentTime() - App.DataManager.SettingsMan.AccountUpdateTime && App.DataManager.SettingsMan.IsSignedIn)
      {
        this.LoadingAccountFromWeb = true;
        App.DataManager.UpdateAccountInformation((RunWorkerCompletedEventHandler) ((obj, arg) => this.LoadingAccountFromWebDone((bool) obj)));
      }
      else
        this.LoadingAccountFromWeb = false;
      this.UpdateDecisionMade = true;
      Deployment.Current.Dispatcher.BeginInvoke(new Action(this.UpdateLoadingBars));
    }

    public void AccountUpdated(bool added, bool logout)
    {
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
      {
        this.LoadAccountInfo();
        this.UpdateAllData();
      }));
    }

    public static void ShowSubreddits()
    {
      List<ItemViewModel> redditList = new List<ItemViewModel>();
      redditList.Add(new ItemViewModel()
      {
        Normal = Visibility.Collapsed,
        Header = Visibility.Visible,
        favIcon = "/Images/FavoriteIcon.png",
        LineOne = ""
      });
      string str = "/Images/FavoriteIcon.png";
      if (DataManager.LIGHT_THEME)
        str = "/Images/FavoriteIconLight.png";
      for (int index = 0; index < MainLandingViewModel.subReddits.Count; ++index)
      {
        if (App.DataManager.SubredditDataManager.checkIfFavorite(MainLandingViewModel.subReddits[index].RName))
          redditList.Add(new ItemViewModel()
          {
            LineOne = MainLandingViewModel.subReddits[index].DisplayName.ToLower(),
            LineTwo = MainLandingViewModel.subReddits[index].Title,
            favIcon = str,
            RName = MainLandingViewModel.subReddits[index].RName,
            URL = MainLandingViewModel.subReddits[index].URL,
            Normal = Visibility.Visible,
            Header = Visibility.Collapsed,
            LineTwoColor = new SolidColorBrush(DataManager.ACCENT_COLOR_COLOR)
          });
      }
      for (int index = 0; index < MainLandingViewModel.subReddits.Count; ++index)
      {
        if (!App.DataManager.SubredditDataManager.checkIfFavorite(MainLandingViewModel.subReddits[index].RName))
          redditList.Add(new ItemViewModel()
          {
            LineOne = MainLandingViewModel.subReddits[index].DisplayName.ToLower(),
            LineTwo = MainLandingViewModel.subReddits[index].Title,
            favIcon = "/Images/NotFavoriteIcon.png",
            RName = MainLandingViewModel.subReddits[index].RName,
            URL = MainLandingViewModel.subReddits[index].URL,
            Normal = Visibility.Visible,
            Header = Visibility.Collapsed
          });
      }
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
      {
        MainLandingViewModel.Reddits.Clear();
        foreach (ItemViewModel itemViewModel in redditList)
          MainLandingViewModel.Reddits.Add(itemViewModel);
        MainLandingViewModel.Reddits[MainLandingViewModel.Reddits.Count - 1].Padding = new Thickness(0.0, 0.0, 0.0, 40.0);
      }));
    }

    public void SubRedditsUpdated(bool success, List<SubReddit> NewList, bool UpdateUI)
    {
      this.LoadingSubredditsFromWeb = false;
      if (success && UpdateUI)
      {
        MainLandingViewModel.subReddits = NewList;
        Deployment.Current.Dispatcher.BeginInvoke((Action) (() => MainLandingViewModel.ShowSubreddits()));
      }
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() => this.UpdateLoadingBars()));
    }

    public static void UpdateFavorite(ItemViewModel item, bool favorite)
    {
      MainLandingViewModel.Reddits.Remove(item);
      bool flag1 = item.favIcon.Equals("/Images/FavoriteIcon.png") || item.favIcon.Equals("/Images/FavoriteIconLight.png");
      for (int index = 1; index < MainLandingViewModel.Reddits.Count; ++index)
      {
        bool flag2 = MainLandingViewModel.Reddits[index].favIcon.Equals("/Images/FavoriteIcon.png") || MainLandingViewModel.Reddits[index].favIcon.Equals("/Images/FavoriteIconLight.png");
        if (MainLandingViewModel.Reddits[index].LineOne.CompareTo(item.LineOne) > 0 && flag1 == flag2)
        {
          MainLandingViewModel.Reddits.Insert(index, item);
          return;
        }
        if (flag1 && !flag2)
        {
          MainLandingViewModel.Reddits.Insert(index, item);
          return;
        }
      }
      MainLandingViewModel.Reddits.Insert(MainLandingViewModel.Reddits.Count, item);
    }

    public void MessagesUpdated(bool success, List<Message> NewList, bool UpdateUI)
    {
      if (!(success & UpdateUI))
        return;
      this.LoadingAccountFromWebDone(true);
    }

    public void LoadingAccountFromWebDone(bool success)
    {
      this.LoadingAccountFromWeb = false;
      if (success)
        Deployment.Current.Dispatcher.BeginInvoke((Action) (() => this.LoadAccountInfo()));
      Deployment.Current.Dispatcher.BeginInvoke(new Action(this.UpdateLoadingBars));
    }

    public void LoadAccountInfo()
    {
      if (MainLanding.me == null)
        return;
      if (App.DataManager.SettingsMan.IsSignedIn)
      {
        MainLanding.me.LoggedInHolderUI.Visibility = Visibility.Visible;
        MainLanding.me.LoggedOutHolderUI.Visibility = Visibility.Collapsed;
        string userName = App.DataManager.SettingsMan.UserName;
        if (!string.IsNullOrWhiteSpace(App.DataManager.SettingsMan.UserName))
          MainLanding.me.AccountSubText.Text = "active account: " + userName;
        if (App.DataManager.SettingsMan.HasMail)
        {
          if (App.DataManager.SettingsMan.UnreadInboxMessageCount <= 0)
          {
            MainLanding.me.MessagesTitleUI.Text = "messages";
            MainLanding.me.MessagesSubTextUI.Text = "new messages!";
            MainLanding.me.TopStoryMessages.Text = "New Messages";
            MainLanding.me.MessagesSubTextUI.Foreground = (Brush) new SolidColorBrush(DataManager.ACCENT_COLOR_COLOR);
            MainLanding.me.TopStoryMessages.Foreground = (Brush) new SolidColorBrush(DataManager.ACCENT_COLOR_COLOR);
          }
          else
          {
            if (App.DataManager.SettingsMan.UnreadInboxMessageCount == 1)
            {
              MainLanding.me.MessagesTitleUI.Text = "1 message";
              MainLanding.me.MessagesSubTextUI.Text = "new message!";
              MainLanding.me.TopStoryMessages.Text = "New Message";
            }
            else
            {
              MainLanding.me.MessagesTitleUI.Text = App.DataManager.SettingsMan.UnreadInboxMessageCount.ToString() + " messages";
              MainLanding.me.MessagesSubTextUI.Text = "new messages!";
              MainLanding.me.TopStoryMessages.Text = App.DataManager.SettingsMan.UnreadInboxMessageCount.ToString() + " New Messages";
            }
            MainLanding.me.MessagesSubTextUI.Foreground = (Brush) new SolidColorBrush(DataManager.ACCENT_COLOR_COLOR);
            MainLanding.me.TopStoryMessages.Foreground = (Brush) new SolidColorBrush(DataManager.ACCENT_COLOR_COLOR);
          }
        }
        else
        {
          MainLanding.me.MessagesTitleUI.Text = "messages";
          MainLanding.me.MessagesSubTextUI.Text = "no new messages";
          MainLanding.me.TopStoryMessages.Text = "No New Messages";
          if (DataManager.LIGHT_THEME)
          {
            MainLanding.me.MessagesSubTextUI.Foreground = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 137, (byte) 137, (byte) 137));
            MainLanding.me.TopStoryMessages.Foreground = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 137, (byte) 137, (byte) 137));
          }
          else
          {
            MainLanding.me.MessagesSubTextUI.Foreground = (Brush) new SolidColorBrush(Color.FromArgb((byte) 143, byte.MaxValue, byte.MaxValue, byte.MaxValue));
            MainLanding.me.TopStoryMessages.Foreground = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 143, (byte) 143, (byte) 143));
          }
        }
      }
      else
      {
        MainLanding.me.LoggedOutHolderUI.Visibility = Visibility.Visible;
        MainLanding.me.LoggedInHolderUI.Visibility = Visibility.Collapsed;
        MainLanding.me.TopStoryMessages.Foreground = !DataManager.LIGHT_THEME ? (Brush) new SolidColorBrush(Color.FromArgb((byte) 143, byte.MaxValue, byte.MaxValue, byte.MaxValue)) : (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 137, (byte) 137, (byte) 137));
        switch (new Random().Next(0, 5))
        {
          case 0:
            MainLanding.me.TopStoryMessages.Text = "Welcome back.";
            break;
          case 1:
            MainLanding.me.TopStoryMessages.Text = "Nice to see you again.";
            break;
          case 2:
            MainLanding.me.TopStoryMessages.Text = "Today is a good day.";
            break;
          case 3:
            MainLanding.me.TopStoryMessages.Text = "How have you been?";
            break;
          case 4:
            MainLanding.me.TopStoryMessages.Text = "To the moon.";
            break;
          case 5:
            MainLanding.me.TopStoryMessages.Text = "You're looking nice.";
            break;
        }
      }
    }

    public void UpdateRecentTiles()
    {
      List<MainLandingTile> mainLandingTiles = App.DataManager.SettingsMan.MainLandingTiles;
      if (mainLandingTiles != null && mainLandingTiles.Count > 0 && MainLandingViewModel.RecentTiles.Count > 0 && MainLandingViewModel.RecentTiles[0].KeyElement.Equals(mainLandingTiles[0].KeyElement))
        return;
      MainLandingViewModel.RecentTiles.Clear();
      foreach (MainLandingTile mainLandingTile in mainLandingTiles)
        MainLandingViewModel.RecentTiles.Add(mainLandingTile);
      this.SetRecentTilesText();
    }

    public void SetRecentTilesText()
    {
      if (MainLanding.me == null)
        return;
      if (MainLandingViewModel.RecentTiles.Count == 0)
      {
        MainLanding.me.RecentNothingText.Visibility = Visibility.Visible;
        MainLanding.me.RecentTiles.Visibility = Visibility.Collapsed;
        if (App.DataManager.SettingsMan.NumberOfRecentTiles == 0)
          MainLanding.me.RecentNothingText.Text = "Recent tiles are disabled";
        else
          MainLanding.me.RecentNothingText.Text = "Nothing here yet...";
      }
      else
      {
        MainLanding.me.RecentNothingText.Visibility = Visibility.Collapsed;
        MainLanding.me.RecentTiles.Visibility = Visibility.Visible;
      }
    }

    public void UpdateLoadingBars()
    {
      if (!this.UpdateDecisionMade)
        return;
      if (this.LoadingAccountFromWeb || this.LoadingSubredditsFromWeb)
      {
        if (MainLanding.me.LoadingBar.Visibility != Visibility.Collapsed)
          return;
        MainLanding.me.LoadingBar.Visibility = Visibility.Visible;
        MainLanding.me.LoadingBar.IsIndeterminate = true;
        MainLanding.me.ShowProgressIndicator.Begin();
      }
      else
        MainLanding.me.HideProgressIndicator.Begin();
    }

    public void UpdateAllData()
    {
      this.LoadingAccountFromWeb = true;
      this.LoadingSubredditsFromWeb = true;
      this.UpdateLoadingBars();
      ThreadPool.QueueUserWorkItem((WaitCallback) (obj => this.UpdateAllDataThread()));
    }

    private void UpdateAllDataThread()
    {
      App.DataManager.BaconitStore.UpdateLastUpdatedTime("!Main_Subreddit", false);
      App.DataManager.SubredditDataManager.UpdateSubReddits();
      App.DataManager.BaconitStore.UpdateLastUpdatedTime("!Main_Account", false);
      App.DataManager.UpdateAccountInformation((RunWorkerCompletedEventHandler) ((obj, arg) => this.LoadingAccountFromWebDone((bool) obj)));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
