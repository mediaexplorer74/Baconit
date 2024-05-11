// Decompiled with JetBrains decompiler
// Type: Baconit.SecretSettings
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Microsoft.Phone.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Baconit
{
  public class SecretSettings : PhoneApplicationPage, INotifyPropertyChanged
  {
    private bool ValuesSet;
    internal Grid LayoutRoot;
    internal Grid TitlePanel;
    internal TextBlock ApplicationTitle;
    internal ListBox EventLog;
    internal ToggleSwitch EnableLog;
    internal TextBox LogSize;
    internal ToggleSwitch Debugging;
    private bool _contentLoaded;

    public ObservableCollection<EventLogListItem> EventLogItems { get; private set; }

    public SecretSettings()
    {
      this.InitializeComponent();
      this.EventLogItems = new ObservableCollection<EventLogListItem>();
      this.DataContext = (object) this;
      this.Loaded += new RoutedEventHandler(this.SSettings_Loaded);
      App.DataManager.BaconitAnalytics.LogPage("Settings - Secerete Settings");
    }

    private void SSettings_Loaded(object sender, RoutedEventArgs e)
    {
      this.Debugging.IsChecked = new bool?(App.DataManager.SettingsMan.DEBUGGING);
      this.EnableLog.IsChecked = new bool?(App.DataManager.SettingsMan.EnableLogging);
      this.LogSize.Text = string.Empty + (object) App.DataManager.SettingsMan.LogLength;
      this.ValuesSet = true;
    }

    private void Debugging_Click(object sender, RoutedEventArgs e)
    {
      if (this.Debugging == null || !this.ValuesSet)
        return;
      App.DataManager.SettingsMan.DEBUGGING = this.Debugging.IsChecked.Value;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    private void EnableLog_Click_1(object sender, RoutedEventArgs e)
    {
      if (this.EnableLog == null || !this.ValuesSet)
        return;
      App.DataManager.SettingsMan.EnableLogging = this.EnableLog.IsChecked.Value;
    }

    private void LogSize_TextChanged_1(object sender, TextChangedEventArgs e)
    {
      if (this.LogSize == null || !this.ValuesSet)
        return;
      try
      {
        int max = int.Parse(this.LogSize.Text);
        App.DataManager.SettingsMan.LogLength = max;
        App.DataManager.LogMan.SetMax(max);
      }
      catch
      {
      }
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) (obj =>
      {
        Thread.Sleep(10000);
        App.DataManager.MessageManager.ShowAprilError();
      }));
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/SettingPages/SecretSettings.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TitlePanel = (Grid) this.FindName("TitlePanel");
      this.ApplicationTitle = (TextBlock) this.FindName("ApplicationTitle");
      this.EventLog = (ListBox) this.FindName("EventLog");
      this.EnableLog = (ToggleSwitch) this.FindName("EnableLog");
      this.LogSize = (TextBox) this.FindName("LogSize");
      this.Debugging = (ToggleSwitch) this.FindName("Debugging");
    }
  }
}
