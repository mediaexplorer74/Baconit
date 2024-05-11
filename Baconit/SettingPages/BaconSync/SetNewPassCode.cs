// Decompiled with JetBrains decompiler
// Type: Baconit.SettingPages.BaconSync.SetNewPassCode
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Microsoft.Phone.Controls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;

#nullable disable
namespace Baconit.SettingPages.BaconSync
{
  public class SetNewPassCode : PhoneApplicationPage
  {
    private string TriedPass = "";
    internal Storyboard OpenLoadingOverLay;
    internal Storyboard CloseLoadingOverLay;
    internal Grid LayoutRoot;
    internal StackPanel TitlePanel;
    internal TextBlock ApplicationTitle;
    internal TextBlock PageTitle;
    internal StackPanel ContentPanel;
    internal TextBox PassCode;
    internal Button setButton;
    internal Grid LoadingOverLay;
    internal Rectangle LoggingInOverlayBack;
    internal ProgressBar OverlayProgress;
    private bool _contentLoaded;

    public SetNewPassCode() => this.InitializeComponent();

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      this.OverlayProgress.IsIndeterminate = false;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      this.LoadingOverLay.Visibility = Visibility.Visible;
      this.OpenLoadingOverLay.Begin();
      this.TriedPass = this.PassCode.Text;
      App.DataManager.BaconSyncObj.SetNewPassCode(this.TriedPass, new RunWorkerCompletedEventHandler(this.SetPassword_RunWorkerCompleted));
    }

    private void SetPassword_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      string msg = (string) e.Result;
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        this.CloseLoadingOverLay.Begin();
        if (msg.Equals("success"))
        {
          App.DataManager.SettingsMan.BaconSyncStatus = "Waiting to sync";
          if (App.DataManager.SettingsMan.BaconSyncEnabled)
            App.DataManager.BaconSyncObj.Bump();
          App.DataManager.SettingsMan.BaconSyncPassCode = this.TriedPass;
          if (this.NavigationService.CanGoBack)
          {
            this.NavigationService.GoBack();
          }
          else
          {
            int num = (int) MessageBox.Show("Pass code chagned successfully.");
          }
        }
        else
        {
          int num1 = (int) MessageBox.Show("Your pass code could not be updated at this time do to a " + msg + ". Please try again later.", "Update Error", MessageBoxButton.OK);
        }
      }));
    }

    private void PassCode_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.PassCode.Text.Trim().Equals(""))
        this.setButton.IsEnabled = false;
      else
        this.setButton.IsEnabled = true;
    }

    private void DoubleAnimation_Completed(object sender, EventArgs e)
    {
      this.LoadingOverLay.Visibility = Visibility.Collapsed;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/SettingPages/BaconSync/SetNewPassCode.xaml", UriKind.Relative));
      this.OpenLoadingOverLay = (Storyboard) this.FindName("OpenLoadingOverLay");
      this.CloseLoadingOverLay = (Storyboard) this.FindName("CloseLoadingOverLay");
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TitlePanel = (StackPanel) this.FindName("TitlePanel");
      this.ApplicationTitle = (TextBlock) this.FindName("ApplicationTitle");
      this.PageTitle = (TextBlock) this.FindName("PageTitle");
      this.ContentPanel = (StackPanel) this.FindName("ContentPanel");
      this.PassCode = (TextBox) this.FindName("PassCode");
      this.setButton = (Button) this.FindName("setButton");
      this.LoadingOverLay = (Grid) this.FindName("LoadingOverLay");
      this.LoggingInOverlayBack = (Rectangle) this.FindName("LoggingInOverlayBack");
      this.OverlayProgress = (ProgressBar) this.FindName("OverlayProgress");
    }
  }
}
