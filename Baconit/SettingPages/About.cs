// Decompiled with JetBrains decompiler
// Type: Baconit.SettingPages.About
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;

#nullable disable
namespace Baconit.SettingPages
{
  public class About : PhoneApplicationPage
  {
    internal Grid LayoutRoot;
    internal StackPanel TitlePanel;
    internal TextBlock ApplicationTitle;
    internal TextBlock PageTitle;
    internal Grid ContentPanel;
    internal Image image1;
    internal TextBlock textBlock1;
    internal TextBlock VersionString;
    internal TextBlock textBlock3;
    internal HyperlinkButton hyperlinkButton1;
    private bool _contentLoaded;

    public About()
    {
      this.InitializeComponent();
      TransitionService.SetNavigationInTransition((UIElement) this, App.FeatherInTransition);
      TransitionService.SetNavigationOutTransition((UIElement) this, App.FeatherOutTransition);
      this.VersionString.Text = "Build " + XDocument.Load("WMAppManifest.xml").Root.Element((XName) "App").Attribute((XName) "Version").Value;
      App.DataManager.BaconitAnalytics.LogPage("Settings - About");
    }

    private void Rate_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        new MarketplaceReviewTask().Show();
      }
      catch
      {
      }
    }

    private void Subreddit_Click(object sender, RoutedEventArgs e)
    {
      this.NavigationService.Navigate(new Uri("/RedditsViewer.xaml?subredditURL=/r/baconit", UriKind.Relative));
      if (MessageBox.Show("Would you like to subscribe to the Baconit subreddit? The subreddit is great for feature requests, feedback, and discussion.  Press ok and we will subscribe you to the subreddit!", "Subscribe to Baconit", MessageBoxButton.OKCancel) != MessageBoxResult.OK)
        return;
      try
      {
        ThreadPool.QueueUserWorkItem((WaitCallback) (obfj => App.DataManager.SubredditDataManager.SubscribeToBaconit()));
      }
      catch
      {
      }
    }

    private void Twitter_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        new WebBrowserTask()
        {
          Uri = new Uri("https://twitter.com/#!/baconitwp")
        }.Show();
      }
      catch
      {
      }
    }

    private void Facebook_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        new WebBrowserTask()
        {
          Uri = new Uri("https://touch.facebook.com/Baconit")
        }.Show();
      }
      catch
      {
      }
    }

    private void Support_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        new EmailComposeTask()
        {
          To = "quinnd@outlook.com",
          Subject = ("Baconit Support - " + (object) new Random().Next(9999999)),
          Body = ("\n\r\n\r\n\r\n\r\n\rLog:\n\r" + App.DataManager.LogMan.ToString())
        }.Show();
      }
      catch
      {
      }
    }

    private void Donate_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        new WebBrowserTask()
        {
          Uri = new Uri("http://windowsphone.com/s?appId=4a3cc839-5f75-40e5-8b5f-8d09479e7370")
        }.Show();
      }
      catch
      {
      }
    }

    private void hyperlinkButton1_Click(object sender, RoutedEventArgs e)
    {
      new WebBrowserTask()
      {
        Uri = new Uri("http://www.quinndamerell.com")
      }.Show();
    }

    private void image1_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
    {
      if (e.TotalManipulation.Translation.X <= 350.0 || e.TotalManipulation.Translation.Y <= 450.0)
        return;
      this.NavigationService.Navigate(new Uri("/SettingPages/SecretSettings.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/SettingPages/About.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TitlePanel = (StackPanel) this.FindName("TitlePanel");
      this.ApplicationTitle = (TextBlock) this.FindName("ApplicationTitle");
      this.PageTitle = (TextBlock) this.FindName("PageTitle");
      this.ContentPanel = (Grid) this.FindName("ContentPanel");
      this.image1 = (Image) this.FindName("image1");
      this.textBlock1 = (TextBlock) this.FindName("textBlock1");
      this.VersionString = (TextBlock) this.FindName("VersionString");
      this.textBlock3 = (TextBlock) this.FindName("textBlock3");
      this.hyperlinkButton1 = (HyperlinkButton) this.FindName("hyperlinkButton1");
    }
  }
}
