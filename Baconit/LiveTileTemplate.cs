// Decompiled with JetBrains decompiler
// Type: Baconit.LiveTileTemplate
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Baconit
{
  public class LiveTileTemplate : UserControl
  {
    internal StackPanel LayoutRoot;
    private bool _contentLoaded;

    public LiveTileTemplate() => this.InitializeComponent();

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/LiveTileTemplates/LiveTileTemplate.xaml", UriKind.Relative));
      this.LayoutRoot = (StackPanel) this.FindName("LayoutRoot");
    }
  }
}
