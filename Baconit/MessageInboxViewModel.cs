// Decompiled with JetBrains decompiler
// Type: Baconit.MessageInboxViewModel
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Baconit.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;

#nullable disable
namespace Baconit
{
  public class MessageInboxViewModel : INotifyPropertyChanged
  {
    public static bool LoadingMessagesFromWeb = false;
    public static string notNewColor = "#FF666666";
    private DateTime LastLoadTime = new DateTime(1989, 4, 19, 0, 0, 0);

    public MessageInboxViewModel()
    {
      MessageInboxViewModel.MessageUIList = new ObservableCollection<Message>();
      App.DataManager.MessageInboxUpdated += new DataManager.MessageInboxUpdatedEventHandler(this.MessagesUpdated);
    }

    public static ObservableCollection<Message> MessageUIList { get; private set; }

    public static List<Message> MessageList { get; set; }

    public bool IsDataLoaded { get; set; }

    public void LoadData(bool forceRefresh)
    {
      if (DateTime.Now.Subtract(this.LastLoadTime).TotalMinutes > 1.0 | forceRefresh)
      {
        this.LastLoadTime = DateTime.Now;
        ThreadPool.QueueUserWorkItem((WaitCallback) (obj => this.LoadDataThread()));
        if (DataManager.LIGHT_THEME)
          MessageInboxViewModel.notNewColor = "#FFAAAAAA";
      }
      this.IsDataLoaded = true;
    }

    public void LoadDataThread()
    {
      MessageInboxViewModel.MessageList = App.DataManager.GetInboxMessages();
      MessageInboxViewModel.LoadingMessagesFromWeb = App.DataManager.BaconitStore.LastUpdatedTime("!InboxMessages") < BaconitStore.currentTime() - 60000.0 || MessageInboxViewModel.MessageList.Count == 0 || MessageInbox.me.ForceRefresh;
      MessageInboxViewModel.FormatAndSetMessages(false);
      if (!MessageInboxViewModel.LoadingMessagesFromWeb)
        return;
      App.DataManager.UpdateInboxMessages(false);
      Deployment.Current.Dispatcher.BeginInvoke(new Action(MessageInboxViewModel.setLoadingBar));
    }

    public void MessagesUpdated(bool success, List<Message> NewList, bool UpdateUI)
    {
      if (!(success & UpdateUI))
        return;
      if (NewList != null)
        MessageInboxViewModel.MessageList = NewList;
      MessageInboxViewModel.FormatAndSetMessages(true);
    }

    public static void FormatAndSetMessages(bool fromWeb)
    {
      if (MessageInboxViewModel.MessageList == null)
        return;
      foreach (Message message1 in MessageInboxViewModel.MessageList)
      {
        message1.FirstLineAccent = message1.Subject + " ";
        if (message1.wasComment == 1)
        {
          message1.FirstLineNormal = message1.Author + " via " + message1.Subreddit;
        }
        else
        {
          Message message2 = message1;
          message2.FirstLineNormal = message2.Author;
        }
        message1.Content = DataManager.SimpleFormatString(message1.body + message1.bodyOver);
      }
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() => MessageInboxViewModel.SetMessages(fromWeb)));
    }

    public static void SetMessages(bool fromWeb)
    {
      if (MessageInboxViewModel.MessageList == null || MessageInboxViewModel.MessageUIList == null)
        return;
      if (fromWeb)
        MessageInboxViewModel.LoadingMessagesFromWeb = false;
      MessageInboxViewModel.MessageUIList.Clear();
      foreach (Message message in MessageInboxViewModel.MessageList)
      {
        message.BarColor = message.isNew != 1 ? MessageInboxViewModel.notNewColor : DataManager.ACCENT_COLOR;
        switch (MessageInbox.me.ShowingMessageType)
        {
          case 0:
            MessageInboxViewModel.MessageUIList.Add(message);
            continue;
          case 1:
            if (message.isNew == 1)
            {
              MessageInboxViewModel.MessageUIList.Add(message);
              continue;
            }
            continue;
          case 2:
            if (message.Subject != null && !message.Subject.Equals("post reply") && !message.Subject.Equals("comment reply"))
            {
              MessageInboxViewModel.MessageUIList.Add(message);
              continue;
            }
            continue;
          case 3:
            if (message.Subject != null && message.Subject.Equals("comment reply"))
            {
              MessageInboxViewModel.MessageUIList.Add(message);
              continue;
            }
            continue;
          case 4:
            if (message.Subject != null && message.Subject.Equals("post reply"))
            {
              MessageInboxViewModel.MessageUIList.Add(message);
              continue;
            }
            continue;
          default:
            continue;
        }
      }
      if (!MessageInboxViewModel.LoadingMessagesFromWeb)
      {
        if (MessageInboxViewModel.MessageUIList.Count == 0)
        {
          MessageInbox.me.NoMessagesUI.Visibility = Visibility.Visible;
          MessageInbox.me.MessageList.Visibility = Visibility.Collapsed;
          MessageInbox.me.NoMessagesTextOneUI.Text = "no messages";
        }
        else
        {
          MessageInbox.me.NoMessagesUI.Visibility = Visibility.Collapsed;
          MessageInbox.me.MessageList.Visibility = Visibility.Visible;
        }
      }
      else if (MessageInboxViewModel.MessageUIList.Count != 0)
      {
        MessageInbox.me.NoMessagesUI.Visibility = Visibility.Collapsed;
        MessageInbox.me.MessageList.Visibility = Visibility.Visible;
        MessageInbox.me.ShowMessages.Begin();
      }
      else
      {
        MessageInbox.me.NoMessagesUI.Visibility = Visibility.Visible;
        MessageInbox.me.MessageList.Visibility = Visibility.Collapsed;
      }
      MessageInboxViewModel.setLoadingBar();
    }

    public static void setLoadingBar()
    {
      if (MessageInbox.me == null)
        return;
      if (MessageInboxViewModel.LoadingMessagesFromWeb)
      {
        MessageInbox.me.InboxLoadingProgressBar.Visibility = Visibility.Visible;
        MessageInbox.me.InboxLoadingProgressBar.IsIndeterminate = true;
      }
      else
      {
        MessageInbox.me.InboxLoadingProgressBar.Visibility = Visibility.Collapsed;
        MessageInbox.me.InboxLoadingProgressBar.IsIndeterminate = false;
      }
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
