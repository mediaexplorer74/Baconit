// Decompiled with JetBrains decompiler
// Type: Baconit.SettingPages.BaconSync.BaconSyncAccountInfo
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.GamerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;

#nullable disable
namespace Baconit.SettingPages.BaconSync
{
  public class BaconSyncAccountInfo : PhoneApplicationPage
  {
    internal Storyboard OpenLoadingOverLay;
    internal Storyboard CloseLoadingOverLay;
    internal Grid LayoutRoot;
    internal StackPanel TitlePanel;
    internal TextBlock ApplicationTitle;
    internal TextBlock PageTitle;
    internal StackPanel ContentPanel;
    internal TextBox DeviceName;
    internal TextBox PassCode;
    internal Button NextPage;
    internal Grid LoadingOverLay;
    internal Rectangle LoggingInOverlayBack;
    internal ProgressBar OverlayProgress;
    private bool _contentLoaded;

    public BaconSyncAccountInfo()
    {
      this.InitializeComponent();
      this.DeviceName.Text = App.DataManager.SettingsMan.BaconSyncDeviceName;
      this.PassCode.Text = App.DataManager.SettingsMan.BaconSyncPassCode;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      if (App.DataManager.SettingsMan.BaconSyncEnabled)
      {
        this.LayoutRoot.Visibility = Visibility.Collapsed;
        this.Dispatcher.BeginInvoke((Action) (() =>
        {
          if (!this.NavigationService.CanGoBack)
            return;
          this.NavigationService.GoBack();
        }));
      }
      else
        this.LayoutRoot.Visibility = Visibility.Visible;
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      this.OverlayProgress.IsIndeterminate = false;
    }

    private void NextPage_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        if (this.DeviceName.Text.Trim().Equals(""))
        {
          int num1 = (int) MessageBox.Show("Your device must have a unique device name!", "Name Required", MessageBoxButton.OK);
        }
        else if (this.PassCode.Text.Trim().Equals(""))
        {
          int num2 = (int) MessageBox.Show("You must enter a pass code for your account!", "Pass Code Required", MessageBoxButton.OK);
        }
        else
        {
          if (this.DeviceName.Text.Length > 50)
            this.DeviceName.Text = this.DeviceName.Text.Substring(0, 50);
          App.DataManager.SettingsMan.BaconSyncPassCode = this.PassCode.Text;
          App.DataManager.SettingsMan.BaconSyncDeviceName = this.DeviceName.Text;
          App.DataManager.BaconSyncObj.CheckAccount(new RunWorkerCompletedEventHandler(this.AccountCheck_RunWorkerCompleted));
          this.LoadingOverLay.Visibility = Visibility.Visible;
          this.OverlayProgress.IsIndeterminate = true;
          this.OpenLoadingOverLay.Begin();
        }
      }
      catch
      {
      }
    }

    private void AccountCheck_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      string msg = (string) e.Result;
      if (msg.Equals("success"))
      {
        App.DataManager.SettingsMan.BaconSyncEnabled = true;
        App.DataManager.SettingsMan.BaconSyncPushURL = "";
        App.DataManager.SettingsMan.BaconSyncStatus = "Waiting for first sync";
        App.DataManager.BaconSyncObj.Bump();
        App.DataManager.BaconSyncObj.StartUpPush();
        this.Dispatcher.BeginInvoke((Action) (() =>
        {
          this.CloseLoadingOverLay.Begin();
          this.NavigationService.Navigate(new Uri("/SettingPages/BaconSync/SyncStatus.xaml", UriKind.Relative));
        }));
      }
      else if (msg.Equals("Failed - Wrong Pass Code"))
      {
        Deployment.Current.Dispatcher.BeginInvoke((Action) (() => Guide.BeginShowMessageBox("Incorrect Pass Code", "The pass code you have entered is incorrect.", (IEnumerable<string>) new string[2]
        {
          "try again",
          "reset"
        }, 0, MessageBoxIcon.None, (AsyncCallback) (resul =>
        {
          int? nullable = Guide.EndShowMessageBox(resul);
          if (!nullable.HasValue)
            return;
          switch (nullable.GetValueOrDefault())
          {
            case 1:
              Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
              {
                try
                {
                  this.NavigationService.Navigate(new Uri("/SettingPages/BaconSync/SetNewPassCode.xaml", UriKind.Relative));
                }
                catch
                {
                }
              }));
              break;
          }
        }), (object) null)));
        this.Dispatcher.BeginInvoke((Action) (() =>
        {
          this.CloseLoadingOverLay.Begin();
          App.DataManager.SettingsMan.BaconSyncEnabled = false;
        }));
      }
      else
        this.Dispatcher.BeginInvoke((Action) (() =>
        {
          int num = (int) MessageBox.Show("Baconit is unable to check your account at this time due to a " + msg + " error.", "Error", MessageBoxButton.OK);
          this.CloseLoadingOverLay.Begin();
          App.DataManager.SettingsMan.BaconSyncEnabled = false;
        }));
    }

    private void DoubleAnimation_Completed(object sender, EventArgs e)
    {
      this.LoadingOverLay.Visibility = Visibility.Collapsed;
      this.OverlayProgress.IsIndeterminate = false;
    }

    private void PassCode_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Enter)
        return;
      this.Focus();
      this.NextPage_Click(sender, (RoutedEventArgs) null);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/SettingPages/BaconSync/BaconSyncAccountInfo.xaml", UriKind.Relative));
      this.OpenLoadingOverLay = (Storyboard) this.FindName("OpenLoadingOverLay");
      this.CloseLoadingOverLay = (Storyboard) this.FindName("CloseLoadingOverLay");
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TitlePanel = (StackPanel) this.FindName("TitlePanel");
      this.ApplicationTitle = (TextBlock) this.FindName("ApplicationTitle");
      this.PageTitle = (TextBlock) this.FindName("PageTitle");
      this.ContentPanel = (StackPanel) this.FindName("ContentPanel");
      this.DeviceName = (TextBox) this.FindName("DeviceName");
      this.PassCode = (TextBox) this.FindName("PassCode");
      this.NextPage = (Button) this.FindName("NextPage");
      this.LoadingOverLay = (Grid) this.FindName("LoadingOverLay");
      this.LoggingInOverlayBack = (Rectangle) this.FindName("LoggingInOverlayBack");
      this.OverlayProgress = (ProgressBar) this.FindName("OverlayProgress");
    }
  }
}
