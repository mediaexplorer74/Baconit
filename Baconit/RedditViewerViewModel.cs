// Decompiled with JetBrains decompiler
// Type: Baconit.RedditViewerViewModel
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Baconit.Database;
using BaconitData.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace Baconit
{
  public class RedditViewerViewModel : INotifyPropertyChanged, RedditViewerViewModelInterface
  {
    public RedditsViewer Host;
    public static string ARROW_NO_SELECT = "#99FFFFFF";
    private List<SubRedditData> stories = new List<SubRedditData>();
    public bool[] typeLoaded;
    public bool[] LoadingFromWeb;
    public Dictionary<string, bool>[] RedditIDList = new Dictionary<string, bool>[4];
    public bool[] WebHasStarted = new bool[4];

    public RedditViewerViewModel(RedditsViewer host)
    {
      this.Host = host;
      this.HotStories = new ObservableCollection<SubRedditData>();
      this.TopStories = new ObservableCollection<SubRedditData>();
      this.ConvStories = new ObservableCollection<SubRedditData>();
      this.NewStories = new ObservableCollection<SubRedditData>();
      this.typeLoaded = new bool[4];
      this.LoadingFromWeb = new bool[4];
      RedditViewerViewModel.ARROW_NO_SELECT = ((Color) Application.Current.Resources[(object) "PhoneSubtleColor"]).ToString();
      this.RedditIDList[0] = new Dictionary<string, bool>();
      this.RedditIDList[1] = new Dictionary<string, bool>();
      this.RedditIDList[2] = new Dictionary<string, bool>();
      this.RedditIDList[3] = new Dictionary<string, bool>();
    }

    public ObservableCollection<SubRedditData> HotStories { get; private set; }

    public ObservableCollection<SubRedditData> TopStories { get; private set; }

    public ObservableCollection<SubRedditData> ConvStories { get; private set; }

    public ObservableCollection<SubRedditData> NewStories { get; private set; }

    public bool IsDataLoaded { get; set; }

    public void LoadData() => this.IsDataLoaded = true;

    public void SubRedditDataFeeder_Start(int whichType, int VirturalType, bool isFromWeb)
    {
      if (VirturalType > 3 && this.Host.TypeOfNewShowing != VirturalType && this.Host.TypeOfTopShowing != VirturalType)
        return;
      if (isFromWeb)
        this.WebHasStarted[whichType] = true;
      if (this.WebHasStarted[whichType] && !isFromWeb)
        return;
      this.Host.SubRedditImageStory = (SubRedditData) null;
      this.RedditIDList[whichType].Clear();
    }

    public void SubRedditDataFeeder_Feed(
      SubRedditData data,
      DataManager.CountHolder countHolder,
      int whichType,
      int VirturalType,
      bool fromWeb)
    {
      if (this.WebHasStarted[whichType] && !fromWeb || VirturalType > 3 && this.Host.TypeOfNewShowing != VirturalType && this.Host.TypeOfTopShowing != VirturalType)
        return;
      if (countHolder.StoryCount < 3)
        this.stories.Add(data);
      if (this.RedditIDList[whichType].ContainsKey(data.RedditID))
        return;
      this.RedditIDList[whichType].Add(data.RedditID, true);
      int pos = countHolder.StoryCount;
      ++countHolder.StoryCount;
      this.FormatAndAddStory(data, whichType, fromWeb);
      ObservableCollection<SubRedditData> toAddTo = (ObservableCollection<SubRedditData>) null;
      switch (whichType)
      {
        case 0:
          toAddTo = this.HotStories;
          break;
        case 1:
          toAddTo = this.NewStories;
          break;
        case 2:
          toAddTo = this.TopStories;
          break;
        case 3:
          toAddTo = this.ConvStories;
          break;
      }
      if (pos == 2)
        this.Host.SetFlipModeButton(whichType, true);
      using (AutoResetEvent are = new AutoResetEvent(false))
      {
        Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
        {
          switch (whichType)
          {
            case 0:
              this.Host.HotNoStories.Visibility = Visibility.Collapsed;
              break;
            case 1:
              this.Host.NewNoStories.Visibility = Visibility.Collapsed;
              break;
            case 2:
              this.Host.TopNoStories.Visibility = Visibility.Collapsed;
              break;
            case 3:
              this.Host.ConvNoStories.Visibility = Visibility.Collapsed;
              break;
          }
          int num = toAddTo.Count > pos ? 0 : (toAddTo.Count < 30 ? 1 : 0);
          if (num != 0)
            data.StoryVisible = Visibility.Collapsed;
          RedditViewerViewModel.setStory(data, pos, whichType, (Collection<SubRedditData>) toAddTo);
          if (pos > 0 && toAddTo.Count > pos)
          {
            if (toAddTo[pos - 1].isHidden)
              toAddTo[pos - 1].StoryVisible = Visibility.Collapsed;
            else
              toAddTo[pos - 1].StoryVisible = Visibility.Visible;
          }
          if (num != 0)
          {
            ListBoxItem listBoxItem = (ListBoxItem) this.Host.ListBoxes[whichType].ItemContainerGenerator.ContainerFromIndex(pos - 1);
            if (listBoxItem != null)
            {
              VisualStateManager.GoToState((Control) listBoxItem, "BeforeLoaded", false);
              VisualStateManager.GoToState((Control) this.Host.ListBoxes[whichType].ItemContainerGenerator.ContainerFromIndex(pos - 1), "AfterLoaded", true);
            }
          }
          are.Set();
        }));
        are.WaitOne();
      }
      if (!data.thumbnail.Equals("") && data.thumbnail.StartsWith("http"))
        ThreadPool.QueueUserWorkItem(new WaitCallback(new RedditImage(App.DataManager, false, new RunWorkerCompletedEventHandler(this.ImageRequest_RunWorkerCompleted))
        {
          StoryID = data.RedditID,
          URL = data.thumbnail,
          SubRedditRName = this.Host.LocalSubreddit.RName,
          SubRedditType = VirturalType
        }.run));
      if (DataManager.GetImageURL(data, false) == null)
        return;
      data.ImagePostNumber = countHolder.ImageCount;
      ++countHolder.ImageCount;
      this.Host.FlipModeNumImages[whichType] = countHolder.ImageCount;
    }

    public void SubRedditDataFeeder_Done(
      int whichType,
      int VirturalType,
      DataManager.CountHolder countHolder,
      bool FromWeb)
    {
      if (this.WebHasStarted[whichType] && !FromWeb || VirturalType > 3 && this.Host.TypeOfNewShowing != VirturalType && this.Host.TypeOfTopShowing != VirturalType)
        return;
      int finalPos = countHolder.StoryCount;
      this.typeLoaded[whichType] = true;
      this.Host.FlipModeNumImages[whichType] = countHolder.ImageCount;
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
      {
        ObservableCollection<SubRedditData> observableCollection1 = (ObservableCollection<SubRedditData>) null;
        switch (whichType)
        {
          case 0:
            observableCollection1 = this.HotStories;
            break;
          case 1:
            observableCollection1 = this.NewStories;
            break;
          case 2:
            observableCollection1 = this.TopStories;
            break;
          case 3:
            observableCollection1 = this.ConvStories;
            break;
        }
        if (finalPos > 0 && observableCollection1.Count >= finalPos)
          observableCollection1[finalPos - 1].StoryVisible = Visibility.Visible;
        while (observableCollection1.Count > finalPos)
        {
          ObservableCollection<SubRedditData> observableCollection2 = observableCollection1;
          observableCollection2.RemoveAt(observableCollection2.Count - 1);
        }
        bool flag = false;
        if (this.LoadingFromWeb[whichType])
        {
          if (FromWeb)
            flag = true;
        }
        else
          flag = true;
        if (!flag || observableCollection1.Count != 0)
          return;
        switch (whichType)
        {
          case 0:
            this.Host.HotNoStories.Visibility = Visibility.Visible;
            break;
          case 1:
            this.Host.NewNoStories.Visibility = Visibility.Visible;
            break;
          case 2:
            this.Host.TopNoStories.Visibility = Visibility.Visible;
            break;
          case 3:
            this.Host.ConvNoStories.Visibility = Visibility.Visible;
            break;
        }
      }));
    }

    public int updateStories(int whichType, int VirturalType)
    {
      List<SubRedditData> subRedditStories = App.DataManager.GetSubRedditStories(this.Host.LocalSubreddit.URL, VirturalType);
      this.SubRedditDataFeeder_Start(whichType, VirturalType, false);
      DataManager.CountHolder countHolder = this.setStories(whichType, VirturalType, subRedditStories);
      this.SubRedditDataFeeder_Done(whichType, VirturalType, countHolder, false);
      this.typeLoaded[whichType] = true;
      return subRedditStories.Count;
    }

    public DataManager.CountHolder setStories(
      int whichType,
      int VirturalType,
      List<SubRedditData> stories)
    {
      switch (whichType)
      {
        case 0:
          ObservableCollection<SubRedditData> hotStories = this.HotStories;
          break;
        case 1:
          ObservableCollection<SubRedditData> newStories = this.NewStories;
          break;
        case 2:
          ObservableCollection<SubRedditData> topStories = this.TopStories;
          break;
        case 3:
          ObservableCollection<SubRedditData> convStories = this.ConvStories;
          break;
      }
      DataManager.CountHolder countHolder = new DataManager.CountHolder();
      foreach (SubRedditData storey in stories)
        this.SubRedditDataFeeder_Feed(storey, countHolder, whichType, VirturalType, false);
      return countHolder;
    }

    public void SetLoadingBar(int which, bool enabled)
    {
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
      {
        if (enabled)
        {
          this.Host.LoadingBars[which].Visibility = Visibility.Visible;
          this.Host.LoadingBars[which].IsIndeterminate = true;
        }
        else
        {
          this.Host.LoadingBars[which].Visibility = Visibility.Collapsed;
          this.Host.LoadingBars[which].IsIndeterminate = false;
        }
      }));
    }

    public static void setStory(
      SubRedditData data,
      int pos,
      int whichType,
      Collection<SubRedditData> toAddTo)
    {
      if (pos == -1)
        toAddTo.Add(data);
      else if (toAddTo.Count > pos)
      {
        toAddTo.RemoveAt(pos);
        toAddTo.Insert(pos, data);
      }
      else
        toAddTo.Add(data);
    }

    public static void setStory(
      SubRedditData data,
      int pos,
      int whichType,
      List<SubRedditData> toAddTo)
    {
      if (pos == -1)
        toAddTo.Add(data);
      else if (toAddTo.Count > pos)
      {
        toAddTo.RemoveAt(pos);
        toAddTo.Insert(pos, data);
      }
      else
        toAddTo.Add(data);
    }

    public void FormatAndAddStory(SubRedditData data, int whichType, bool fromWeb)
    {
      data.Title = HttpUtility.HtmlDecode(data.Title.Trim());
      if (data.isNotSafeForWork && !data.Title.ToLower().Contains("nsfw"))
        data.Title = "(NSFW) " + data.Title;
      data.MaxTitleHeight = !App.DataManager.SettingsMan.ShowWholeTitle ? 62 : 999;
      DateTime dateTime1 = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds((double) data.created);
      DateTime dateTime2 = DateTime.Now;
      dateTime2 = dateTime2.ToUniversalTime();
      TimeSpan timeSpan = dateTime2.Subtract(dateTime1);
      data.LineOne = Math.Abs(timeSpan.Days) <= 0 ? (Math.Abs(timeSpan.Hours) <= 0 ? (Math.Abs(timeSpan.Minutes) <= 0 ? (Math.Abs(timeSpan.Seconds) != 1 ? Math.Abs(timeSpan.Seconds).ToString() + " secs" : "1 sec") : (Math.Abs(timeSpan.Minutes) != 1 ? Math.Abs(timeSpan.Minutes).ToString() + " mins" : "1 min")) : (Math.Abs(timeSpan.Hours) != 1 ? Math.Abs(timeSpan.Hours).ToString() + " hrs" : "1 hr")) : (Math.Abs(timeSpan.Days) != 1 ? Math.Abs(timeSpan.Days).ToString() + " days" : "1 day");
      if (this.Host.LocalSubreddit.URL.Equals("/") || this.Host.LocalSubreddit.URL.Equals("/r/all/") || this.Host.LocalSubreddit.URL.Equals("/saved/") || this.Host.LocalSubreddit.URL.Contains("+"))
      {
        SubRedditData subRedditData = data;
        subRedditData.LineOne = subRedditData.LineOne + " ago to " + data.SubReddit;
        data.GoToSubRedditVis = Visibility.Visible;
      }
      else
      {
        SubRedditData subRedditData = data;
        subRedditData.LineOne = subRedditData.LineOne + " ago by " + data.Author;
      }
      if (App.DataManager.SettingsMan.ReadStories.ContainsKey(data.RedditID))
      {
        int readStorey = App.DataManager.SettingsMan.ReadStories[data.RedditID];
        if (data.comments != readStorey)
        {
          data.LineTwo = data.comments.ToString() + " comments; " + data.Domain;
          data.NewCommentsText = data.comments - readStorey <= 0 ? "(" + (object) (data.comments - readStorey) + ") " : "(+" + (object) (data.comments - readStorey) + ") ";
          data.NewCommentsTextColor = SubRedditData.NewCommentsColor;
        }
        else
          data.LineTwo = data.comments.ToString() + " comments; " + data.Domain;
      }
      else
        data.LineTwo = data.comments.ToString() + " comments; " + data.Domain;
      if (data.isSaved)
        data.SaveStoryText = "unsave story";
      if (App.DataManager.SettingsMan.ReadStories.ContainsKey(data.RedditID))
      {
        data.TitleColor = SubRedditData.ReadTitleColor;
        if (!App.DataManager.SettingsMan.ShowReadStories)
          data.StoryReadVis = Visibility.Collapsed;
      }
      RedditViewerViewModel.SetVoteImages(data);
      data.ThumbnailVis = Visibility.Collapsed;
      if (!fromWeb || this.Host.SubRedditImageStory != null || data.isNotSafeForWork || DataManager.GetImageURL(data, true) == null)
        return;
      this.Host.SubRedditImageStory = data;
    }

    public void ImageRequest_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      RedditImage image = (RedditImage) e.Result;
      int num = RedditsViewer.ReverseVirtural(image.SubRedditType);
      ObservableCollection<SubRedditData> observableCollection = (ObservableCollection<SubRedditData>) null;
      switch (num)
      {
        case 0:
          observableCollection = this.HotStories;
          break;
        case 1:
          observableCollection = this.NewStories;
          break;
        case 2:
          observableCollection = this.TopStories;
          break;
        case 3:
          observableCollection = this.ConvStories;
          break;
      }
      if (!image.CallBackStatus)
        return;
      foreach (SubRedditData subRedditData1 in (Collection<SubRedditData>) observableCollection)
      {
        SubRedditData subRedditData = subRedditData1;
        if (subRedditData.RedditID.Equals(image.StoryID))
        {
          Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
          {
            subRedditData.Thumbnail = image.Image;
            subRedditData.ThumbnailVis = Visibility.Visible;
          }));
          break;
        }
      }
    }

    public static void SetVoteImages(SubRedditData subReddit)
    {
      if (subReddit.Like == 0)
      {
        subReddit.UpVoteColor = RedditViewerViewModel.ARROW_NO_SELECT;
        subReddit.DownVoteColor = RedditViewerViewModel.ARROW_NO_SELECT;
      }
      else if (subReddit.Like == 1)
      {
        subReddit.UpVoteColor = DataManager.ACCENT_COLOR;
        subReddit.DownVoteColor = RedditViewerViewModel.ARROW_NO_SELECT;
      }
      else
      {
        subReddit.UpVoteColor = RedditViewerViewModel.ARROW_NO_SELECT;
        subReddit.DownVoteColor = DataManager.ACCENT_COLOR;
      }
    }

    public void ClearStoryList()
    {
      this.HotStories.Clear();
      this.TopStories.Clear();
      this.NewStories.Clear();
      this.ConvStories.Clear();
      for (int index = 0; index < this.typeLoaded.Length; ++index)
        this.typeLoaded[index] = false;
    }

    public void SetLoadingFromWeb(int type, bool setTo) => this.WebHasStarted[type] = setTo;

    public bool GetisOpen() => this.Host.isOpen;

    public void UpdateReadStoryVis()
    {
      for (int index1 = 0; index1 < 4; ++index1)
      {
        ObservableCollection<SubRedditData> observableCollection = (ObservableCollection<SubRedditData>) null;
        switch (index1)
        {
          case 0:
            observableCollection = this.HotStories;
            break;
          case 1:
            observableCollection = this.NewStories;
            break;
          case 2:
            observableCollection = this.TopStories;
            break;
          case 3:
            observableCollection = this.ConvStories;
            break;
        }
        int index2 = 0;
        foreach (SubRedditData story in (Collection<SubRedditData>) observableCollection)
        {
          if (story.TitleColor.Equals(SubRedditData.ReadTitleColor))
          {
            if (App.DataManager.SettingsMan.ShowReadStories)
            {
              if (this.Host.CurrentShownItem == index1)
              {
                ListBoxItem element = (ListBoxItem) this.Host.ListBoxes[index1].ItemContainerGenerator.ContainerFromIndex(index2);
                if (element != null)
                  this.FadeInStory((FrameworkElement) element, story);
                else
                  story.StoryReadVis = Visibility.Visible;
              }
              else
                story.StoryReadVis = Visibility.Visible;
            }
            else if (this.Host.CurrentShownItem == index1)
            {
              ListBoxItem element = (ListBoxItem) this.Host.ListBoxes[index1].ItemContainerGenerator.ContainerFromIndex(index2);
              if (element != null)
                this.FadeOutStory((FrameworkElement) element, story);
              else
                story.StoryReadVis = Visibility.Collapsed;
            }
            else
              story.StoryReadVis = Visibility.Collapsed;
          }
          ++index2;
        }
      }
    }

    public void FadeOutStory(FrameworkElement element, SubRedditData story)
    {
      story.StoryReadVis = Visibility.Visible;
      Duration duration = new Duration(TimeSpan.FromMilliseconds(450.0));
      DoubleAnimation element1 = new DoubleAnimation();
      DoubleAnimation element2 = new DoubleAnimation();
      element1.Duration = duration;
      element2.Duration = duration;
      Storyboard storyboard = new Storyboard();
      storyboard.Duration = duration;
      storyboard.Children.Add((Timeline) element1);
      storyboard.Children.Add((Timeline) element2);
      Storyboard.SetTarget((Timeline) element2, (DependencyObject) element);
      Storyboard.SetTargetProperty((Timeline) element2, new PropertyPath("Opacity", new object[0]));
      Storyboard.SetTarget((Timeline) element1, (DependencyObject) element);
      Storyboard.SetTargetProperty((Timeline) element1, new PropertyPath("MaxHeight", new object[0]));
      element1.To = new double?(0.0);
      element1.From = new double?(400.0);
      element2.To = new double?(0.0);
      element2.From = new double?(1.0);
      element1.Completed += (EventHandler) ((sender, e) =>
      {
        story.StoryReadVis = Visibility.Collapsed;
        element.Opacity = 1.0;
        element.MaxHeight = double.PositiveInfinity;
      });
      storyboard.Begin();
    }

    public void FadeInStory(FrameworkElement element, SubRedditData story)
    {
      element.Opacity = 0.0;
      story.StoryReadVis = Visibility.Visible;
      element.MaxHeight = 0.0;
      Duration duration = new Duration(TimeSpan.FromMilliseconds(450.0));
      DoubleAnimation element1 = new DoubleAnimation();
      DoubleAnimation element2 = new DoubleAnimation();
      element1.Duration = duration;
      element2.Duration = duration;
      Storyboard storyboard = new Storyboard();
      storyboard.Duration = duration;
      storyboard.Children.Add((Timeline) element1);
      storyboard.Children.Add((Timeline) element2);
      Storyboard.SetTarget((Timeline) element2, (DependencyObject) element);
      Storyboard.SetTargetProperty((Timeline) element2, new PropertyPath("Opacity", new object[0]));
      Storyboard.SetTarget((Timeline) element1, (DependencyObject) element);
      Storyboard.SetTargetProperty((Timeline) element1, new PropertyPath("MaxHeight", new object[0]));
      element1.To = new double?(400.0);
      element1.From = new double?(0.0);
      element2.To = new double?(1.0);
      element2.From = new double?(0.0);
      element1.Completed += (EventHandler) ((sender, e) => element.MaxHeight = double.PositiveInfinity);
      storyboard.Begin();
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
