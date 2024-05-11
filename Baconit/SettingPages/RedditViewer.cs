// Decompiled with JetBrains decompiler
// Type: Baconit.SettingPages.RedditViewer
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
  public class RedditViewer : PhoneApplicationPage
  {
    private bool ValuesSet;
    internal Grid LayoutRoot;
    internal StackPanel TitlePanel;
    internal TextBlock ApplicationTitle;
    internal TextBlock PageTitle;
    internal Grid ContentPanel;
    internal ListPicker storyUpdatePicker;
    internal ListPicker OpenSelfTextInto;
    internal ToggleSwitch ShowFullTitle;
    private bool _contentLoaded;

    public RedditViewer()
    {
      this.InitializeComponent();
      TransitionService.SetNavigationInTransition((UIElement) this, App.FeatherInTransition);
      TransitionService.SetNavigationOutTransition((UIElement) this, App.FeatherOutTransition);
      App.DataManager.BaconitAnalytics.LogPage("Settings - Reddit Viewer");
      switch ((int) App.DataManager.SettingsMan.StoryUpdateTime)
      {
        case 0:
          this.storyUpdatePicker.SelectedIndex = 0;
          break;
        case 120000:
          this.storyUpdatePicker.SelectedIndex = 1;
          break;
        case 300000:
          this.storyUpdatePicker.SelectedIndex = 2;
          break;
        case 600000:
          this.storyUpdatePicker.SelectedIndex = 3;
          break;
        case 900000:
          this.storyUpdatePicker.SelectedIndex = 4;
          break;
        case 1200000:
          this.storyUpdatePicker.SelectedIndex = 5;
          break;
        case 1800000:
          this.storyUpdatePicker.SelectedIndex = 6;
          break;
        case 3600000:
          this.storyUpdatePicker.SelectedIndex = 7;
          break;
        case 7200000:
          this.storyUpdatePicker.SelectedIndex = 8;
          break;
        case 18000000:
          this.storyUpdatePicker.SelectedIndex = 9;
          break;
        case 86400000:
          this.storyUpdatePicker.SelectedIndex = 10;
          break;
      }
      this.ShowFullTitle.IsChecked = new bool?(App.DataManager.SettingsMan.ShowWholeTitle);
      this.OpenSelfTextInto.SelectedIndex = !App.DataManager.SettingsMan.OpenSelfTextIntoFlipView ? 1 : 0;
      this.ValuesSet = true;
    }

    private void storyUpdatePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!this.ValuesSet || this.storyUpdatePicker == null || this.storyUpdatePicker.SelectedIndex < 0)
        return;
      switch (this.storyUpdatePicker.SelectedIndex)
      {
        case 0:
          App.DataManager.SettingsMan.StoryUpdateTime = 0.0;
          break;
        case 1:
          App.DataManager.SettingsMan.StoryUpdateTime = 120000.0;
          break;
        case 2:
          App.DataManager.SettingsMan.StoryUpdateTime = 300000.0;
          break;
        case 3:
          App.DataManager.SettingsMan.StoryUpdateTime = 600000.0;
          break;
        case 4:
          App.DataManager.SettingsMan.StoryUpdateTime = 900000.0;
          break;
        case 5:
          App.DataManager.SettingsMan.StoryUpdateTime = 1200000.0;
          break;
        case 6:
          App.DataManager.SettingsMan.StoryUpdateTime = 1800000.0;
          break;
        case 7:
          App.DataManager.SettingsMan.StoryUpdateTime = 3600000.0;
          break;
        case 8:
          App.DataManager.SettingsMan.StoryUpdateTime = 7200000.0;
          break;
        case 9:
          App.DataManager.SettingsMan.StoryUpdateTime = 18000000.0;
          break;
        case 10:
          App.DataManager.SettingsMan.StoryUpdateTime = 86400000.0;
          break;
      }
    }

    private void ShowFullTitle_Click(object sender, RoutedEventArgs e)
    {
      if (!this.ValuesSet || this.ShowFullTitle == null)
        return;
      App.DataManager.SettingsMan.ShowWholeTitle = this.ShowFullTitle.IsChecked.Value;
    }

    private void OpenSelfTextInto_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!this.ValuesSet || this.OpenSelfTextInto == null)
        return;
      if (this.OpenSelfTextInto.SelectedIndex == 0)
        App.DataManager.SettingsMan.OpenSelfTextIntoFlipView = true;
      else
        App.DataManager.SettingsMan.OpenSelfTextIntoFlipView = false;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/SettingPages/RedditViewer.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TitlePanel = (StackPanel) this.FindName("TitlePanel");
      this.ApplicationTitle = (TextBlock) this.FindName("ApplicationTitle");
      this.PageTitle = (TextBlock) this.FindName("PageTitle");
      this.ContentPanel = (Grid) this.FindName("ContentPanel");
      this.storyUpdatePicker = (ListPicker) this.FindName("storyUpdatePicker");
      this.OpenSelfTextInto = (ListPicker) this.FindName("OpenSelfTextInto");
      this.ShowFullTitle = (ToggleSwitch) this.FindName("ShowFullTitle");
    }
  }
}
