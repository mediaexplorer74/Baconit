// Decompiled with JetBrains decompiler
// Type: Baconit.CustomControls.GIFViewer
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Baconit.CustomControls
{
  public class GIFViewer : UserControl
  {
    private TimeSpan PositionZero = new TimeSpan(0L);
    private GIFViewer.GIFInformation CurrentGIF;
    internal Grid LayoutRoot;
    internal Grid VideoRoot;
    internal Grid PlayButton;
    internal LoadingIndicator LoadingUi;
    private bool _contentLoaded;

    public GIFViewer() => this.InitializeComponent();

    public void PlayVideo(string url)
    {
      this.LoadingUi.ShowLoading(false, "Loading Animation...");
      this.StopVideo();
      this.CurrentGIF = new GIFViewer.GIFInformation();
      this.CurrentGIF.StartLoadingTime = DateTime.Now;
      this.CurrentGIF.Url = url;
      try
      {
        App.DataManager.GFYCatGif(url, new RunWorkerCompletedEventHandler(this.GFYCatGif_Callback));
      }
      catch (Exception ex)
      {
        App.DataManager.DebugDia("Failed to query GFYCat", ex);
        this.CurrentElement_MediaFailed((object) null, (ExceptionRoutedEventArgs) null);
      }
    }

    public void GFYCatGif_Callback(object url, RunWorkerCompletedEventArgs result)
    {
      if (this.CurrentGIF == null || !this.CurrentGIF.Url.Equals((string) url))
        return;
      if (result.Result != null)
      {
        this.CurrentGIF.VideoUrl = (string) result.Result;
        this.Dispatcher.BeginInvoke((Action) (() =>
        {
          try
          {
            if (MediaPlayer.State != MediaState.Stopped)
            {
              this.CurrentGIF.isWaitingForButton = true;
              this.PlayButton.Visibility = Visibility.Visible;
              this.LoadingUi.HideLoading();
            }
            else
              this.SetupVideoAndPlay();
          }
          catch
          {
            this.CurrentElement_MediaFailed((object) null, (ExceptionRoutedEventArgs) null);
          }
        }));
      }
      else
        this.CurrentElement_MediaFailed((object) null, (ExceptionRoutedEventArgs) null);
    }

    public void SetupVideoAndPlay()
    {
      this.CurrentGIF.Element = new MediaElement();
      this.CurrentGIF.Element.Volume = 0.0;
      this.CurrentGIF.Element.IsMuted = true;
      this.CurrentGIF.Element.CurrentStateChanged += new RoutedEventHandler(this.CurrentElement_CurrentStateChanged);
      this.CurrentGIF.Element.MediaEnded += new RoutedEventHandler(this.CurrentElement_MediaEnded);
      this.CurrentGIF.Element.MediaFailed += new EventHandler<ExceptionRoutedEventArgs>(this.CurrentElement_MediaFailed);
      this.CurrentGIF.Element.BufferingProgressChanged += new RoutedEventHandler(this.CurrentElement_BufferingProgressChanged);
      this.CurrentGIF.Element.Source = new Uri(this.CurrentGIF.VideoUrl, UriKind.Absolute);
      this.VideoRoot.Children.Add((UIElement) this.CurrentGIF.Element);
      FrameworkDispatcher.Update();
      this.CurrentGIF.Element.Play();
    }

    private void CurrentElement_BufferingProgressChanged(object sender, RoutedEventArgs e)
    {
      MediaElement mediaElement = (MediaElement) sender;
      if (this.CurrentGIF == null || this.CurrentGIF.Shown || DateTime.Now.Subtract(this.CurrentGIF.StartLoadingTime).TotalSeconds <= 3.0)
        return;
      this.LoadingUi.SetProgress((int) Math.Floor(mediaElement.BufferingProgress * 100.0));
    }

    private void CurrentElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
    {
      this.Dispatcher.BeginInvoke((Action) (() => this.LoadingUi.ShowText(false, "Failed to Load Animation")));
    }

    private void CurrentElement_CurrentStateChanged(object sender, RoutedEventArgs e)
    {
      MediaElement mediaElement = (MediaElement) sender;
      if (mediaElement.CurrentState != MediaElementState.Playing && mediaElement.CurrentState != MediaElementState.AcquiringLicense && mediaElement.CurrentState != MediaElementState.Buffering && mediaElement.CurrentState != MediaElementState.Opening && mediaElement.CurrentState != MediaElementState.Individualizing)
        return;
      if (this.CurrentGIF != null)
        this.CurrentGIF.Shown = true;
      this.LoadingUi.HideLoading();
    }

    private void CurrentElement_MediaEnded(object sender, RoutedEventArgs e)
    {
      MediaElement mediaElement = (MediaElement) sender;
      mediaElement.Position = this.PositionZero;
      mediaElement.Play();
    }

    public void StopVideo()
    {
      GIFViewer.GIFInformation tempInfo = this.CurrentGIF;
      this.CurrentGIF = (GIFViewer.GIFInformation) null;
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        if (tempInfo != null && tempInfo.Element != null)
        {
          tempInfo.Element.CurrentStateChanged -= new RoutedEventHandler(this.CurrentElement_CurrentStateChanged);
          tempInfo.Element.MediaEnded -= new RoutedEventHandler(this.CurrentElement_MediaEnded);
          tempInfo.Element.MediaFailed -= new EventHandler<ExceptionRoutedEventArgs>(this.CurrentElement_MediaFailed);
          tempInfo.Element.BufferingProgressChanged -= new RoutedEventHandler(this.CurrentElement_BufferingProgressChanged);
          tempInfo.Element.Stop();
        }
        this.VideoRoot.Children.Clear();
      }));
    }

    public void PauseVideo()
    {
      if (this.CurrentGIF == null || this.CurrentGIF.Element == null)
        return;
      this.CurrentGIF.Element.Pause();
    }

    public void PlayVideo()
    {
      if (this.CurrentGIF == null || this.CurrentGIF.Element == null)
        return;
      if (this.CurrentGIF.Element.CurrentState == MediaElementState.Closed)
        this.GFYCatGif_Callback((object) this.CurrentGIF.Url, new RunWorkerCompletedEventArgs((object) this.CurrentGIF.VideoUrl, (Exception) null, false));
      else
        this.CurrentGIF.Element.Play();
    }

    private void PlayButton_Tap(object sender, GestureEventArgs e)
    {
      this.CurrentGIF.isWaitingForButton = false;
      this.PlayButton.Visibility = Visibility.Collapsed;
      this.LoadingUi.ShowLoading(true);
      this.SetupVideoAndPlay();
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/CustomControls/GIFViewer.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.VideoRoot = (Grid) this.FindName("VideoRoot");
      this.PlayButton = (Grid) this.FindName("PlayButton");
      this.LoadingUi = (LoadingIndicator) this.FindName("LoadingUi");
    }

    private class GIFInformation
    {
      public MediaElement Element;
      public DateTime StartLoadingTime;
      public string Url;
      public string VideoUrl;
      public bool Shown;
      public bool isWaitingForButton;
    }
  }
}
