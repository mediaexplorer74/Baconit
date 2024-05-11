// Decompiled with JetBrains decompiler
// Type: Baconit.SettingPages.CommentView
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
  public class CommentView : PhoneApplicationPage
  {
    private bool ValuesSet;
    internal Grid LayoutRoot;
    internal StackPanel TitlePanel;
    internal TextBlock ApplicationTitle;
    internal TextBlock PageTitle;
    internal Grid ContentPanel;
    internal ListPicker commentUpdatePicker;
    internal ToggleSwitch ShrinkStoryHeaderOnScroll;
    internal ToggleSwitch ShowSelfText;
    private bool _contentLoaded;

    public CommentView()
    {
      this.InitializeComponent();
      TransitionService.SetNavigationInTransition((UIElement) this, App.FeatherInTransition);
      TransitionService.SetNavigationOutTransition((UIElement) this, App.FeatherOutTransition);
      App.DataManager.BaconitAnalytics.LogPage("Settings - Comment View");
      switch ((int) App.DataManager.SettingsMan.CommentUpdateTime)
      {
        case 0:
          this.commentUpdatePicker.SelectedIndex = 0;
          break;
        case 120000:
          this.commentUpdatePicker.SelectedIndex = 1;
          break;
        case 300000:
          this.commentUpdatePicker.SelectedIndex = 2;
          break;
        case 600000:
          this.commentUpdatePicker.SelectedIndex = 3;
          break;
        case 900000:
          this.commentUpdatePicker.SelectedIndex = 4;
          break;
        case 1200000:
          this.commentUpdatePicker.SelectedIndex = 5;
          break;
        case 1800000:
          this.commentUpdatePicker.SelectedIndex = 6;
          break;
        case 3600000:
          this.commentUpdatePicker.SelectedIndex = 7;
          break;
        case 7200000:
          this.commentUpdatePicker.SelectedIndex = 8;
          break;
        case 18000000:
          this.commentUpdatePicker.SelectedIndex = 9;
          break;
        case 86400000:
          this.commentUpdatePicker.SelectedIndex = 10;
          break;
      }
      this.ShowSelfText.IsChecked = new bool?(App.DataManager.SettingsMan.ShowExpandedSelfText);
      this.ShrinkStoryHeaderOnScroll.IsChecked = new bool?(App.DataManager.SettingsMan.ShrinkStoryHeaderOnScroll);
      this.ValuesSet = true;
    }

    private void ShowSelfText_Click(object sender, RoutedEventArgs e)
    {
      if (!this.ValuesSet || this.ShowSelfText == null)
        return;
      App.DataManager.SettingsMan.ShowExpandedSelfText = this.ShowSelfText.IsChecked.Value;
    }

    private void commentUpdatePicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (!this.ValuesSet || this.commentUpdatePicker == null || this.commentUpdatePicker.SelectedIndex < 0)
        return;
      switch (this.commentUpdatePicker.SelectedIndex)
      {
        case 0:
          App.DataManager.SettingsMan.CommentUpdateTime = 0.0;
          break;
        case 1:
          App.DataManager.SettingsMan.CommentUpdateTime = 120000.0;
          break;
        case 2:
          App.DataManager.SettingsMan.CommentUpdateTime = 300000.0;
          break;
        case 3:
          App.DataManager.SettingsMan.CommentUpdateTime = 600000.0;
          break;
        case 4:
          App.DataManager.SettingsMan.CommentUpdateTime = 900000.0;
          break;
        case 5:
          App.DataManager.SettingsMan.CommentUpdateTime = 1200000.0;
          break;
        case 6:
          App.DataManager.SettingsMan.CommentUpdateTime = 1800000.0;
          break;
        case 7:
          App.DataManager.SettingsMan.CommentUpdateTime = 3600000.0;
          break;
        case 8:
          App.DataManager.SettingsMan.CommentUpdateTime = 7200000.0;
          break;
        case 9:
          App.DataManager.SettingsMan.CommentUpdateTime = 18000000.0;
          break;
        case 10:
          App.DataManager.SettingsMan.CommentUpdateTime = 86400000.0;
          break;
      }
    }

    private void ShrinkStoryHeaderOnScroll_Click(object sender, RoutedEventArgs e)
    {
      if (!this.ValuesSet || this.ShrinkStoryHeaderOnScroll == null)
        return;
      App.DataManager.SettingsMan.ShrinkStoryHeaderOnScroll = this.ShrinkStoryHeaderOnScroll.IsChecked.Value;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/SettingPages/CommentView.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TitlePanel = (StackPanel) this.FindName("TitlePanel");
      this.ApplicationTitle = (TextBlock) this.FindName("ApplicationTitle");
      this.PageTitle = (TextBlock) this.FindName("PageTitle");
      this.ContentPanel = (Grid) this.FindName("ContentPanel");
      this.commentUpdatePicker = (ListPicker) this.FindName("commentUpdatePicker");
      this.ShrinkStoryHeaderOnScroll = (ToggleSwitch) this.FindName("ShrinkStoryHeaderOnScroll");
      this.ShowSelfText = (ToggleSwitch) this.FindName("ShowSelfText");
    }
  }
}
