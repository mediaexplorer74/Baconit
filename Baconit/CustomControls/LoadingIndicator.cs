// Decompiled with JetBrains decompiler
// Type: Baconit.CustomControls.LoadingIndicator
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace Baconit.CustomControls
{
  public class LoadingIndicator : UserControl
  {
    internal Storyboard FadeInLoading;
    internal Storyboard FadeOutLoading;
    internal Grid LayoutRoot;
    internal ProgressBar ProgressBarUi;
    internal TextBlock Text;
    private bool _contentLoaded;

    public LoadingIndicator()
    {
      this.InitializeComponent();
      if (!DataManager.LIGHT_THEME)
        return;
      this.LayoutRoot.Background = (Brush) new SolidColorBrush(Color.FromArgb((byte) 227, byte.MaxValue, byte.MaxValue, byte.MaxValue));
    }

    public void ShowLoading(bool animated) => this.ShowLoading(animated, "Loading...");

    public void ShowLoading(bool animated, string loadingText)
    {
      this.ProgressBarUi.IsIndeterminate = true;
      this.Text.Text = loadingText;
      this.ProgressBarUi.Visibility = Visibility.Visible;
      this.LayoutRoot.Visibility = Visibility.Visible;
      if (!animated)
        return;
      this.FadeInLoading.Begin();
    }

    public void ShowText(bool animated, string text)
    {
      this.ProgressBarUi.IsIndeterminate = false;
      this.ProgressBarUi.Visibility = Visibility.Collapsed;
      this.Text.Text = text;
      this.LayoutRoot.Visibility = Visibility.Visible;
      if (!animated || this.LayoutRoot.Opacity == 1.0)
        return;
      this.FadeInLoading.Begin();
    }

    public void SetProgress(int progress)
    {
      if (this.ProgressBarUi.IsIndeterminate)
        this.ProgressBarUi.IsIndeterminate = false;
      this.ProgressBarUi.Value = (double) progress;
    }

    public void HideLoading()
    {
      if (this.LayoutRoot.Opacity != 1.0)
        return;
      this.FadeOutLoading.Begin();
    }

    private void FadeoutLoading_Complete(object sender, EventArgs e)
    {
      this.LayoutRoot.Visibility = Visibility.Collapsed;
      this.ProgressBarUi.IsIndeterminate = false;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/CustomControls/LoadingIndicator.xaml", UriKind.Relative));
      this.FadeInLoading = (Storyboard) this.FindName("FadeInLoading");
      this.FadeOutLoading = (Storyboard) this.FindName("FadeOutLoading");
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.ProgressBarUi = (ProgressBar) this.FindName("ProgressBarUi");
      this.Text = (TextBlock) this.FindName("Text");
    }
  }
}
