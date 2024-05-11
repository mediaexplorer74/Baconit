// Decompiled with JetBrains decompiler
// Type: Baconit.InAppWebBrowser
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

#nullable disable
namespace Baconit
{
  public class InAppWebBrowser : PhoneApplicationPage
  {
    private bool isFirstTime = true;
    private string BaseUrl;
    private bool IngoreNav = true;
    private int backCount;
    private int forwardCount;
    private bool RestoreLoadingBar;
    internal Grid LayoutRoot;
    internal WebBrowser WebControl;
    internal ProgressBar LoadingControl;
    private bool _contentLoaded;

    public InAppWebBrowser()
    {
      this.InitializeComponent();
      TransitionService.SetNavigationInTransition((UIElement) this, App.SlideInTransistion);
      TransitionService.SetNavigationOutTransition((UIElement) this, App.SlideOutTransistion);
      this.ApplicationBar = (IApplicationBar) new Microsoft.Phone.Shell.ApplicationBar();
      this.ApplicationBar.IsVisible = true;
      ApplicationBarIconButton applicationBarIconButton1 = new ApplicationBarIconButton(new Uri("/Images/back.png", UriKind.Relative));
      applicationBarIconButton1.Text = "back";
      applicationBarIconButton1.Click += new EventHandler(this.back_Click);
      applicationBarIconButton1.IsEnabled = false;
      this.ApplicationBar.Buttons.Add((object) applicationBarIconButton1);
      ApplicationBarIconButton applicationBarIconButton2 = new ApplicationBarIconButton(new Uri("/Images/appbar.door.leave.png", UriKind.Relative));
      applicationBarIconButton2.Text = "open in ie";
      applicationBarIconButton2.Click += new EventHandler(this.openInIE_Click);
      this.ApplicationBar.Buttons.Add((object) applicationBarIconButton2);
      ApplicationBarIconButton applicationBarIconButton3 = new ApplicationBarIconButton(new Uri("/Images/appbar.cogs.png", UriKind.Relative));
      applicationBarIconButton3.Text = "optimize";
      applicationBarIconButton3.Click += new EventHandler(this.optimize_Click);
      this.ApplicationBar.Buttons.Add((object) applicationBarIconButton3);
      ApplicationBarIconButton applicationBarIconButton4 = new ApplicationBarIconButton(new Uri("/Images/next.png", UriKind.Relative));
      applicationBarIconButton4.Text = "forward";
      applicationBarIconButton4.Click += new EventHandler(this.forward_Click);
      applicationBarIconButton4.IsEnabled = false;
      this.ApplicationBar.Buttons.Add((object) applicationBarIconButton4);
      this.ApplicationBar.StateChanged += (EventHandler<ApplicationBarStateChangedEventArgs>) ((obj, e) =>
      {
        if (e.IsMenuVisible)
        {
          SystemTray.IsVisible = true;
          SystemTray.Opacity = 0.8;
        }
        else
        {
          SystemTray.IsVisible = false;
          SystemTray.Opacity = 0.0;
        }
      });
      this.WebControl.Navigated += new EventHandler<NavigationEventArgs>(this.WebControl_Navigated);
      this.WebControl.Navigating += new EventHandler<NavigatingEventArgs>(this.WebControl_Navigating);
    }

    private void WebControl_Navigating(object sender, NavigatingEventArgs e)
    {
      this.LoadingControl.IsIndeterminate = true;
      this.LoadingControl.Visibility = Visibility.Visible;
    }

