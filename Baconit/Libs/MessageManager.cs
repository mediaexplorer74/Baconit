// Decompiled with JetBrains decompiler
// Type: Baconit.Libs.MessageManager
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Baconit.Database;
using BaconitData.Interfaces;
using BaconitData.Libs;
using Coding4Fun.Phone.Controls;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.GamerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace Baconit.Libs
{
  public class MessageManager : MessageManagerInterface
  {
    private List<BaconitUserMessage> Queue;
    private BaconitUserMessage CurrentlyShownMessage;
    private DataManager DataMan;
    private double LastResumeTime;
    private int ErrorsSoFar;

    public MessageManager(DataManager data)
    {
      this.DataMan = data;
      this.Queue = new List<BaconitUserMessage>();
    }

    public void QueueMessage(BaconitUserMessage message)
    {
      if (BaconitStore.currentTime() - this.LastResumeTime < 1000.0)
        return;
      this.Queue.Add(message);
      this.SendMessage();
    }

    private void SendMessage()
    {
      lock (this.Queue)
      {
        if (this.Queue.Count == 0 || this.CurrentlyShownMessage != null)
          return;
        BaconitUserMessage message = this.Queue[0];
        this.CurrentlyShownMessage = message;
        this.Queue.Remove(message);
        if (message.hasToast)
          Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
          {
            ToastPrompt toastPrompt = new ToastPrompt();
            toastPrompt.Title = "Baconit";
            toastPrompt.Message = message.ToastText;
            toastPrompt.ImageSource = (ImageSource) new BitmapImage(new Uri("/Images/CutsomToastImage.png", UriKind.RelativeOrAbsolute));
            toastPrompt.Foreground = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
            toastPrompt.Completed += (EventHandler<PopUpEventArgs<string, PopUpResult>>) ((sender, e) =>
            {
              if (e.PopUpResult == PopUpResult.Ok && message.hasClick)
              {
                this.MakeMessageBox(message);
              }
              else
              {
                this.CurrentlyShownMessage = (BaconitUserMessage) null;
                Deployment.Current.Dispatcher.BeginInvoke((Action) (() => this.SendMessage()));
              }
            });
            toastPrompt.Show();
          }));
        else
          this.MakeMessageBox(message);
      }
    }

    private void MakeMessageBox(BaconitUserMessage message)
    {
      if (message.BoxMessage == null || message.BoxMessage.Equals("") || message.BoxTitle == null || message.BoxMessage.Equals(""))
      {
        this.CurrentlyShownMessage = (BaconitUserMessage) null;
        Deployment.Current.Dispatcher.BeginInvoke((Action) (() => this.SendMessage()));
      }
      else if (message.Button1 == null || message.Button2 == null)
      {
        MessageBoxResult result = MessageBox.Show(message.BoxMessage, message.BoxTitle, MessageBoxButton.OK);
        if (message.MessageCallback != null)
          message.MessageCallback((object) null, new RunWorkerCompletedEventArgs((object) result, (Exception) null, false));
        this.CurrentlyShownMessage = (BaconitUserMessage) null;
        Deployment.Current.Dispatcher.BeginInvoke((Action) (() => this.SendMessage()));
      }
      else
        Guide.BeginShowMessageBox(message.BoxTitle, message.BoxMessage, (IEnumerable<string>) new string[2]
        {
          message.Button1,
          message.Button2
        }, 0, MessageBoxIcon.None, (AsyncCallback) (resul =>
        {
          if (message.MessageCallback != null)
            message.MessageCallback((object) null, new RunWorkerCompletedEventArgs((object) Guide.EndShowMessageBox(resul), (Exception) null, false));
          this.CurrentlyShownMessage = (BaconitUserMessage) null;
          Deployment.Current.Dispatcher.BeginInvoke((Action) (() => this.SendMessage()));
        }), (object) null);
    }

    public void SetResumeTime(double time) => this.LastResumeTime = time;

    public void DebugDia(string msg, Exception e)
    {
      if (!this.DataMan.SettingsMan.DEBUGGING || this.DataMan.RunningFromUpdater)
        return;
      string Boxmessage;
      if (e == null)
        Boxmessage = msg + "\nException:\n";
      else
        Boxmessage = msg + "\nException:\n" + e.Message + "\n\n" + e.StackTrace;
      BaconitUserMessage message = new BaconitUserMessage("Critical Error, tap here", true, true, "Exception", Boxmessage);
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() => this.DataMan.MessageManager.QueueMessage(message)));
    }

    public void showErrorMessage(string doingWhat, Exception e)
    {
      if (this.DataMan.RunningFromUpdater)
        return;
      string Boxtitle;
      string Boxmessage;
      if (this.ErrorsSoFar == 3)
      {
        Boxtitle = "Wow that's a lot of errors";
        Boxmessage = "Baconit encountered an error while " + doingWhat + ", either you have a very bad internet connection or reddit is acting up again :/";
        ++this.ErrorsSoFar;
      }
      else
      {
        Boxtitle = "Error";
        Boxmessage = "Baconit encountered an error while " + doingWhat + ", make sure you have an internet connection and try again later.";
        ++this.ErrorsSoFar;
      }
      if (this.DataMan.SettingsMan.DEBUGGING && e != null)
        Boxmessage = Boxmessage + "\n\nGENERAL ERROR: " + e.Message + " \n " + e.StackTrace;
      BaconitUserMessage message = new BaconitUserMessage("Error " + doingWhat, true, true, Boxtitle, Boxmessage);
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() => this.QueueMessage(message)));
    }

    public void showWebErrorMessage(string doingWhat, Exception e)
    {
      this.showWebErrorMessage(doingWhat, e, false);
    }

    public void showWebErrorMessage(string doingWhat, Exception e, bool forceMessageBox)
    {
      string Boxtitle = "Web Error";
      string Boxmessage = "Baconit encountered an error while " + doingWhat + " because an internet connection is unavailable or reddit is having trouble. Please try again later.";
      if (this.DataMan.SettingsMan.DEBUGGING && e != null)
        Boxmessage = Boxmessage + "\n\\nWEB ERROR: " + e.Message + " \n " + e.StackTrace;
      BaconitUserMessage message = new BaconitUserMessage("Error " + doingWhat, !forceMessageBox, true, Boxtitle, Boxmessage);
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() => this.QueueMessage(message)));
    }

    public void showRedditErrorMessage(string doingWhat, string redditError)
    {
      this.showRedditErrorMessage(doingWhat, redditError, false);
    }

    public void ShowSignInMessage(string action)
    {
      using (AutoResetEvent are = new AutoResetEvent(false))
      {
        Guide.BeginShowMessageBox("Account Required", "You must logged in to " + action + ", would you like to log in now?", (IEnumerable<string>) new string[2]
        {
          "log in",
          "cancel"
        }, 0, MessageBoxIcon.None, (AsyncCallback) (resul =>
        {
          int? nullable = Guide.EndShowMessageBox(resul);
          if (nullable.HasValue)
          {
            switch (nullable.GetValueOrDefault())
            {
              case 0:
                if (App.navService != null)
                {
                  Deployment.Current.Dispatcher.BeginInvoke((Action) (() => App.navService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative))));
                  break;
                }
                break;
            }
          }
          are.Set();
        }), (object) null);
        are.WaitOne();
      }
    }

    public void showRedditErrorMessage(string doingWhat, string redditError, bool ShowRedditError)
    {
      string Boxtitle = "Reddit Error";
      string Boxmessage = !redditError.Equals(".error.BAD_CAPTCHA.field-captcha") ? (!redditError.StartsWith(".error.NO_LINKS") ? (!redditError.StartsWith(".error.ALREADY_SUB") ? (redditError.StartsWith(".error.QUOTA_FILLED") || redditError.StartsWith(".error.RATELIMIT.field-ratelimit") ? "Slow down there, you’re doing that too fast and reddit is denying your requests. Try again in a few minutes." : "Reddit has returned and unknown error and Baconit is currently unable proceed " + doingWhat + ".") : "This link has already been posted to this subreddit!") : "This subreddit is for self text only, links are not allowed.") : "Reddit requires you to fill out a captcha to " + doingWhat + " because either your posting too quickly, or your account it too new. Baconit currently does not support this functionality, sorry.";
      if (this.DataMan.SettingsMan.DEBUGGING | ShowRedditError)
        Boxmessage = Boxmessage + "\n\nreddit error: " + redditError;
      BaconitUserMessage message = new BaconitUserMessage(string.Empty, false, true, Boxtitle, Boxmessage);
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() => this.QueueMessage(message)));
    }

    public void ShowRateMessage()
    {
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
      {
        BaconitUserMessage message = new BaconitUserMessage("Please rate Baconit! tap here!", true, true, "Show Some <3", "Baconit loves you, do you love Baconit? Please take a moment to leave a review!");
        message.SetLongMessage("review", "later", (RunWorkerCompletedEventHandler) ((o, arg) =>
        {
          if (arg.Result != null)
          {
            switch ((int) arg.Result)
            {
              case 0:
                try
                {
                  new MarketplaceReviewTask().Show();
                  this.DataMan.BaconitAnalytics.LogEvent("Rate App Message - Good");
                  break;
                }
                catch
                {
                  break;
                }
              case 1:
                this.DataMan.BaconitAnalytics.LogEvent("Rate App Message - Close");
                break;
            }
          }
          else
            this.DataMan.BaconitAnalytics.LogEvent("Rate App Message - Close");
        }));
        this.DataMan.MessageManager.QueueMessage(message);
      }));
    }

    public void ShowSubMessage()
    {
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
      {
        BaconitUserMessage message = new BaconitUserMessage("Subscribe to /r/Baconit! tap here!", true, true, "Subscribe to Baconit", "The Baconit subreddit is great for feature requests, feedback, and discussion. Would you like to subscribe to the Baconit subreddit?");
        message.SetLongMessage("subscribe me", "not now", (RunWorkerCompletedEventHandler) ((o, arg) =>
        {
          switch ((int) arg.Result)
          {
            case 0:
              try
              {
                ThreadPool.QueueUserWorkItem((WaitCallback) (obfj =>
                {
                  this.DataMan.SubredditDataManager.SubscribeToBaconit();
                  this.DataMan.BaconitAnalytics.LogEvent("Baconit Subreddit Sub Message - Good");
                }));
                break;
              }
              catch
              {
                break;
              }
            case 1:
              this.DataMan.BaconitAnalytics.LogEvent("Baconit Subreddit Sub Message - Ingored");
              break;
          }
        }));
        this.DataMan.MessageManager.QueueMessage(message);
      }));
    }

    public void ShowDonateMessage()
    {
      this.DataMan.BaconitAnalytics.LogEvent("Donate Message Shown");
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
      {
        BaconitUserMessage message = new BaconitUserMessage("Consider a Donation To Baconit", false, true, "Consider a Donation", "If you enjoy Baconit please consider giving a donation. Donations can be made in many amounts and they are greatly appreciated. Donation options can always be found in settings under Donate.");
        message.SetLongMessage("see options", "not now", (RunWorkerCompletedEventHandler) ((o, arg) =>
        {
          if (arg.Result != null)
          {
            switch ((int) arg.Result)
            {
              case 0:
                Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
                {
                  try
                  {
                    this.DataMan.BaconitAnalytics.LogEvent("Donate Message - Success");
                    if (App.navService == null)
                      return;
                    App.navService.Navigate(new Uri("/SettingPages/Donate.xaml", UriKind.Relative));
                  }
                  catch
                  {
                  }
                }));
                break;
              case 1:
                this.DataMan.BaconitAnalytics.LogEvent("Donate Message - Ingored");
                break;
            }
          }
          else
            this.DataMan.BaconitAnalytics.LogEvent("Donate Message - Ingored");
        }));
        this.DataMan.MessageManager.QueueMessage(message);
      }));
    }

    public MessageBoxResult AskAboutCrash()
    {
      MessageBoxResult result = MessageBoxResult.No;
      using (AutoResetEvent are = new AutoResetEvent(false))
      {
        Deployment.Current.Dispatcher.BeginInvoke((Action) (() => Guide.BeginShowMessageBox("Baconit Crash", "It appears that Baconit had some trouble last time it ran. Would you like to send this and future crash reports anonymously to the developer so these problems can be fixed?", (IEnumerable<string>) new string[2]
        {
          "send reports",
          "no thanks"
        }, 0, MessageBoxIcon.None, (AsyncCallback) (resul =>
        {
          int? nullable = Guide.EndShowMessageBox(resul);
          if (nullable.HasValue)
          {
            switch (nullable.GetValueOrDefault())
            {
              case 0:
                result = MessageBoxResult.OK;
                break;
              case 1:
                result = MessageBoxResult.Cancel;
                break;
            }
          }
          are.Set();
        }), (object) null)));
        are.WaitOne();
      }
      return result;
    }

    public void ShowCrashSent(string crashEx)
    {
      int num;
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() => num = (int) MessageBox.Show(crashEx, "Crash Report", MessageBoxButton.OK)));
    }

    public void ShowAprilError()
    {
      this.DataMan.BaconitAnalytics.LogEvent("April Fools");
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
      {
        if (App.navService == null)
          return;
        App.navService.Navigate(new Uri("/Error.xaml", UriKind.Relative));
      }));
    }
  }
}
