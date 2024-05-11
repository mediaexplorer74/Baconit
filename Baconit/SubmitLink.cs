// Decompiled with JetBrains decompiler
// Type: Baconit.SubmitLink
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Baconit.Database;
using BaconitData.Database;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.GamerServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

#nullable disable
namespace Baconit
{
  public class SubmitLink : PhoneApplicationPage
  {
    public static SubmitLink me;
    public static bool isOpen = false;
    public static BitmapImage SelectedImage = (BitmapImage) null;
    private WordCompletions wordCom;
    private PhotoChooserTask photoTask;
    private bool hasTitle;
    private bool hasUrl;
    private bool WaringingTitleLen;
    private bool WarningSelfTextLen;
    private bool isSelf;
    private bool FormattingHelpOpen;
    private bool RestoreProgressBar;
    private string PostTitle;
    private string PostUrlOrSelf;
    private bool PostIsSelf;
    private string PostSubReddit;
    private string ShowSubreddit = "";
    private bool SaveInfo = true;
    private bool PictureIngore;
    private bool isOpening = true;
    public string ImageURL = "";
    public static string PostURL = "";
    internal Storyboard CloseHelpAnmi;
    internal Storyboard OpenHelpAnmi;
    internal Storyboard OpenWorkingOverlayAn;
    internal Storyboard CloseWorkingOverlayAn;
    internal Grid LayoutRoot;
    internal StackPanel TitlePanel;
    internal TextBlock ApplicationTitle;
    internal ScrollViewer ContentScroller;
    internal StackPanel ContentPanel;
    internal TextBox TitleTextBox;
    internal TextBlock UrlDisplayText;
    internal TextBox UrlTextBox;
    internal CheckBox isSelfText;
    internal TextBlock FormattedPreviewText;
    internal SuperRichTextBox FormattedPreview;
    internal ListPicker SubRedditPicker;
    internal Grid WorkingOverlay;
    internal TextBlock OverlayText;
    internal ProgressBar OverlayProgress;
    internal StackPanel FormattingHelp;
    internal Grid CloseFormatting;
    internal ApplicationBarIconButton SubmitLinkButton;
    internal ApplicationBarIconButton AddPicture;
    internal ApplicationBarMenuItem ShowFormatting;
    private bool _contentLoaded;

