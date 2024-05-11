// Decompiled with JetBrains decompiler
// Type: Baconit.SettingPages.FlipView
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
  public class FlipView : PhoneApplicationPage
  {
    internal Grid LayoutRoot;
    internal StackPanel TitlePanel;
    internal TextBlock ApplicationTitle;
    internal TextBlock PageTitle;
    internal Grid ContentPanel;
    internal ToggleSwitch PreLoadWebsites;
    internal ToggleSwitch WebOptimize;
    internal ToggleSwitch SwipeVote;
    internal ToggleSwitch NSFWClickThrough;
    internal ToggleSwitch ResumeFlipMode;
    private bool _contentLoaded;

    public FlipView()
    {
      this.InitializeComponent();
      TransitionService.SetNavigationInTransition((UIElement) this, App.FeatherInTransition);
      TransitionService.SetNavigationOutTransition((UIElement) this, App.FeatherOutTransition);
      this.NSFWClickThrough.IsChecked = new bool?(App.DataManager.SettingsMan.NSFWClickThrough);
      this.ResumeFlipMode.IsChecked = new bool?(App.DataManager.SettingsMan.ResumeFlipMode);
      this.PreLoadWebsites.IsChecked = new bool?(App.DataManager.SettingsMan.FlipViewCacheNextWebPage);
      this.WebOptimize.IsChecked = new bool?(App.DataManager.SettingsMan.OptimizeWebByDefault);
      this.SwipeVote.IsChecked = new bool?(App.DataManager.SettingsMan.SwipeToVote);
      App.DataManager.BaconitAnalytics.LogPage("Settings - Flip View");
    }

    private void NSFWClickThrough_Click(object sender, RoutedEventArgs e)
    {
      if (this.NSFWClickThrough == null)
        return;
      App.DataManager.SettingsMan.NSFWClickThrough = this.NSFWClickThrough.IsChecked.Value;
    }

    private void ResumeFlipMode_Click(object sender, RoutedEventArgs e)
    {
      if (this.ResumeFlipMode == null)
        return;
      App.DataManager.SettingsMan.ResumeFlipMode = this.ResumeFlipMode.IsChecked.Value;
    }

    private void PreLoadWebsites_Click_1(object sender, RoutedEventArgs e)
    {
      if (this.PreLoadWebsites == null)
        return;
      App.DataManager.SettingsMan.FlipViewCacheNextWebPage = this.PreLoadWebsites.IsChecked.Value;
    }

    private void SwipeVote_Click_1(object sender, RoutedEventArgs e)
    {
      if (this.SwipeVote == null)
        return;
      App.DataManager.SettingsMan.SwipeToVote = this.SwipeVote.IsChecked.Value;
    }

    private void WebOptimize_Click_1(object sender, RoutedEventArgs e)
    {
      if (this.WebOptimize == null)
        return;
      App.DataManager.SettingsMan.OptimizeWebByDefault = this.WebOptimize.IsChecked.Value;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/SettingPages/FlipView.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TitlePanel = (StackPanel) this.FindName("TitlePanel");
      this.ApplicationTitle = (TextBlock) this.FindName("ApplicationTitle");
      this.PageTitle = (TextBlock) this.FindName("PageTitle");
      this.ContentPanel = (Grid) this.FindName("ContentPanel");
      this.PreLoadWebsites = (ToggleSwitch) this.FindName("PreLoadWebsites");
      this.WebOptimize = (ToggleSwitch) this.FindName("WebOptimize");
      this.SwipeVote = (ToggleSwitch) this.FindName("SwipeVote");
      this.NSFWClickThrough = (ToggleSwitch) this.FindName("NSFWClickThrough");
      this.ResumeFlipMode = (ToggleSwitch) this.FindName("ResumeFlipMode");
    }
  }
}
