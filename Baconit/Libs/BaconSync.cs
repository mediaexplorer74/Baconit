// Decompiled with JetBrains decompiler
// Type: Baconit.Libs.BaconSync
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Baconit.Database;
using BaconitData.Interfaces;
using Microsoft.Phone.BackgroundTransfer;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Notification;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.GamerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows;

#nullable disable
namespace Baconit.Libs
{
  public class BaconSync : BaconSyncInterface
  {
    private DataManager DataMan;
    private HttpNotificationChannel pushChannel;
    public const string DevKey = "n5kBQrhyHwBWVPmywXhDhGTqsMcT7J";
    private bool PushSetup;
    private object lockObj = new object();

    public BaconSync(DataManager host) => this.DataMan = host;

    public void Bump()
    {
      this.DataMan.SettingsMan.BaconSyncShowFurtureError = true;
      ThreadPool.QueueUserWorkItem((WaitCallback) (o =>
      {
        try
        {
          this.StartUpPush();
          this.Sync(true);
        }
        catch
        {
          this.PushSetup = false;
        }
      }));
    }

    public void AppDisactivated() => this.SendInBackground();

    public void SendInBackground()
    {
      if (!this.DataMan.SettingsMan.BaconSyncEnabled)
      {
        this.DataMan.SettingsMan.BaconSyncStatus = "Disabled";
      }
      else
      {
        foreach (BackgroundTransferRequest request in BackgroundTransferService.Requests)
          BackgroundTransferService.Remove(request);
        if (this.DataMan.SettingsMan.BaconSyncReadList.Count == 0)
          return;
        if (!this.DataMan.SettingsMan.IsSignedIn || this.DataMan.SettingsMan.BaconSyncAccountName.Equals(""))
          this.DataMan.SettingsMan.BaconSyncStatus = "Error - Not Signed In";
        else if (this.DataMan.SettingsMan.BaconSyncDeviceName.Equals(""))
          this.DataMan.SettingsMan.BaconSyncStatus = "Error - No Device Name";
        else if (this.DataMan.SettingsMan.BaconSyncPassCode.Equals(""))
        {
          this.DataMan.SettingsMan.BaconSyncStatus = "Error - No Pass Code";
        }
        else
        {
          if (DateTime.Now.Subtract(BaconitStore.DoubleToDateTime(this.DataMan.BaconitStore.LastUpdatedTime(nameof (BaconSync)))).TotalSeconds < 30.0)
            return;
          List<string> baconSyncReadList = this.DataMan.SettingsMan.BaconSyncReadList;
          string url = "";
          foreach (string str in baconSyncReadList)
            url = url + str + ",";
          BackgroundTransferRequest request = new BackgroundTransferRequest(new Uri("http://www.quinndamerell.com/Baconit/BaconSync/v1/ReportAndGetRedditIDs.php" + "?UserName=" + HttpUtility.UrlEncode(this.DataMan.SettingsMan.BaconSyncAccountName) + "&DeviceName=" + HttpUtility.UrlEncode(this.DataMan.SettingsMan.BaconSyncDeviceName) + "&PassCode=" + HttpUtility.UrlEncode(BaconSync.CalculateSHA1(this.DataMan.SettingsMan.BaconSyncAccountName + this.DataMan.SettingsMan.BaconSyncPassCode)) + "&DontReturnReadList=true&RedditIDList=" + HttpUtility.UrlEncode(url) + "&DeveloperKey=" + HttpUtility.UrlEncode("n5kBQrhyHwBWVPmywXhDhGTqsMcT7J"), UriKind.Absolute));
          request.DownloadLocation = new Uri("shared/transfers/text.txt", UriKind.Relative);
          request.Method = "GET";
          request.TransferPreferences = TransferPreferences.AllowCellularAndBattery;
          try
          {
            BackgroundTransferService.Add(request);
          }
          catch (Exception ex)
          {
            this.DataMan.DebugDia("Couldn't set background transfter", ex);
          }
        }
      }
    }

