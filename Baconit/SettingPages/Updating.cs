// Decompiled with JetBrains decompiler
// Type: Baconit.SettingPages.Updating
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Baconit.Database;
using Baconit.Libs;
using Microsoft.Phone.Controls;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Baconit.SettingPages
{
  public class Updating : PhoneApplicationPage
  {
    private bool ValuesSet;
    internal Grid LayoutRoot;
    internal StackPanel TitlePanel;
    internal TextBlock ApplicationTitle;
    internal TextBlock PageTitle;
    internal Grid ContentPanel;
    internal TextBlock LastUpdateTime;
    internal ToggleSwitch EnableBackgroundUpdates;
    internal ToggleSwitch ShowToastNotifications;
    internal ToggleSwitch OnlyUpdateOnWifi;
    internal TextBlock DoNotDistText;
    internal ToggleSwitch EnableDontDisbutb;
    internal TimePicker DontDistFrom;
    internal TimePicker DontDistTo;
    private bool _contentLoaded;

    public Updating()
    {
      this.InitializeComponent();
      App.DataManager.BaconitAnalytics.LogPage("Settings - Updating");
      if (App.DataManager.SettingsMan.BackgroundAgentEnabled == 1)
      {
        this.EnableBackgroundUpdates.IsChecked = new bool?(true);
      }
      else
      {
        this.ShowToastNotifications.IsEnabled = false;
        this.EnableDontDisbutb.IsEnabled = false;
        this.DontDistFrom.IsEnabled = false;
        this.DontDistTo.IsEnabled = false;
        this.OnlyUpdateOnWifi.IsEnabled = false;
      }
      this.ShowToastNotifications.IsChecked = new bool?(App.DataManager.SettingsMan.ShowTostNotifications);
      this.EnableDontDisbutb.IsChecked = new bool?(App.DataManager.SettingsMan.DoNotDistEnabled);
      if (!App.DataManager.SettingsMan.DoNotDistEnabled)
      {
        this.DontDistFrom.IsEnabled = false;
        this.DontDistTo.IsEnabled = false;
      }
      this.DontDistFrom.Value = new DateTime?(App.DataManager.SettingsMan.DoNotDistFrom);
      this.DontDistTo.Value = new DateTime?(App.DataManager.SettingsMan.DoNotDistTo);
      this.OnlyUpdateOnWifi.IsChecked = new bool?(App.DataManager.SettingsMan.OnlyUpdateOnWifi);
      double time = App.DataManager.BaconitStore.LastUpdatedTime("BackgroundUpdater");
      if (time == 0.0)
      {
        this.LastUpdateTime.Text = "Last Update Time: Never";
      }
      else
      {
        DateTime dateTime = BaconitStore.DoubleToDateTime(time);
        CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
        dateTime.ToString(currentCulture.DateTimeFormat.ShortDatePattern.ToString());
        this.LastUpdateTime.Text = "Last Update Time: " + dateTime.ToString(currentCulture.DateTimeFormat.ShortDatePattern.ToString() + " " + currentCulture.DateTimeFormat.ShortTimePattern.ToString());
      }
      TransitionService.SetNavigationInTransition((UIElement) this, App.FeatherInTransition);
      TransitionService.SetNavigationOutTransition((UIElement) this, App.FeatherOutTransition);
      this.ValuesSet = true;
    }

    private void EnableBackgroundUpdates_Click(object sender, RoutedEventArgs e)
    {
      if (!this.ValuesSet || this.EnableBackgroundUpdates == null)
        return;
      if (this.EnableBackgroundUpdates.IsChecked.Value)
      {
        App.DataManager.SettingsMan.BackgroundAgentEnabled = 1;
        this.ShowToastNotifications.IsEnabled = true;
        this.EnableDontDisbutb.IsEnabled = true;
        this.OnlyUpdateOnWifi.IsEnabled = true;
        if (App.DataManager.SettingsMan.DoNotDistEnabled)
        {
          this.DontDistFrom.IsEnabled = true;
          this.DontDistTo.IsEnabled = true;
          this.DoNotDistText.Foreground = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
        }
      }
      else
      {
        App.DataManager.SettingsMan.BackgroundAgentEnabled = 0;
        this.ShowToastNotifications.IsEnabled = false;
        this.EnableDontDisbutb.IsEnabled = false;
        this.DontDistFrom.IsEnabled = false;
        this.DontDistTo.IsEnabled = false;
        this.OnlyUpdateOnWifi.IsEnabled = false;
      }
      new Thread((ThreadStart) (() => UpdaterMan.UpdateAgents())).Start();
    }

    private void EnableDontDisbutb_Click(object sender, RoutedEventArgs e)
    {
      if (!this.ValuesSet || this.EnableDontDisbutb == null)
        return;
      App.DataManager.SettingsMan.DoNotDistEnabled = this.EnableDontDisbutb.IsChecked.Value;
      this.DontDistFrom.IsEnabled = App.DataManager.SettingsMan.DoNotDistEnabled;
      this.DontDistTo.IsEnabled = App.DataManager.SettingsMan.DoNotDistEnabled;
    }

    private void DontDistFrom_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
    {
      if (!this.ValuesSet || this.DontDistFrom == null)
        return;
      App.DataManager.SettingsMan.DoNotDistFrom = this.DontDistFrom.Value.Value;
    }

    private void DontDistTo_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
    {
      if (!this.ValuesSet || this.DontDistTo == null)
        return;
      App.DataManager.SettingsMan.DoNotDistTo = this.DontDistTo.Value.Value;
    }

    private void ShowToastNotifications_Click(object sender, RoutedEventArgs e)
    {
      if (!this.ValuesSet || this.ShowToastNotifications == null)
        return;
      App.DataManager.SettingsMan.ShowTostNotifications = this.ShowToastNotifications.IsChecked.Value;
    }

    private void OnlyUpdateOnWifi_Click(object sender, RoutedEventArgs e)
    {
      if (!this.ValuesSet || this.OnlyUpdateOnWifi == null)
        return;
      App.DataManager.SettingsMan.OnlyUpdateOnWifi = this.OnlyUpdateOnWifi.IsChecked.Value;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/SettingPages/Updating.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TitlePanel = (StackPanel) this.FindName("TitlePanel");
      this.ApplicationTitle = (TextBlock) this.FindName("ApplicationTitle");
      this.PageTitle = (TextBlock) this.FindName("PageTitle");
      this.ContentPanel = (Grid) this.FindName("ContentPanel");
      this.LastUpdateTime = (TextBlock) this.FindName("LastUpdateTime");
      this.EnableBackgroundUpdates = (ToggleSwitch) this.FindName("EnableBackgroundUpdates");
      this.ShowToastNotifications = (ToggleSwitch) this.FindName("ShowToastNotifications");
      this.OnlyUpdateOnWifi = (ToggleSwitch) this.FindName("OnlyUpdateOnWifi");
      this.DoNotDistText = (TextBlock) this.FindName("DoNotDistText");
      this.EnableDontDisbutb = (ToggleSwitch) this.FindName("EnableDontDisbutb");
      this.DontDistFrom = (TimePicker) this.FindName("DontDistFrom");
      this.DontDistTo = (TimePicker) this.FindName("DontDistTo");
    }
  }
}
