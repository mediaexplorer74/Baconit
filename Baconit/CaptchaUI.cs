// Decompiled with JetBrains decompiler
// Type: Baconit.CaptchaUI
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Baconit.Database;
using BaconitData.Database;
using BaconitData.Libs;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

#nullable disable
namespace Baconit
{
  public class CaptchaUI : PhoneApplicationPage
  {
    private CaptchaInfo CapInfo;
    internal Grid LayoutRoot;
    internal Image CapImage;
    internal TextBox UserResponse;
    internal Button submitButton;
    private bool _contentLoaded;

    public CaptchaUI()
    {
      this.InitializeComponent();
      App.DataManager.LogMan.Info("CaptchaUI opened");
      TransitionService.SetNavigationInTransition((UIElement) this, App.SlideInTransistion);
      TransitionService.SetNavigationOutTransition((UIElement) this, App.SlideOutTransistion);
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      App.DataManager.BaconitAnalytics.LogPage("Captcha UI Opened");
      if (this.CapInfo != null)
        return;
      if (this.NavigationService.CanGoBack)
        this.NavigationService.RemoveBackEntry();
      if (App.DataManager.CaptchaInfoHolder == null)
        this.NavigationService.GoBack();
      this.CapInfo = App.DataManager.CaptchaInfoHolder;
      App.DataManager.CaptchaInfoHolder = (CaptchaInfo) null;
      this.GetCapImage();
    }

    public void Image_Callback(object obj, RunWorkerCompletedEventArgs e)
    {
      RedditImage image = (RedditImage) e.Result;
      if (image.CallBackStatus)
        this.Dispatcher.BeginInvoke((Action) (() =>
        {
          this.CapImage.Source = (ImageSource) image.Image;
          this.submitButton.IsEnabled = true;
          this.UserResponse.IsEnabled = true;
          SystemTray.SetProgressIndicator((DependencyObject) this, new ProgressIndicator()
          {
            IsIndeterminate = false,
            IsVisible = false
          });
          this.UserResponse.Focus();
        }));
      else
        App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("", false, false, "Web Error", "Baconit is currently not able to get the CAPTCHA image."));
    }

    private void TextBlock_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
    {
      App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("", false, false, "CAPTCHA Info", "Reddit requires users who have newer and low karma accounts to enter CAPTCHAs. This is to protect against bad people who use computer bots to spam reddit." + Environment.NewLine + Environment.NewLine + "You will only have to enter a CAPTCHA a few times before reddit will trust your account and will stop asking for them."));
    }

    private void UserResponse_KeyDown_1(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Enter)
        return;
      this.submitButton_Click_1((object) null, (RoutedEventArgs) null);
    }

    private void submitButton_Click_1(object sender, RoutedEventArgs e)
    {
      this.submitButton.IsEnabled = false;
      this.UserResponse.IsEnabled = false;
      SystemTray.SetProgressIndicator((DependencyObject) this, new ProgressIndicator()
      {
        IsIndeterminate = true,
        IsVisible = true,
        Text = "Submitting your input..."
      });
      switch (this.CapInfo.type)
      {
        case 0:
          SubmitALinkInfo info1 = (SubmitALinkInfo) this.CapInfo.Info;
          info1.CapID = this.CapInfo.CaptchaID;
          info1.CapResponse = this.UserResponse.Text.Trim();
          App.DataManager.SubmitRedditStory((SubmitALinkInfo) this.CapInfo.Info, new RunWorkerCompletedEventHandler(this.GenericSubmit_Callback));
          break;
        case 1:
          MessageReply info2 = (MessageReply) this.CapInfo.Info;
          info2.CapID = this.CapInfo.CaptchaID;
          info2.CapResponse = this.UserResponse.Text.Trim();
          App.DataManager.PostMessageReply((MessageReply) this.CapInfo.Info, new RunWorkerCompletedEventHandler(this.GenericSubmit_Callback));
          break;
        case 2:
          MessageComposed info3 = (MessageComposed) this.CapInfo.Info;
          info3.CapID = this.CapInfo.CaptchaID;
          info3.CapResponse = this.UserResponse.Text.Trim();
          App.DataManager.SendMessage((MessageComposed) this.CapInfo.Info, new RunWorkerCompletedEventHandler(this.GenericSubmit_Callback));
          break;
      }
    }

    public void GenericSubmit_Callback(object obj, RunWorkerCompletedEventArgs e)
    {
      if (e.Error is CaptchaException)
      {
        if (obj is string && ((string) obj).Equals("captcha-failed"))
        {
          App.DataManager.BaconitAnalytics.LogEvent("Captcha Failed Again");
          this.Dispatcher.BeginInvoke((Action) (() =>
          {
            App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("", false, false, "Invalid CAPTCHA", "That was not the correct response for that CAPTCHA, please try a new one."));
            this.CapInfo.CaptchaID = ((CaptchaException) e.Error).NewCaptchaID;
            this.GetCapImage();
          }));
        }
        else
        {
          App.DataManager.BaconitAnalytics.LogEvent("Captcha Failed Unknown Error");
          this.Dispatcher.BeginInvoke((Action) (() =>
          {
            this.submitButton.IsEnabled = true;
            this.UserResponse.IsEnabled = true;
            SystemTray.SetProgressIndicator((DependencyObject) this, new ProgressIndicator()
            {
              IsIndeterminate = false,
              IsVisible = false
            });
          }));
          if (obj is string && ((string) obj).Equals("captcha-failed-net-error"))
          {
            App.DataManager.MessageManager.showWebErrorMessage("submitting your response", (Exception) null, true);
          }
          else
          {
            if (!(obj is string))
              return;
            App.DataManager.MessageManager.showRedditErrorMessage("submitting your response", (string) obj, true);
          }
        }
      }
      else
      {
        try
        {
          App.DataManager.BaconitAnalytics.LogEvent("Captcha successful");
          App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("Submitted successfully", true, false, "", ""));
          this.Dispatcher.BeginInvoke((Action) (() => this.NavigationService.GoBack()));
        }
        catch
        {
        }
      }
    }

    public void GetCapImage()
    {
      RedditImage image = new RedditImage(App.DataManager, true, new RunWorkerCompletedEventHandler(this.Image_Callback));
      image.URL = "https://www.reddit.com/captcha/" + this.CapInfo.CaptchaID + ".png";
      image.StoryID = this.CapInfo.CaptchaID;
      image.SubRedditRName = "cap";
      image.SubRedditType = 0;
      this.UserResponse.Text = "";
      ThreadPool.QueueUserWorkItem((WaitCallback) (obj => image.run(obj)));
      SystemTray.SetProgressIndicator((DependencyObject) this, new ProgressIndicator()
      {
        IsIndeterminate = true,
        IsVisible = true,
        Text = "Loading CAPTCHA"
      });
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/CaptchaUI.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.CapImage = (Image) this.FindName("CapImage");
      this.UserResponse = (TextBox) this.FindName("UserResponse");
      this.submitButton = (Button) this.FindName("submitButton");
    }
  }
}