    public SubmitLink()
    {
      this.InitializeComponent();
      SubmitLink.me = this;
      TransitionService.SetNavigationInTransition((UIElement) this, App.SlideInTransistion);
      TransitionService.SetNavigationOutTransition((UIElement) this, App.SlideOutTransistion);
      this.ApplicationBar = (IApplicationBar) new Microsoft.Phone.Shell.ApplicationBar();
      this.ApplicationBar.IsVisible = true;
      ApplicationBarIconButton applicationBarIconButton1 = new ApplicationBarIconButton(new Uri("/Images/appbar.send.png", UriKind.Relative));
      applicationBarIconButton1.Text = "submit link";
      applicationBarIconButton1.IsEnabled = false;
      applicationBarIconButton1.Click += new EventHandler(this.SubmitLinkButton_Click);
      this.ApplicationBar.Buttons.Add((object) applicationBarIconButton1);
      ApplicationBarIconButton applicationBarIconButton2 = new ApplicationBarIconButton(new Uri("/Images/appbar.addImage.png", UriKind.Relative));
      applicationBarIconButton2.Text = "add picture";
      applicationBarIconButton2.Click += new EventHandler(this.AddPicture_Click);
      this.ApplicationBar.Buttons.Add((object) applicationBarIconButton2);
      ApplicationBarMenuItem applicationBarMenuItem = new ApplicationBarMenuItem("show formatting help");
      applicationBarMenuItem.Click += new EventHandler(this.ShowFormatting_Click);
      this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem);
      ThreadPool.QueueUserWorkItem((WaitCallback) (obfj => this.LoadSubReddits()));
      this.photoTask = new PhotoChooserTask();
      this.photoTask.ShowCamera = true;
      this.photoTask.Completed += new EventHandler<PhotoResult>(this.photoChooserTask_Completed);
      this.CloseWorkingOverlayAn.Completed += new EventHandler(this.HideWorkingComplete);
      this.CloseHelpAnmi.Completed += (EventHandler) ((sender, e) => this.FormattingHelp.Visibility = Visibility.Collapsed);
      if (DataManager.LIGHT_THEME)
      {
        this.FormattingHelp.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
        this.FormattedPreview.Foreground = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, (byte) 0));
      }
      this.Loaded += (RoutedEventHandler) ((sender, e) => this.isOpening = false);
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      base.OnNavigatedFrom(e);
      if (this.SaveInfo && this.UrlTextBox.IsEnabled && !this.PictureIngore)
      {
        App.DataManager.SettingsMan.SubmitLinkText = this.TitleTextBox.Text;
        App.DataManager.SettingsMan.SubmitLinkURL = this.UrlTextBox.Text;
        App.DataManager.SettingsMan.SubmitLinkSubreddit = (string) this.SubRedditPicker.SelectedItem;
        App.DataManager.SettingsMan.SubmitLinkIsSelfText = this.isSelfText.IsChecked.Value;
      }
      else
      {
        App.DataManager.SettingsMan.SubmitLinkText = "";
        App.DataManager.SettingsMan.SubmitLinkURL = "";
        App.DataManager.SettingsMan.SubmitLinkSubreddit = "";
        App.DataManager.SettingsMan.SubmitLinkIsSelfText = false;
      }
      if (this.OverlayProgress.IsIndeterminate)
        this.RestoreProgressBar = true;
      this.OverlayProgress.IsIndeterminate = false;
      SubmitLink.isOpen = false;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      SubmitLink.isOpen = true;
      App.DataManager.BaconitAnalytics.LogPage("Submit Link");
      if (App.navService == null)
        App.navService = this.NavigationService;
      string str;
      if (this.NavigationContext.QueryString.TryGetValue("ImageAlreadyReady", out str))
      {
        this.UrlTextBox.IsEnabled = false;
        this.UrlTextBox.Text = "Share Image";
        this.isSelfText.Content = (object) "scale image";
        this.isSelfText.IsChecked = new bool?(false);
        this.UrlDisplayText.Text = "image";
        this.UrlTextBox.TextWrapping = TextWrapping.NoWrap;
        this.UrlTextBox.AcceptsReturn = false;
        this.hasUrl = true;
        ((ApplicationBarIconButton) this.ApplicationBar.Buttons[1]).IsEnabled = false;
      }
      if (this.UrlTextBox.IsEnabled)
      {
        this.TitleTextBox.Text = App.DataManager.SettingsMan.SubmitLinkText;
        this.UrlTextBox.Text = App.DataManager.SettingsMan.SubmitLinkURL;
        this.ShowSubreddit = App.DataManager.SettingsMan.SubmitLinkSubreddit;
        this.isSelfText.IsChecked = new bool?(App.DataManager.SettingsMan.SubmitLinkIsSelfText);
        this.isSelfText_Click((object) null, (RoutedEventArgs) null);
        if (this.TitleTextBox.Text != null && !this.TitleTextBox.Text.Equals(""))
          this.hasTitle = true;
        if (this.UrlTextBox.Text != null && !this.UrlTextBox.Text.Equals(""))
          this.hasUrl = true;
        if (this.hasUrl && this.hasTitle)
          ((ApplicationBarIconButton) this.ApplicationBar.Buttons[0]).IsEnabled = true;
      }
      if (this.NavigationContext.QueryString.TryGetValue("RemoveBack", out str))
        this.NavigationService.RemoveBackEntry();
      if (this.NavigationContext.QueryString.ContainsKey("Subreddit"))
        this.ShowSubreddit = this.NavigationContext.QueryString["Subreddit"];
      if (this.RestoreProgressBar)
        this.OverlayProgress.IsIndeterminate = true;
      this.RestoreProgressBar = false;
    }

    protected override void OnBackKeyPress(CancelEventArgs e)
    {
      if (this.isOpening)
      {
        e.Cancel = true;
      }
      else
      {
        base.OnBackKeyPress(e);
        if (this.FormattingHelpOpen)
        {
          this.CloseFormatting_Tap((object) null, (System.Windows.Input.GestureEventArgs) null);
          e.Cancel = true;
        }
        else if ((!this.UrlTextBox.Text.Equals("") || !this.TitleTextBox.Text.Equals("")) && this.UrlTextBox.IsEnabled)
        {
          using (AutoResetEvent are = new AutoResetEvent(false))
          {
            Guide.BeginShowMessageBox("Save or Delete Draft", "Would you like to save this post as a draft or delete it? Saving as a draft will allow you to continue the post later.", (IEnumerable<string>) new string[2]
            {
              "save",
              "delete"
            }, 0, MessageBoxIcon.None, (AsyncCallback) (resul =>
            {
              int? nullable = Guide.EndShowMessageBox(resul);
              if (nullable.HasValue)
              {
                switch (nullable.GetValueOrDefault())
                {
                  case 0:
                    this.SaveInfo = true;
                    break;
                  case 1:
                    this.SaveInfo = false;
                    break;
                }
              }
              are.Set();
            }), (object) null);
            are.WaitOne();
          }
        }
        else
          this.SaveInfo = false;
      }
    }

    private void HideWorkingComplete(object sender, EventArgs e)
    {
      this.WorkingOverlay.Visibility = Visibility.Collapsed;
      this.OverlayProgress.IsIndeterminate = false;
    }

    public void LoadSubReddits()
    {
      this.wordCom = new WordCompletions();
      Deployment.Current.Dispatcher.BeginInvoke(new Action(this.setSubReddits));
    }

    public void setSubReddits()
    {
      this.SubRedditPicker.ItemsSource = this.wordCom.AutoCompletions;
      if (this.ShowSubreddit == null || this.ShowSubreddit.Equals(""))
        return;
      int num = -1;
      foreach (string autoCompletion in this.wordCom.AutoCompletions)
      {
        ++num;
        string showSubreddit = this.ShowSubreddit;
        if (autoCompletion.Equals(showSubreddit))
          break;
      }
      this.SubRedditPicker.SelectedIndex = num;
    }

    private void isSelfText_Click(object sender, RoutedEventArgs e)
    {
      if (SubmitLink.SelectedImage != null)
        return;
      if (this.isSelfText.IsChecked.Value)
      {
        this.isSelf = true;
        this.UrlDisplayText.Text = "self text";
        this.UrlTextBox.TextWrapping = TextWrapping.Wrap;
        this.UrlTextBox.AcceptsReturn = true;
        this.UrlTextBox.InputScope = new InputScope()
        {
          Names = {
            (object) new InputScopeName()
            {
              NameValue = InputScopeNameValue.Text
            }
          }
        };
        this.FormattedPreview.Visibility = Visibility.Visible;
        this.FormattedPreviewText.Visibility = Visibility.Visible;
      }
      else
      {
        this.isSelf = false;
        this.UrlDisplayText.Text = "url";
        this.UrlTextBox.TextWrapping = TextWrapping.NoWrap;
        this.UrlTextBox.AcceptsReturn = false;
        this.UrlTextBox.InputScope = new InputScope()
        {
          Names = {
            (object) new InputScopeName()
            {
              NameValue = InputScopeNameValue.Url
            }
          }
        };
        this.FormattedPreview.Visibility = Visibility.Collapsed;
        this.FormattedPreviewText.Visibility = Visibility.Collapsed;
      }
    }

    private void ContentPanel_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (this.Orientation == PageOrientation.Portrait || this.Orientation == PageOrientation.PortraitUp)
      {
        if (e.NewSize.Height > 660.0)
          this.ContentScroller.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        else
          this.ContentScroller.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
      }
      else
        this.ContentScroller.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
    }

    private void TitleTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      this.hasTitle = this.TitleTextBox.Text.Length > 0;
      if (this.TitleTextBox.Text.Length > 300 && !this.WaringingTitleLen)
      {
        this.WaringingTitleLen = true;
        int num = (int) MessageBox.Show("Title has a max length of 300 characters, which you have just exceeded.", "ALL OF THE TEXT", MessageBoxButton.OK);
      }
      if (this.hasTitle && this.hasUrl)
        ((ApplicationBarIconButton) this.ApplicationBar.Buttons[0]).IsEnabled = true;
      else
        ((ApplicationBarIconButton) this.ApplicationBar.Buttons[0]).IsEnabled = false;
    }

    private void UrlTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      this.hasUrl = this.UrlTextBox.Text.Length > 0;
      if (this.UrlTextBox.Text.Length > 10000 && !this.WarningSelfTextLen)
      {
        this.WarningSelfTextLen = true;
        int num = (int) MessageBox.Show("Self text has a max length of 10,000 characters, which you have just exceeded.", "ALL OF THE TEXT", MessageBoxButton.OK);
      }
      if (this.isSelf)
      {
        try
        {
          this.FormattedPreview.SetText(this.UrlTextBox.Text);
        }
        catch
        {
          this.FormattedPreview.ClearText();
          this.FormattedPreview.SetText("Error Formatting Text");
        }
      }
      if (this.hasTitle && this.hasUrl)
        ((ApplicationBarIconButton) this.ApplicationBar.Buttons[0]).IsEnabled = true;
      else
        ((ApplicationBarIconButton) this.ApplicationBar.Buttons[0]).IsEnabled = false;
    }

    private void AddPicture_Click(object sender, EventArgs e)
    {
      try
      {
        this.photoTask.Show();
        this.PictureIngore = true;
      }
      catch
      {
        int num = (int) MessageBox.Show("Baconit coundn't launch the photo chooser right now, make sure your phone is not syncing with zune on the PC.", "Can't Open Images", MessageBoxButton.OK);
        this.PictureIngore = false;
      }
    }

    private void photoChooserTask_Completed(object sender, PhotoResult e)
    {
      if (e.TaskResult == TaskResult.OK)
      {
        try
        {
          SubmitLink.SelectedImage = new BitmapImage();
          SubmitLink.SelectedImage.SetSource(e.ChosenPhoto);
        }
        catch (Exception ex)
        {
          SubmitLink.SelectedImage = (BitmapImage) null;
          App.DataManager.DebugDia("Image problem", ex);
          int num = (int) MessageBox.Show("Unable to load image.", "Error", MessageBoxButton.OK);
        }
        if (SubmitLink.SelectedImage == null)
          return;
        this.UrlDisplayText.Text = "image";
        this.UrlTextBox.TextWrapping = TextWrapping.NoWrap;
        this.UrlTextBox.AcceptsReturn = false;
        this.UrlTextBox.InputScope = new InputScope()
        {
          Names = {
            (object) new InputScopeName()
            {
              NameValue = InputScopeNameValue.Url
            }
          }
        };
        try
        {
          string originalFileName = e.OriginalFileName;
          this.UrlTextBox.Text = originalFileName.Substring(originalFileName.LastIndexOf('\\') + 1);
        }
        catch
        {
          this.UrlTextBox.Text = e.OriginalFileName;
        }
        this.UrlTextBox.IsEnabled = false;
        this.isSelfText.Content = (object) "scale image";
        this.isSelfText.IsChecked = new bool?(false);
        ((ApplicationBarIconButton) this.ApplicationBar.Buttons[0]).IsEnabled = true;
        this.hasUrl = true;
      }
      else
      {
        if (e.Error == null || e.Error.Message == null || !e.Error.Message.Trim().Equals("InvalidOperationException"))
          return;
        int num = (int) MessageBox.Show("Baconit coundn't launch the photo chooser right now, make sure your phone is not syncing with zune.", "Error", MessageBoxButton.OK);
      }
    }

    private void SubmitLinkButton_Click(object sender, EventArgs e)
    {
      if (!this.SubRedditPicker.IsEnabled)
      {
        int num = (int) MessageBox.Show("Baconit are still waiting for you subreddit list to be refreshed. Please wait a few moments.", "Still Waiting", MessageBoxButton.OK);
      }
      else
      {
        this.Focus();
        this.ApplicationBar.IsVisible = false;
        this.WorkingOverlay.Visibility = Visibility.Visible;
        this.OverlayProgress.IsIndeterminate = true;
        this.OpenWorkingOverlayAn.Begin();
        if (SubmitLink.SelectedImage != null)
        {
          if (this.ImageURL != null && !this.ImageURL.Equals(""))
          {
            this.PostTitle = this.TitleTextBox.Text;
            if (this.PostTitle.Length > 300)
              this.PostTitle = this.PostTitle.Substring(0, 299);
            this.PostUrlOrSelf = this.ImageURL;
            this.PostIsSelf = false;
            this.PostSubReddit = (string) this.SubRedditPicker.SelectedItem;
            this.OverlayText.Text = "Posting to reddit...";
            ThreadPool.QueueUserWorkItem((WaitCallback) (obfj => this.postLink()));
          }
          else
          {
            this.OverlayText.Text = "Sending Image to Imgur...";
            bool tryCompress = this.isSelfText.IsChecked.Value;
            ThreadPool.QueueUserWorkItem((WaitCallback) (obfj => this.sendImage(tryCompress)));
          }
        }
        else
        {
          this.PostTitle = this.TitleTextBox.Text;
          if (this.PostTitle.Length > 300)
            this.PostTitle = this.PostTitle.Substring(0, 299);
          this.PostUrlOrSelf = this.UrlTextBox.Text;
          if (this.PostUrlOrSelf.Length > 10000)
            this.PostUrlOrSelf = this.PostUrlOrSelf.Substring(0, 10000);
          this.PostIsSelf = this.isSelfText.IsChecked.Value;
          this.PostSubReddit = (string) this.SubRedditPicker.SelectedItem;
          ThreadPool.QueueUserWorkItem((WaitCallback) (obfj => this.postLink()));
        }
      }
    }

    private void sendImage(bool tryCompress)
    {
      if (tryCompress)
      {
        try
        {
          BitmapImage temp = (BitmapImage) null;
          bool succeded = false;
          using (AutoResetEvent are = new AutoResetEvent(false))
          {
            Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
            {
              try
              {
                int pixelWidth = SubmitLink.SelectedImage.PixelWidth;
                int pixelHeight = SubmitLink.SelectedImage.PixelHeight;
                if (pixelWidth > 800)
                {
                  float num = 800f / (float) pixelWidth;
                  WriteableBitmap bitmap = new WriteableBitmap((UIElement) new Image()
                  {
                    Width = (double) pixelWidth,
                    Height = (double) pixelHeight,
                    Visibility = Visibility.Collapsed,
                    Source = (ImageSource) SubmitLink.SelectedImage
                  }, (Transform) new ScaleTransform()
                  {
                    ScaleX = (double) num,
                    ScaleY = (double) num
                  });
                  MemoryStream memoryStream = new MemoryStream();
                  bitmap.SaveJpeg((Stream) memoryStream, bitmap.PixelWidth, bitmap.PixelHeight, 0, 90);
                  temp = new BitmapImage();
                  temp.SetSource((Stream) memoryStream);
                  succeded = true;
                }
                else
                  succeded = false;
              }
              catch
              {
                succeded = false;
              }
              are.Set();
            }));
            are.WaitOne();
          }
          if (succeded)
          {
            if (SubmitLink.SelectedImage != null)
              SubmitLink.SelectedImage = temp;
          }
        }
        catch (Exception ex)
        {
          App.DataManager.DebugDia("resize fail", ex);
        }
      }
      try
      {
        App.DataManager.PostImageToImagur(SubmitLink.SelectedImage, (string) null, new RunWorkerCompletedEventHandler(this.ImageUpload_Callback));
      }
      catch
      {
      }
    }

    private void postLink()
    {
      try
      {
        App.DataManager.SubmitRedditStory(new SubmitALinkInfo()
        {
          title = this.PostTitle,
          urlOrSelf = this.PostUrlOrSelf,
          isSelf = this.PostIsSelf,
          subreddit = this.PostSubReddit
        }, new RunWorkerCompletedEventHandler(this.Post_Callback));
      }
      catch
      {
      }
    }

    public void ImageUpload_Callback(object obj, RunWorkerCompletedEventArgs e)
    {
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        if ((bool) e.Result && obj != null)
        {
          this.UrlTextBox.Text = (string) obj;
          this.UrlTextBox.IsEnabled = true;
          this.PostIsSelf = false;
          this.PostTitle = this.TitleTextBox.Text;
          if (this.PostTitle.Length > 300)
            this.PostTitle = this.PostTitle.Substring(0, 299);
          this.PostUrlOrSelf = (string) obj;
          this.ImageURL = (string) obj;
          this.PostSubReddit = (string) this.SubRedditPicker.SelectedItem;
          this.OverlayText.Text = "Success, Posting to reddit...";
          ThreadPool.QueueUserWorkItem((WaitCallback) (obfj => this.postLink()));
          App.DataManager.BaconitAnalytics.LogEvent("Image Uploaded For Submit");
        }
        else
        {
          this.CloseWorkingOverlayAn.Begin();
          this.ApplicationBar.IsVisible = true;
          int num = (int) MessageBox.Show("Baconit currently can't upload your image to Imgur. Please try again later.");
          App.DataManager.DebugDia("failed image", (Exception) obj);
        }
      }));
    }

    public void Post_Callback(object obj, RunWorkerCompletedEventArgs e)
    {
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        if (e.Error is CaptchaException)
        {
          this.SaveInfo = false;
          this.NavigationService.Navigate(new Uri("/CaptchaUI.xaml", UriKind.Relative));
        }
        else if ((bool) e.Result)
        {
          SubmitLink.PostURL = (string) obj;
          int num1 = SubmitLink.PostURL.IndexOf('/');
          int num2 = SubmitLink.PostURL.IndexOf('/', num1 + 1);
          int num3 = SubmitLink.PostURL.IndexOf('/', num2 + 1);
          int num4 = SubmitLink.PostURL.IndexOf('/', num3 + 1);
          int num5 = SubmitLink.PostURL.IndexOf('/', num4 + 1);
          int num6 = SubmitLink.PostURL.IndexOf('/', num5 + 1);
          int num7 = SubmitLink.PostURL.IndexOf('/', num6 + 1);
          string str = "t3_" + SubmitLink.PostURL.Substring(num6 + 1, num7 - num6 - 1);
          App.ActiveStoryData = (SubRedditData) null;
          this.SaveInfo = false;
          this.NavigationService.Navigate(new Uri("/StoryDetails.xaml?StoryDataRedditID=" + str + "&ClearBack=1", UriKind.Relative));
          App.DataManager.BaconitAnalytics.LogEvent("Story Submitted");
        }
        else
        {
          this.CloseWorkingOverlayAn.Begin();
          this.ApplicationBar.IsVisible = true;
        }
      }));
    }

    private void CloseFormatting_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.CloseHelpAnmi.Begin();
      this.ApplicationBar.Mode = ApplicationBarMode.Default;
      this.FormattingHelpOpen = false;
    }

    private void ShowFormatting_Click(object sender, EventArgs e)
    {
      this.ApplicationBar.Mode = ApplicationBarMode.Minimized;
      this.FormattingHelp.Visibility = Visibility.Visible;
      this.OpenHelpAnmi.Begin();
      this.FormattingHelpOpen = true;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/SubmitLink.xaml", UriKind.Relative));
      this.CloseHelpAnmi = (Storyboard) this.FindName("CloseHelpAnmi");
      this.OpenHelpAnmi = (Storyboard) this.FindName("OpenHelpAnmi");
      this.OpenWorkingOverlayAn = (Storyboard) this.FindName("OpenWorkingOverlayAn");
      this.CloseWorkingOverlayAn = (Storyboard) this.FindName("CloseWorkingOverlayAn");
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TitlePanel = (StackPanel) this.FindName("TitlePanel");
      this.ApplicationTitle = (TextBlock) this.FindName("ApplicationTitle");
      this.ContentScroller = (ScrollViewer) this.FindName("ContentScroller");
      this.ContentPanel = (StackPanel) this.FindName("ContentPanel");
      this.TitleTextBox = (TextBox) this.FindName("TitleTextBox");
      this.UrlDisplayText = (TextBlock) this.FindName("UrlDisplayText");
      this.UrlTextBox = (TextBox) this.FindName("UrlTextBox");
      this.isSelfText = (CheckBox) this.FindName("isSelfText");
      this.FormattedPreviewText = (TextBlock) this.FindName("FormattedPreviewText");
      this.FormattedPreview = (SuperRichTextBox) this.FindName("FormattedPreview");
      this.SubRedditPicker = (ListPicker) this.FindName("SubRedditPicker");
      this.WorkingOverlay = (Grid) this.FindName("WorkingOverlay");
      this.OverlayText = (TextBlock) this.FindName("OverlayText");
      this.OverlayProgress = (ProgressBar) this.FindName("OverlayProgress");
      this.FormattingHelp = (StackPanel) this.FindName("FormattingHelp");
      this.CloseFormatting = (Grid) this.FindName("CloseFormatting");
      this.SubmitLinkButton = (ApplicationBarIconButton) this.FindName("SubmitLinkButton");
      this.AddPicture = (ApplicationBarIconButton) this.FindName("AddPicture");
      this.ShowFormatting = (ApplicationBarMenuItem) this.FindName("ShowFormatting");
    }
  }
}
