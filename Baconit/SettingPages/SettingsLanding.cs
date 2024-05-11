// Decompiled with JetBrains decompiler
// Type: Baconit.SettingPages.SettingsLanding
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Microsoft.Phone.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Baconit.SettingPages
{
  public class SettingsLanding : PhoneApplicationPage
  {
    internal Grid LayoutRoot;
    internal StackPanel TitlePanel;
    internal TextBlock ApplicationTitle;
    internal TextBlock Settings;
    internal Grid ContentPanel;
    internal ListBox ListBox;
    private bool _contentLoaded;

    public SettingsLanding()
    {
      this.InitializeComponent();
      TransitionService.SetNavigationInTransition((UIElement) this, App.FeatherInTransition);
      TransitionService.SetNavigationOutTransition((UIElement) this, App.FeatherOutTransition);
      App.DataManager.BaconitAnalytics.LogPage("Settings - Landing");
    }

    private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      switch (this.ListBox.SelectedIndex)
      {
        case 0:
          this.NavigationService.Navigate(new Uri("/SettingPages/General.xaml", UriKind.Relative));
          break;
        case 1:
          this.NavigationService.Navigate(new Uri("/SettingPages/RedditViewer.xaml", UriKind.Relative));
          break;
        case 2:
          this.NavigationService.Navigate(new Uri("/SettingPages/FlipView.xaml", UriKind.Relative));
          break;
        case 3:
          this.NavigationService.Navigate(new Uri("/SettingPages/CommentView.xaml", UriKind.Relative));
          break;
        case 4:
          this.NavigationService.Navigate(new Uri("/SettingPages/LiveTiles.xaml", UriKind.Relative));
          break;
        case 5:
          this.NavigationService.Navigate(new Uri("/SettingPages/Updating.xaml", UriKind.Relative));
          break;
        case 6:
          this.NavigationService.Navigate(new Uri("/SettingPages/BaconSync/BaconSyncLanding.xaml", UriKind.Relative));
          break;
        case 7:
          this.NavigationService.Navigate(new Uri("/SettingPages/Donate.xaml", UriKind.Relative));
          break;
        case 8:
          this.NavigationService.Navigate(new Uri("/SettingPages/Credits.xaml", UriKind.Relative));
          break;
        case 9:
          this.NavigationService.Navigate(new Uri("/SettingPages/About.xaml", UriKind.Relative));
          break;
      }
      if (this.ListBox.SelectedIndex == -1)
        return;
      this.ListBox.SelectedIndex = -1;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/SettingPages/SettingsLanding.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TitlePanel = (StackPanel) this.FindName("TitlePanel");
      this.ApplicationTitle = (TextBlock) this.FindName("ApplicationTitle");
      this.Settings = (TextBlock) this.FindName("Settings");
      this.ContentPanel = (Grid) this.FindName("ContentPanel");
      this.ListBox = (ListBox) this.FindName("ListBox");
    }
  }
}
