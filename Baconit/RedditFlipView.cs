// Decompiled with JetBrains decompiler
// Type: Baconit.RedditFlipView
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Baconit.CustomControls;
using Baconit.Database;
using BaconitData.Interfaces;
using BaconitData.Libs;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Media;
using Phone.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

#nullable disable
namespace Baconit
{
  public class RedditFlipView : PhoneApplicationPage
  {
    public const int REQ_IMAGE_FAILED = 2;
    public const int REQ_IMAGE_GOOD = 1;
    public const int REQ_Image_WAITING = 0;
    public const int REQ_Image_GIF = 3;
    public const int PANEL_TYPE_WEB = 1;
    public const int PANEL_TYPE_TEXT = 2;
    public const int PANEL_TYPE_NONE = 3;
    public const int PANEL_TYPE_IMAGE = 4;
    public const int PANEL_TYPE_GIF = 5;
    public static string DEBUG_LastLocation;
    public RedditsViewer Host;
    public RedditViewerViewModel ViewModel;
    public ObservableCollection<SubRedditData> StoryList;
    private RedditImage NextTempImageHolder;
    private object NextTempImageHolderLock = new object();
    private Dictionary<string, int> RequestedImages = new Dictionary<string, int>();
    private BitmapImage BlankImage = new BitmapImage();
    public bool UseSmallerImages = true;
    private int[] CurrentPanelType = new int[3];
    private bool[] PanelLoaded = new bool[3];
    private bool isLoaded;
    private bool firstOpen = true;
    private bool isOpening = true;
    private bool isLocked;
    private double LastClickTime;
    private WebBrowser WebControlObj;
    private string BrowserCurrentURL = "";
    private string BrowserCurrentHTML = "";
    private bool RestoreGIF;
    private PageOrientation CurOrientation;
    private TextBlock[] StoryTitle;
    private TextBlock[] StoryLineOne;
    private TextBlock[] StoryLineTwo;
    private ProgressBar[] LoadingImageProgress;
    private int[] PiviotStoryNumber;
    private string[] lastImageInSlot;
    private Grid[] LoadingImageUI;
    private Grid[] TitlePanel;
    private string OpenToStroy;
    private int currentList;
    private int currentVirturalList;
    private int currentStoryType = -1;
    private PickerBoxDialog sharePicker;
    private string[] PickerData = new string[5]
    {
      "Social Networks",
      "Tap + Send",
      "SMS",
      "Email",
      "Copy Link"
    };
    private Rectangle[] UpVoteBottom;
    private Rectangle[] DownVoteTop;
    private Polygon[] UpVoteTop;
    private Polygon[] DownVoteBottom;
    private TextBlock[] StoryPoints;
    private Image[] StoryImages;
    private Grid[] ImageHolders;
    private Grid[] WebControlHolder;
    private Grid[] NSFWBlockHolder;
    private Grid[] WebControlBlock;
    private Grid[] WebControlPlaceHolder;
    private Grid[] SelfTextHolder;
    private SuperRichTextBox[] SelfTextRichBox;
    private Grid[] OnlyComments;
    private ScrollViewer[] SelfTextScroller;
    private SolidColorBrush AccentColorBrush;
    private SolidColorBrush GrayColorBrush;
    private ProgressBar[] WebControlProgressBars;
    private GIFViewer[] GIFViewerHolder;
    private CompositeTransform[] ImageTransforms;
    private Storyboard[] ShowStoryInfo;
    private Storyboard[] HideStoryInfo;
    private bool[] RestoreProgressBars;
    private bool IsFrist = true;
    public const int APPBAR_MODE_IMAGE = 0;
    public const int APPBAR_MODE_SELFTEXT = 1;
    public const int APPBAR_MODE_WEBPAGE = 2;
    public const int APPBAR_MODE_COMMENTS = 3;
    public const int APPBAR_MODE_GIF = 4;
    private int currentMode = -1;
    private int WebControlInPane;
    private double _initialScale;
    private double _initialXCenter;
    private double _initialYCenter;
    private bool OrgScaleSet;
    private double OrgScale;
    internal Storyboard ShowTitleBox1;
    internal Storyboard HideTitleBox1;
    internal Storyboard ShowTitleBox2;
    internal EasingDoubleKeyFrame ShowTitleBoxTranKeyFrame2;
    internal Storyboard HideTitleBox2;
    internal EasingDoubleKeyFrame HideTitleBoxTranKeyFrame2;
    internal Storyboard ShowTitleBox3;
    internal Storyboard HideTitleBox3;
    internal Storyboard FadeOutTips;
    internal Storyboard FadeInTips;
    internal Grid LayoutRoot;
    internal Grid ContentPanel;
    internal Pivot MainPivot;
    internal Grid TitlePanel1;
    internal StackPanel UpVoteButton1;
    internal Polygon UpVoteTop1;
    internal Rectangle UpVoteBottom1;
    internal TextBlock StoryScore1;
    internal StackPanel DownVoteButton1;
    internal Rectangle DownVoteTop1;
    internal Polygon DownVoteBottom1;
    internal TextBlock StoryTitle1;
    internal TextBlock LineOne1;
    internal TextBlock LineTwo1;
    internal Grid WebControlHolder1;
    internal Grid WebControlPlaceHolder1;
    internal WebBrowser WebControlUI;
    internal ProgressBar WebControlLoading1;
    internal Grid WebControlBlock1;
    internal Grid SelfTextHolder1;
    internal ScrollViewer SelfTextScroller1;
    internal SuperRichTextBox SelfTextRichBox1;
    internal Grid OnlyComments1;
    internal Grid ImageHolder1;
    internal Grid LoadingImageBlock1;
    internal ProgressBar LoadingImageBlock1Progress;
    internal Grid NSFWBlock1;
    internal Grid TitlePanel2;
    internal StackPanel UpVoteButton2;
    internal Polygon UpVoteTop2;
    internal Rectangle UpVoteBottom2;
    internal TextBlock StoryScore2;
    internal StackPanel DownVoteButton2;
    internal Rectangle DownVoteTop2;
    internal Polygon DownVoteBottom2;
    internal TextBlock StoryTitle2;
    internal TextBlock LineOne2;
    internal TextBlock LineTwo2;
    internal Grid WebControlHolder2;
    internal Grid WebControlPlaceHolder2;
    internal ProgressBar WebControlLoading2;
    internal Grid WebControlBlock2;
    internal Grid SelfTextHolder2;
    internal ScrollViewer SelfTextScroller2;
    internal SuperRichTextBox SelfTextRichBox2;
    internal Grid OnlyComments2;
    internal Grid ImageHolder2;
    internal Grid LoadingImageBlock2;
    internal ProgressBar LoadingImageBlock2Progress;
    internal Grid NSFWBlock2;
    internal Grid TitlePanel3;
    internal StackPanel UpVoteButton3;
    internal Polygon UpVoteTop3;
    internal Rectangle UpVoteBottom3;
    internal TextBlock StoryScore3;
    internal StackPanel DownVoteButton3;
    internal Rectangle DownVoteTop3;
    internal Polygon DownVoteBottom3;
    internal TextBlock StoryTitle3;
    internal TextBlock LineOne3;
    internal TextBlock LineTwo3;
    internal Grid WebControlHolder3;
    internal Grid WebControlPlaceHolder3;
    internal ProgressBar WebControlLoading3;
    internal Grid WebControlBlock3;
    internal Grid SelfTextHolder3;
    internal ScrollViewer SelfTextScroller3;
    internal SuperRichTextBox SelfTextRichBox3;
    internal Grid OnlyComments3;
    internal Grid ImageHolder3;
    internal Grid LoadingImageBlock3;
    internal ProgressBar LoadingImageBlock3Progress;
    internal Grid NSFWBlock3;
    internal Image ImageViewerHelp;
    private bool _contentLoaded;