    private void Sync(bool GetNewRead)
    {
      if (!this.DataMan.SettingsMan.BaconSyncEnabled)
        this.DataMan.SettingsMan.BaconSyncStatus = "Disabled";
      else if (!this.DataMan.SettingsMan.IsSignedIn || this.DataMan.SettingsMan.BaconSyncAccountName.Equals(""))
        this.DataMan.SettingsMan.BaconSyncStatus = "Error - Not Signed In";
      else if (this.DataMan.SettingsMan.BaconSyncDeviceName.Equals(""))
        this.DataMan.SettingsMan.BaconSyncStatus = "Error - No Device Name";
      else if (this.DataMan.SettingsMan.BaconSyncPassCode.Equals(""))
      {
        this.DataMan.SettingsMan.BaconSyncStatus = "Error - No Pass Code";
      }
      else
      {
        if (!GetNewRead && this.DataMan.SettingsMan.BaconSyncReadList.Count == 0 || DateTime.Now.Subtract(BaconitStore.DoubleToDateTime(this.DataMan.BaconitStore.LastUpdatedTime(nameof (BaconSync)))).TotalSeconds < 30.0)
          return;
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("UserName", (object) HttpUtility.UrlEncode(this.DataMan.SettingsMan.BaconSyncAccountName));
        parameters.Add("DeviceName", (object) HttpUtility.UrlEncode(this.DataMan.SettingsMan.BaconSyncDeviceName));
        parameters.Add("PassCode", (object) HttpUtility.UrlEncode(BaconSync.CalculateSHA1(this.DataMan.SettingsMan.BaconSyncAccountName + this.DataMan.SettingsMan.BaconSyncPassCode)));
        parameters.Add("DeveloperKey", (object) HttpUtility.UrlEncode("n5kBQrhyHwBWVPmywXhDhGTqsMcT7J"));
        parameters.Add("CheckPush", (object) "true");
        if (!GetNewRead)
          parameters.Add("DontReturnReadList", (object) "true");
        List<string> tempList = (List<string>) null;
        if (this.DataMan.SettingsMan.BaconSyncReadList.Count > 0)
        {
          tempList = this.DataMan.SettingsMan.BaconSyncReadList;
          this.DataMan.SettingsMan.BaconSyncReadList = new List<string>();
          this.DataMan.SettingsMan.SetBaconSyncReadList();
          string url = "";
          foreach (string str in tempList)
            url = url + str + ",";
          parameters.Add("RedditIDList", (object) HttpUtility.UrlEncode(url));
        }
        this.DataMan.BaconitStore.UpdateLastUpdatedTime(nameof (BaconSync));
        PostClient postClient = new PostClient((IDictionary<string, object>) parameters);
        postClient.DownloadStringCompleted += (PostClient.DownloadStringCompletedHandler) ((sender, e) =>
        {
          bool flag = false;
          if (e.Error == null)
          {
            try
            {
              JObject jobject = JObject.Parse(e.Result);
              JToken jtoken1 = jobject["HasPushUrl"];
              if (jtoken1 != null && ((string) jtoken1).Equals("false"))
                this.BumpPush();
              if (((string) jobject["status"]).Equals("success"))
              {
                JToken jtoken2 = jobject["RedditIDList"];
                List<string> stringList = new List<string>();
                foreach (JToken jtoken3 in (IEnumerable<JToken>) jtoken2)
                  this.DataMan.SettingsMan.ReadStories.AddIfNotAlready((string) jtoken3[(object) "RedditID"], 0);
                this.DataMan.SettingsMan.SetReadStories();
                this.DataMan.SettingsMan.BaconSyncStatus = "Success";
                this.DataMan.SettingsMan.BaconSyncShowFurtureError = true;
                flag = true;
              }
              else
              {
                if (((string) jobject["status"]).Equals("Failed - Wrong Pass Code"))
                {
                  if (this.DataMan.SettingsMan.BaconSyncShowFurtureError)
                    Deployment.Current.Dispatcher.BeginInvoke((Action) (() => Guide.BeginShowMessageBox("BaconSync Error", "Your BaconSync pass code is no longer valid, you need to either update your pass code or disable BaconSync for now.", (IEnumerable<string>) new string[2]
                    {
                      "update code",
                      "disable"
                    }, 0, MessageBoxIcon.None, (AsyncCallback) (resul =>
                    {
                      int? nullable = Guide.EndShowMessageBox(resul);
                      if (!nullable.HasValue)
                        return;
                      switch (nullable.GetValueOrDefault())
                      {
                        case 0:
                          Deployment.Current.Dispatcher.BeginInvoke((Action) (() => (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri("/SettingPages/BaconSync/BaconSyncLanding.xaml", UriKind.Relative))));
                          this.DataMan.SettingsMan.BaconSyncShowFurtureError = false;
                          break;
                        case 1:
                          this.DataMan.SettingsMan.BaconSyncShowFurtureError = false;
                          break;
                      }
                    }), (object) null)));
                  this.DataMan.SettingsMan.BaconSyncPushURL = "";
                }
                this.DataMan.SettingsMan.BaconSyncStatus = (string) jobject["status"];
                this.DataMan.DebugDia("BaconitSync Failed " + this.DataMan.SettingsMan.BaconSyncStatus, (Exception) null);
              }
            }
            catch (JsonReaderException ex)
            {
              this.DataMan.SettingsMan.BaconSyncStatus = "Failed - Server Error";
              this.DataMan.DebugDia("BaconitSync Failed " + this.DataMan.SettingsMan.BaconSyncStatus, (Exception) ex);
            }
            catch (Exception ex)
            {
              this.DataMan.SettingsMan.BaconSyncStatus = "Failed - Network Error";
              this.DataMan.DebugDia("BaconitSync Failed " + this.DataMan.SettingsMan.BaconSyncStatus, ex);
            }
          }
          else
          {
            this.DataMan.SettingsMan.BaconSyncStatus = "Failed - Network Error";
            this.DataMan.DebugDia("BaconitSync Failed " + this.DataMan.SettingsMan.BaconSyncStatus, (Exception) null);
          }
          if (flag || tempList == null)
            return;
          foreach (string str in tempList)
            this.DataMan.SettingsMan.BaconSyncReadList.Add(str);
          this.DataMan.SettingsMan.SetBaconSyncReadList();
        });
        postClient.DownloadStringAsync(new Uri("http://www.quinndamerell.com/Baconit/BaconSync/v1/ReportAndGetRedditIDs.php", UriKind.Absolute));
      }
    }