    private void WebControl_Navigated(object sender, NavigationEventArgs e)
    {
      if (!this.IngoreNav)
      {
        ++this.backCount;
        this.forwardCount = 0;
        ((ApplicationBarIconButton) this.ApplicationBar.Buttons[3]).IsEnabled = false;
      }
      this.IngoreNav = false;
      if (e.Uri.OriginalString.Equals(this.BaseUrl))
        this.backCount = 0;
      if (this.backCount > 0)
        ((ApplicationBarIconButton) this.ApplicationBar.Buttons[0]).IsEnabled = true;
      this.LoadingControl.IsIndeterminate = false;
      this.LoadingControl.Visibility = Visibility.Collapsed;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      App.DataManager.BaconitAnalytics.LogPage("In App Web Browser");
      if (this.isFirstTime)
      {
        this.isFirstTime = false;
        IDictionary<string, string> queryString = this.NavigationContext.QueryString;
        if (queryString.ContainsKey("Url"))
          this.BaseUrl = queryString["Url"];
        if (this.BaseUrl.Contains("youtube.com") || this.BaseUrl.Contains("youtu.be/"))
        {
          if (this.BaseUrl.Contains("https://"))
            this.BaseUrl = this.BaseUrl.Replace("https://", "http://");
          this.WebControl.Navigate(new Uri(this.BaseUrl, UriKind.Absolute), (byte[]) null, "User-Agent: User-Agent: Mozilla/5.0 (Windows Phone 8.1; ARM; Trident/7.0; Touch; rv:11.0; IEMobile/11.0; NOKIA; 909) like Gecko");
        }
        else
          this.WebControl.Navigate(new Uri(this.BaseUrl, UriKind.Absolute));
        App.DataManager.BaconitAnalytics.LogEvent("InAppBrowser - Used for link");
        if (App.navService == null)
          App.navService = this.NavigationService;
      }
      if (this.RestoreLoadingBar)
        this.LoadingControl.IsIndeterminate = true;
      this.RestoreLoadingBar = false;
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      if (this.LoadingControl.IsIndeterminate)
        this.RestoreLoadingBar = true;
      this.LoadingControl.IsIndeterminate = false;
    }

    private void back_Click(object sender, EventArgs e)
    {
      try
      {
        this.IngoreNav = true;
        this.WebControl.InvokeScript("eval", "history.go(-1)");
        --this.backCount;
        ++this.forwardCount;
        if (this.backCount <= 0)
        {
          this.backCount = 0;
          ((ApplicationBarIconButton) this.ApplicationBar.Buttons[0]).IsEnabled = false;
        }
        if (this.forwardCount <= 0)
          return;
        ((ApplicationBarIconButton) this.ApplicationBar.Buttons[3]).IsEnabled = true;
      }
      catch
      {
      }
    }

    private void forward_Click(object sender, EventArgs e)
    {
      try
      {
        this.WebControl.InvokeScript("eval", "history.go(1)");
        --this.forwardCount;
        ++this.backCount;
        if (this.forwardCount <= 0)
        {
          this.forwardCount = 0;
          ((ApplicationBarIconButton) this.ApplicationBar.Buttons[3]).IsEnabled = false;
        }
        if (this.backCount <= 0)
          return;
        ((ApplicationBarIconButton) this.ApplicationBar.Buttons[0]).IsEnabled = true;
      }
      catch
      {
      }
    }

    private void openInIE_Click(object sender, EventArgs e)
    {
      WebBrowserTask webBrowserTask = new WebBrowserTask();
      webBrowserTask.Uri = new Uri(this.BaseUrl, UriKind.Absolute);
      try
      {
        webBrowserTask.Show();
      }
      catch
      {
      }
    }

    private void optimize_Click(object sender, EventArgs e)
    {
      try
      {
        this.WebControl.Navigate(new Uri("http://www.readability.com/m?url=" + HttpUtility.UrlEncode((string) this.WebControl.InvokeScript("eval", "document.URL")), UriKind.Absolute));
        App.DataManager.BaconitAnalytics.LogEvent("InAppBrowser - Optimize");
      }
      catch
      {
      }
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/InAppWebBrowser.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.WebControl = (WebBrowser) this.FindName("WebControl");
      this.LoadingControl = (ProgressBar) this.FindName("LoadingControl");
    }
  }
}
