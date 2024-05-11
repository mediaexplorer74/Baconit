// Decompiled with JetBrains decompiler
// Type: Baconit.ComposeMessage
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using BaconitData.Database;
using BaconitData.Libs;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

#nullable disable
namespace Baconit
{
  public class ComposeMessage : PhoneApplicationPage
  {
    private bool isOpen;
    private string OpenedFor = "";
    private bool hasTo;
    private bool hasSubject;
    private bool hasMessage;
    private PageOrientation CurOrientation = PageOrientation.Portrait;
    private bool isOpening = true;
    private char lastChar = '+';
    internal Grid LayoutRoot;
    internal StackPanel TitlePanel;
    internal TextBlock ApplicationTitle;
    internal Grid ContentPanel;
    internal ScrollViewer ContentScroller;
    internal TextBox ToTextBox;
    internal TextBox SubjectTextBox;
    internal TextBox MessageTextBox;
    private bool _contentLoaded;

    public ComposeMessage()
    {
      this.InitializeComponent();
      TransitionService.SetNavigationInTransition((UIElement) this, App.SlideInTransistion);
      TransitionService.SetNavigationOutTransition((UIElement) this, App.SlideOutTransistion);
      this.ApplicationBar = (IApplicationBar) new Microsoft.Phone.Shell.ApplicationBar();
      this.ApplicationBar.IsVisible = true;
      ApplicationBarIconButton applicationBarIconButton = new ApplicationBarIconButton(new Uri("/Images/appbar.send.png", UriKind.Relative));
      applicationBarIconButton.Text = "send";
      applicationBarIconButton.IsEnabled = false;
      applicationBarIconButton.Click += new EventHandler(this.Send_Click);
      this.ApplicationBar.Buttons.Add((object) applicationBarIconButton);
      this.Loaded += (RoutedEventHandler) ((sender, e) => this.isOpening = false);
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      this.isOpen = true;
      App.DataManager.BaconitAnalytics.LogPage("Compose Message");
      if (App.navService == null)
        App.navService = this.NavigationService;
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        if (!this.NavigationContext.QueryString.TryGetValue("UserName", out this.OpenedFor))
          this.OpenedFor = "";
        if (this.OpenedFor == null || this.OpenedFor.Equals(""))
          return;
        this.ToTextBox.Text = this.OpenedFor;
      }));
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      base.OnNavigatedFrom(e);
      this.isOpen = true;
    }

    protected override void OnBackKeyPress(CancelEventArgs e)
    {
      if (this.isOpening)
        e.Cancel = true;
      else
        base.OnBackKeyPress(e);
    }

    private void StackPanel_SizeChanged(object sender, SizeChangedEventArgs e)
    {
    }

    private void ToTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      this.hasTo = this.ToTextBox != null && !this.ToTextBox.Text.Equals("");
      this.CheckButton();
    }

    private void SubjectTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      this.hasSubject = this.SubjectTextBox != null && !this.SubjectTextBox.Text.Equals("");
      this.CheckButton();
    }

    private void MessageTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      this.hasMessage = this.MessageTextBox != null && !this.MessageTextBox.Text.Equals("");
      if ((int) this.MessageTextBox.Text[this.MessageTextBox.Text.Length - 1] != (int) this.lastChar)
      {
        this.lastChar = this.MessageTextBox.Text[this.MessageTextBox.Text.Length - 1];
        this.ContentScroller.ScrollToVerticalOffset(99999999.0);
      }
      this.CheckButton();
    }

    private void CheckButton()
    {
      ((ApplicationBarIconButton) this.ApplicationBar.Buttons[0]).IsEnabled = this.hasTo && this.hasSubject && this.hasMessage;
    }

    private void PhoneApplicationPage_OrientationChanged(
      object sender,
      OrientationChangedEventArgs e)
    {
      PageOrientation orientation = e.Orientation;
      RotateTransition rotateTransition = new RotateTransition();
      switch (orientation)
      {
        case PageOrientation.Portrait:
        case PageOrientation.PortraitUp:
          rotateTransition.Mode = this.CurOrientation != PageOrientation.LandscapeLeft ? RotateTransitionMode.In90Clockwise : RotateTransitionMode.In90Counterclockwise;
          break;
        case PageOrientation.Landscape:
        case PageOrientation.LandscapeRight:
          rotateTransition.Mode = this.CurOrientation != PageOrientation.PortraitUp ? RotateTransitionMode.In180Clockwise : RotateTransitionMode.In90Counterclockwise;
          break;
        case PageOrientation.LandscapeLeft:
          rotateTransition.Mode = this.CurOrientation != PageOrientation.LandscapeRight ? RotateTransitionMode.In90Clockwise : RotateTransitionMode.In180Counterclockwise;
          break;
      }
      PhoneApplicationPage content = (PhoneApplicationPage) ((ContentControl) Application.Current.RootVisual).Content;
      ITransition transition = rotateTransition.GetTransition((UIElement) content);
      transition.Completed += (EventHandler) delegate
      {
        transition.Stop();
      };
      transition.Begin();
      this.CurOrientation = e.Orientation;
    }

    private void Send_Click(object sender, EventArgs e)
    {
      this.IsHitTestVisible = true;
      this.IsTabStop = true;
      this.Focus();
      this.ApplicationBar.IsVisible = false;
      ThreadPool.QueueUserWorkItem((WaitCallback) (obj => App.DataManager.SendMessage(new MessageComposed()
      {
        userName = HttpUtility.HtmlEncode(this.ToTextBox.Text),
        subject = HttpUtility.HtmlEncode(this.SubjectTextBox.Text),
        text = HttpUtility.HtmlEncode(this.MessageTextBox.Text)
      }, new RunWorkerCompletedEventHandler(this.MessageSendCallBack))));
      SystemTray.SetProgressIndicator((DependencyObject) this, new ProgressIndicator()
      {
        IsIndeterminate = true,
        IsVisible = true,
        Text = "Sending message..."
      });
      this.ToTextBox.IsEnabled = false;
      this.SubjectTextBox.IsEnabled = false;
      this.MessageTextBox.IsEnabled = false;
      ((ApplicationBarIconButton) this.ApplicationBar.Buttons[0]).IsEnabled = false;
    }

    public void MessageSendCallBack(object obj, RunWorkerCompletedEventArgs e)
    {
      if (e.Error is CaptchaException)
        this.Dispatcher.BeginInvoke((Action) (() =>
        {
          SystemTray.SetProgressIndicator((DependencyObject) this, new ProgressIndicator()
          {
            IsIndeterminate = false,
            IsVisible = false
          });
          this.ToTextBox.IsEnabled = true;
          this.SubjectTextBox.IsEnabled = true;
          this.MessageTextBox.IsEnabled = true;
          ((ApplicationBarIconButton) this.ApplicationBar.Buttons[0]).IsEnabled = true;
          this.NavigationService.Navigate(new Uri("/CaptchaUI.xaml", UriKind.Relative));
        }));
      else if ((bool) e.Result)
      {
        App.DataManager.BaconitAnalytics.LogEvent("Account Message Sent");
        if (!this.isOpen)
          return;
        this.Dispatcher.BeginInvoke((Action) (() =>
        {
          try
          {
            this.NavigationService.GoBack();
          }
          catch
          {
            int num = (int) MessageBox.Show("Your message has been sent.", "Message Sent", MessageBoxButton.OK);
          }
        }));
      }
      else
      {
        if (!this.isOpen)
          return;
        this.ApplicationBar.IsVisible = true;
        if (obj == null)
          return;
        this.Dispatcher.BeginInvoke((Action) (() =>
        {
          string str = (string) obj;
          if (str == null || str.Equals(""))
            App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("Can't send message, tap here", true, true, "Can't Send Message", "Baconit currently can't send this message."));
          else
            App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("Can't send message, tap here", true, true, "Can't Send Message", "Baconit currently can't send this message.\n\nError: " + str));
          SystemTray.SetProgressIndicator((DependencyObject) this, new ProgressIndicator()
          {
            IsIndeterminate = false,
            IsVisible = false
          });
          this.ToTextBox.IsEnabled = true;
          this.SubjectTextBox.IsEnabled = true;
          this.MessageTextBox.IsEnabled = true;
          ((ApplicationBarIconButton) this.ApplicationBar.Buttons[0]).IsEnabled = true;
        }));
      }
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/ComposeMessage.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TitlePanel = (StackPanel) this.FindName("TitlePanel");
      this.ApplicationTitle = (TextBlock) this.FindName("ApplicationTitle");
      this.ContentPanel = (Grid) this.FindName("ContentPanel");
      this.ContentScroller = (ScrollViewer) this.FindName("ContentScroller");
      this.ToTextBox = (TextBox) this.FindName("ToTextBox");
      this.SubjectTextBox = (TextBox) this.FindName("SubjectTextBox");
      this.MessageTextBox = (TextBox) this.FindName("MessageTextBox");
    }
  }
}