    public void AddToReadList(string redditID)
    {
      this.DataMan.SettingsMan.BaconSyncReadList.Add(redditID);
      this.DataMan.SettingsMan.SetBaconSyncReadList();
    }

    public void StartUpPush()
    {
      if (!this.DataMan.SettingsMan.BaconSyncEnabled || !this.DataMan.SettingsMan.IsSignedIn || this.DataMan.SettingsMan.BaconSyncAccountName.Equals("") || this.DataMan.SettingsMan.BaconSyncDeviceName.Equals("") || this.DataMan.SettingsMan.BaconSyncPassCode.Equals(""))
        return;
      lock (this.lockObj)
      {
        if (this.PushSetup)
          return;
        this.PushSetup = true;
        this.pushChannel = HttpNotificationChannel.Find(DataManager.PUSH_CHANNEL);
        if (this.pushChannel == null)
        {
          this.pushChannel = new HttpNotificationChannel(DataManager.PUSH_CHANNEL);
          this.pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(this.PushChannel_ChannelUriUpdated);
          this.pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(this.PushChannel_ShellToastNotificationReceived);
          this.pushChannel.Open();
          this.pushChannel.BindToShellTile();
          this.pushChannel.BindToShellToast();
        }
        else
        {
          this.pushChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(this.PushChannel_ChannelUriUpdated);
          this.pushChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(this.PushChannel_ShellToastNotificationReceived);
          this.checkSendPushURL(this.pushChannel.ChannelUri.ToString());
        }
      }
    }

    private void PushChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
    {
      this.checkSendPushURL(e.ChannelUri.ToString());
    }

    private void checkSendPushURL(string newUrl)
    {
      if (newUrl.Equals(this.DataMan.SettingsMan.BaconSyncPushURL))
        return;
      this.SendPushURL(newUrl);
    }

    private void BumpPush()
    {
      if (this.pushChannel == null)
        return;
      this.SendPushURL(this.pushChannel.ChannelUri.ToString());
    }

