// Decompiled with JetBrains decompiler
// Type: Baconit.MessageInbox
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Baconit.Database;
using BaconitData.Database;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

#nullable disable
namespace Baconit
{
  public class MessageInbox : PhoneApplicationPage
  {
    public const int ALL = 0;
    public const int UNREAD = 1;
    public const int MESSAGES = 2;
    public const int COMMENT_REP = 3;
    public const int POST_REP = 4;
    public static MessageInbox me;
    public bool ForceRefresh;
    public bool isOpen;
    public bool ActionsOpened;
    public bool ReplyOpened;
    public bool ReplayBeingSent;
    public int ShowingMessageType;
    private bool isOpening = true;
    private bool RestoreProgressIndicator;
    private Message MessageOpenedFor;
    private string text = "";
    internal Storyboard OpenReplyAn;
    internal Storyboard CloseReplyAn;
    internal Storyboard ShowSending;
    internal Storyboard HideSending;
    internal Storyboard ShowMessages;
    internal Storyboard OpenMessageActionAnm;
    internal Storyboard CloseMessageActionAnm;
    internal Grid LayoutRoot;
    internal StackPanel TitlePanel;
    internal TextBlock ApplicationTitle;
    internal ListPicker MessageFilter;
    internal Grid ContentPanel;
    internal ListBox MessageList;
    internal StackPanel NoMessagesUI;
    internal TextBlock NoMessagesTextOneUI;
    internal Grid BlackBackground;
    internal Grid MessageActionPanel;
    internal StackPanel MessageActionPanelConentHolder;
    internal TextBlock MessageActionLineOneOne;
    internal TextBlock MessageActionLineOneTwo;
    internal SuperRichTextBox MessageActionMessageContent;
    internal StackPanel MessageActionButtonHolder;
    internal Grid markRead;
    internal TextBlock markReadText;
    internal Grid reply;
    internal Grid EditButtons;
    internal Grid openContext;
    internal TextBlock openContextText;
    internal Grid profileButton;
    internal TextBlock openprofileText;
    internal StackPanel MessageActionReplyBox;
    internal ScrollViewer ReplyScroller;
    internal TextBox ReplyText;
    internal Button ReplySubmit;
    internal Button ReplyClose;
    internal Grid SendingProgressOverlay;
    internal TextBlock LoadingText;
    internal ProgressBar SendingProgressBar;
    internal ProgressBar InboxLoadingProgressBar;
    private bool _contentLoaded;

    public MessageInbox()
    {
      this.InitializeComponent();
      TransitionService.SetNavigationInTransition((UIElement) this, App.FeatherInTransition);
      TransitionService.SetNavigationOutTransition((UIElement) this, App.FeatherOutTransition);
      this.ApplicationBar = (IApplicationBar) new Microsoft.Phone.Shell.ApplicationBar();
      this.ApplicationBar.IsVisible = true;
      ApplicationBarIconButton applicationBarIconButton = new ApplicationBarIconButton(new Uri("/Images/appbar.send.png", UriKind.Relative));
      applicationBarIconButton.Text = "compose";
      applicationBarIconButton.Click += new EventHandler(this.compose_Click);
      this.ApplicationBar.Buttons.Add((object) applicationBarIconButton);
      ApplicationBarMenuItem applicationBarMenuItem = new ApplicationBarMenuItem("refresh");
      applicationBarMenuItem.Click += new EventHandler(this.refresh_Click);
      this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem);
      MessageInbox.me = this;
      this.DataContext = (object) App.MessageInboxViewModel;
      this.Loaded += new RoutedEventHandler(this.MessageInbox_Loaded);
      if (DataManager.LIGHT_THEME)
      {
        this.BlackBackground.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
        this.MessageActionPanelConentHolder.Background = (Brush) new SolidColorBrush(Color.FromArgb((byte) 207, byte.MaxValue, byte.MaxValue, byte.MaxValue));
        this.MessageActionMessageContent.Foreground = (Brush) new SolidColorBrush(Color.FromArgb((byte) 207, (byte) 0, (byte) 0, (byte) 0));
      }
      this.CloseMessageActionAnm.Completed += (EventHandler) ((sender, e) =>
      {
        this.MessageActionPanel.Visibility = Visibility.Collapsed;
        this.BlackBackground.Visibility = Visibility.Collapsed;
        this.ActionsOpened = false;
      });
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      this.isOpen = true;
      App.DataManager.BaconitAnalytics.LogPage("Message Inbox");
      if (App.navService == null)
        App.navService = this.NavigationService;
      if (this.NavigationContext.QueryString.ContainsKey("ForceRefresh"))
        this.ForceRefresh = true;
      App.MessageInboxViewModel.LoadData(this.ForceRefresh);
      App.ActiveStoryData = (SubRedditData) null;
      if (this.RestoreProgressIndicator)
        this.InboxLoadingProgressBar.IsIndeterminate = true;
      this.RestoreProgressIndicator = false;
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      this.isOpen = false;
      if (this.InboxLoadingProgressBar.IsIndeterminate)
        this.RestoreProgressIndicator = true;
      this.InboxLoadingProgressBar.IsIndeterminate = false;
      this.SendingProgressBar.IsIndeterminate = false;
    }

