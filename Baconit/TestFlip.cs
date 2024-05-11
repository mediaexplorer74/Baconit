// Decompiled with JetBrains decompiler
// Type: Baconit.TestFlip
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Microsoft.Phone.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#nullable disable
namespace Baconit
{
  public class TestFlip : PhoneApplicationPage
  {
    private double y = -800.0;
    private double y1;
    private double y2 = 800.0;
    internal Grid LayoutRoot;
    internal Grid TestScroll;
    internal Grid TestScroll1;
    internal Grid TestScroll2;
    private bool _contentLoaded;

    public TestFlip() => this.InitializeComponent();

    private void Grid_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
    {
      this.y += e.DeltaManipulation.Translation.Y;
      this.y1 += e.DeltaManipulation.Translation.Y;
      this.y2 += e.DeltaManipulation.Translation.Y;
      this.TestScroll.Margin = new Thickness(0.0, (double) (int) this.y, 0.0, 0.0);
      this.TestScroll1.Margin = new Thickness(0.0, (double) (int) this.y1, 0.0, 0.0);
      this.TestScroll2.Margin = new Thickness(0.0, (double) (int) this.y2, 0.0, 0.0);
    }

    private void TestScroll_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
    {
      if (this.y < -800.0)
        this.y += 1600.0;
      if (this.y1 < -800.0)
        this.y1 += 1600.0;
      if (this.y2 < -800.0)
        this.y2 += 1600.0;
      if (this.y > 800.0)
        this.y -= 1600.0;
      if (this.y1 > 800.0)
        this.y1 -= 1600.0;
      if (this.y2 <= 800.0)
        return;
      this.y2 -= 1600.0;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/TestFlip.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TestScroll = (Grid) this.FindName("TestScroll");
      this.TestScroll1 = (Grid) this.FindName("TestScroll1");
      this.TestScroll2 = (Grid) this.FindName("TestScroll2");
    }
  }
}