    public void SendPushURL(string NewURL)
    {
      if (this.DataMan.SettingsMan.BaconSyncStatus.Equals("Failed - Wrong Pass Code"))
        return;
      PostClient postClient = new PostClient((IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "UserName",
          (object) HttpUtility.UrlEncode(this.DataMan.SettingsMan.BaconSyncAccountName)
        },
        {
          "DeviceName",
          (object) HttpUtility.UrlEncode(this.DataMan.SettingsMan.BaconSyncDeviceName)
        },
        {
          "PassCode",
          (object) HttpUtility.UrlEncode(BaconSync.CalculateSHA1(this.DataMan.SettingsMan.BaconSyncAccountName + this.DataMan.SettingsMan.BaconSyncPassCode))
        },
        {
          "NewPushUrl",
          (object) HttpUtility.UrlEncode(NewURL)
        },
        {
          "DeveloperKey",
          (object) HttpUtility.UrlEncode("n5kBQrhyHwBWVPmywXhDhGTqsMcT7J")
        }
      });
      postClient.DownloadStringCompleted += (PostClient.DownloadStringCompletedHandler) ((sender, e) =>
      {
        if (e.Error == null)
        {
          try
          {
            JObject jobject = JObject.Parse(e.Result);
            if (((string) jobject["status"]).Equals("success"))
            {
              this.DataMan.SettingsMan.BaconSyncPushURL = NewURL;
            }
            else
            {
              this.DataMan.DebugDia("BaconitSync Counldn't Update Push " + (string) jobject["status"], (Exception) null);
              this.DataMan.SettingsMan.BaconSyncPushURL = "";
            }
          }
          catch (JsonReaderException ex)
          {
            this.DataMan.DebugDia("BaconitSync Counldn't Update Push", (Exception) ex);
            this.DataMan.SettingsMan.BaconSyncPushURL = "";
          }
          catch (Exception ex)
          {
            this.DataMan.DebugDia("BaconitSync Counldn't Update Push", ex);
            this.DataMan.SettingsMan.BaconSyncPushURL = "";
          }
        }
        else
        {
          this.DataMan.DebugDia("BaconitSync Counldn't Update Push", (Exception) null);
          this.DataMan.SettingsMan.BaconSyncPushURL = "";
        }
      });
      postClient.DownloadStringAsync(new Uri("http://www.quinndamerell.com/Baconit/BaconSync/v1/SetPushURL.php", UriKind.Absolute));
    }

    private void PushChannel_ShellToastNotificationReceived(object sender, NotificationEventArgs e)
    {
      try
      {
        string url = HttpUtility.UrlDecode(e.Collection["wp:Param"]);
        int num = url.IndexOf("BaconSyncUrl");
        if (num == -1)
          return;
        url = url.Substring(num + 13);
        this.DataMan.BaconitAnalytics.LogEvent("BaconSync Web Content Recieved App Open");
        Deployment.Current.Dispatcher.BeginInvoke((Action) (() => Guide.BeginShowMessageBox("Web Content Received", "You have received  web content, would you like to show it now or ignore it?", (IEnumerable<string>) new string[2]
        {
          "show",
          "ignore"
        }, 0, MessageBoxIcon.None, (AsyncCallback) (resul =>
        {
          int? nullable = Guide.EndShowMessageBox(resul);
          if (!nullable.HasValue)
            return;
          if (nullable.GetValueOrDefault() == 0)
            this.HandlePushUrl(url);
        }), (object) null)));
      }
      catch
      {
      }
    }

    public void HandlePushUrl(string url)
    {
      if (!url.StartsWith("http"))
        url = "http://" + url;
      bool flag = false;
      string localNavString = (string) null;
      try
      {
        localNavString = DataManager.GetLocalNavStringFromRedditURL(url);
        if (!string.IsNullOrEmpty(localNavString))
          flag = true;
      }
      catch
      {
        flag = false;
      }
      if (flag && localNavString != null)
        Deployment.Current.Dispatcher.BeginInvoke((Action) (() => (Application.Current.RootVisual as PhoneApplicationFrame).Navigate(new Uri(localNavString, UriKind.Relative))));
      else
        new WebBrowserTask()
        {
          Uri = new Uri(url, UriKind.Absolute)
        }.Show();
    }

    public void SignOutOfBaconSync()
    {
      this.DataMan.SettingsMan.BaconSyncAccountName = "";
      this.DataMan.SettingsMan.BaconSyncDeviceName = "";
      this.DataMan.SettingsMan.BaconSyncEnabled = false;
      this.DataMan.SettingsMan.BaconSyncPassCode = "";
      this.DataMan.SettingsMan.BaconSyncPushURL = "";
      this.DataMan.SettingsMan.BaconSyncReadList = new List<string>();
      this.DataMan.SettingsMan.SetBaconSyncReadList();
      this.DataMan.SettingsMan.BaconSyncShowFurtureError = true;
    }

