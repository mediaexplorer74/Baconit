// Decompiled with JetBrains decompiler
// Type: Baconit.Error
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Microsoft.Phone.Controls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;

#nullable disable
namespace Baconit
{
  public class Error : PhoneApplicationPage
  {
    private bool backLocked = true;
    internal Grid LayoutRoot;
    internal Image ErrorImage;
    internal Grid FillGrid;
    internal TextBlock GoodbyeText;
    internal TextBlock SystemRestore;
    private bool _contentLoaded;

    public Error()
    {
      this.InitializeComponent();
      this.Loaded += new RoutedEventHandler(this.Error_Loaded);
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      App.DataManager.BaconitAnalytics.LogPage("April Fools Joke");
    }

    protected override void OnBackKeyPress(CancelEventArgs e) => e.Cancel = this.backLocked;

    private void Error_Loaded(object sender, RoutedEventArgs e)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) (obj =>
      {
        Thread.Sleep(50);
        this.Dispatcher.BeginInvoke((Action) (() => this.ErrorImage.Visibility = Visibility.Collapsed));
        Thread.Sleep(420);
        for (int index = 0; index < 2; ++index)
        {
          Thread.Sleep(250);
          this.Dispatcher.BeginInvoke((Action) (() => this.FillGrid.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, byte.MaxValue, (byte) 0))));
          Thread.Sleep(20);
          this.Dispatcher.BeginInvoke((Action) (() => this.FillGrid.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, (byte) 0))));
        }
        Thread.Sleep(300);
        AutoResetEvent are = new AutoResetEvent(false);
        this.Dispatcher.BeginInvoke((Action) (() =>
        {
          int num = (int) MessageBox.Show("Windows Phone has encountered a fatal disk corruption error. Your device will now shut down and attempt a factory restore to recover the corrupt operating system sector. If your device is unable to restore successfully, please contact customer support.\n\nError Code: 0x8d53001d", "System Error", MessageBoxButton.OK);
          are.Set();
        }));
        are.WaitOne();
        Thread.Sleep(200);
        this.Dispatcher.BeginInvoke((Action) (() => this.GoodbyeText.Visibility = Visibility.Visible));
        Thread.Sleep(2500);
        this.Dispatcher.BeginInvoke((Action) (() => this.GoodbyeText.Visibility = Visibility.Collapsed));
        Thread.Sleep(200);
        string[] strArray = new string[28]
        {
          "### SYSTEM RECOVERY ###",
          "CHECKING SYSTEM RAM",
          "OK",
          "CHECKING SYSTEM BUS",
          "OK",
          "CHECKING HEADLIGHT FLUID LEVELS",
          "OK",
          "CHECKING OS PARTITION",
          "**ERRORS FOUND",
          "ATTEMPTING TO RECOVER...",
          "10%",
          "20%",
          "30%",
          "40%",
          "50%",
          "39%",
          "ERRORS",
          "THIS IS BAD",
          "ATTEMPTING NARWHAL MEMORY RECOVERY ALGORITHM",
          "SUCCESS",
          "THIS IS GOOD NEWS EVERYONE",
          "REAPPLYING KITTENS TO SYSTEM IMAGE",
          "SUCCESS",
          "I THINK WE MADE IT",
          "",
          "",
          "HAPPY APRIL FOOLS",
          "PRESS THE BACK BUTTON TO GET BACK TO REDDIT :D"
        };
        Random random = new Random();
        foreach (string str1 in strArray)
        {
          string str = str1;
          this.Dispatcher.BeginInvoke((Action) (() =>
          {
            TextBlock systemRestore = this.SystemRestore;
            systemRestore.Text = systemRestore.Text + Environment.NewLine + str;
          }));
          Thread.Sleep(random.Next(500, 2000) + 1000);
        }
        this.backLocked = false;
      }));
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/Error.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.ErrorImage = (Image) this.FindName("ErrorImage");
      this.FillGrid = (Grid) this.FindName("FillGrid");
      this.GoodbyeText = (TextBlock) this.FindName("GoodbyeText");
      this.SystemRestore = (TextBlock) this.FindName("SystemRestore");
    }
  }
}
