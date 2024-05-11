// Decompiled with JetBrains decompiler
// Type: Baconit.SettingPages.BaconSync.BaconSycnInfo
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Microsoft.Phone.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Baconit.SettingPages.BaconSync
{
  public class BaconSycnInfo : PhoneApplicationPage
  {
    internal Grid LayoutRoot;
    internal StackPanel TitlePanel;
    internal TextBlock ApplicationTitle;
    internal TextBlock PageTitle;
    internal Grid ContentPanel;
    private bool _contentLoaded;

    public BaconSycnInfo() => this.InitializeComponent();

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/SettingPages/BaconSync/BaconSyncInfo.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TitlePanel = (StackPanel) this.FindName("TitlePanel");
      this.ApplicationTitle = (TextBlock) this.FindName("ApplicationTitle");
      this.PageTitle = (TextBlock) this.FindName("PageTitle");
      this.ContentPanel = (Grid) this.FindName("ContentPanel");
    }
  }
}