    public void SetNewPassCode(string NewPassword, RunWorkerCompletedEventHandler callback)
    {
      PostClient postClient = new PostClient((IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "UserName",
          (object) HttpUtility.UrlEncode(this.DataMan.SettingsMan.BaconSyncAccountName)
        },
        {
          "NewPassCode",
          (object) HttpUtility.UrlEncode(BaconSync.CalculateSHA1(this.DataMan.SettingsMan.BaconSyncAccountName + NewPassword))
        },
        {
          "DeveloperKey",
          (object) HttpUtility.UrlEncode("n5kBQrhyHwBWVPmywXhDhGTqsMcT7J")
        }
      });
      postClient.DownloadStringCompleted += (PostClient.DownloadStringCompletedHandler) ((sender, e) =>
      {
        if (e.Error == null)
        {
          try
          {
            JObject jobject = JObject.Parse(e.Result);
            if (((string) jobject["status"]).Equals("success"))
            {
              callback((object) null, new RunWorkerCompletedEventArgs((object) "success", (Exception) null, false));
              this.DataMan.SettingsMan.BaconSyncPushURL = "";
            }
            else
            {
              callback((object) null, new RunWorkerCompletedEventArgs((object) (string) jobject["status"], (Exception) null, false));
              this.DataMan.DebugDia("BaconitSync Pass Code Update Failed " + (string) jobject["status"], (Exception) null);
            }
          }
          catch (JsonReaderException ex)
          {
            callback((object) null, new RunWorkerCompletedEventArgs((object) "server error", (Exception) null, false));
            this.DataMan.DebugDia("BaconitSync Pass Code Update Failed ", (Exception) ex);
          }
          catch (Exception ex)
          {
            callback((object) null, new RunWorkerCompletedEventArgs((object) "network error", (Exception) null, false));
            this.DataMan.DebugDia("BaconitSync Pass Code Update Failed ", ex);
          }
        }
        else
        {
          callback((object) null, new RunWorkerCompletedEventArgs((object) "network error", (Exception) null, false));
          this.DataMan.DebugDia("BaconitSync Pass Code Update Failed Web Error", (Exception) null);
        }
      });
      postClient.DownloadStringAsync(new Uri("http://www.quinndamerell.com/Baconit/BaconSync/v1/SetNewPassCode.php", UriKind.Absolute));
    }

    public void CheckAccount(RunWorkerCompletedEventHandler callback)
    {
      PostClient postClient = new PostClient((IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "UserName",
          (object) HttpUtility.UrlEncode(this.DataMan.SettingsMan.BaconSyncAccountName)
        },
        {
          "DeviceName",
          (object) HttpUtility.UrlEncode(this.DataMan.SettingsMan.BaconSyncDeviceName)
        },
        {
          "PassCode",
          (object) HttpUtility.UrlEncode(BaconSync.CalculateSHA1(this.DataMan.SettingsMan.BaconSyncAccountName + this.DataMan.SettingsMan.BaconSyncPassCode))
        },
        {
          "DeveloperKey",
          (object) HttpUtility.UrlEncode("n5kBQrhyHwBWVPmywXhDhGTqsMcT7J")
        }
      });
      postClient.DownloadStringCompleted += (PostClient.DownloadStringCompletedHandler) ((sender, e) =>
      {
        if (e.Error == null)
        {
          try
          {
            JObject jobject = JObject.Parse(e.Result);
            if (((string) jobject["status"]).Equals("success"))
              callback((object) null, new RunWorkerCompletedEventArgs((object) "success", (Exception) null, false));
            else
              callback((object) null, new RunWorkerCompletedEventArgs((object) (string) jobject["status"], (Exception) null, false));
          }
          catch (JsonReaderException ex)
          {
            callback((object) null, new RunWorkerCompletedEventArgs((object) "server error", (Exception) null, false));
          }
          catch
          {
            callback((object) null, new RunWorkerCompletedEventArgs((object) "network error", (Exception) null, false));
          }
        }
        else
          callback((object) null, new RunWorkerCompletedEventArgs((object) "network error", (Exception) null, false));
      });
      postClient.DownloadStringAsync(new Uri("http://www.quinndamerell.com/Baconit/BaconSync/v1/CheckAccount.php", UriKind.Absolute));
    }

    private static string CalculateSHA1(string text)
    {
      SHA1Managed shA1Managed = new SHA1Managed();
      shA1Managed.ComputeHash(new UTF8Encoding().GetBytes(text.ToCharArray()));
      return BitConverter.ToString(shA1Managed.Hash).Replace("-", "").ToLowerInvariant();
    }
  }
}
