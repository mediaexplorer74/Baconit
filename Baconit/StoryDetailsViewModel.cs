// Decompiled with JetBrains decompiler
// Type: Baconit.StoryDetailsViewModel
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Baconit.Database;
using BaconitData.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Baconit
{
  public class StoryDetailsViewModel : CommentDetailsViewModelInterface
  {
    public bool LoadingFromWeb;
    public bool HasCommentsFromWeb;
    public StoryDetails Host;
    public int IndentRollerOver = -1;
    public int IndentRollerOverTwo = -1;

    public ObservableCollection<CommentData> Comments { get; set; }

    public bool IsDataLoaded { get; set; }

    public StoryDetailsViewModel(StoryDetails h)
    {
      this.Host = h;
      this.Comments = new ObservableCollection<CommentData>();
    }

    public void LoadData()
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) (obj => this.FirstSetComments()));
      if (App.DataManager.BaconitStore.LastUpdatedTime(this.Host.LocalStory.RedditID + (object) this.Host.SortShowing + "comments") < BaconitStore.currentTime() - App.DataManager.SettingsMan.CommentUpdateTime || this.Host.SortShowing == 6)
      {
        this.LoadingFromWeb = true;
        Deployment.Current.Dispatcher.BeginInvoke((Action) (() => this.SetLoadingBar(true)));
        App.DataManager.UpdateStoryComments((CommentDetailsViewModelInterface) this, this.Host.LocalStory.RedditRealID, this.Host.LocalStory.SubRedditURL, this.Host.IsPinned, this.Host.SortShowing, this.Host.ShowComment);
      }
      else
        this.LoadingFromWeb = false;
      this.IsDataLoaded = true;
    }

    public void FirstSetComments()
    {
      if (this.HasCommentsFromWeb)
        return;
      List<CommentData> storyComments = App.DataManager.GetStoryComments(this.Host.LocalStory.RedditRealID, this.Host.LocalStory.SubRedditURL, this.Host.SortShowing);
      int pos = 0;
      foreach (CommentData data in storyComments)
      {
        if (this.HasCommentsFromWeb)
          return;
        this.CommentFeeder_Feed(data, pos, this.Host.SortShowing);
        ++pos;
      }
      if (this.HasCommentsFromWeb)
        return;
      this.CommentFeeder_Done(pos, this.Host.SortShowing);
    }

    public void CommentFeeder_Start(bool fromWeb, int sort)
    {
      if (!fromWeb || this.Host.SortShowing != sort)
        return;
      this.HasCommentsFromWeb = true;
    }

    public void CommentFeeder_Feed(CommentData data, int pos, int sort)
    {
      if (this.Host.SortShowing != sort)
        return;
      StoryDetailsViewModel.PrepareComment(data);
      using (AutoResetEvent are = new AutoResetEvent(false))
      {
        Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
        {
          int num = this.Comments.Count > pos ? 0 : (this.Comments.Count < 30 ? 1 : 0);
          if (num != 0)
            data.isVisible = Visibility.Collapsed;
          this.SetComment(data, pos);
          if (pos > 0 && this.Comments.Count > pos)
            this.Comments[pos - 1].isVisible = Visibility.Visible;
          if (num != 0)
          {
            ListBoxItem listBoxItem = (ListBoxItem) this.Host.CommentList.ItemContainerGenerator.ContainerFromIndex(pos - 1);
            if (listBoxItem != null)
            {
              VisualStateManager.GoToState((Control) listBoxItem, "BeforeLoaded", false);
              VisualStateManager.GoToState((Control) this.Host.CommentList.ItemContainerGenerator.ContainerFromIndex(pos - 1), "AfterLoaded", true);
            }
          }
          are.Set();
        }));
        are.WaitOne();
      }
    }

    public void CommentFeeder_Done(int pos, int sort)
    {
      if (this.Host.SortShowing != sort)
        return;
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
      {
        if (this.Comments.Count >= pos && pos > 0)
          this.Comments[pos - 1].isVisible = Visibility.Visible;
        while (this.Comments.Count > pos)
          this.Comments.RemoveAt(this.Comments.Count - 1);
      }));
    }

    public void LoadingFromWebDone(bool hasComments)
    {
      this.LoadingFromWeb = false;
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
      {
        if (hasComments)
        {
          this.Host.NoComments.Visibility = Visibility.Collapsed;
          this.Host.CommentList.Visibility = Visibility.Visible;
        }
        else
        {
          this.Host.NoComments.Visibility = Visibility.Visible;
          this.Host.CommentList.Visibility = Visibility.Collapsed;
        }
        this.SetLoadingBar(false);
      }));
    }

    public static void PrepareComment(CommentData com)
    {
      CommentData commentData1 = com;
      commentData1.LineOne = commentData1.Author;
      com.LineOnePartTwo = " " + (object) (com.Up - com.Downs) + " points ";
      DateTime dateTime1 = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(com.Created);
      DateTime dateTime2 = DateTime.Now;
      dateTime2 = dateTime2.ToUniversalTime();
      TimeSpan timeSpan = dateTime2.Subtract(dateTime1);
      if (Math.Abs(timeSpan.Days) > 0)
      {
        if (Math.Abs(timeSpan.Days) == 1)
        {
          com.LineOnePartTwo += "1 day ago";
        }
        else
        {
          CommentData commentData2 = com;
          commentData2.LineOnePartTwo = commentData2.LineOnePartTwo + (object) Math.Abs(timeSpan.Days) + " days ago";
        }
      }
      else if (Math.Abs(timeSpan.Hours) > 0)
      {
        if (Math.Abs(timeSpan.Hours) == 1)
        {
          com.LineOnePartTwo += "1 hr ago";
        }
        else
        {
          CommentData commentData3 = com;
          commentData3.LineOnePartTwo = commentData3.LineOnePartTwo + (object) Math.Abs(timeSpan.Hours) + " hrs ago";
        }
      }
      else if (Math.Abs(timeSpan.Minutes) > 0)
      {
        if (Math.Abs(timeSpan.Minutes) == 1)
        {
          com.LineOnePartTwo += "1 min ago";
        }
        else
        {
          CommentData commentData4 = com;
          commentData4.LineOnePartTwo = commentData4.LineOnePartTwo + (object) Math.Abs(timeSpan.Minutes) + " mins ago";
        }
      }
      else if (Math.Abs(timeSpan.Seconds) == 1)
      {
        com.LineOnePartTwo += "1 sec ago";
      }
      else
      {
        CommentData commentData5 = com;
        commentData5.LineOnePartTwo = commentData5.LineOnePartTwo + (object) Math.Abs(timeSpan.Seconds) + " secs ago";
      }
      com.Content = DataManager.SimpleFormatString(com.Body + com.Body2 + com.Body3);
      CommentData commentData6 = com;
      commentData6.Padding = new Thickness((double) (commentData6.IndentLevel * 10), 0.0, 0.0, 0.0);
      com.PanelWidth = 455 - 15 * com.IndentLevel;
      com.DownVoteStatus = true;
      com.UpVoteStatus = true;
      switch (com.Likes)
      {
        case -1:
          com.DownVoteStatus = false;
          com.DownVoteIcon = Visibility.Visible;
          break;
        case 1:
          com.UpVoteStatus = false;
          com.UpVoteIcon = Visibility.Visible;
          break;
      }
      if (App.ActiveStoryData == null || App.ActiveStoryData.Author == null || !com.Author.Equals(App.ActiveStoryData.Author))
        return;
      if (DataManager.LIGHT_THEME)
        com.AuthorBackColor = "#30000000";
      else
        com.AuthorBackColor = "#FF404040";
    }

    public void SetComment(CommentData data, int pos)
    {
      Color color = new Color();
      if (data.IndentLevel == 0)
      {
        color.A = byte.MaxValue;
        color.B = (byte) Math.Min((int) DataManager.ACCENT_COLOR_COLOR.B + 35, (int) byte.MaxValue);
        color.G = (byte) Math.Min((int) DataManager.ACCENT_COLOR_COLOR.G + 35, (int) byte.MaxValue);
        color.R = (byte) Math.Min((int) DataManager.ACCENT_COLOR_COLOR.R + 35, (int) byte.MaxValue);
      }
      else
      {
        color.A = byte.MaxValue;
        color.B = (byte) Math.Max((int) DataManager.ACCENT_COLOR_COLOR.B - (data.IndentLevel + 1) * 25, 0);
        color.G = (byte) Math.Max((int) DataManager.ACCENT_COLOR_COLOR.G - (data.IndentLevel + 1) * 25, 0);
        color.R = (byte) Math.Max((int) DataManager.ACCENT_COLOR_COLOR.R - (data.IndentLevel + 1) * 25, 0);
        if (color.B < (byte) 15 && color.G < (byte) 15 && color.R < (byte) 15)
        {
          if (this.IndentRollerOver == -1)
            this.IndentRollerOver = data.IndentLevel;
          int num1 = data.IndentLevel - this.IndentRollerOver;
          color.B = (byte) Math.Max((int) DataManager.ACCENT_COLOR_COLOR.B - (num1 + 1) * 25, 0);
          color.G = (byte) Math.Max((int) DataManager.ACCENT_COLOR_COLOR.G - (num1 + 1) * 25, 0);
          color.R = (byte) Math.Max((int) DataManager.ACCENT_COLOR_COLOR.R - (num1 + 1) * 25, 0);
          if (color.B < (byte) 15 && color.G < (byte) 15 && color.R < (byte) 15)
          {
            if (this.IndentRollerOverTwo == -1)
              this.IndentRollerOverTwo = data.IndentLevel;
            int num2 = data.IndentLevel - this.IndentRollerOverTwo;
            color.B = (byte) Math.Max((int) DataManager.ACCENT_COLOR_COLOR.B - (num2 + 1) * 25, 0);
            color.G = (byte) Math.Max((int) DataManager.ACCENT_COLOR_COLOR.G - (num2 + 1) * 25, 0);
            color.R = (byte) Math.Max((int) DataManager.ACCENT_COLOR_COLOR.R - (num2 + 1) * 25, 0);
          }
        }
      }
      data.Color = new SolidColorBrush(color);
      if (data.RName.Equals(this.Host.ShowComment))
      {
        data.BackGroundColor = !DataManager.LIGHT_THEME ? "#DD404040" : "#DDCCCCCC";
        data.AuthorBackColor = "#00000000";
      }
      if (pos == -1)
        this.Comments.Add(data);
      else if (this.Comments.Count > pos)
      {
        this.Comments.RemoveAt(pos);
        this.Comments.Insert(pos, data);
      }
      else
        this.Comments.Add(data);
    }

    public void ClearData()
    {
      this.Comments.Clear();
      this.IsDataLoaded = false;
      this.LoadingFromWeb = false;
    }

    public void SetStoryData(SubRedditData story, bool update)
    {
      this.Host.SetStoryData(story, update);
    }

    public void LoadingStoryFailed()
    {
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() => this.SetLoadingBar(false)));
    }

    public void SetLoadingBar(bool status)
    {
      if (status)
      {
        this.Host.LoadingCommentsBar.IsIndeterminate = true;
        this.Host.LoadingCommentsBar.Visibility = Visibility.Visible;
      }
      else
      {
        this.Host.LoadingCommentsBar.IsIndeterminate = false;
        this.Host.LoadingCommentsBar.Visibility = Visibility.Collapsed;
      }
    }

    public bool GetisOpen() => this.Host.isOpen;

    public bool GetisPinned() => this.Host.IsPinned;
  }
}