    private void MessageInbox_Loaded(object sender, RoutedEventArgs e)
    {
      this.MessageFilter.SelectedIndex = this.ShowingMessageType;
      this.isOpening = false;
    }

    protected override void OnBackKeyPress(CancelEventArgs e)
    {
      if (this.isOpening)
      {
        e.Cancel = true;
      }
      else
      {
        base.OnBackKeyPress(e);
        if (this.ReplayBeingSent)
          e.Cancel = true;
        else if (this.ReplyOpened)
        {
          this.CloseReply();
          e.Cancel = true;
        }
        else if (this.ActionsOpened)
        {
          this.CloseMessageActions();
          e.Cancel = true;
        }
        else
          App.MessageInboxViewModel.IsDataLoaded = false;
      }
    }

    private void refresh_Click(object sender, EventArgs e)
    {
      App.DataManager.UpdateInboxMessages(false);
      MessageInboxViewModel.LoadingMessagesFromWeb = true;
      Deployment.Current.Dispatcher.BeginInvoke(new Action(MessageInboxViewModel.setLoadingBar));
    }

    private void MessageFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.MessageFilter == null)
        return;
      this.ShowingMessageType = this.MessageFilter.SelectedIndex;
      ThreadPool.QueueUserWorkItem((WaitCallback) (obfj => MessageInboxViewModel.FormatAndSetMessages(false)));
      App.DataManager.BaconitAnalytics.LogEvent("Message Filter");
    }

    private void compose_Click(object sender, EventArgs e)
    {
      this.NavigationService.Navigate(new Uri("/ComposeMessage.xaml", UriKind.Relative));
    }

    private void MessageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.MessageList.SelectedIndex == -1)
        return;
      this.MessageOpenedFor = (Message) this.MessageList.SelectedItem;
      this.OpenCommentActions((Message) this.MessageList.SelectedItem);
      this.MessageList.SelectedIndex = -1;
    }

    public void OpenCommentActions(Message message)
    {
      App.DataManager.BaconitAnalytics.LogEvent("Message Viewed");
      this.MessageActionLineOneOne.Text = message.FirstLineAccent;
      this.MessageActionMessageContent.SetText(message.body + message.bodyOver);
      if (this.MessageOpenedFor.isNew == 1)
      {
        this.MessageOpenedFor.BarColor = MessageInboxViewModel.notNewColor;
        this.MessageOpenedFor.isNew = 0;
        ThreadPool.QueueUserWorkItem((WaitCallback) (obfj => new MessageInbox.MarkRead()
        {
          rname = this.MessageOpenedFor.RName
        }.work()));
      }
      DateTime dateTime1 = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds((double) message.Created);
      DateTime dateTime2 = DateTime.Now;
      dateTime2 = dateTime2.ToUniversalTime();
      TimeSpan timeSpan = dateTime2.Subtract(dateTime1);
      this.MessageActionLineOneTwo.Text = Math.Abs(timeSpan.Days) <= 0 ? (Math.Abs(timeSpan.Hours) <= 0 ? (Math.Abs(timeSpan.Minutes) <= 0 ? (Math.Abs(timeSpan.Seconds) != 1 ? Math.Abs(timeSpan.Seconds).ToString() + " secs ago" : "1 sec ago") : (Math.Abs(timeSpan.Minutes) != 1 ? Math.Abs(timeSpan.Minutes).ToString() + " mins ago" : "1 min ago")) : (Math.Abs(timeSpan.Hours) != 1 ? Math.Abs(timeSpan.Hours).ToString() + " hrs ago" : "1 hr ago")) : (Math.Abs(timeSpan.Days) != 1 ? Math.Abs(timeSpan.Days).ToString() + " days ago" : "1 day ago");
      TextBlock actionLineOneTwo1 = this.MessageActionLineOneTwo;
      actionLineOneTwo1.Text = actionLineOneTwo1.Text + " by " + message.Author;
      if (message.Subreddit != null && !message.Subreddit.Equals(""))
      {
        TextBlock actionLineOneTwo2 = this.MessageActionLineOneTwo;
        actionLineOneTwo2.Text = actionLineOneTwo2.Text + " via " + message.Subreddit;
      }
      this.markReadText.Text = message.isNew != 1 ? "mark as unread" : "mark as read";
      this.ReplyText.Text = "";
      SolidColorBrush solidColorBrush1 = new SolidColorBrush(Color.FromArgb((byte) 150, (byte) 0, (byte) 0, (byte) 0));
      SolidColorBrush solidColorBrush2 = new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 100, (byte) 100, (byte) 100));
      SolidColorBrush solidColorBrush3 = new SolidColorBrush((Color) Application.Current.Resources[(object) "PhoneAccentColor"]);
      SolidColorBrush solidColorBrush4 = new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
      if (DataManager.LIGHT_THEME)
        solidColorBrush4 = new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, (byte) 0));
      if (!this.MessageOpenedFor.Context.Equals(""))
      {
        this.openContext.Background = (Brush) solidColorBrush3;
        this.openContextText.Foreground = (Brush) solidColorBrush4;
      }
      else
      {
        this.openContext.Background = (Brush) solidColorBrush2;
        this.openContextText.Foreground = (Brush) solidColorBrush1;
      }
      this.MessageActionButtonHolder.Visibility = Visibility.Visible;
      this.MessageActionReplyBox.Visibility = Visibility.Collapsed;
      this.SendingProgressOverlay.Visibility = Visibility.Collapsed;
      this.MessageActionPanel.Visibility = Visibility.Visible;
      this.BlackBackground.Visibility = Visibility.Visible;
      this.ApplicationBar.IsVisible = false;
      this.OpenMessageActionAnm.Begin();
      this.ActionsOpened = true;
    }

    public void CloseMessageActions()
    {
      this.CloseMessageActionAnm.Begin();
      this.ApplicationBar.IsVisible = true;
    }

    private void markRead_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.CloseMessageActions();
      if (this.MessageOpenedFor.isNew == 1)
      {
        this.MessageOpenedFor.BarColor = MessageInboxViewModel.notNewColor;
        this.MessageOpenedFor.isNew = 0;
        ThreadPool.QueueUserWorkItem((WaitCallback) (obfj => App.DataManager.ChangeMessageReadStatus(this.MessageOpenedFor.RName, 0)));
      }
      else
      {
        this.MessageOpenedFor.BarColor = DataManager.ACCENT_COLOR;
        this.MessageOpenedFor.isNew = 1;
        ThreadPool.QueueUserWorkItem((WaitCallback) (obfj => App.DataManager.ChangeMessageReadStatus(this.MessageOpenedFor.RName, 1)));
      }
    }

    private void reply_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.OpenReply();
    }

    private void openContext_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      if (this.MessageOpenedFor.Context.Equals(""))
        return;
      this.CloseMessageActions();
      string context = this.MessageOpenedFor.Context;
      int num1 = context.IndexOf("/", 3);
      int num2 = context.IndexOf("/", num1 + 1);
      int num3 = context.IndexOf("/", num2 + 1);
      string str = "t3_" + context.Substring(num2 + 1, num3 - num2 - 1);
      App.DataManager.BaconitAnalytics.LogEvent("Message Inbox View Context");
      App.ActiveStoryData = (SubRedditData) null;
      this.NavigationService.Navigate(new Uri("/StoryDetails.xaml?StoryDataRedditID=" + str + "&ShowComment=" + this.MessageOpenedFor.RName + "&Recenttile=true", UriKind.Relative));
    }

    private void ReplyText_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.text.Length == 0 || this.ReplyText.Text.Length == 0)
      {
        this.text = this.ReplyText.Text;
      }
      else
      {
        if ((int) this.text[this.text.Length - 1] == (int) this.ReplyText.Text[this.ReplyText.Text.Length - 1])
          return;
        this.ReplyScroller.ScrollToVerticalOffset(500000.0);
        this.text = this.ReplyText.Text;
      }
    }

    private void OpenReply()
    {
      this.MessageActionButtonHolder.Visibility = Visibility.Visible;
      this.MessageActionReplyBox.Visibility = Visibility.Visible;
      this.ReplyOpened = true;
      this.OpenReplyAn.Begin();
    }

    private void CloseReply()
    {
      this.MessageActionButtonHolder.Visibility = Visibility.Visible;
      this.MessageActionReplyBox.Visibility = Visibility.Visible;
      this.ReplyOpened = false;
      this.CloseReplyAn.Begin();
    }

    private void OpenReply_Completed(object sender, EventArgs e)
    {
      this.MessageActionButtonHolder.Visibility = Visibility.Collapsed;
      this.MessageActionReplyBox.Visibility = Visibility.Visible;
    }

    private void CloseReply_Completed(object sender, EventArgs e)
    {
      this.MessageActionButtonHolder.Visibility = Visibility.Visible;
      this.MessageActionReplyBox.Visibility = Visibility.Collapsed;
    }

    private void HideSending_Completed(object sender, EventArgs e)
    {
      this.SendingProgressOverlay.Visibility = Visibility.Collapsed;
      this.SendingProgressBar.IsIndeterminate = false;
    }

    private void ReplySubmit_Click(object sender, RoutedEventArgs e)
    {
      this.ReplayBeingSent = true;
      this.SendingProgressBar.IsIndeterminate = true;
      this.SendingProgressOverlay.Visibility = Visibility.Visible;
      this.ShowSending.Begin();
      App.DataManager.PostMessageReply(new MessageReply()
      {
        text = this.ReplyText.Text,
        RName = this.MessageOpenedFor.RName
      }, new RunWorkerCompletedEventHandler(this.ReplySubmit_Callback));
      App.DataManager.BaconitAnalytics.LogEvent("User Message Reply");
    }

    public void ReplySubmit_Callback(object obj, RunWorkerCompletedEventArgs e)
    {
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        if (e.Error is CaptchaException)
        {
          this.ReplayBeingSent = false;
          this.HideSending.Begin();
          this.CloseReply();
          this.ReplyText.Text = "";
          this.text = "";
          this.NavigationService.Navigate(new Uri("/CaptchaUI.xaml", UriKind.Relative));
        }
        else
        {
          this.ReplayBeingSent = false;
          if (!this.isOpen)
            return;
          if (e.Cancelled)
          {
            this.HideSending.Begin();
            this.CloseReply();
            this.ReplyText.Text = "";
            this.text = "";
            App.DataManager.BaconitAnalytics.LogEvent("Message Inbox Reply Sent");
          }
          else
            this.HideSending.Begin();
        }
      }));
    }

    private void ReplyClose_Click(object sender, RoutedEventArgs e) => this.CloseReply();

    private void profileButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      if (this.MessageOpenedFor == null || this.MessageOpenedFor.Author == null)
        return;
      this.NavigationService.Navigate(new Uri("/ProfileView.xaml?userName=" + this.MessageOpenedFor.Author, UriKind.Relative));
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/MessageInbox.xaml", UriKind.Relative));
      this.OpenReplyAn = (Storyboard) this.FindName("OpenReplyAn");
      this.CloseReplyAn = (Storyboard) this.FindName("CloseReplyAn");
      this.ShowSending = (Storyboard) this.FindName("ShowSending");
      this.HideSending = (Storyboard) this.FindName("HideSending");
      this.ShowMessages = (Storyboard) this.FindName("ShowMessages");
      this.OpenMessageActionAnm = (Storyboard) this.FindName("OpenMessageActionAnm");
      this.CloseMessageActionAnm = (Storyboard) this.FindName("CloseMessageActionAnm");
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TitlePanel = (StackPanel) this.FindName("TitlePanel");
      this.ApplicationTitle = (TextBlock) this.FindName("ApplicationTitle");
      this.MessageFilter = (ListPicker) this.FindName("MessageFilter");
      this.ContentPanel = (Grid) this.FindName("ContentPanel");
      this.MessageList = (ListBox) this.FindName("MessageList");
      this.NoMessagesUI = (StackPanel) this.FindName("NoMessagesUI");
      this.NoMessagesTextOneUI = (TextBlock) this.FindName("NoMessagesTextOneUI");
      this.BlackBackground = (Grid) this.FindName("BlackBackground");
      this.MessageActionPanel = (Grid) this.FindName("MessageActionPanel");
      this.MessageActionPanelConentHolder = (StackPanel) this.FindName("MessageActionPanelConentHolder");
      this.MessageActionLineOneOne = (TextBlock) this.FindName("MessageActionLineOneOne");
      this.MessageActionLineOneTwo = (TextBlock) this.FindName("MessageActionLineOneTwo");
      this.MessageActionMessageContent = (SuperRichTextBox) this.FindName("MessageActionMessageContent");
      this.MessageActionButtonHolder = (StackPanel) this.FindName("MessageActionButtonHolder");
      this.markRead = (Grid) this.FindName("markRead");
      this.markReadText = (TextBlock) this.FindName("markReadText");
      this.reply = (Grid) this.FindName("reply");
      this.EditButtons = (Grid) this.FindName("EditButtons");
      this.openContext = (Grid) this.FindName("openContext");
      this.openContextText = (TextBlock) this.FindName("openContextText");
      this.profileButton = (Grid) this.FindName("profileButton");
      this.openprofileText = (TextBlock) this.FindName("openprofileText");
      this.MessageActionReplyBox = (StackPanel) this.FindName("MessageActionReplyBox");
      this.ReplyScroller = (ScrollViewer) this.FindName("ReplyScroller");
      this.ReplyText = (TextBox) this.FindName("ReplyText");
      this.ReplySubmit = (Button) this.FindName("ReplySubmit");
      this.ReplyClose = (Button) this.FindName("ReplyClose");
      this.SendingProgressOverlay = (Grid) this.FindName("SendingProgressOverlay");
      this.LoadingText = (TextBlock) this.FindName("LoadingText");
      this.SendingProgressBar = (ProgressBar) this.FindName("SendingProgressBar");
      this.InboxLoadingProgressBar = (ProgressBar) this.FindName("InboxLoadingProgressBar");
    }

    private class MarkRead
    {
      public string rname = "";

      public void work() => App.DataManager.ChangeMessageReadStatus(this.rname, 0);
    }
  }
}
