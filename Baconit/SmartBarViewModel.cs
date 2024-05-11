// Decompiled with JetBrains decompiler
// Type: Baconit.SmartBarViewModel
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Baconit.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Windows;

#nullable disable
namespace Baconit
{
  public class SmartBarViewModel : INotifyPropertyChanged
  {
    public SmartBarViewModel()
    {
      SmartBarViewModel.SearchResults = new ObservableCollection<SmartBarListItem>();
    }

    public static ObservableCollection<SmartBarListItem> SearchResults { get; private set; }

    public bool IsDataLoaded { get; set; }

    public void LoadData()
    {
    }

    public void SetUser(UserAccountInformation user)
    {
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
      {
        SmartBarViewModel.SearchResults.Insert(0, new SmartBarListItem()
        {
          UserName = user.Name.Trim(),
          SubRedditLineOne = user.Name.Trim(),
          SubRedditLineTwo = "link karma " + (object) user.LinkKarma + "; comment karma " + (object) user.CommentKarma,
          ShowSubSection = Visibility.Collapsed,
          ShowSubReddit = Visibility.Visible,
          ShowSearchResult = Visibility.Collapsed
        });
        SmartBarViewModel.SearchResults.Insert(0, new SmartBarListItem()
        {
          SubSectionTitle = "User Result",
          ShowSubSection = Visibility.Visible,
          ShowSubReddit = Visibility.Collapsed,
          ShowSearchResult = Visibility.Collapsed
        });
      }));
    }

    public void SetSubReddit(SubReddit subreddit)
    {
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
      {
        SmartBarViewModel.SearchResults.Insert(0, new SmartBarListItem()
        {
          subreddit = subreddit,
          SubRedditLineOne = subreddit.DisplayName,
          SubRedditLineTwo = subreddit.URL,
          ShowSubSection = Visibility.Collapsed,
          ShowSubReddit = Visibility.Visible,
          ShowSearchResult = Visibility.Collapsed
        });
        SmartBarViewModel.SearchResults.Insert(0, new SmartBarListItem()
        {
          SubSectionTitle = "Subreddit Result",
          ShowSubSection = Visibility.Visible,
          ShowSubReddit = Visibility.Collapsed,
          ShowSearchResult = Visibility.Collapsed
        });
      }));
    }

    public void FormatAndSetResults(List<SubRedditData> subreddits)
    {
      foreach (SubRedditData subreddit in subreddits)
        SmartBarViewModel.FormatAndAddStory(subreddit);
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
      {
        SmartBarViewModel.SearchResults.Add(new SmartBarListItem()
        {
          SubSectionTitle = "Search Results",
          ShowSubSection = Visibility.Visible,
          ShowSubReddit = Visibility.Collapsed,
          ShowSearchResult = Visibility.Collapsed
        });
        foreach (SubRedditData subreddit in subreddits)
          SmartBarViewModel.SearchResults.Add(new SmartBarListItem()
          {
            storyName = subreddit.RedditID,
            SearchResOne = subreddit.Title,
            SearchResTwo = subreddit.LineOne,
            SearchResThree = subreddit.LineTwo,
            MaxTitleHeight = subreddit.MaxTitleHeight,
            ShowSubSection = Visibility.Collapsed,
            ShowSubReddit = Visibility.Collapsed,
            ShowSearchResult = Visibility.Visible
          });
      }));
    }

    public static void FormatAndAddStory(SubRedditData data)
    {
      data.Title = HttpUtility.HtmlDecode(data.Title.Trim());
      if (data.isNotSafeForWork && !data.Title.StartsWith("("))
        data.Title = "(NSFW) " + data.Title;
      data.MaxTitleHeight = !App.DataManager.SettingsMan.ShowWholeTitle ? 62 : 999;
      DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds((double) data.created);
      TimeSpan timeSpan = DateTime.Now.ToUniversalTime().Subtract(dateTime);
      data.LineOne = Math.Abs(timeSpan.Days) <= 0 ? (Math.Abs(timeSpan.Hours) <= 0 ? (Math.Abs(timeSpan.Minutes) <= 0 ? (Math.Abs(timeSpan.Seconds) != 1 ? Math.Abs(timeSpan.Seconds).ToString() + " secs" : "1 sec") : (Math.Abs(timeSpan.Minutes) != 1 ? Math.Abs(timeSpan.Minutes).ToString() + " mins" : "1 min")) : (Math.Abs(timeSpan.Hours) != 1 ? Math.Abs(timeSpan.Hours).ToString() + " hrs" : "1 hr")) : (Math.Abs(timeSpan.Days) != 1 ? Math.Abs(timeSpan.Days).ToString() + " days" : "1 day");
      SubRedditData subRedditData1 = data;
      subRedditData1.LineOne = subRedditData1.LineOne + " ago to " + data.SubReddit;
      SubRedditData subRedditData2 = data;
      subRedditData2.LineTwo = subRedditData2.LineTwo + "(" + (object) data.ups + "," + (object) data.downs + ") points; " + (object) data.comments + " comments";
      data.ThumbnailVis = Visibility.Collapsed;
    }

    public void ClearResults()
    {
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() => SmartBarViewModel.SearchResults.Clear()));
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