    public RedditFlipView()
    {
      this.InitializeComponent();
      TransitionService.SetNavigationInTransition((UIElement) this, App.SlideInTransistion);
      TransitionService.SetNavigationOutTransition((UIElement) this, App.SlideOutTransistion);
      this.ApplicationBar = (IApplicationBar) new Microsoft.Phone.Shell.ApplicationBar();
      this.ApplicationBar.IsVisible = true;
      this.ApplicationBar.Mode = ApplicationBarMode.Minimized;
      this.ApplicationBar.Opacity = 0.0;
      this.ApplicationBar.StateChanged += new EventHandler<ApplicationBarStateChangedEventArgs>(this.ApplicationBar_StateChanged);
      this.ApplicationBar.BackgroundColor = Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, (byte) 0);
      this.RestoreProgressBars = new bool[3];
      this.StoryTitle = new TextBlock[3];
      this.StoryTitle[0] = this.StoryTitle1;
      this.StoryTitle[1] = this.StoryTitle2;
      this.StoryTitle[2] = this.StoryTitle3;
      this.StoryLineOne = new TextBlock[3];
      this.StoryLineOne[0] = this.LineOne1;
      this.StoryLineOne[1] = this.LineOne2;
      this.StoryLineOne[2] = this.LineOne3;
      this.StoryLineTwo = new TextBlock[3];
      this.StoryLineTwo[0] = this.LineTwo1;
      this.StoryLineTwo[1] = this.LineTwo2;
      this.StoryLineTwo[2] = this.LineTwo3;
      this.PiviotStoryNumber = new int[3];
      this.PiviotStoryNumber[0] = 0;
      this.StoryPoints = new TextBlock[3];
      this.StoryPoints[0] = this.StoryScore1;
      this.StoryPoints[1] = this.StoryScore2;
      this.StoryPoints[2] = this.StoryScore3;
      this.StoryImages = new Image[3];
      this.ImageTransforms = new CompositeTransform[3];
      this.ImageHolders = new Grid[3];
      this.ImageHolders[0] = this.ImageHolder1;
      this.ImageHolders[1] = this.ImageHolder2;
      this.ImageHolders[2] = this.ImageHolder3;
      this.lastImageInSlot = new string[3];
      this.ShowStoryInfo = new Storyboard[3];
      this.ShowStoryInfo[0] = this.ShowTitleBox1;
      this.ShowStoryInfo[1] = this.ShowTitleBox2;
      this.ShowStoryInfo[2] = this.ShowTitleBox3;
      this.HideStoryInfo = new Storyboard[3];
      this.HideStoryInfo[0] = this.HideTitleBox1;
      this.HideStoryInfo[1] = this.HideTitleBox2;
      this.HideStoryInfo[2] = this.HideTitleBox3;
      this.LoadingImageUI = new Grid[3];
      this.LoadingImageUI[0] = this.LoadingImageBlock1;
      this.LoadingImageUI[1] = this.LoadingImageBlock2;
      this.LoadingImageUI[2] = this.LoadingImageBlock3;
      this.LoadingImageProgress = new ProgressBar[3];
      this.LoadingImageProgress[0] = this.LoadingImageBlock1Progress;
      this.LoadingImageProgress[1] = this.LoadingImageBlock2Progress;
      this.LoadingImageProgress[2] = this.LoadingImageBlock3Progress;
      this.NSFWBlockHolder = new Grid[3];
      this.NSFWBlockHolder[0] = this.NSFWBlock1;
      this.NSFWBlockHolder[1] = this.NSFWBlock2;
      this.NSFWBlockHolder[2] = this.NSFWBlock3;
      this.WebControlHolder = new Grid[3];
      this.WebControlHolder[0] = this.WebControlHolder1;
      this.WebControlHolder[1] = this.WebControlHolder2;
      this.WebControlHolder[2] = this.WebControlHolder3;
      this.WebControlProgressBars = new ProgressBar[3];
      this.WebControlProgressBars[0] = this.WebControlLoading1;
      this.WebControlProgressBars[1] = this.WebControlLoading2;
      this.WebControlProgressBars[2] = this.WebControlLoading3;
      this.GIFViewerHolder = new GIFViewer[3];
      this.GIFViewerHolder[0] = (GIFViewer) null;
      this.GIFViewerHolder[1] = (GIFViewer) null;
      this.GIFViewerHolder[2] = (GIFViewer) null;
      this.WebControlPlaceHolder = new Grid[3];
      this.WebControlPlaceHolder[0] = this.WebControlPlaceHolder1;
      this.WebControlPlaceHolder[1] = this.WebControlPlaceHolder2;
      this.WebControlPlaceHolder[2] = this.WebControlPlaceHolder3;
      this.WebControlBlock = new Grid[3];
      this.WebControlBlock[0] = this.WebControlBlock1;
      this.WebControlBlock[1] = this.WebControlBlock2;
      this.WebControlBlock[2] = this.WebControlBlock3;
      this.SelfTextHolder = new Grid[3];
      this.SelfTextHolder[0] = this.SelfTextHolder1;
      this.SelfTextHolder[1] = this.SelfTextHolder2;
      this.SelfTextHolder[2] = this.SelfTextHolder3;
      this.TitlePanel = new Grid[3];
      this.TitlePanel[0] = this.TitlePanel1;
      this.TitlePanel[1] = this.TitlePanel2;
      this.TitlePanel[2] = this.TitlePanel3;
      this.SelfTextRichBox = new SuperRichTextBox[3];
      this.SelfTextRichBox[0] = this.SelfTextRichBox1;
      this.SelfTextRichBox[1] = this.SelfTextRichBox2;
      this.SelfTextRichBox[2] = this.SelfTextRichBox3;
      this.SelfTextScroller = new ScrollViewer[3];
      this.SelfTextScroller[0] = this.SelfTextScroller1;
      this.SelfTextScroller[1] = this.SelfTextScroller1;
      this.SelfTextScroller[2] = this.SelfTextScroller1;
      this.OnlyComments = new Grid[3];
      this.OnlyComments[0] = this.OnlyComments1;
      this.OnlyComments[1] = this.OnlyComments2;
      this.OnlyComments[2] = this.OnlyComments3;
      this.UpVoteBottom = new Rectangle[3];
      this.UpVoteBottom[0] = this.UpVoteBottom1;
      this.UpVoteBottom[1] = this.UpVoteBottom2;
      this.UpVoteBottom[2] = this.UpVoteBottom3;
      this.UpVoteTop = new Polygon[3];
      this.UpVoteTop[0] = this.UpVoteTop1;
      this.UpVoteTop[1] = this.UpVoteTop2;
      this.UpVoteTop[2] = this.UpVoteTop3;
      this.DownVoteBottom = new Polygon[3];
      this.DownVoteBottom[0] = this.DownVoteBottom1;
      this.DownVoteBottom[1] = this.DownVoteBottom2;
      this.DownVoteBottom[2] = this.DownVoteBottom3;
      this.DownVoteTop = new Rectangle[3];
      this.DownVoteTop[0] = this.DownVoteTop1;
      this.DownVoteTop[1] = this.DownVoteTop2;
      this.DownVoteTop[2] = this.DownVoteTop3;
      this.AccentColorBrush = new SolidColorBrush((Color) Application.Current.Resources[(object) "PhoneAccentColor"]);
      this.GrayColorBrush = new SolidColorBrush((Color) Application.Current.Resources[(object) "PhoneSubtleColor"]);
      if (DataManager.LIGHT_THEME)
      {
        this.TitlePanel1.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 222, (byte) 222, (byte) 222));
        this.TitlePanel2.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 222, (byte) 222, (byte) 222));
        this.TitlePanel3.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 222, (byte) 222, (byte) 222));
        this.SelfTextHolder1.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 208, (byte) 208, (byte) 208));
        this.SelfTextHolder2.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 208, (byte) 208, (byte) 208));
        this.SelfTextHolder3.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 208, (byte) 208, (byte) 208));
        this.ApplicationBar.BackgroundColor = Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
        this.NSFWBlock1.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
        this.NSFWBlock2.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
        this.NSFWBlock3.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
      }
      this.sharePicker = new PickerBoxDialog();
      this.sharePicker.ItemSource = (IEnumerable) this.PickerData;
      this.sharePicker.Title = "SHARE VIA";
      this.sharePicker.Closed += new EventHandler(this.sharePicker_closed);
      this.WebControlObj = this.WebControlUI;
      this.WebControlObj.IsScriptEnabled = true;
      this.WebControlObj.IsGeolocationEnabled = true;
      this.Loaded += new RoutedEventHandler(this.RedditImageViewer_Loaded);
      this.FadeOutTips.Completed += (EventHandler) ((sender, e) =>
      {
        this.ImageViewerHelp.Visibility = Visibility.Collapsed;
        this.ApplicationBar.IsVisible = true;
        if (App.DataManager.SettingsMan.ScreenOrenLocked)
        {
          if (App.DataManager.SettingsMan.ScreenOrenLockedtoPortrait)
            this.SupportedOrientations = SupportedPageOrientation.Portrait;
          else
            this.SupportedOrientations = SupportedPageOrientation.Landscape;
        }
        else
          this.SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;
        this.SetAppBarMode(this.currentMode, true);
      });
    }

    private void RedditImageViewer_Loaded(object sender, RoutedEventArgs e)
    {
      this.isOpening = false;
      if (this.StoryList == null)
        return;
      if (!this.isLoaded && this.StoryList.Count > 0)
      {
        this.isLoaded = true;
        if (this.OpenToStroy == null || this.OpenToStroy.Equals(""))
        {
          int num = 0;
          if (App.DataManager.SettingsMan.ShowImagesOnlyInFlipView && this.Host.FlipModeNumImages[this.currentList] > 2)
          {
            RedditFlipView.DEBUG_LastLocation = "redditImageViewer_loaded";
            for (int index = 0; index < this.StoryList.Count; ++index)
            {
              if (num >= this.StoryList.Count)
              {
                num = 0;
                break;
              }
              if (this.StoryList[num].ImagePostNumber == -1)
                ++num;
              else
                break;
            }
          }
          this.PiviotStoryNumber[0] = -1;
          RedditFlipView.DEBUG_LastLocation = "redditImageViewer_loaded_settopiviot";
          this.setStoryToPiviot(this.StoryList[num], num, 0, false);
          if (num != 0)
            this.Pivot_SelectionChanged((object) this.MainPivot, (SelectionChangedEventArgs) null);
        }
      }
      this.WebControlObj.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      App.DataManager.BaconitAnalytics.LogPage("Flip View");
      if (App.navService == null)
        App.navService = this.NavigationService;
      if (this.Host == null || this.ViewModel == null)
      {
        this.Host = App.RedditViewerTransfter;
        this.ViewModel = App.ViewModelTransfter;
        if (this.Host == null || this.ViewModel == null)
        {
          this.Dispatcher.BeginInvoke((Action) (() =>
          {
            try
            {
              if (!this.NavigationService.CanGoBack)
                return;
              this.NavigationService.GoBack();
            }
            catch (Exception ex)
            {
              App.DataManager.DebugDia("Can't back", ex);
            }
          }));
          return;
        }
      }
      for (int index = 0; index < this.LoadingImageProgress.Length; ++index)
      {
        if (this.RestoreProgressBars[index])
          this.LoadingImageProgress[index].IsIndeterminate = true;
        this.RestoreProgressBars[index] = false;
      }
      if (this.firstOpen)
      {
        this.firstOpen = false;
        if (!this.NavigationContext.QueryString.ContainsKey("WhichList"))
        {
          this.Dispatcher.BeginInvoke((Action) (() =>
          {
            try
            {
              if (!this.NavigationService.CanGoBack)
                return;
              this.NavigationService.GoBack();
            }
            catch
            {
            }
          }));
          return;
        }
        this.currentList = int.Parse(this.NavigationContext.QueryString["WhichList"]);
        this.currentVirturalList = int.Parse(this.NavigationContext.QueryString["WhichVirtList"]);
        switch (this.currentList)
        {
          case 0:
            this.StoryList = this.ViewModel.HotStories;
            break;
          case 1:
            this.StoryList = this.ViewModel.NewStories;
            break;
          case 2:
            this.StoryList = this.ViewModel.TopStories;
            break;
          case 3:
            this.StoryList = this.ViewModel.ConvStories;
            break;
          default:
            this.Dispatcher.BeginInvoke((Action) (() =>
            {
              try
              {
                if (!this.NavigationService.CanGoBack)
                  return;
                this.NavigationService.GoBack();
              }
              catch
              {
              }
            }));
            return;
        }
        this.OpenToStroy = "";
        if (this.NavigationContext.QueryString.ContainsKey("StoryID"))
          this.OpenToStroy = this.NavigationContext.QueryString["StoryID"];
        if (string.IsNullOrEmpty(this.OpenToStroy) && App.DataManager.SettingsMan.ResumeFlipMode && App.DataManager.SettingsMan.FlipPlaceHolders.ContainsKey(this.Host.LocalSubreddit.RName))
        {
          SettingsManager.FlipPlaceHolder place = App.DataManager.SettingsMan.FlipPlaceHolders[this.Host.LocalSubreddit.RName];
          if (DateTime.Now.Subtract(place.added).TotalHours < 3.0)
          {
            int num = 0;
            RedditFlipView.DEBUG_LastLocation = "OnNavTo_foreachLoop";
            foreach (SubRedditData story in (Collection<SubRedditData>) this.StoryList)
            {
              if (story.RedditID.Equals(place.Entry))
              {
                if (num != 0)
                {
                  using (AutoResetEvent are = new AutoResetEvent(false))
                  {
                    string str = story.Title;
                    if (str.Length > 100)
                      str = str.Substring(0, 100) + "...";
                    Guide.BeginShowMessageBox("Flip Resume", "Press ok if you would like to resume to where you left off in flip view, titled '" + str + "'.", (IEnumerable<string>) new string[2]
                    {
                      "resume",
                      "start fresh"
                    }, 0, MessageBoxIcon.None, (AsyncCallback) (resul =>
                    {
                      int? nullable = Guide.EndShowMessageBox(resul);
                      if (nullable.HasValue)
                      {
                        switch (nullable.GetValueOrDefault())
                        {
                          case 0:
                            this.OpenToStroy = place.Entry;
                            break;
                        }
                      }
                      are.Set();
                    }), (object) null);
                    are.WaitOne();
                    break;
                  }
                }
                else
                  break;
              }
              else
                ++num;
            }
          }
        }
        if (!string.IsNullOrEmpty(this.OpenToStroy))
        {
          int num1 = 0;
          RedditFlipView.DEBUG_LastLocation = "OnNavTo_foreachAgain";
          foreach (SubRedditData story in (Collection<SubRedditData>) this.StoryList)
          {
            if (story.RedditID.Equals(this.OpenToStroy))
            {
              int num2 = num1 + 1;
              int num3 = num1 - 1;
              if (num2 >= this.StoryList.Count)
                num2 = 0;
              if (num3 < 0)
                num3 = this.StoryList.Count - 1;
              this.PiviotStoryNumber[0] = -1;
              this.setStoryToPiviot(this.StoryList[num1], num1, 0, false);
              RedditFlipView.DEBUG_LastLocation = "OnNavTo_foreachAgain_prev";
              this.PiviotStoryNumber[2] = -1;
              this.setStoryToPiviot(this.StoryList[num3], num3, 2, true);
              RedditFlipView.DEBUG_LastLocation = "OnNavTo_foreachAgain_next";
              this.PiviotStoryNumber[1] = -1;
              this.setStoryToPiviot(this.StoryList[num2], num2, 1, false);
              break;
            }
            ++num1;
          }
        }
        if (App.DataManager.SettingsMan.ShowImageViewerHelpOverlay)
        {
          App.DataManager.SettingsMan.ShowImageViewerHelpOverlay = false;
          this.Dispatcher.BeginInvoke((Action) (() =>
          {
            this.ImageViewerHelp.Source = (ImageSource) new BitmapImage(new Uri("/Images/ImageViewerOverlay-" + (object) App.DataManager.SettingsMan.ScaleFactor + ".png", UriKind.Relative));
            this.ImageViewerHelp.Visibility = Visibility.Visible;
            this.SupportedOrientations = SupportedPageOrientation.Portrait;
            this.ApplicationBar.IsVisible = false;
          }));
        }
      }
      else if (App.ActiveStoryData != null && this.StoryList != null)
      {
        for (int piviotNum = 0; piviotNum < 3; ++piviotNum)
        {
          if (this.PiviotStoryNumber[piviotNum] > -1 && this.PiviotStoryNumber[piviotNum] < this.StoryList.Count && this.StoryList[this.PiviotStoryNumber[piviotNum]].RedditID.Equals(App.ActiveStoryData.RedditID))
          {
            this.StoryList[this.PiviotStoryNumber[piviotNum]] = App.ActiveStoryData;
            this.SetStoryInformation(App.ActiveStoryData, piviotNum);
          }
        }
      }
      if (this.RestoreGIF && this.MainPivot.SelectedIndex != -1 && this.CurrentPanelType[this.MainPivot.SelectedIndex] == 5 && this.GIFViewerHolder[this.MainPivot.SelectedIndex] != null)
        this.GIFViewerHolder[this.MainPivot.SelectedIndex].PlayVideo();
      this.RestoreGIF = false;
      if (App.DataManager.SettingsMan.ScreenOrenLocked)
      {
        if (App.DataManager.SettingsMan.ScreenOrenLockedtoPortrait)
          this.SupportedOrientations = SupportedPageOrientation.Portrait;
        else
          this.SupportedOrientations = SupportedPageOrientation.Landscape;
      }
      else
        this.SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;
      this.SetAppBarMode(this.currentMode, true);
    }

    private void PhoneApplicationPage_OrientationChanged(
      object sender,
      OrientationChangedEventArgs e)
    {
      PageOrientation orientation = e.Orientation;
      RotateTransition rotateTransition = new RotateTransition();
      switch (orientation)
      {
        case PageOrientation.Portrait:
        case PageOrientation.PortraitUp:
          rotateTransition.Mode = this.CurOrientation != PageOrientation.LandscapeLeft ? RotateTransitionMode.In90Clockwise : RotateTransitionMode.In90Counterclockwise;
          break;
        case PageOrientation.Landscape:
        case PageOrientation.LandscapeRight:
          rotateTransition.Mode = this.CurOrientation != PageOrientation.PortraitUp ? RotateTransitionMode.In180Clockwise : RotateTransitionMode.In90Counterclockwise;
          break;
        case PageOrientation.LandscapeLeft:
          rotateTransition.Mode = this.CurOrientation != PageOrientation.LandscapeRight ? RotateTransitionMode.In90Clockwise : RotateTransitionMode.In180Counterclockwise;
          break;
      }
      PhoneApplicationPage content = (PhoneApplicationPage) ((ContentControl) Application.Current.RootVisual).Content;
      ITransition transition = rotateTransition.GetTransition((UIElement) content);
      transition.Completed += (EventHandler) delegate
      {
        transition.Stop();
      };
      transition.Begin();
      this.CurOrientation = e.Orientation;
    }

    private void ApplicationBar_StateChanged(object sender, ApplicationBarStateChangedEventArgs e)
    {
      if (e.IsMenuVisible)
      {
        this.ApplicationBar.Opacity = 0.8;
        SystemTray.Opacity = 0.99;
        SystemTray.IsVisible = true;
      }
      else
      {
        if (this.currentMode == 2)
          this.ApplicationBar.Opacity = 0.2;
        else
          this.ApplicationBar.Opacity = 0.0;
        SystemTray.IsVisible = false;
      }
    }

    private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.StoryList == null)
        return;
      RedditFlipView.DEBUG_LastLocation = "Piviot_changed_first";
      if (this.StoryList.Count == 0)
        return;
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        int selectedIndex = this.MainPivot.SelectedIndex;
        int piviotNum1 = selectedIndex + 1;
        int piviotNum2 = selectedIndex - 1;
        if (piviotNum1 == 3)
          piviotNum1 = 0;
        if (piviotNum2 == -1)
          piviotNum2 = 2;
        int num1 = this.PiviotStoryNumber[selectedIndex] + 1;
        int num2 = this.PiviotStoryNumber[selectedIndex] - 1;
        if (App.DataManager.SettingsMan.ShowImagesOnlyInFlipView && this.Host.FlipModeNumImages[this.currentList] > 2)
        {
          bool flag1 = false;
          RedditFlipView.DEBUG_LastLocation = "Piviot_changed_2";
          for (int index = 0; index < this.StoryList.Count; ++index)
          {
            if (num1 >= this.StoryList.Count)
            {
              if (!flag1)
              {
                num1 = 0;
                flag1 = true;
              }
              else
                break;
            }
            if (this.StoryList[num1].ImagePostNumber == -1)
              ++num1;
            else
              break;
          }
          RedditFlipView.DEBUG_LastLocation = "Piviot_changed_3";
          bool flag2 = false;
          for (int index = 0; index < this.StoryList.Count; --index)
          {
            if (num2 < 0)
            {
              if (!flag2)
              {
                flag2 = true;
                num2 = this.StoryList.Count - 1;
              }
              else
                break;
            }
            if (this.StoryList[num2].ImagePostNumber == -1)
              --num2;
            else
              break;
          }
        }
        if (num1 >= this.StoryList.Count)
          num1 = 0;
        if (num2 < 0)
          num2 = this.StoryList.Count - 1;
        RedditFlipView.DEBUG_LastLocation = "Piviot_changed_setpiviot";
        this.setStoryToPiviot(this.StoryList[this.PiviotStoryNumber[selectedIndex]], this.PiviotStoryNumber[selectedIndex], selectedIndex, false);
        this.setStoryToPiviot(this.StoryList[num1], num1, piviotNum1, false);
        this.setStoryToPiviot(this.StoryList[num2], num2, piviotNum2, true);
        try
        {
          string redditId = this.StoryList[this.PiviotStoryNumber[selectedIndex]].RedditID;
          App.DataManager.SettingsMan.AddReadStory(redditId);
          this.Host.SetStoryRead(redditId);
          if (num1 + 1 < this.StoryList.Count)
          {
            SubRedditData story = this.StoryList[num1 + 1];
            if (story.ImagePostNumber != -1)
            {
              if (!story.URL.ToLower().EndsWith(".gif"))
              {
                if (!story.URL.ToLower().EndsWith(".gifv"))
                {
                  string imageUrl = DataManager.GetImageURL(story, this.UseSmallerImages);
                  ThreadPool.QueueUserWorkItem(new WaitCallback(new RedditImage(App.DataManager, false, new RunWorkerCompletedEventHandler(this.ImageRequest_RunWorkerCompleted))
                  {
                    StoryID = story.RedditID,
                    URL = imageUrl,
                    SubRedditRName = this.Host.LocalSubreddit.RName,
                    SubRedditType = 0,
                    CacheOnly = true,
                    GetLowRes = this.UseSmallerImages
                  }.run));
                }
              }
            }
          }
        }
        catch
        {
        }
        RedditFlipView.DEBUG_LastLocation = "Piviot_changed_check more";
        if (num1 >= this.StoryList.Count - 6 || App.DataManager.SettingsMan.ShowImagesOnlyInFlipView && this.Host.FlipModeNumImages[this.currentList] - this.StoryList[num1].ImagePostNumber < 6)
          this.GetMoreStories();
        SubRedditData story1 = this.StoryList[this.PiviotStoryNumber[selectedIndex]];
        if (this.CurrentPanelType[selectedIndex] == 5)
          this.currentStoryType = 4;
        else if (story1.ImagePostNumber != -1)
          this.currentStoryType = 0;
        else if (story1.hasSelfText)
        {
          this.currentStoryType = 1;
        }
        else
        {
          this.currentStoryType = 2;
          if (App.DataManager.SettingsMan.FlipViewShowBrowserHelp && num1 > 1)
            ThreadPool.QueueUserWorkItem((WaitCallback) (obj =>
            {
              Thread.Sleep(500);
              this.Dispatcher.BeginInvoke((Action) (() =>
              {
                App.DataManager.SettingsMan.FlipViewShowBrowserHelp = false;
                App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("New tip! Tap here", true, true, "Web Page Tip", "To interact with a web page in flip view, tap on the web page anywere."));
              }));
            }));
        }
        this.SetAppBarMode(this.currentStoryType, false);
      }));
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      RedditFlipView.DEBUG_LastLocation = "nav_from";
      if (this.Host != null && this.Host.LocalSubreddit != null && this.Host.LocalSubreddit.RName != null && this.StoryList != null && this.StoryList.Count > this.PiviotStoryNumber[this.MainPivot.SelectedIndex])
      {
        string redditId = this.StoryList[this.PiviotStoryNumber[this.MainPivot.SelectedIndex]].RedditID;
        App.DataManager.SettingsMan.FlipPlaceHolders[this.Host.LocalSubreddit.RName] = new SettingsManager.FlipPlaceHolder()
        {
          added = DateTime.Now,
          Entry = redditId
        };
        App.DataManager.SettingsMan.SetFlipPlaceHolders();
      }
      for (int index = 0; index < this.LoadingImageProgress.Length; ++index)
      {
        if (this.LoadingImageProgress[index].IsIndeterminate)
          this.RestoreProgressBars[index] = true;
        this.LoadingImageProgress[index].IsIndeterminate = false;
      }
      this.WebControlLoading1.IsIndeterminate = false;
      this.WebControlLoading2.IsIndeterminate = false;
      this.WebControlLoading3.IsIndeterminate = false;
      if (this.MainPivot.SelectedIndex == -1 || this.CurrentPanelType[this.MainPivot.SelectedIndex] != 5 || this.GIFViewerHolder[this.MainPivot.SelectedIndex] == null)
        return;
      this.GIFViewerHolder[this.MainPivot.SelectedIndex].PauseVideo();
      this.RestoreGIF = true;
    }

    private void GetMoreStories()
    {
      RedditFlipView.DEBUG_LastLocation = "getmorestoreis";
      int pos = this.StoryList.Count;
      ThreadPool.QueueUserWorkItem((WaitCallback) (obfj => App.DataManager.UpdateSubRedditStories((RedditViewerViewModelInterface) this.ViewModel, this.Host.LocalSubreddit.URL, this.currentList, this.currentVirturalList, pos)));
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
        if (!this.isLocked)
          return;
        int selectedIndex = this.MainPivot.SelectedIndex;
        this.isLocked = false;
        this.MainPivot.IsLocked = false;
        this.ShowStoryInfo[selectedIndex].Begin();
        this.ApplicationBar.IsVisible = true;
        e.Cancel = true;
        if (this.StoryImages[selectedIndex] != null)
          this.StoryImages[selectedIndex].CacheMode = (CacheMode) null;
        if (this.ImageTransforms[selectedIndex] != null)
        {
          this.ImageTransforms[selectedIndex].ClearValue(CompositeTransform.ScaleXProperty);
          this.ImageTransforms[selectedIndex].ClearValue(CompositeTransform.ScaleYProperty);
          this.ImageTransforms[selectedIndex].ClearValue(CompositeTransform.CenterXProperty);
          this.ImageTransforms[selectedIndex].ClearValue(CompositeTransform.CenterYProperty);
        }
        this.WebControlBlock[selectedIndex].Visibility = Visibility.Visible;
      }
    }

    public void setStoryToPiviot(
      SubRedditData story,
      int storyNum,
      int piviotNum,
      bool DontPreLoad)
    {
      if (((this.PiviotStoryNumber[piviotNum] != storyNum || !this.PanelLoaded[piviotNum] ? 1 : (story.ImagePostNumber == -1 || this.RequestedImages.ContainsKey(story.RedditID) ? 0 : (this.CurrentPanelType[piviotNum] != 5 ? 1 : 0))) | (DontPreLoad ? 1 : 0)) == 0 && this.CurrentPanelType[piviotNum] != 1)
        return;
      this.PiviotStoryNumber[piviotNum] = storyNum;
      this.SetStoryInformation(story, piviotNum);
      this.OrgScaleSet = false;
      switch (this.CurrentPanelType[piviotNum])
      {
        case 1:
          this.NavBrowserToURL((string) null, "<html bgcolor='black'><body bgcolor='black'></body></html>", piviotNum);
          this.RemoveWebControlFromUI();
          this.OnlyComments[piviotNum].Visibility = Visibility.Collapsed;
          this.CurrentPanelType[piviotNum] = 3;
          break;
        case 2:
          this.SelfTextHolder[piviotNum].Visibility = Visibility.Collapsed;
          this.SelfTextScroller[piviotNum].ScrollToVerticalOffset(0.0);
          this.SelfTextRichBox[piviotNum].ClearText();
          break;
        case 3:
          this.OnlyComments[piviotNum].Visibility = Visibility.Collapsed;
          break;
        case 4:
          this.DestroyImage(piviotNum);
          this.LoadingImageUI[piviotNum].Visibility = Visibility.Collapsed;
          this.LoadingImageProgress[piviotNum].IsIndeterminate = false;
          this.ImageHolders[piviotNum].Visibility = Visibility.Collapsed;
          break;
        case 5:
          this.DestroyGIF(piviotNum);
          break;
      }
      if (DontPreLoad)
      {
        this.PanelLoaded[piviotNum] = false;
      }
      else
      {
        this.PanelLoaded[piviotNum] = true;
        if (story.ImagePostNumber != -1)
        {
          this.CurrentPanelType[piviotNum] = 4;
          if (this.NextTempImageHolder != null && this.NextTempImageHolder.StoryID.Equals(story.RedditID))
          {
            RedditImage nextTempImageHolder;
            lock (this.NextTempImageHolderLock)
            {
              nextTempImageHolder = this.NextTempImageHolder;
              this.NextTempImageHolder = (RedditImage) null;
            }
            this.CreateImage(piviotNum, nextTempImageHolder);
          }
          else if (story.URL.ToLower().EndsWith(".gif") || story.URL.ToLower().EndsWith(".gifv"))
            this.SetGif(piviotNum, story.URL);
          else if (!this.RequestedImages.ContainsKey(story.RedditID))
          {
            string imageUrl = DataManager.GetImageURL(story, this.UseSmallerImages);
            ThreadPool.QueueUserWorkItem(new WaitCallback(new RedditImage(App.DataManager, false, new RunWorkerCompletedEventHandler(this.ImageRequest_RunWorkerCompleted))
            {
              StoryID = story.RedditID,
              URL = imageUrl,
              SubRedditRName = this.Host.LocalSubreddit.RName,
              SubRedditType = 0,
              GetLowRes = this.UseSmallerImages
            }.run));
            this.RequestedImages.Add(story.RedditID, 0);
            this.LoadingImageUI[piviotNum].Visibility = Visibility.Visible;
            this.LoadingImageProgress[piviotNum].IsIndeterminate = true;
          }
          else if (this.RequestedImages[story.RedditID] == 0)
          {
            this.LoadingImageUI[piviotNum].Visibility = Visibility.Visible;
            this.LoadingImageProgress[piviotNum].IsIndeterminate = true;
          }
          else if (this.RequestedImages[story.RedditID] == 3)
          {
            this.SetGif(piviotNum, story.URL);
          }
          else
          {
            this.CurrentPanelType[piviotNum] = 1;
            this.currentStoryType = 2;
            if (piviotNum != this.MainPivot.SelectedIndex)
            {
              this.PanelLoaded[piviotNum] = false;
              if (!App.DataManager.SettingsMan.FlipViewCacheNextWebPage)
                return;
              this.NavBrowserToURL(story.URL, (string) null, piviotNum);
            }
            else
            {
              this.AddWebControlToUI(piviotNum);
              this.NavBrowserToURL(story.URL, (string) null, piviotNum);
              this.PanelLoaded[piviotNum] = true;
            }
          }
        }
        else if (story.hasSelfText)
        {
          this.CurrentPanelType[piviotNum] = 2;
          this.SelfTextHolder[piviotNum].Visibility = Visibility.Visible;
          this.SelfTextScroller[piviotNum].ScrollToVerticalOffset(0.0);
          ThreadPool.QueueUserWorkItem((WaitCallback) (obj =>
          {
            if (string.IsNullOrEmpty(story.SelfText))
            {
              int num = 0;
              while (true)
              {
                story.SelfText = App.DataManager.BaconitStore.GetSingleLongText(story.RedditID, story.SubRedditURL);
                if (string.IsNullOrWhiteSpace(story.SelfText))
                {
                  Thread.Sleep(1000);
                  if (num <= 5)
                    ++num;
                  else
                    break;
                }
                else
                  break;
              }
            }
            this.Dispatcher.BeginInvoke((Action) (() =>
            {
              RedditFlipView.DEBUG_LastLocation = "settoPiviot_1";
              if (!this.StoryList[this.PiviotStoryNumber[piviotNum]].RedditID.Equals(story.RedditID))
                return;
              this.SelfTextRichBox[piviotNum].SetText(story.SelfText);
            }));
          }));
        }
        else if (story.URL.Contains("reddit.com/") && story.URL.Contains("comments/"))
        {
          this.CurrentPanelType[piviotNum] = 3;
          this.OnlyComments[piviotNum].Visibility = Visibility.Visible;
          this.NSFWBlockHolder[piviotNum].Visibility = Visibility.Collapsed;
        }
        else
        {
          this.CurrentPanelType[piviotNum] = 1;
          if (piviotNum != this.MainPivot.SelectedIndex)
          {
            this.PanelLoaded[piviotNum] = false;
            if (!App.DataManager.SettingsMan.FlipViewCacheNextWebPage || story.URL.ToLower().Contains("windowsphone.com"))
              return;
            this.NavBrowserToURL(story.URL, (string) null, piviotNum);
          }
          else
          {
            this.AddWebControlToUI(piviotNum);
            this.NavBrowserToURL(story.URL, (string) null, piviotNum);
            this.PanelLoaded[piviotNum] = true;
          }
        }
      }
    }

    private void SetStoryInformation(SubRedditData story, int piviotNum)
    {
      this.StoryTitle[piviotNum].Text = story.Title.Replace('\n', ' ');
      this.StoryLineOne[piviotNum].Text = story.LineOne;
      this.StoryLineTwo[piviotNum].Text = story.LineTwo;
      this.StoryPoints[piviotNum].Text = (story.ups - story.downs).ToString() + string.Empty;
      if (story.isNotSafeForWork && App.DataManager.SettingsMan.NSFWClickThrough)
        this.NSFWBlockHolder[piviotNum].Visibility = Visibility.Visible;
      else
        this.NSFWBlockHolder[piviotNum].Visibility = Visibility.Collapsed;
      if (story.Like == 0)
      {
        this.UpVoteTop[piviotNum].Fill = (Brush) this.GrayColorBrush;
        this.UpVoteBottom[piviotNum].Fill = (Brush) this.GrayColorBrush;
        this.DownVoteTop[piviotNum].Fill = (Brush) this.GrayColorBrush;
        this.DownVoteBottom[piviotNum].Fill = (Brush) this.GrayColorBrush;
      }
      else if (story.Like == 1)
      {
        this.UpVoteTop[piviotNum].Fill = (Brush) this.AccentColorBrush;
        this.UpVoteBottom[piviotNum].Fill = (Brush) this.AccentColorBrush;
        this.DownVoteTop[piviotNum].Fill = (Brush) this.GrayColorBrush;
        this.DownVoteBottom[piviotNum].Fill = (Brush) this.GrayColorBrush;
      }
      else
      {
        this.UpVoteTop[piviotNum].Fill = (Brush) this.GrayColorBrush;
        this.UpVoteBottom[piviotNum].Fill = (Brush) this.GrayColorBrush;
        this.DownVoteTop[piviotNum].Fill = (Brush) this.AccentColorBrush;
        this.DownVoteBottom[piviotNum].Fill = (Brush) this.AccentColorBrush;
      }
    }

    public void ImageRequest_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      RedditImage image = (RedditImage) e.Result;
      bool flag = false;
      for (int index1 = 0; index1 < 3; ++index1)
      {
        int index2 = this.PiviotStoryNumber[index1];
        RedditFlipView.DEBUG_LastLocation = "runworker_complete";
        if (index2 >= 0 && index2 < this.StoryList.Count && this.StoryList[index2].RedditID.Equals(image.StoryID))
        {
          flag = true;
          int local = index1;
          if (image.CallBackStatus)
          {
            this.RequestedImages[image.StoryID] = 1;
            this.Dispatcher.BeginInvoke((Action) (() =>
            {
              this.CreateImage(local, image);
              this.LoadingImageUI[local].Visibility = Visibility.Collapsed;
              this.LoadingImageProgress[local].IsIndeterminate = false;
              this.WebControlHolder[local].Visibility = Visibility.Collapsed;
              this.ImageHolders[local].Visibility = Visibility.Visible;
              Storyboard storyboard = new Storyboard();
              ExponentialEase exponentialEase = new ExponentialEase()
              {
                EasingMode = EasingMode.EaseOut,
                Exponent = 6.0
              };
              DoubleAnimation element = new DoubleAnimation();
              PropertyPath path = new PropertyPath("(UIElement.Opacity)", new object[0]);
              element.Duration = new Duration(TimeSpan.FromMilliseconds(1000.0));
              element.From = new double?(0.0);
              element.To = new double?(1.0);
              element.EasingFunction = (IEasingFunction) exponentialEase;
              Storyboard.SetTarget((Timeline) element, (DependencyObject) this.StoryImages[local]);
              Storyboard.SetTargetProperty((Timeline) element, path);
              storyboard.Children.Add((Timeline) element);
              storyboard.Begin();
              if (local != this.MainPivot.SelectedIndex)
                return;
              this.OrgScaleSet = false;
            }));
            break;
          }
          this.Dispatcher.BeginInvoke((Action) (() =>
          {
            this.LoadingImageUI[local].Visibility = Visibility.Collapsed;
            this.LoadingImageProgress[local].IsIndeterminate = false;
            this.ImageHolders[local].Visibility = Visibility.Collapsed;
            this.RequestedImages[image.StoryID] = 2;
            if (image.URL.ToLower().EndsWith(".gif") || image.URL.ToLower().EndsWith(".gifv") || image.IsGif)
            {
              this.RequestedImages[image.StoryID] = 3;
              this.WebControlHolder[local].Visibility = Visibility.Visible;
              this.SetGif(local, image.URL);
            }
            else
            {
              this.CurrentPanelType[local] = 1;
              this.currentStoryType = 2;
              this.PanelLoaded[local] = false;
              if (local != this.MainPivot.SelectedIndex)
                return;
              this.AddWebControlToUI(local);
              this.NavBrowserToURL(image.URL, (string) null, local);
              this.PanelLoaded[local] = true;
            }
          }));
          break;
        }
      }
      if (!image.CallBackStatus && (image.URL.ToLower().EndsWith(".gif") || image.URL.ToLower().EndsWith(".gifv") || image.IsGif))
        this.RequestedImages[image.StoryID] = 3;
      else if (!image.CallBackStatus)
      {
        this.RequestedImages[image.StoryID] = 2;
      }
      else
      {
        if (flag)
          return;
        if (!DataManager.IS_LOW_MEMORY_DEVICE)
        {
          lock (this.NextTempImageHolderLock)
          {
            if (this.NextTempImageHolder != null)
            {
              this.NextTempImageHolder.Image = (BitmapSource) null;
              try
              {
                this.RequestedImages.Remove(this.NextTempImageHolder.StoryID);
              }
              catch
              {
              }
              this.NextTempImageHolder = (RedditImage) null;
            }
            this.NextTempImageHolder = image;
            this.RequestedImages[image.StoryID] = 1;
          }
        }
        else
          this.RequestedImages.Remove(image.StoryID);
      }
    }

    public void SetGif(int piviotNum, string storyUrl)
    {
      this.LoadingImageUI[piviotNum].Visibility = Visibility.Collapsed;
      this.ImageHolders[piviotNum].Visibility = Visibility.Collapsed;
      this.WebControlHolder[piviotNum].Visibility = Visibility.Collapsed;
      this.SelfTextHolder[piviotNum].Visibility = Visibility.Collapsed;
      this.OnlyComments[piviotNum].Visibility = Visibility.Collapsed;
      this.CurrentPanelType[piviotNum] = 5;
      if (piviotNum != this.MainPivot.SelectedIndex)
      {
        this.PanelLoaded[piviotNum] = false;
      }
      else
      {
        if (storyUrl.ToLower().Trim().EndsWith(".gifv"))
          storyUrl = storyUrl.Substring(0, storyUrl.Length - 1);
        if (!storyUrl.ToLower().Trim().EndsWith(".gif"))
          storyUrl += ".gif";
        this.GIFViewerHolder[piviotNum] = new GIFViewer();
        this.ImageHolders[piviotNum].Children.Clear();
        this.ImageHolders[piviotNum].Children.Add((UIElement) this.GIFViewerHolder[piviotNum]);
        this.ImageHolders[piviotNum].Visibility = Visibility.Visible;
        this.GIFViewerHolder[piviotNum].PlayVideo(storyUrl);
        this.PanelLoaded[piviotNum] = true;
      }
    }

    private void DestroyGIF(int piviotNum)
    {
      if (this.GIFViewerHolder[piviotNum] != null)
        this.GIFViewerHolder[piviotNum].StopVideo();
      this.GIFViewerHolder[piviotNum] = (GIFViewer) null;
      this.ImageHolders[piviotNum].Children.Clear();
      this.ImageHolders[piviotNum].Visibility = Visibility.Collapsed;
      this.CurrentPanelType[piviotNum] = 3;
      this.PanelLoaded[piviotNum] = false;
    }

    private void OnlyComments_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      RedditFlipView.DEBUG_LastLocation = "onlycomment";
      SubRedditData story = this.StoryList[this.PiviotStoryNumber[this.MainPivot.SelectedIndex]];
      App.ActiveStoryData = story;
      this.NavigationService.Navigate(new Uri("/StoryDetails.xaml?StoryDataRedditID=" + story.RedditID, UriKind.Relative));
    }

    public void SetAppBarMode(int toMode, bool force)
    {
      if (this.currentMode == toMode && !force && toMode != 2)
        return;
      this.currentMode = toMode;
      this.ApplicationBar.MenuItems.Clear();
      this.ApplicationBar.Opacity = 0.0;
      switch (toMode)
      {
        case 0:
          ApplicationBarMenuItem applicationBarMenuItem1 = new ApplicationBarMenuItem("share image...");
          applicationBarMenuItem1.Click += new EventHandler(this.Share_Click);
          this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem1);
          ApplicationBarMenuItem applicationBarMenuItem2 = new ApplicationBarMenuItem("save to phone");
          applicationBarMenuItem2.Click += new EventHandler(this.SaveToPhone_Click);
          this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem2);
          ApplicationBarMenuItem applicationBarMenuItem3 = !this.UseSmallerImages ? new ApplicationBarMenuItem("use smaller resolution images") : new ApplicationBarMenuItem("use full resolution images");
          applicationBarMenuItem3.Click += new EventHandler(this.UseFullSize_Click);
          this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem3);
          break;
        case 1:
          ApplicationBarMenuItem applicationBarMenuItem4 = new ApplicationBarMenuItem("share...");
          applicationBarMenuItem4.Click += new EventHandler(this.Share_Click);
          this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem4);
          break;
        case 2:
          ApplicationBarMenuItem applicationBarMenuItem5 = new ApplicationBarMenuItem("share...");
          applicationBarMenuItem5.Click += new EventHandler(this.Share_Click);
          this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem5);
          this.ApplicationBar.Opacity = 0.2;
          ApplicationBarMenuItem applicationBarMenuItem6 = new ApplicationBarMenuItem("refresh web page");
          applicationBarMenuItem6.Click += new EventHandler(this.refreshWeb_Click);
          this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem6);
          ApplicationBarMenuItem applicationBarMenuItem7 = new ApplicationBarMenuItem("open in browser");
          applicationBarMenuItem7.Click += new EventHandler(this.openInBroser_Click);
          this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem7);
          string webDomain = this.GetWebDomain(this.StoryList[this.PiviotStoryNumber[this.MainPivot.SelectedIndex]].URL);
          ApplicationBarMenuItem applicationBarMenuItem8 = !App.DataManager.SettingsMan.OptimizeWebsites.ContainsKey(webDomain) ? (!App.DataManager.SettingsMan.OptimizeWebByDefault ? new ApplicationBarMenuItem("optimize " + webDomain) : new ApplicationBarMenuItem("don't optimize " + webDomain)) : (!App.DataManager.SettingsMan.OptimizeWebsites[webDomain] ? new ApplicationBarMenuItem("optimize " + webDomain) : new ApplicationBarMenuItem("don't optimize " + webDomain));
          applicationBarMenuItem8.Click += new EventHandler(this.optimize_Click);
          this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem8);
          break;
        case 4:
          ApplicationBarMenuItem applicationBarMenuItem9 = new ApplicationBarMenuItem("share...");
          applicationBarMenuItem9.Click += new EventHandler(this.Share_Click);
          this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem9);
          this.ApplicationBar.Opacity = 0.2;
          ApplicationBarMenuItem applicationBarMenuItem10 = new ApplicationBarMenuItem("open in browser");
          applicationBarMenuItem10.Click += new EventHandler(this.openInBroser_Click);
          this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem10);
          break;
      }
      ApplicationBarMenuItem applicationBarMenuItem11 = !App.DataManager.SettingsMan.ShowImagesOnlyInFlipView ? new ApplicationBarMenuItem("show images only") : new ApplicationBarMenuItem("show all stories");
      applicationBarMenuItem11.Click += new EventHandler(this.ShowImagesOnly_click);
      this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem11);
      ApplicationBarMenuItem applicationBarMenuItem12 = !App.DataManager.SettingsMan.ScreenOrenLocked ? new ApplicationBarMenuItem("lock screen orientation") : new ApplicationBarMenuItem("unlock screen orientation");
      applicationBarMenuItem12.Click += new EventHandler(this.lockOren_Click);
      this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem12);
      ApplicationBarMenuItem applicationBarMenuItem13 = new ApplicationBarMenuItem("show help tips");
      applicationBarMenuItem13.Click += new EventHandler(this.showHelpTips_Click);
      this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem13);
    }

    private void UpVoteButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      if (App.DataManager.SettingsMan.IsSignedIn)
      {
        int selectedIndex = this.MainPivot.SelectedIndex;
        SubRedditData StoryData = this.StoryList[this.PiviotStoryNumber[selectedIndex]];
        int vote = 0;
        switch (StoryData.Like)
        {
          case -1:
            --StoryData.downs;
            ++StoryData.score;
            StoryData.DownVoteColor = RedditViewerViewModel.ARROW_NO_SELECT;
            this.DownVoteBottom[selectedIndex].Fill = (Brush) this.GrayColorBrush;
            this.DownVoteTop[selectedIndex].Fill = (Brush) this.GrayColorBrush;
            StoryData.UpVoteColor = DataManager.ACCENT_COLOR;
            this.UpVoteBottom[selectedIndex].Fill = (Brush) this.AccentColorBrush;
            this.UpVoteTop[selectedIndex].Fill = (Brush) this.AccentColorBrush;
            StoryData.Like = 1;
            ++StoryData.ups;
            ++StoryData.score;
            vote = 1;
            break;
          case 0:
            StoryData.UpVoteColor = DataManager.ACCENT_COLOR;
            this.UpVoteBottom[selectedIndex].Fill = (Brush) this.AccentColorBrush;
            this.UpVoteTop[selectedIndex].Fill = (Brush) this.AccentColorBrush;
            StoryData.Like = 1;
            ++StoryData.ups;
            ++StoryData.score;
            vote = 1;
            break;
          case 1:
            StoryData.Like = 0;
            --StoryData.ups;
            --StoryData.score;
            StoryData.UpVoteColor = RedditViewerViewModel.ARROW_NO_SELECT;
            this.UpVoteBottom[selectedIndex].Fill = (Brush) this.GrayColorBrush;
            this.UpVoteTop[selectedIndex].Fill = (Brush) this.GrayColorBrush;
            vote = 0;
            break;
        }
        this.StoryPoints[selectedIndex].Text = string.Concat((object) StoryData.score);
        ThreadPool.QueueUserWorkItem((WaitCallback) (obfj => App.DataManager.Vote(StoryData.RedditID, vote)));
      }
      else
        App.DataManager.MessageManager.ShowSignInMessage("vote");
    }

    private void DownVoteButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      if (App.DataManager.SettingsMan.IsSignedIn)
      {
        int selectedIndex = this.MainPivot.SelectedIndex;
        SubRedditData StoryData = this.StoryList[this.PiviotStoryNumber[selectedIndex]];
        int vote = 0;
        switch (StoryData.Like)
        {
          case -1:
            StoryData.Like = 0;
            --StoryData.downs;
            ++StoryData.score;
            StoryData.DownVoteColor = RedditViewerViewModel.ARROW_NO_SELECT;
            this.DownVoteBottom[selectedIndex].Fill = (Brush) this.GrayColorBrush;
            this.DownVoteTop[selectedIndex].Fill = (Brush) this.GrayColorBrush;
            vote = 0;
            break;
          case 0:
            StoryData.DownVoteColor = DataManager.ACCENT_COLOR;
            this.DownVoteBottom[selectedIndex].Fill = (Brush) this.AccentColorBrush;
            this.DownVoteTop[selectedIndex].Fill = (Brush) this.AccentColorBrush;
            StoryData.Like = -1;
            ++StoryData.downs;
            --StoryData.score;
            vote = -1;
            break;
          case 1:
            --StoryData.ups;
            --StoryData.score;
            StoryData.UpVoteColor = RedditViewerViewModel.ARROW_NO_SELECT;
            this.UpVoteBottom[selectedIndex].Fill = (Brush) this.GrayColorBrush;
            this.UpVoteTop[selectedIndex].Fill = (Brush) this.GrayColorBrush;
            StoryData.DownVoteColor = DataManager.ACCENT_COLOR;
            this.DownVoteBottom[selectedIndex].Fill = (Brush) this.AccentColorBrush;
            this.DownVoteTop[selectedIndex].Fill = (Brush) this.AccentColorBrush;
            StoryData.Like = -1;
            ++StoryData.downs;
            --StoryData.score;
            vote = -1;
            break;
        }
        this.StoryPoints[selectedIndex].Text = string.Concat((object) StoryData.score);
        ThreadPool.QueueUserWorkItem((WaitCallback) (obfj => App.DataManager.Vote(StoryData.RedditID, vote)));
      }
      else
        App.DataManager.MessageManager.ShowSignInMessage("vote");
    }

    private void StoryTitle_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      RedditFlipView.DEBUG_LastLocation = "titletap";
      SubRedditData story = this.StoryList[this.PiviotStoryNumber[this.MainPivot.SelectedIndex]];
      App.ActiveStoryData = story;
      this.NavigationService.Navigate(new Uri("/StoryDetails.xaml?StoryDataRedditID=" + story.RedditID, UriKind.Relative));
    }

    private void LineOne_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      RedditFlipView.DEBUG_LastLocation = "lienonetap";
      SubRedditData story = this.StoryList[this.PiviotStoryNumber[this.MainPivot.SelectedIndex]];
      App.ActiveStoryData = story;
      this.NavigationService.Navigate(new Uri("/StoryDetails.xaml?StoryDataRedditID=" + story.RedditID, UriKind.Relative));
    }

    private void UseFullSize_Click(object sender, EventArgs e)
    {
      this.UseSmallerImages = !this.UseSmallerImages;
      this.SetAppBarMode(this.currentStoryType, true);
      this.RequestedImages.Clear();
      try
      {
        RedditFlipView.DEBUG_LastLocation = "fullsizeclick";
        int num1 = this.PiviotStoryNumber[0];
        this.PiviotStoryNumber[0] = -1;
        this.setStoryToPiviot(this.StoryList[num1], num1, 0, false);
        int num2 = this.PiviotStoryNumber[1];
        this.PiviotStoryNumber[1] = -1;
        this.setStoryToPiviot(this.StoryList[num2], num2, 1, false);
        int num3 = this.PiviotStoryNumber[2];
        this.PiviotStoryNumber[2] = -1;
        this.setStoryToPiviot(this.StoryList[num3], num3, 2, false);
      }
      catch (Exception ex)
      {
        App.DataManager.DebugDia("Error chnging res", ex);
      }
    }

    private void SaveToPhone_Click(object sender, EventArgs e)
    {
      RedditFlipView.DEBUG_LastLocation = "savetophone";
      SubRedditData data = this.StoryList[this.PiviotStoryNumber[this.MainPivot.SelectedIndex]];
      if (this.StoryImages[this.MainPivot.SelectedIndex] == null)
        return;
      BitmapSource source = (BitmapSource) this.StoryImages[this.MainPivot.SelectedIndex].Source;
      MediaLibrary library = new MediaLibrary();
      byte[] imgByte = RedditFlipView.ConvertToBytes(source);
      ThreadPool.QueueUserWorkItem((WaitCallback) (obfj =>
      {
        library.SavePicture(data.Title + "_Baconit", imgByte);
        this.Dispatcher.BeginInvoke((Action) (() => App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("Image saved", true, true, "Image Saved", "'" + data.Title + "' was successfully saved to your phone."))));
      }));
    }

    public static byte[] ConvertToBytes(BitmapSource bitmapImage)
    {
      using (MemoryStream targetStream = new MemoryStream())
      {
        new WriteableBitmap(bitmapImage).SaveJpeg((Stream) targetStream, bitmapImage.PixelWidth, bitmapImage.PixelHeight, 0, 100);
        return targetStream.ToArray();
      }
    }

    private void ImageViewerHelp_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      if (this.IngoreClick())
        return;
      this.FadeOutTips.Begin();
    }

    private void showHelpTips_Click(object sender, EventArgs e)
    {
      this.ImageViewerHelp.Source = (ImageSource) new BitmapImage(new Uri("/Images/ImageViewerOverlay-" + (object) App.DataManager.SettingsMan.ScaleFactor + ".png", UriKind.Relative));
      this.ImageViewerHelp.Visibility = Visibility.Visible;
      this.SupportedOrientations = SupportedPageOrientation.Portrait;
      this.FadeInTips.Begin();
    }

    private void refreshWeb_Click(object sender, EventArgs e)
    {
      RedditFlipView.DEBUG_LastLocation = "refresh";
      SubRedditData story = this.StoryList[this.PiviotStoryNumber[this.MainPivot.SelectedIndex]];
      this.BrowserCurrentURL = "";
      this.NavBrowserToURL(story.URL, (string) null, this.MainPivot.SelectedIndex);
    }

    private void openInBroser_Click(object sender, EventArgs e)
    {
      SubRedditData story = this.StoryList[this.PiviotStoryNumber[this.MainPivot.SelectedIndex]];
      if (App.DataManager.SettingsMan.UseInAppBroser)
      {
        this.NavigationService.Navigate(new Uri("/InAppWebBrowser.xaml?Url=" + HttpUtility.UrlEncode(story.URL), UriKind.Relative));
      }
      else
      {
        WebBrowserTask webBrowserTask = new WebBrowserTask();
        webBrowserTask.Uri = new Uri(story.URL, UriKind.Absolute);
        try
        {
          webBrowserTask.Show();
        }
        catch
        {
        }
      }
    }

    private void optimize_Click(object sender, EventArgs e)
    {
      RedditFlipView.DEBUG_LastLocation = "optimize_click";
      SubRedditData story = this.StoryList[this.PiviotStoryNumber[this.MainPivot.SelectedIndex]];
      string webDomain = this.GetWebDomain(story.URL);
      bool flag = !App.DataManager.SettingsMan.OptimizeWebsites.ContainsKey(webDomain) ? !App.DataManager.SettingsMan.OptimizeWebByDefault : !App.DataManager.SettingsMan.OptimizeWebsites[webDomain];
      App.DataManager.SettingsMan.OptimizeWebsites[webDomain] = flag;
      this.BrowserCurrentURL = "";
      this.NavBrowserToURL(story.URL, (string) null, this.MainPivot.SelectedIndex);
      this.SetAppBarMode(2, true);
    }

    private void lockOren_Click(object sender, EventArgs e)
    {
      App.DataManager.SettingsMan.ScreenOrenLocked = !App.DataManager.SettingsMan.ScreenOrenLocked;
      if (App.DataManager.SettingsMan.ScreenOrenLocked)
      {
        if (this.Orientation == PageOrientation.Portrait || this.Orientation == PageOrientation.PortraitUp)
        {
          App.DataManager.SettingsMan.ScreenOrenLockedtoPortrait = true;
          this.SupportedOrientations = SupportedPageOrientation.Portrait;
        }
        else
        {
          App.DataManager.SettingsMan.ScreenOrenLockedtoPortrait = false;
          this.SupportedOrientations = SupportedPageOrientation.Landscape;
        }
      }
      else
        this.SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;
      this.SetAppBarMode(this.currentStoryType, true);
    }

    private void ShowImagesOnly_click(object sender, EventArgs e)
    {
      if (this.Host.FlipModeNumImages[this.currentList] > 2 || App.DataManager.SettingsMan.ShowImagesOnlyInFlipView)
      {
        App.DataManager.SettingsMan.ShowImagesOnlyInFlipView = !App.DataManager.SettingsMan.ShowImagesOnlyInFlipView;
        this.SetAppBarMode(this.currentStoryType, true);
      }
      else
      {
        int num = (int) MessageBox.Show("There are currently not enough images in this subreddit to show images only.", "Not Enough Images", MessageBoxButton.OK);
      }
    }

    private void WebBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      if (this.IngoreClick())
        return;
      int selectedIndex = this.MainPivot.SelectedIndex;
      if (this.isLocked)
      {
        this.isLocked = false;
        this.MainPivot.IsLocked = false;
        this.ShowStoryInfo[selectedIndex].Begin();
        this.WebControlBlock[selectedIndex].Visibility = Visibility.Visible;
        this.ApplicationBar.IsVisible = true;
      }
      else
      {
        this.isLocked = true;
        this.MainPivot.IsLocked = true;
        this.HideStoryInfo[selectedIndex].Begin();
        this.WebControlBlock[selectedIndex].Visibility = Visibility.Collapsed;
        this.ApplicationBar.IsVisible = false;
      }
      if (!App.DataManager.SettingsMan.FlipViewShowBrowserHelpTwo)
        return;
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        App.DataManager.SettingsMan.FlipViewShowBrowserHelpTwo = false;
        App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("New tip! Tap here", true, true, "Web Page Tip", "To exit web page interaction mode press the hardware back button."));
      }));
    }

    private void AddWebControlToUI(int pane)
    {
      if (this.WebControlInPane != pane)
      {
        this.RemoveWebControlFromUI();
        this.WebControlPlaceHolder[pane].Children.Add((UIElement) this.WebControlObj);
        this.WebControlInPane = pane;
      }
      this.WebControlHolder[pane].Visibility = Visibility.Visible;
    }

    private void RemoveWebControlFromUI()
    {
      if (this.WebControlInPane == this.MainPivot.SelectedIndex || this.WebControlInPane == -1)
        return;
      this.WebControlPlaceHolder[this.WebControlInPane].Children.Clear();
      this.WebControlHolder[this.WebControlInPane].Visibility = Visibility.Collapsed;
      this.WebControlInPane = -1;
    }

    private bool NavBrowserToURL(string url, string html, int RequestPane)
    {
      if (url != null && this.BrowserCurrentURL.Equals(url) || html != null && this.BrowserCurrentHTML.Equals(html))
        return false;
      bool flag = false;
      if (this.WebControlInPane == this.MainPivot.SelectedIndex)
      {
        if (RequestPane == this.MainPivot.SelectedIndex)
          flag = true;
      }
      else
        flag = true;
      if (flag)
      {
        if (!string.IsNullOrWhiteSpace(url))
        {
          string webDomain = this.GetWebDomain(url);
          if (App.DataManager.SettingsMan.OptimizeWebsites.ContainsKey(webDomain))
          {
            if (App.DataManager.SettingsMan.OptimizeWebsites[webDomain])
              url = "http://www.readability.com/m?url=" + url;
          }
          else if (App.DataManager.SettingsMan.OptimizeWebByDefault)
            url = "http://www.readability.com/m?url=" + url;
          Uri result;
          if (Uri.TryCreate(url, UriKind.Absolute, out result))
          {
            if (url.Contains("youtube.com") || url.Contains("youtu.be/"))
            {
              if (url.Contains("https://"))
                result = new Uri(url.Replace("https://", "http://"), UriKind.Absolute);
              string additionalHeaders = "User-Agent: User-Agent: Mozilla/5.0 (Windows Phone 8.1; ARM; Trident/7.0; Touch; rv:11.0; IEMobile/11.0; NOKIA; 909) like Gecko";
              this.WebControlObj.Navigate(result, (byte[]) null, additionalHeaders);
            }
            else
              this.WebControlObj.Navigate(result);
            this.BrowserCurrentURL = url;
            this.BrowserCurrentHTML = "";
            return true;
          }
        }
        else if (!string.IsNullOrWhiteSpace(html))
        {
          this.WebControlObj.NavigateToString(html);
          this.BrowserCurrentHTML = html;
          this.BrowserCurrentURL = "";
          return true;
        }
      }
      return false;
    }

    private void WebControlUI_Navigating_1(object sender, NavigatingEventArgs e)
    {
      int index = this.WebControlInPane;
      if (index == -1 && this.CurrentPanelType[this.MainPivot.SelectedIndex] == 1)
        index = this.MainPivot.SelectedIndex;
      if (index == -1)
        return;
      this.WebControlProgressBars[index].IsIndeterminate = true;
    }

    private void WebControlUI_Navigated_1(object sender, NavigationEventArgs e)
    {
      this.WebControlProgressBars[0].IsIndeterminate = false;
      this.WebControlProgressBars[1].IsIndeterminate = false;
      this.WebControlProgressBars[2].IsIndeterminate = false;
    }

    private void GestureListener_PinchStarted(object sender, PinchStartedGestureEventArgs e)
    {
      int selectedIndex = this.MainPivot.SelectedIndex;
      if (this.ImageTransforms[selectedIndex] == null)
        return;
      this._initialScale = this.ImageTransforms[selectedIndex].ScaleX;
      if (!this.OrgScaleSet)
      {
        this.OrgScaleSet = true;
        this.OrgScale = this._initialScale;
      }
      this.StoryImages[selectedIndex].CacheMode = (CacheMode) new BitmapCache();
      Point position1 = e.GetPosition((UIElement) this.StoryImages[selectedIndex], 0);
      Point position2 = e.GetPosition((UIElement) this.StoryImages[selectedIndex], 1);
      Point point = new Point(position1.X + (position2.X - position1.X) / 2.0, position1.Y + (position2.Y - position1.Y) / 2.0);
      this.ImageTransforms[selectedIndex].CenterX = point.X;
      this.ImageTransforms[selectedIndex].CenterY = point.Y;
    }

    private void GestureListener_PinchDelta(object sender, PinchGestureEventArgs e)
    {
      int selectedIndex = this.MainPivot.SelectedIndex;
      if (this.ImageTransforms[selectedIndex] == null)
        return;
      double num = this._initialScale * e.DistanceRatio;
      if (num < this.OrgScale)
        num = this.OrgScale;
      else if (!this.isLocked)
      {
        this.isLocked = true;
        this.MainPivot.IsLocked = true;
        this.HideStoryInfo[selectedIndex].Begin();
        this.ApplicationBar.IsVisible = false;
      }
      this.ImageTransforms[selectedIndex].ScaleX = num;
      this.ImageTransforms[selectedIndex].ScaleY = this.ImageTransforms[selectedIndex].ScaleX;
    }

    private void GestureListener_PinchCompleted(object sender, PinchGestureEventArgs e)
    {
      double num = this._initialScale * e.DistanceRatio;
      int selectedIndex = this.MainPivot.SelectedIndex;
      double orgScale = this.OrgScale;
      if (num > orgScale || !this.isLocked)
        return;
      this.isLocked = false;
      this.MainPivot.IsLocked = false;
      this.StoryImages[selectedIndex].CacheMode = (CacheMode) null;
      this.ShowStoryInfo[selectedIndex].Begin();
      this.ApplicationBar.IsVisible = true;
    }

    private void GestureListener_DragStarted(object sender, DragStartedGestureEventArgs e)
    {
      int selectedIndex = this.MainPivot.SelectedIndex;
      if (this.ImageTransforms[selectedIndex] == null)
        return;
      this._initialXCenter = this.ImageTransforms[selectedIndex].CenterX;
      this._initialYCenter = this.ImageTransforms[selectedIndex].CenterY;
    }

    private void GestureListener_DragDelta(object sender, DragDeltaGestureEventArgs e)
    {
      int selectedIndex = this.MainPivot.SelectedIndex;
      if (this.ImageTransforms[selectedIndex] == null)
        return;
      double scaleX = this.ImageTransforms[selectedIndex].ScaleX;
      this.ImageTransforms[selectedIndex].CenterX -= e.HorizontalChange * (6.0 / scaleX / scaleX);
      this.ImageTransforms[selectedIndex].CenterY -= e.VerticalChange * (6.0 / scaleX / scaleX);
    }

    private void GestureListener_Tap(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
    {
      if (this.IngoreClick())
        return;
      int selectedIndex = this.MainPivot.SelectedIndex;
      if (this.ImageTransforms[selectedIndex] == null)
        return;
      if (this.isLocked)
      {
        this.isLocked = false;
        this.MainPivot.IsLocked = false;
        this.ShowStoryInfo[selectedIndex].Begin();
        this.ApplicationBar.IsVisible = true;
        this.StoryImages[selectedIndex].CacheMode = (CacheMode) null;
      }
      else
      {
        this.isLocked = true;
        this.MainPivot.IsLocked = true;
        this.HideStoryInfo[selectedIndex].Begin();
        this.ApplicationBar.IsVisible = false;
      }
      this.ImageTransforms[selectedIndex].ClearValue(CompositeTransform.ScaleXProperty);
      this.ImageTransforms[selectedIndex].ClearValue(CompositeTransform.ScaleYProperty);
      this.ImageTransforms[selectedIndex].ClearValue(CompositeTransform.CenterXProperty);
      this.ImageTransforms[selectedIndex].ClearValue(CompositeTransform.CenterYProperty);
      e.Handled = true;
    }

    private void NSFWBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      if (this.IngoreClick())
        return;
      int SelectedIndex = this.MainPivot.SelectedIndex;
      if (this.NSFWBlockHolder[SelectedIndex].Visibility != Visibility.Visible)
        return;
      this.LastClickTime = BaconitStore.currentTime();
      Storyboard storyboard = new Storyboard();
      ExponentialEase exponentialEase1 = new ExponentialEase();
      exponentialEase1.EasingMode = EasingMode.EaseOut;
      exponentialEase1.Exponent = 6.0;
      ExponentialEase exponentialEase2 = exponentialEase1;
      PropertyPath path = new PropertyPath("(UIElement.Opacity)", new object[0]);
      DoubleAnimation element = new DoubleAnimation();
      element.Duration = new Duration(TimeSpan.FromMilliseconds(400.0));
      element.From = new double?(1.0);
      element.To = new double?(0.0);
      element.EasingFunction = (IEasingFunction) exponentialEase2;
      element.Completed += (EventHandler) delegate
      {
        this.NSFWBlockHolder[SelectedIndex].Opacity = 1.0;
        this.NSFWBlockHolder[SelectedIndex].Visibility = Visibility.Collapsed;
      };
      Storyboard.SetTarget((Timeline) element, (DependencyObject) this.NSFWBlockHolder[SelectedIndex]);
      Storyboard.SetTargetProperty((Timeline) element, path);
      storyboard.Children.Add((Timeline) element);
      storyboard.Begin();
    }

    private void ImageFailedBlock_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      if (this.IngoreClick())
        return;
      RedditFlipView.DEBUG_LastLocation = "imageFailed";
      SubRedditData story = this.StoryList[this.PiviotStoryNumber[this.MainPivot.SelectedIndex]];
      App.ActiveStoryData = story;
      this.NavigationService.Navigate(new Uri("/StoryDetails.xaml?StoryDataRedditID=" + story.RedditID, UriKind.Relative));
    }

    public void CreateImage(int piviotNum, RedditImage redditImage)
    {
      if (this.StoryImages[piviotNum] != null)
        this.DestroyImage(piviotNum);
      Image image = new Image();
      image.Stretch = Stretch.Uniform;
      image.Margin = new Thickness(12.0);
      image.Source = (ImageSource) redditImage.Image;
      CompositeTransform compositeTransform = new CompositeTransform();
      this.ImageTransforms[piviotNum] = compositeTransform;
      image.RenderTransform = (Transform) compositeTransform;
      GestureListener gestureListener = GestureService.GetGestureListener((DependencyObject) image);
      gestureListener.PinchStarted += new EventHandler<PinchStartedGestureEventArgs>(this.GestureListener_PinchStarted);
      gestureListener.Tap += new EventHandler<Microsoft.Phone.Controls.GestureEventArgs>(this.GestureListener_Tap);
      gestureListener.PinchDelta += new EventHandler<PinchGestureEventArgs>(this.GestureListener_PinchDelta);
      gestureListener.PinchCompleted += new EventHandler<PinchGestureEventArgs>(this.GestureListener_PinchCompleted);
      gestureListener.DragStarted += new EventHandler<DragStartedGestureEventArgs>(this.GestureListener_DragStarted);
      gestureListener.DragDelta += new EventHandler<DragDeltaGestureEventArgs>(this.GestureListener_DragDelta);
      this.StoryImages[piviotNum] = image;
      this.ImageHolders[piviotNum].Children.Add((UIElement) image);
      this.ImageHolders[piviotNum].Visibility = Visibility.Visible;
      this.lastImageInSlot[piviotNum] = redditImage.StoryID;
    }

    public void DestroyImage(int piviotNum)
    {
      if (this.StoryImages[piviotNum] == null)
        return;
      this.StoryImages[piviotNum].RenderTransform = (Transform) null;
      this.ImageTransforms[piviotNum] = (CompositeTransform) null;
      GestureListener gestureListener = GestureService.GetGestureListener((DependencyObject) this.StoryImages[piviotNum]);
      gestureListener.PinchStarted -= new EventHandler<PinchStartedGestureEventArgs>(this.GestureListener_PinchStarted);
      gestureListener.Tap -= new EventHandler<Microsoft.Phone.Controls.GestureEventArgs>(this.GestureListener_Tap);
      gestureListener.PinchDelta -= new EventHandler<PinchGestureEventArgs>(this.GestureListener_PinchDelta);
      gestureListener.PinchCompleted -= new EventHandler<PinchGestureEventArgs>(this.GestureListener_PinchCompleted);
      gestureListener.DragStarted -= new EventHandler<DragStartedGestureEventArgs>(this.GestureListener_DragStarted);
      gestureListener.DragDelta -= new EventHandler<DragDeltaGestureEventArgs>(this.GestureListener_DragDelta);
      this.ImageHolders[piviotNum].Visibility = Visibility.Collapsed;
      this.StoryImages[piviotNum].CacheMode = (CacheMode) null;
      this.StoryImages[piviotNum].Source = (ImageSource) null;
      this.StoryImages[piviotNum] = (Image) null;
      this.ImageHolders[piviotNum].Children.Clear();
      GC.Collect();
      try
      {
        if (this.lastImageInSlot[piviotNum] != null)
          this.RequestedImages.Remove(this.lastImageInSlot[piviotNum]);
        this.lastImageInSlot[piviotNum] = (string) null;
      }
      catch
      {
      }
    }

    public bool IngoreClick()
    {
      if (BaconitStore.currentTime() - this.LastClickTime < 50.0)
        return true;
      this.LastClickTime = BaconitStore.currentTime();
      return false;
    }

    private void sharePicker_closed(object sender, EventArgs e)
    {
      RedditFlipView.DEBUG_LastLocation = "sharepickerclosed";
      SubRedditData story = this.StoryList[this.PiviotStoryNumber[this.MainPivot.SelectedIndex]];
      switch (this.sharePicker.SelectedIndex)
      {
        case 0:
          ShareLinkTask shareLinkTask = new ShareLinkTask();
          shareLinkTask.Title = story.Title;
          shareLinkTask.LinkUri = new Uri(story.URL, UriKind.Absolute);
          shareLinkTask.Message = story.Title;
          try
          {
            shareLinkTask.Show();
            break;
          }
          catch
          {
            break;
          }
        case 1:
          App.DataManager.ShareHelper.ShareNFC(story.URL);
          break;
        case 2:
          SmsComposeTask smsComposeTask = new SmsComposeTask();
          smsComposeTask.Body = "\"" + story.Title + "\" " + story.URL;
          try
          {
            smsComposeTask.Show();
            break;
          }
          catch
          {
            break;
          }
        case 3:
          EmailComposeTask emailComposeTask = new EmailComposeTask();
          emailComposeTask.Body = story.Title + "\n\r" + story.URL;
          emailComposeTask.Subject = "\"" + story.Title + "\"";
          try
          {
            emailComposeTask.Show();
            break;
          }
          catch
          {
            break;
          }
        case 4:
          Clipboard.SetText(story.URL);
          App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("Link copied", true, true, "Link Copied", "Story link copied to the clipboard."));
          break;
      }
    }

    private void Share_Click(object sender, EventArgs e) => this.sharePicker.Show();

    private void VoteFlic_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
    {
      if (!App.DataManager.SettingsMan.SwipeToVote || Math.Abs(e.FinalVelocities.LinearVelocity.Y) <= 900.0 || Math.Abs(e.TotalManipulation.Translation.Y) <= 60.0 || !App.DataManager.SettingsMan.IsSignedIn || this.MainPivot.IsLocked || Math.Abs(e.TotalManipulation.Translation.X) >= 80.0)
        return;
      if (e.TotalManipulation.Translation.Y < 0.0)
        this.UpVoteButton_Tap((object) null, (System.Windows.Input.GestureEventArgs) null);
      else
        this.DownVoteButton_Tap((object) null, (System.Windows.Input.GestureEventArgs) null);
    }

    private string GetWebDomain(string url)
    {
      try
      {
        url = url.Trim().ToLower();
        if (url.StartsWith("http://"))
          url = url.Substring(7);
        else if (url.StartsWith("https://"))
          url = url.Substring(8);
        int length = url.IndexOf("/");
        if (length != -1)
          url = url.Substring(0, length);
        int num = url.IndexOf(".");
        return url.IndexOf(".", num + 1) == -1 ? url : url.Substring(num + 1);
      }
      catch (Exception ex)
      {
        App.DataManager.MessageManager.DebugDia("getWebDomain failed", ex);
      }
      return "";
    }

    private void CopyTitle_Click(object sender, RoutedEventArgs e)
    {
      Clipboard.SetText(this.StoryList[this.PiviotStoryNumber[this.MainPivot.SelectedIndex]].Title);
      App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("Text copied", true, false, "", ""));
    }

    private void SelfTextCopy_Click(object sender, RoutedEventArgs e)
    {
      SubRedditData story = this.StoryList[this.PiviotStoryNumber[this.MainPivot.SelectedIndex]];
      try
      {
        Clipboard.SetText(story.SelfText);
        App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("Text copied", true, false, "", ""));
      }
      catch
      {
      }
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/RedditFlipView.xaml", UriKind.Relative));
      this.ShowTitleBox1 = (Storyboard) this.FindName("ShowTitleBox1");
      this.HideTitleBox1 = (Storyboard) this.FindName("HideTitleBox1");
      this.ShowTitleBox2 = (Storyboard) this.FindName("ShowTitleBox2");
      this.ShowTitleBoxTranKeyFrame2 = (EasingDoubleKeyFrame) this.FindName("ShowTitleBoxTranKeyFrame2");
      this.HideTitleBox2 = (Storyboard) this.FindName("HideTitleBox2");
      this.HideTitleBoxTranKeyFrame2 = (EasingDoubleKeyFrame) this.FindName("HideTitleBoxTranKeyFrame2");
      this.ShowTitleBox3 = (Storyboard) this.FindName("ShowTitleBox3");
      this.HideTitleBox3 = (Storyboard) this.FindName("HideTitleBox3");
      this.FadeOutTips = (Storyboard) this.FindName("FadeOutTips");
      this.FadeInTips = (Storyboard) this.FindName("FadeInTips");
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.ContentPanel = (Grid) this.FindName("ContentPanel");
      this.MainPivot = (Pivot) this.FindName("MainPivot");
      this.TitlePanel1 = (Grid) this.FindName("TitlePanel1");
      this.UpVoteButton1 = (StackPanel) this.FindName("UpVoteButton1");
      this.UpVoteTop1 = (Polygon) this.FindName("UpVoteTop1");
      this.UpVoteBottom1 = (Rectangle) this.FindName("UpVoteBottom1");
      this.StoryScore1 = (TextBlock) this.FindName("StoryScore1");
      this.DownVoteButton1 = (StackPanel) this.FindName("DownVoteButton1");
      this.DownVoteTop1 = (Rectangle) this.FindName("DownVoteTop1");
      this.DownVoteBottom1 = (Polygon) this.FindName("DownVoteBottom1");
      this.StoryTitle1 = (TextBlock) this.FindName("StoryTitle1");
      this.LineOne1 = (TextBlock) this.FindName("LineOne1");
      this.LineTwo1 = (TextBlock) this.FindName("LineTwo1");
      this.WebControlHolder1 = (Grid) this.FindName("WebControlHolder1");
      this.WebControlPlaceHolder1 = (Grid) this.FindName("WebControlPlaceHolder1");
      this.WebControlUI = (WebBrowser) this.FindName("WebControlUI");
      this.WebControlLoading1 = (ProgressBar) this.FindName("WebControlLoading1");
      this.WebControlBlock1 = (Grid) this.FindName("WebControlBlock1");
      this.SelfTextHolder1 = (Grid) this.FindName("SelfTextHolder1");
      this.SelfTextScroller1 = (ScrollViewer) this.FindName("SelfTextScroller1");
      this.SelfTextRichBox1 = (SuperRichTextBox) this.FindName("SelfTextRichBox1");
      this.OnlyComments1 = (Grid) this.FindName("OnlyComments1");
      this.ImageHolder1 = (Grid) this.FindName("ImageHolder1");
      this.LoadingImageBlock1 = (Grid) this.FindName("LoadingImageBlock1");
      this.LoadingImageBlock1Progress = (ProgressBar) this.FindName("LoadingImageBlock1Progress");
      this.NSFWBlock1 = (Grid) this.FindName("NSFWBlock1");
      this.TitlePanel2 = (Grid) this.FindName("TitlePanel2");
      this.UpVoteButton2 = (StackPanel) this.FindName("UpVoteButton2");
      this.UpVoteTop2 = (Polygon) this.FindName("UpVoteTop2");
      this.UpVoteBottom2 = (Rectangle) this.FindName("UpVoteBottom2");
      this.StoryScore2 = (TextBlock) this.FindName("StoryScore2");
      this.DownVoteButton2 = (StackPanel) this.FindName("DownVoteButton2");
      this.DownVoteTop2 = (Rectangle) this.FindName("DownVoteTop2");
      this.DownVoteBottom2 = (Polygon) this.FindName("DownVoteBottom2");
      this.StoryTitle2 = (TextBlock) this.FindName("StoryTitle2");
      this.LineOne2 = (TextBlock) this.FindName("LineOne2");
      this.LineTwo2 = (TextBlock) this.FindName("LineTwo2");
      this.WebControlHolder2 = (Grid) this.FindName("WebControlHolder2");
      this.WebControlPlaceHolder2 = (Grid) this.FindName("WebControlPlaceHolder2");
      this.WebControlLoading2 = (ProgressBar) this.FindName("WebControlLoading2");
      this.WebControlBlock2 = (Grid) this.FindName("WebControlBlock2");
      this.SelfTextHolder2 = (Grid) this.FindName("SelfTextHolder2");
      this.SelfTextScroller2 = (ScrollViewer) this.FindName("SelfTextScroller2");
      this.SelfTextRichBox2 = (SuperRichTextBox) this.FindName("SelfTextRichBox2");
      this.OnlyComments2 = (Grid) this.FindName("OnlyComments2");
      this.ImageHolder2 = (Grid) this.FindName("ImageHolder2");
      this.LoadingImageBlock2 = (Grid) this.FindName("LoadingImageBlock2");
      this.LoadingImageBlock2Progress = (ProgressBar) this.FindName("LoadingImageBlock2Progress");
      this.NSFWBlock2 = (Grid) this.FindName("NSFWBlock2");
      this.TitlePanel3 = (Grid) this.FindName("TitlePanel3");
      this.UpVoteButton3 = (StackPanel) this.FindName("UpVoteButton3");
      this.UpVoteTop3 = (Polygon) this.FindName("UpVoteTop3");
      this.UpVoteBottom3 = (Rectangle) this.FindName("UpVoteBottom3");
      this.StoryScore3 = (TextBlock) this.FindName("StoryScore3");
      this.DownVoteButton3 = (StackPanel) this.FindName("DownVoteButton3");
      this.DownVoteTop3 = (Rectangle) this.FindName("DownVoteTop3");
      this.DownVoteBottom3 = (Polygon) this.FindName("DownVoteBottom3");
      this.StoryTitle3 = (TextBlock) this.FindName("StoryTitle3");
      this.LineOne3 = (TextBlock) this.FindName("LineOne3");
      this.LineTwo3 = (TextBlock) this.FindName("LineTwo3");
      this.WebControlHolder3 = (Grid) this.FindName("WebControlHolder3");
      this.WebControlPlaceHolder3 = (Grid) this.FindName("WebControlPlaceHolder3");
      this.WebControlLoading3 = (ProgressBar) this.FindName("WebControlLoading3");
      this.WebControlBlock3 = (Grid) this.FindName("WebControlBlock3");
      this.SelfTextHolder3 = (Grid) this.FindName("SelfTextHolder3");
      this.SelfTextScroller3 = (ScrollViewer) this.FindName("SelfTextScroller3");
      this.SelfTextRichBox3 = (SuperRichTextBox) this.FindName("SelfTextRichBox3");
      this.OnlyComments3 = (Grid) this.FindName("OnlyComments3");
      this.ImageHolder3 = (Grid) this.FindName("ImageHolder3");
      this.LoadingImageBlock3 = (Grid) this.FindName("LoadingImageBlock3");
      this.LoadingImageBlock3Progress = (ProgressBar) this.FindName("LoadingImageBlock3Progress");
      this.NSFWBlock3 = (Grid) this.FindName("NSFWBlock3");
      this.ImageViewerHelp = (Image) this.FindName("ImageViewerHelp");
    }
  }
}
