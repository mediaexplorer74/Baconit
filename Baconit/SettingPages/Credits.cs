// Decompiled with JetBrains decompiler
// Type: Baconit.SettingPages.Credits
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Baconit.SettingPages
{
  public class Credits : PhoneApplicationPage
  {
    internal Grid LayoutRoot;
    internal StackPanel TitlePanel;
    internal TextBlock ApplicationTitle;
    internal TextBlock PageTitle;
    internal Grid ContentPanel;
    private bool _contentLoaded;

    public Credits()
    {
      this.InitializeComponent();
      TransitionService.SetNavigationInTransition((UIElement) this, App.FeatherInTransition);
      TransitionService.SetNavigationOutTransition((UIElement) this, App.FeatherOutTransition);
      App.DataManager.BaconitAnalytics.LogPage("Settings - Credits");
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        new WebBrowserTask()
        {
          Uri = new Uri("http://silverlight.codeplex.com/")
        }.Show();
      }
      catch
      {
      }
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
      try
      {
        new WebBrowserTask()
        {
          Uri = new Uri("http://json.codeplex.com/")
        }.Show();
      }
      catch
      {
      }
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
      try
      {
        new WebBrowserTask()
        {
          Uri = new Uri("http://imagetools.codeplex.com/")
        }.Show();
      }
      catch
      {
      }
    }

    private void Button_Click_3(object sender, RoutedEventArgs e)
    {
      try
      {
        new WebBrowserTask()
        {
          Uri = new Uri("http://blogs.msdn.com/b/delay/archive/2012/04/19/quot-if-i-have-seen-further-it-is-by-standing-on-the-shoulders-of-giants-quot-an-alternate-implementation-of-http-gzip-decompression-for-windows-phone.aspx")
        }.Show();
      }
      catch
      {
      }
    }

    private void Button_Click_4(object sender, RoutedEventArgs e)
    {
      try
      {
        new WebBrowserTask()
        {
          Uri = new Uri("http://www.readability.com/")
        }.Show();
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
      Application.LoadComponent((object) this, new Uri("/Baconit;component/SettingPages/Credits.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TitlePanel = (StackPanel) this.FindName("TitlePanel");
      this.ApplicationTitle = (TextBlock) this.FindName("ApplicationTitle");
      this.PageTitle = (TextBlock) this.FindName("PageTitle");
      this.ContentPanel = (Grid) this.FindName("ContentPanel");
    }
  }
}
