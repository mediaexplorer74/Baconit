// Decompiled with JetBrains decompiler
// Type: Baconit.RedditsViewer
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Baconit.Database;
using Baconit.SettingPages;
using BaconitData.Interfaces;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Phone.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

#nullable disable
namespace Baconit
{
  public class RedditsViewer : PhoneApplicationPage
  {
    public static NavigationInTransition TurnstileInTransition;
    public static NavigationOutTransition TurnstileOutTransition;
    public SubReddit LocalSubreddit;
    public RedditViewerViewModel ViewModel;
    public ProgressBar[] LoadingBars = new ProgressBar[4];
    public ListBox[] ListBoxes = new ListBox[4];
    public bool[] FlipModeEnabled = new bool[4];
    public int[] FlipModeNumImages = new int[4];
    public int CurrentShownItem;
    public List<KeyValuePair<FrameworkElement, SubRedditData>> AnimatedOut = new List<KeyValuePair<FrameworkElement, SubRedditData>>();
    public bool isOpen;
    private bool isOpening = true;
    private ShellTile SubRedditTileData;
    public SubRedditData SubRedditImageStory;
    private PageOrientation CurOrientation;
    private bool isPinned;
    private bool isLoadingSubreddit;
    private bool isPinnedOptionsOpened;
    private bool[] RestoreProgressBars = new bool[4];
    public int TypeOfTopShowing = 2;
    public int TypeOfNewShowing = 1;
    private bool OnNavInitDone;
    private bool SecondPartSetupDone;
    public static RedditsViewer Current;
    public readonly DependencyProperty ListVerticalOffsetPropertyTop = DependencyProperty.Register("ListVerticalOffset", typeof (double), typeof (RedditsViewer), new PropertyMetadata(new PropertyChangedCallback(RedditsViewer.OnListVerticalOffsetChangedTop)));
    public readonly DependencyProperty ListVerticalOffsetPropertyNew = DependencyProperty.Register("ListVerticalOffset", typeof (double), typeof (RedditsViewer), new PropertyMetadata(new PropertyChangedCallback(RedditsViewer.OnListVerticalOffsetChangedNew)));
    public readonly DependencyProperty ListVerticalOffsetPropertyConv = DependencyProperty.Register("ListVerticalOffset", typeof (double), typeof (RedditsViewer), new PropertyMetadata(new PropertyChangedCallback(RedditsViewer.OnListVerticalOffsetChangedConv)));
    public readonly DependencyProperty ListVerticalOffsetPropertyHot = DependencyProperty.Register("ListVerticalOffset", typeof (double), typeof (RedditsViewer), new PropertyMetadata(new PropertyChangedCallback(RedditsViewer.OnListVerticalOffsetChangedHot)));
    private ScrollViewer _listScrollViewerTop;
    private ScrollViewer _listScrollViewerNew;
    private ScrollViewer _listScrollViewerHot;
    private ScrollViewer _listScrollViewerConv;
    public int lastCountHot = -1;
    public int lastCountTop = -1;
    public int lastCountNew = -1;
    public int lastCountConv = -1;
    internal Storyboard FadeOutTips;
    internal Storyboard FadeInTips;
    internal Storyboard FadeOutLoading;
    internal Storyboard OpenPinPicker;
    internal Storyboard ClosePinPicker;
    internal Grid LayoutRoot;
    internal Pivot mainPivot;
    internal ProgressBar HotLoading;
    internal ListBox HotList;
    internal TextBlock HotNoStories;
    internal ProgressBar NewLoading;
    internal ListBox NewList;
    internal TextBlock NewNoStories;
    internal ProgressBar TopLoading;
    internal ListBox TopList;
    internal TextBlock TopNoStories;
    internal ProgressBar ControversialLoading;
    internal ListBox ControversialList;
    internal TextBlock ConvNoStories;
    internal Image StoryHelpOverlay;
    internal Grid LoadingSubreddit;
    internal Rectangle LoggingInOverlayBack;
    internal ProgressBar LoadingSubredditLoadingBar;
    internal Grid FullScreenBlackBackground;
    internal Grid TilePicker;
    internal ListPicker TileIcon;
    internal TextBlock PinSettingsSubNameImage;
    internal Image PinSettingsSampleImage;
    internal TextBlock PinSettingsSubName;
    private bool _contentLoaded;

    public RedditsViewer()
    {
      this.InitializeComponent();
      if (RedditsViewer.TurnstileInTransition == null || RedditsViewer.TurnstileOutTransition == null)
      {
        NavigationInTransition navigationInTransition = new NavigationInTransition();
        navigationInTransition.Forward = (TransitionElement) new TurnstileFeatherTransition()
        {
          Mode = TurnstileFeatherTransitionMode.ForwardIn
        };
        RedditsViewer.TurnstileInTransition = navigationInTransition;
        NavigationOutTransition navigationOutTransition = new NavigationOutTransition();
        navigationOutTransition.Backward = (TransitionElement) new TurnstileFeatherTransition()
        {
          Mode = TurnstileFeatherTransitionMode.BackwardOut
        };
        RedditsViewer.TurnstileOutTransition = navigationOutTransition;
      }
      TransitionService.SetNavigationInTransition((UIElement) this, RedditsViewer.TurnstileInTransition);
      TransitionService.SetNavigationOutTransition((UIElement) this, RedditsViewer.TurnstileOutTransition);
      this.ApplicationBar = (IApplicationBar) new Microsoft.Phone.Shell.ApplicationBar();
      this.ApplicationBar.IsVisible = !App.DataManager.SettingsMan.ShowStoryListOverLay;
      ApplicationBarIconButton applicationBarIconButton1 = new ApplicationBarIconButton(new Uri("/Images/appbar.sync.rest.png", UriKind.Relative));
      applicationBarIconButton1.Text = "refresh";
      applicationBarIconButton1.Click += new EventHandler(this.refresh_Click);
      this.ApplicationBar.Buttons.Add((object) applicationBarIconButton1);
      ApplicationBarIconButton applicationBarIconButton2 = new ApplicationBarIconButton(new Uri("/Images/appbar.flipview.png", UriKind.Relative));
      applicationBarIconButton2.Text = "flip view";
      applicationBarIconButton2.IsEnabled = false;
      applicationBarIconButton2.Click += new EventHandler(this.gotoImageViewer_Click);
      this.ApplicationBar.Buttons.Add((object) applicationBarIconButton2);
      ApplicationBarIconButton applicationBarIconButton3 = new ApplicationBarIconButton(new Uri("/Images/appbar.add.png", UriKind.Relative));
      applicationBarIconButton3.Text = "submit";
      applicationBarIconButton3.Click += new EventHandler(this.submitorSortStory_Click);
      this.ApplicationBar.Buttons.Add((object) applicationBarIconButton3);
      ApplicationBarIconButton applicationBarIconButton4 = new ApplicationBarIconButton(new Uri("/Images/pushpin.png", UriKind.Relative));
      applicationBarIconButton4.Text = "pin to start";
      applicationBarIconButton4.Click += new EventHandler(this.PinToStart_Click);
      this.ApplicationBar.Buttons.Add((object) applicationBarIconButton4);
      ApplicationBarMenuItem applicationBarMenuItem1 = new ApplicationBarMenuItem("lock screen orientation");
      applicationBarMenuItem1.Click += new EventHandler(this.lockOren_Click);
      this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem1);
      ApplicationBarMenuItem applicationBarMenuItem2 = !App.DataManager.SettingsMan.ShowReadStories ? new ApplicationBarMenuItem("show read stories") : new ApplicationBarMenuItem("hide read stories");
      applicationBarMenuItem2.Click += new EventHandler(this.ShowReadStories_Click);
      this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem2);
      ApplicationBarMenuItem applicationBarMenuItem3 = new ApplicationBarMenuItem("show help tips");
      applicationBarMenuItem3.Click += new EventHandler(this.showTips_Click);
      this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem3);
      this.LoadingBars[0] = this.HotLoading;
      this.LoadingBars[1] = this.NewLoading;
      this.LoadingBars[2] = this.TopLoading;
      this.LoadingBars[3] = this.ControversialLoading;
      this.ListBoxes[0] = this.HotList;
      this.ListBoxes[1] = this.NewList;
      this.ListBoxes[2] = this.TopList;
      this.ListBoxes[3] = this.ControversialList;
      this.ViewModel = new RedditViewerViewModel(this);
      this.DataContext = (object) this.ViewModel;
      this.Loaded += new RoutedEventHandler(this.RedditsViewer_Loaded);
      if (SubRedditData.NewCommentsColor == null)
      {
        Color resource = (Color) Application.Current.Resources[(object) "PhoneAccentColor"];
        SubRedditData.NewCommentsColor = new Color()
        {
          A = byte.MaxValue,
          B = ((byte) Math.Min((int) resource.B + 80, (int) byte.MaxValue)),
          G = ((byte) Math.Min((int) resource.G + 80, (int) byte.MaxValue)),
          R = ((byte) Math.Min((int) resource.R + 80, (int) byte.MaxValue))
        }.ToString();
        SubRedditData.UnReadTitleColor = ((Color) Application.Current.Resources[(object) "PhoneForegroundColor"]).ToString();
        SubRedditData.ReadTitleColor = ((Color) Application.Current.Resources[(object) "PhoneSubtleColor"]).ToString();
      }
      if (!DataManager.LIGHT_THEME)
        return;
      this.FullScreenBlackBackground.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      this.isOpen = true;
      RedditsViewer.Current = this;
      App.DataManager.BaconitAnalytics.LogPage("Reddit Viewer");
      if (App.navService == null)
        App.navService = this.NavigationService;
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        if (this.Orientation == PageOrientation.Landscape || this.Orientation == PageOrientation.LandscapeRight || this.Orientation == PageOrientation.LandscapeLeft)
          SystemTray.IsVisible = false;
        if (App.DataManager.SettingsMan.ScreenOrenLocked)
        {
          if (App.DataManager.SettingsMan.ScreenOrenLockedtoPortrait)
          {
            SystemTray.IsVisible = true;
            this.SupportedOrientations = SupportedPageOrientation.Portrait;
          }
          else
          {
            this.SupportedOrientations = SupportedPageOrientation.Landscape;
            SystemTray.IsVisible = false;
          }
          ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[0]).Text = "unlock screen orientation";
        }
        else
        {
          this.SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;
          ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[0]).Text = "lock screen orientation";
        }
        this.CurOrientation = this.Orientation;
        for (int index = 0; index < 4; ++index)
        {
          switch (index)
          {
            case 0:
              if (this.RestoreProgressBars[0])
                this.HotLoading.IsIndeterminate = true;
              this.RestoreProgressBars[0] = false;
              break;
            case 1:
              if (this.RestoreProgressBars[1])
                this.NewLoading.IsIndeterminate = true;
              this.RestoreProgressBars[1] = false;
              break;
            case 2:
              if (this.RestoreProgressBars[2])
                this.TopLoading.IsIndeterminate = true;
              this.RestoreProgressBars[2] = false;
              break;
            case 3:
              if (this.RestoreProgressBars[3])
                this.ControversialLoading.IsIndeterminate = true;
              this.RestoreProgressBars[3] = false;
              break;
          }
        }
        if (this.OnNavInitDone)
          return;
        if (this.NavigationContext.QueryString.ContainsKey("ClearBack"))
          this.NavigationService.RemoveBackEntry();
        this.OnNavInitDone = true;
      }));
      string url = "";
      if (this.LocalSubreddit == null && this.NavigationContext.QueryString.ContainsKey("subredditURL"))
      {
        url = this.NavigationContext.QueryString["subredditURL"].ToLower();
        foreach (SubReddit subSortedReddit in App.DataManager.SubredditDataManager.GetSubSortedReddits())
        {
          if (url.Equals(subSortedReddit.URL.ToLower()))
          {
            this.LocalSubreddit = subSortedReddit;
            break;
          }
        }
      }
      if (this.NavigationContext.QueryString.ContainsKey("TileType") && this.NavigationService.CanGoBack)
        this.NavigationService.RemoveBackEntry();
      if (this.LocalSubreddit == null)
      {
        this.isLoadingSubreddit = true;
        this.ApplicationBar.IsVisible = false;
        if (url != null && !url.Equals(""))
        {
          this.LoadingSubreddit.Visibility = Visibility.Visible;
          this.LoadingSubredditLoadingBar.IsIndeterminate = true;
          App.DataManager.CheckIfValidSubreddit(url, false, new RunWorkerCompletedEventHandler(this.LoadSubreddit_RunWorkerCompleted));
        }
        else
        {
          int num = (int) MessageBox.Show("This subreddit can't be loaded. If pinned, please unpin and repin the subreddit.", "Error Loading Subreddit", MessageBoxButton.OK);
          if (!this.NavigationService.CanGoBack)
            return;
          this.Dispatcher.BeginInvoke((Action) (() => this.NavigationService.GoBack()));
        }
      }
      else
        this.SecondPartSetup();
    }

    private void SecondPartSetup()
    {
      if (this.SecondPartSetupDone)
        return;
      if (App.navService == null)
        App.navService = this.NavigationService;
      ThreadPool.QueueUserWorkItem((WaitCallback) (obfj =>
      {
        if (!this.LocalSubreddit.URL.Equals("/") && !this.LocalSubreddit.URL.Equals("/r/all/") && !this.LocalSubreddit.URL.Equals("/saved/") && !this.LocalSubreddit.URL.Equals("/r/friends/") && !this.LocalSubreddit.URL.Contains("+"))
        {
          List<SubReddit> subSortedReddits = App.DataManager.SubredditDataManager.GetSubSortedReddits();
          SubReddit subReddit1 = (SubReddit) null;
          foreach (SubReddit subReddit2 in subSortedReddits)
          {
            if (subReddit2.RName.Equals(this.LocalSubreddit.RName))
            {
              subReddit1 = subReddit2;
              break;
            }
          }
          ApplicationBarMenuItem Account;
          ApplicationBarMenuItem Local;
          if (subReddit1 != null)
          {
            Account = !subReddit1.isAccount ? new ApplicationBarMenuItem("subscribe to account") : new ApplicationBarMenuItem("unsubscribe from account");
            if (subReddit1.isLocal)
            {
              Local = new ApplicationBarMenuItem("unsubscribe from local");
              Account.IsEnabled = false;
            }
            else
              Local = new ApplicationBarMenuItem("subscribe to local");
            if (subReddit1.isAccount)
              Local.IsEnabled = false;
          }
          else
          {
            Account = new ApplicationBarMenuItem("subscribe to account");
            Local = new ApplicationBarMenuItem("subscribe to local");
          }
          this.Dispatcher.BeginInvoke((Action) (() =>
          {
            if (App.DataManager.SettingsMan.IsSignedIn)
            {
              Account.Click += new EventHandler(this.changeSubAccount_Click);
              this.ApplicationBar.MenuItems.Add((object) Account);
            }
            Local.Click += new EventHandler(this.changeSubLocal_Click);
            this.ApplicationBar.MenuItems.Add((object) Local);
          }));
        }
        App.DataManager.SettingsMan.AddSubredditViewCount(this.LocalSubreddit);
        foreach (ShellTile activeTile in ShellTile.ActiveTiles)
        {
          if (HttpUtility.UrlDecode(activeTile.NavigationUri.ToString()).Contains("subredditURL=" + this.LocalSubreddit.URL) && (!this.LocalSubreddit.URL.Equals("/") || !activeTile.NavigationUri.ToString().Contains("subredditURL=/r/")))
          {
            this.SubRedditTileData = activeTile;
            break;
          }
        }
        if (this.SubRedditTileData == null)
          return;
        this.isPinned = true;
        this.Dispatcher.BeginInvoke((Action) (() => ((ApplicationBarIconButton) this.ApplicationBar.Buttons[3]).IsEnabled = false));
      }));
      this.SecondPartSetupDone = true;
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      base.OnNavigatedFrom(e);
      this.isOpen = false;
      App.RedditViewerTransfter = this;
      App.ViewModelTransfter = this.ViewModel;
      this.LoadingSubredditLoadingBar.IsIndeterminate = false;
      for (int index = 0; index < 4; ++index)
      {
        switch (index)
        {
          case 0:
            if (this.HotLoading.IsIndeterminate)
              this.RestoreProgressBars[0] = true;
            this.HotLoading.IsIndeterminate = false;
            break;
          case 1:
            if (this.NewLoading.IsIndeterminate)
              this.RestoreProgressBars[1] = true;
            this.NewLoading.IsIndeterminate = false;
            break;
          case 2:
            if (this.TopLoading.IsIndeterminate)
              this.RestoreProgressBars[2] = true;
            this.TopLoading.IsIndeterminate = false;
            break;
          case 3:
            if (this.ControversialLoading.IsIndeterminate)
              this.RestoreProgressBars[3] = true;
            this.ControversialLoading.IsIndeterminate = false;
            break;
        }
      }
    }

    protected override void OnBackKeyPress(CancelEventArgs e)
    {
      if (this.isOpening)
      {
        e.Cancel = true;
      }
      else
      {
        if (this.isPinnedOptionsOpened)
        {
          e.Cancel = true;
          this.ClosePinPicker.Begin();
        }
        else if (!this.NavigationService.CanGoBack)
        {
          this.NavigationService.Navigate(new Uri("/MainLanding.xaml?RemoveBack=true", UriKind.Relative));
          e.Cancel = true;
        }
        base.OnBackKeyPress(e);
      }
    }

    private void RedditsViewer_Loaded(object sender, RoutedEventArgs e)
    {
      this.isOpening = false;
      if (this.isLoadingSubreddit || this.ViewModel.IsDataLoaded)
        return;
      this.ViewModel.LoadData();
      this.CurrentShownItem = 0;
      if (this.LocalSubreddit != null)
        this.mainPivot.Title = (object) this.LocalSubreddit.DisplayName.ToUpper();
      ThreadPool.QueueUserWorkItem((WaitCallback) (obfj =>
      {
        if (!this.ViewModel.IsDataLoaded)
          this.ViewModel.LoadData();
        if (!App.DataManager.SettingsMan.ShowStoryListOverLay)
          return;
        this.Dispatcher.BeginInvoke((Action) (() =>
        {
          this.ApplicationBar.IsVisible = false;
          this.StoryHelpOverlay.Source = (ImageSource) new BitmapImage(new Uri("/Images/StoryListOverlay-" + (object) App.DataManager.SettingsMan.ScaleFactor + ".png", UriKind.Relative));
          this.StoryHelpOverlay.Visibility = Visibility.Visible;
          this.SupportedOrientations = SupportedPageOrientation.Portrait;
        }));
      }));
    }

    private void RedditViewer_OrientationChanged(object sender, OrientationChangedEventArgs e)
    {
      PageOrientation orientation = e.Orientation;
      RotateTransition rotateTransition = new RotateTransition();
      switch (orientation)
      {
        case PageOrientation.Portrait:
        case PageOrientation.PortraitUp:
          SystemTray.IsVisible = true;
          rotateTransition.Mode = this.CurOrientation != PageOrientation.LandscapeLeft ? RotateTransitionMode.In90Clockwise : RotateTransitionMode.In90Counterclockwise;
          break;
        case PageOrientation.Landscape:
        case PageOrientation.LandscapeRight:
          SystemTray.IsVisible = false;
          rotateTransition.Mode = this.CurOrientation != PageOrientation.PortraitUp ? RotateTransitionMode.In180Clockwise : RotateTransitionMode.In90Counterclockwise;
          break;
        case PageOrientation.LandscapeLeft:
          SystemTray.IsVisible = false;
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

    public EasingFunctionBase LocalEasingFunction { get; set; }

    private void StoryTitle_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      bool flag1 = false;
      if (sender.GetType().Name.Equals("StackPanel"))
        flag1 = true;
      SubRedditData dataContext = (sender as FrameworkElement).DataContext as SubRedditData;
      if (flag1)
        this.AnimateStoryOut((FrameworkElement) ((FrameworkElement) sender).Parent, dataContext);
      else
        this.AnimateStoryOut((FrameworkElement) ((FrameworkElement) ((FrameworkElement) sender).Parent).Parent, dataContext);
      App.ActiveStoryData = dataContext;
      dataContext.NewCommentsText = "";
      App.DataManager.SettingsMan.AddReadStory(dataContext.RedditID);
      dataContext.TitleColor = SubRedditData.ReadTitleColor;
      if (App.ActiveStoryData.isSelf)
      {
        if (App.DataManager.SettingsMan.OpenSelfTextIntoFlipView)
          this.NavigationService.Navigate(new Uri("/RedditFlipView.xaml?WhichList=" + (object) this.CurrentShownItem + "&WhichVirtList=" + (object) this.GetVirturalShowing() + "&StoryID=" + App.ActiveStoryData.RedditID, UriKind.Relative));
        else
          this.NavigationService.Navigate(new Uri("/StoryDetails.xaml?StoryDataRedditID=" + App.ActiveStoryData.RedditID, UriKind.Relative));
      }
      else
      {
        bool flag2 = false;
        string uriString = (string) null;
        try
        {
          uriString = DataManager.GetLocalNavStringFromRedditURL(dataContext.URL);
          if (!string.IsNullOrEmpty(uriString))
            flag2 = true;
        }
        catch
        {
          flag2 = false;
        }
        if (flag2 && uriString != null)
          this.NavigationService.Navigate(new Uri(uriString, UriKind.Relative));
        else
          this.NavigationService.Navigate(new Uri("/RedditFlipView.xaml?WhichList=" + (object) this.CurrentShownItem + "&WhichVirtList=" + (object) this.GetVirturalShowing() + "&StoryID=" + App.ActiveStoryData.RedditID, UriKind.Relative));
      }
    }

    public void SetStoryRead(string redditid)
    {
      foreach (SubRedditData hotStorey in (Collection<SubRedditData>) this.ViewModel.HotStories)
      {
        if (hotStorey.RedditID.Equals(redditid))
        {
          hotStorey.TitleColor = SubRedditData.ReadTitleColor;
          if (App.DataManager.SettingsMan.ShowReadStories)
            return;
          hotStorey.StoryReadVis = Visibility.Collapsed;
          return;
        }
      }
      foreach (SubRedditData newStorey in (Collection<SubRedditData>) this.ViewModel.NewStories)
      {
        if (newStorey.RedditID.Equals(redditid))
        {
          newStorey.TitleColor = SubRedditData.ReadTitleColor;
          if (App.DataManager.SettingsMan.ShowReadStories)
            return;
          newStorey.StoryReadVis = Visibility.Collapsed;
          return;
        }
      }
      foreach (SubRedditData topStorey in (Collection<SubRedditData>) this.ViewModel.TopStories)
      {
        if (topStorey.RedditID.Equals(redditid))
        {
          topStorey.TitleColor = SubRedditData.ReadTitleColor;
          if (App.DataManager.SettingsMan.ShowReadStories)
            return;
          topStorey.StoryReadVis = Visibility.Collapsed;
          return;
        }
      }
      foreach (SubRedditData convStorey in (Collection<SubRedditData>) this.ViewModel.ConvStories)
      {
        if (convStorey.RedditID.Equals(redditid))
        {
          convStorey.TitleColor = SubRedditData.ReadTitleColor;
          if (App.DataManager.SettingsMan.ShowReadStories)
            break;
          convStorey.StoryReadVis = Visibility.Collapsed;
          break;
        }
      }
    }

    private void AnimateStoryOut(FrameworkElement element, SubRedditData story)
    {
      KeyValuePair<FrameworkElement, SubRedditData> keyValuePair = new KeyValuePair<FrameworkElement, SubRedditData>(element, story);
      if (this.AnimatedOut.Contains(keyValuePair))
        return;
      Duration duration = new Duration(TimeSpan.FromMilliseconds(450.0));
      DoubleAnimation element1 = new DoubleAnimation();
      CubicEase cubicEase = new CubicEase();
      cubicEase.EasingMode = EasingMode.EaseIn;
      element1.EasingFunction = (IEasingFunction) cubicEase;
      element1.Duration = duration;
      Storyboard storyboard = new Storyboard();
      storyboard.Duration = duration;
      storyboard.Children.Add((Timeline) element1);
      Storyboard.SetTarget((Timeline) element1, (DependencyObject) element);
      Storyboard.SetTargetProperty((Timeline) element1, new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.TranslateX)", new object[0]));
      element1.To = new double?(800.0);
      storyboard.Begin();
      this.AnimatedOut.Add(keyValuePair);
    }

    private void PhoneApplicationPage_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      foreach (KeyValuePair<FrameworkElement, SubRedditData> keyValuePair in this.AnimatedOut)
      {
        if (App.DataManager.SettingsMan.ShowReadStories)
        {
          Duration duration = new Duration(TimeSpan.FromMilliseconds(500.0));
          DoubleAnimation element = new DoubleAnimation();
          element.Duration = duration;
          ElasticEase elasticEase = new ElasticEase();
          elasticEase.Oscillations = 1;
          elasticEase.EasingMode = EasingMode.EaseOut;
          elasticEase.Springiness = 6.0;
          element.EasingFunction = (IEasingFunction) elasticEase;
          Storyboard storyboard = new Storyboard();
          storyboard.Duration = duration;
          storyboard.Children.Add((Timeline) element);
          Storyboard.SetTarget((Timeline) element, (DependencyObject) keyValuePair.Key);
          Storyboard.SetTargetProperty((Timeline) element, new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.TranslateX)", new object[0]));
          element.To = new double?(0.0);
          element.From = new double?(500.0);
          storyboard.BeginTime = new TimeSpan?(TimeSpan.FromMilliseconds(350.0));
          storyboard.Begin();
        }
        else
        {
          keyValuePair.Value.StoryReadVis = Visibility.Collapsed;
          ((CompositeTransform) keyValuePair.Key.RenderTransform).TranslateX = 0.0;
        }
      }
      this.AnimatedOut.Clear();
    }

    private void UpVoteButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      if (App.DataManager.SettingsMan.IsSignedIn)
      {
        SubRedditData StoryData = (sender as StackPanel).DataContext as SubRedditData;
        int vote = 0;
        switch (StoryData.Like)
        {
          case -1:
            --StoryData.downs;
            ++StoryData.score;
            StoryData.DownVoteColor = RedditViewerViewModel.ARROW_NO_SELECT;
            StoryData.UpVoteColor = DataManager.ACCENT_COLOR;
            StoryData.Like = 1;
            ++StoryData.ups;
            ++StoryData.score;
            vote = 1;
            break;
          case 0:
            StoryData.UpVoteColor = DataManager.ACCENT_COLOR;
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
            vote = 0;
            break;
        }
        ThreadPool.QueueUserWorkItem((WaitCallback) (obfj => App.DataManager.Vote(StoryData.RedditID, vote)));
      }
      else
        App.DataManager.MessageManager.ShowSignInMessage("vote");
    }

    private void DownVoteButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      if (App.DataManager.SettingsMan.IsSignedIn)
      {
        SubRedditData StoryData = (sender as StackPanel).DataContext as SubRedditData;
        int vote = 0;
        switch (StoryData.Like)
        {
          case -1:
            StoryData.Like = 0;
            --StoryData.downs;
            ++StoryData.score;
            StoryData.DownVoteColor = RedditViewerViewModel.ARROW_NO_SELECT;
            vote = 0;
            break;
          case 0:
            StoryData.DownVoteColor = DataManager.ACCENT_COLOR;
            StoryData.Like = -1;
            ++StoryData.downs;
            --StoryData.score;
            vote = -1;
            break;
          case 1:
            --StoryData.ups;
            --StoryData.score;
            StoryData.UpVoteColor = RedditViewerViewModel.ARROW_NO_SELECT;
            StoryData.DownVoteColor = DataManager.ACCENT_COLOR;
            StoryData.Like = -1;
            ++StoryData.downs;
            --StoryData.score;
            vote = -1;
            break;
        }
        ThreadPool.QueueUserWorkItem((WaitCallback) (obfj => App.DataManager.Vote(StoryData.RedditID, vote)));
      }
      else
        App.DataManager.MessageManager.ShowSignInMessage("vote");
    }

    private void GoToDetails_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      SubRedditData dataContext;
      if (sender.GetType().Name.Equals("StackPanel"))
      {
        dataContext = (sender as StackPanel).DataContext as SubRedditData;
        this.AnimateStoryOut((FrameworkElement) ((FrameworkElement) ((FrameworkElement) sender).Parent).Parent, dataContext);
      }
      else
      {
        if (!sender.GetType().Name.Equals("TextBlock"))
          return;
        dataContext = (sender as TextBlock).DataContext as SubRedditData;
        this.AnimateStoryOut((FrameworkElement) ((FrameworkElement) ((FrameworkElement) sender).Parent).Parent, dataContext);
      }
      dataContext.NewCommentsText = "";
      App.DataManager.SettingsMan.AddReadStory(dataContext.RedditID);
      dataContext.TitleColor = SubRedditData.ReadTitleColor;
      App.ActiveStoryData = dataContext;
      this.NavigationService.Navigate(new Uri("/StoryDetails.xaml?StoryDataRedditID=" + App.ActiveStoryData.RedditID, UriKind.Relative));
    }

    private void saveStory_Click(object sender, RoutedEventArgs e)
    {
      if (!App.DataManager.SettingsMan.IsSignedIn)
      {
        App.DataManager.MessageManager.ShowSignInMessage("save a story");
      }
      else
      {
        SubRedditData StoryData = (sender as MenuItem).DataContext as SubRedditData;
        StoryData.isSaved = !StoryData.isSaved;
        ThreadPool.QueueUserWorkItem((WaitCallback) (obfj => App.DataManager.SaveStory(StoryData.RedditID, StoryData.isSaved)));
        if (App.DataManager.SettingsMan.ShowSaveNote)
        {
          App.DataManager.SettingsMan.ShowSaveNote = false;
          int num = (int) MessageBox.Show("Saving a story will keep it in a saved list on reddit, look for the “saved” subreddit in this app to access these stories later.", "Saving A Story", MessageBoxButton.OK);
        }
        StoryData.SaveStoryText = !StoryData.isSaved ? "save story" : "unsave story";
        App.DataManager.BaconitStore.UpdateStory(StoryData);
      }
    }

    private void hideStory_Click(object sender, RoutedEventArgs e)
    {
      if (!App.DataManager.SettingsMan.IsSignedIn)
      {
        App.DataManager.MessageManager.ShowSignInMessage("hide a story");
      }
      else
      {
        SubRedditData StoryData = (sender as MenuItem).DataContext as SubRedditData;
        StoryData.isHidden = !StoryData.isHidden;
        ThreadPool.QueueUserWorkItem((WaitCallback) (obfj =>
        {
          App.DataManager.HideStory(StoryData.RedditID, StoryData.isHidden);
          App.DataManager.BaconitStore.UpdateStory(StoryData);
        }));
        StoryData.StoryVisible = Visibility.Collapsed;
      }
    }

    private void GoToSubreddit_Click(object sender, RoutedEventArgs e)
    {
      SubRedditData dataContext = (sender as MenuItem).DataContext as SubRedditData;
      try
      {
        this.NavigationService.Navigate(new Uri("/RedditsViewer.xaml?subredditURL=" + HttpUtility.UrlEncode("/r/" + dataContext.SubReddit + "/"), UriKind.Relative));
      }
      catch
      {
      }
    }

    private void OpenIntoFlipView_Click(object sender, RoutedEventArgs e)
    {
      SubRedditData dataContext = (sender as MenuItem).DataContext as SubRedditData;
      App.ActiveStoryData = dataContext;
      dataContext.NewCommentsText = "";
      App.DataManager.SettingsMan.AddReadStory(dataContext.RedditID);
      dataContext.TitleColor = SubRedditData.ReadTitleColor;
      this.NavigationService.Navigate(new Uri("/RedditFlipView.xaml?WhichList=" + (object) this.CurrentShownItem + "&WhichVirtList=" + (object) this.GetVirturalShowing() + "&StoryID=" + App.ActiveStoryData.RedditID, UriKind.Relative));
    }

    public void refreshUI(SubRedditData toUpdate)
    {
      for (int index1 = 0; index1 < 4; ++index1)
      {
        ObservableCollection<SubRedditData> observableCollection = (ObservableCollection<SubRedditData>) null;
        switch (index1)
        {
          case 0:
            observableCollection = this.ViewModel.HotStories;
            break;
          case 1:
            observableCollection = this.ViewModel.NewStories;
            break;
          case 2:
            observableCollection = this.ViewModel.TopStories;
            break;
          case 3:
            observableCollection = this.ViewModel.ConvStories;
            break;
        }
        if (observableCollection != null)
        {
          for (int index2 = 0; index2 < observableCollection.Count; ++index2)
          {
            if (observableCollection[index2].RedditID.Equals(toUpdate.RedditID))
            {
              observableCollection[index2].Title = toUpdate.Title;
              observableCollection[index2].score = toUpdate.score;
              observableCollection[index2].ups = toUpdate.ups;
              observableCollection[index2].downs = toUpdate.downs;
              observableCollection[index2].Like = toUpdate.Like;
              observableCollection[index2].UpVoteColor = toUpdate.UpVoteColor;
              observableCollection[index2].DownVoteColor = toUpdate.DownVoteColor;
              observableCollection[index2].isHidden = toUpdate.isHidden;
              observableCollection[index2].isSaved = toUpdate.isSaved;
            }
          }
        }
      }
    }

    public void SetFlipModeButton(int whichOne, bool isEnabled)
    {
      this.FlipModeEnabled[whichOne] = isEnabled;
      if (whichOne != this.CurrentShownItem)
        return;
      this.Dispatcher.BeginInvoke((Action) (() => ((ApplicationBarIconButton) this.ApplicationBar.Buttons[1]).IsEnabled = isEnabled));
    }

    private void refresh_Click(object sender, EventArgs e)
    {
      this.ViewModel.SetLoadingBar(this.CurrentShownItem, true);
      App.DataManager.UpdateSubRedditStories((RedditViewerViewModelInterface) this.ViewModel, this.LocalSubreddit.URL, this.CurrentShownItem, this.GetVirturalShowing(), -1);
    }

    private void PinToStart_Click(object sender, EventArgs e) => this.ShowPinPicker();

    private void LoadMoreStories_Click(object sender, EventArgs e)
    {
      this.ViewModel.SetLoadingBar(this.CurrentShownItem, true);
      ThreadPool.QueueUserWorkItem((WaitCallback) (obfj => this.GetMoreStories()));
    }

    private void ShowReadStories_Click(object sender, EventArgs e)
    {
      App.DataManager.SettingsMan.ShowReadStories = !App.DataManager.SettingsMan.ShowReadStories;
      if (App.DataManager.SettingsMan.ShowReadStories)
        ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[1]).Text = "hide read stories";
      else
        ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[1]).Text = "show read stories";
      this.Dispatcher.BeginInvoke((Action) (() => this.ViewModel.UpdateReadStoryVis()));
    }

    private void mainPivot_LoadingPivotItem(object sender, PivotItemEventArgs e)
    {
      if (this.isLoadingSubreddit)
        return;
      switch (this.mainPivot.SelectedIndex)
      {
        case 0:
          this.CurrentShownItem = 0;
          break;
        case 1:
          this.CurrentShownItem = 1;
          break;
        case 2:
          this.CurrentShownItem = 2;
          break;
        case 3:
          this.CurrentShownItem = 3;
          break;
      }
      if (this.CurrentShownItem != 0 && this.LocalSubreddit.URL.Equals("/saved/"))
        return;
      ((ApplicationBarIconButton) this.ApplicationBar.Buttons[1]).IsEnabled = this.FlipModeEnabled[this.CurrentShownItem];
      this.SetSubmitAndSortButton();
      ThreadPool.QueueUserWorkItem((WaitCallback) (obfj =>
      {
        int VirturalLoadingItem = this.GetVirturalShowing();
        if (App.DataManager.BaconitStore.LastUpdatedTime(this.LocalSubreddit.RName + (object) VirturalLoadingItem) < BaconitStore.currentTime() - App.DataManager.SettingsMan.StoryUpdateTime)
        {
          App.DataManager.UpdateSubRedditStories((RedditViewerViewModelInterface) this.ViewModel, this.LocalSubreddit.URL, this.CurrentShownItem, VirturalLoadingItem, -1);
          this.Dispatcher.BeginInvoke((Action) (() => this.ViewModel.SetLoadingBar(this.CurrentShownItem, true)));
        }
        if (this.ViewModel.typeLoaded[this.CurrentShownItem])
          return;
        ThreadPool.QueueUserWorkItem((WaitCallback) (objs => this.ViewModel.updateStories(this.CurrentShownItem, VirturalLoadingItem)));
      }));
    }

    public void SetSubmitAndSortButton()
    {
      if (this.CurrentShownItem == 1 || this.CurrentShownItem == 2)
      {
        ((ApplicationBarIconButton) this.ApplicationBar.Buttons[2]).Text = "sort";
        ((ApplicationBarIconButton) this.ApplicationBar.Buttons[2]).IconUri = new Uri("/Images/AppBar.List.png", UriKind.Relative);
        ((ApplicationBarIconButton) this.ApplicationBar.Buttons[2]).IsEnabled = true;
      }
      else
      {
        ((ApplicationBarIconButton) this.ApplicationBar.Buttons[2]).Text = "submit";
        ((ApplicationBarIconButton) this.ApplicationBar.Buttons[2]).IconUri = new Uri("/Images/appbar.add.png", UriKind.Relative);
        if (!this.LocalSubreddit.URL.Equals("/") && !this.LocalSubreddit.URL.Equals("/r/all/") && !this.LocalSubreddit.URL.Equals("/saved/") && !this.LocalSubreddit.URL.Equals("/r/friends/") && !this.LocalSubreddit.URL.Contains("+"))
          return;
        ((ApplicationBarIconButton) this.ApplicationBar.Buttons[2]).IsEnabled = false;
      }
    }

    public void GetMoreStories()
    {
      int pos = 0;
      using (AutoResetEvent are = new AutoResetEvent(false))
      {
        Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
        {
          switch (this.CurrentShownItem)
          {
            case 0:
              pos = this.ViewModel.HotStories.Count;
              break;
            case 1:
              pos = this.ViewModel.NewStories.Count;
              break;
            case 2:
              pos = this.ViewModel.TopStories.Count;
              break;
            case 3:
              pos = this.ViewModel.ConvStories.Count;
              break;
          }
          are.Set();
        }));
        are.WaitOne();
      }
      if (App.DataManager.UpdateSubRedditStories((RedditViewerViewModelInterface) this.ViewModel, this.LocalSubreddit.URL, this.CurrentShownItem, this.GetVirturalShowing(), pos))
        return;
      switch (this.CurrentShownItem)
      {
        case 0:
          this.lastCountHot = 0;
          break;
        case 1:
          this.lastCountNew = 0;
          break;
        case 2:
          this.lastCountTop = 0;
          break;
        case 3:
          this.lastCountConv = 0;
          break;
      }
    }

    public int GetVirturalShowing()
    {
      int virturalShowing = this.CurrentShownItem;
      if (this.CurrentShownItem == 2)
        virturalShowing = this.TypeOfTopShowing;
      if (this.CurrentShownItem == 1)
        virturalShowing = this.TypeOfNewShowing;
      return virturalShowing;
    }

    public static int ReverseVirtural(int reverse)
    {
      if (reverse <= 3)
        return reverse;
      return reverse == 12 ? 1 : 2;
    }

    private void StoryHelpOverlay_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.FadeOutTips.Completed += new EventHandler(this.FadeComplete);
      this.FadeOutTips.Begin();
      App.DataManager.SettingsMan.ShowStoryListOverLay = false;
    }

    private void FadeComplete(object sender, EventArgs e)
    {
      this.StoryHelpOverlay.Visibility = Visibility.Collapsed;
      this.StoryHelpOverlay.Opacity = 1.0;
      this.ApplicationBar.IsVisible = true;
      if (App.DataManager.SettingsMan.ScreenOrenLocked)
      {
        if (App.DataManager.SettingsMan.ScreenOrenLockedtoPortrait)
          this.SupportedOrientations = SupportedPageOrientation.Portrait;
        else
          this.SupportedOrientations = SupportedPageOrientation.Landscape;
        ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[0]).Text = "unlock screen orientation";
      }
      else
      {
        this.SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;
        ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[0]).Text = "lock screen orientation";
      }
    }

    private void showTips_Click(object sender, EventArgs e)
    {
      this.ApplicationBar.IsVisible = false;
      this.StoryHelpOverlay.Source = (ImageSource) new BitmapImage(new Uri("/Images/StoryListOverlay-" + (object) App.DataManager.SettingsMan.ScaleFactor + ".png", UriKind.Relative));
      this.StoryHelpOverlay.Visibility = Visibility.Visible;
      this.FadeInTips.Begin();
      this.SupportedOrientations = SupportedPageOrientation.Portrait;
    }

    private void changeSubAccount_Click(object sender, EventArgs e)
    {
      this.LocalSubreddit.isAccount = !this.LocalSubreddit.isAccount;
      bool sub = this.LocalSubreddit.isAccount;
      ThreadPool.QueueUserWorkItem((WaitCallback) (obfj => App.DataManager.SubredditDataManager.SubscribeToSubReddit(this.LocalSubreddit, sub, true, false)));
      if (sub)
      {
        ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[3]).Text = "unsubscribe from account";
        ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[4]).IsEnabled = false;
      }
      else
      {
        ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[3]).Text = "subscribe to account";
        ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[4]).IsEnabled = true;
      }
    }

    private void changeSubLocal_Click(object sender, EventArgs e)
    {
      this.LocalSubreddit.isLocal = !this.LocalSubreddit.isLocal;
      ThreadPool.QueueUserWorkItem((WaitCallback) (obfj =>
      {
        if (this.LocalSubreddit.isLocal)
          App.DataManager.SubredditDataManager.AddToLocalSubReddit(this.LocalSubreddit, true);
        else
          App.DataManager.SubredditDataManager.RemoveFromLocalSubReddit(this.LocalSubreddit, true);
        App.DataManager.BaconitStore.UpdateLastUpdatedTime("!Main_Subreddit", true);
        App.DataManager.SubredditDataManager.UpdateSubReddits();
      }));
      if (this.LocalSubreddit.isLocal)
      {
        if (App.DataManager.SettingsMan.IsSignedIn)
        {
          ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[4]).Text = "unsubscribe from local";
          ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[3]).IsEnabled = false;
        }
        else
          ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[3]).Text = "unsubscribe from local";
      }
      else if (App.DataManager.SettingsMan.IsSignedIn)
      {
        ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[4]).Text = "subscribe to local";
        ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[3]).IsEnabled = true;
      }
      else
        ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[3]).Text = "subscribe to local";
    }

    public void StopLoading()
    {
      for (int which = 0; which < this.LoadingBars.Length; ++which)
        this.ViewModel.SetLoadingBar(which, false);
    }

    private void submitorSortStory_Click(object sender, EventArgs e)
    {
      if (this.CurrentShownItem == 2)
        this.ShowTopPicker();
      else if (this.CurrentShownItem == 1)
        this.ShowNewPicker();
      else if (App.DataManager.SettingsMan.IsSignedIn)
        this.NavigationService.Navigate(new Uri("/SubmitLink.xaml?Subreddit=" + this.LocalSubreddit.DisplayName, UriKind.Relative));
      else
        App.DataManager.MessageManager.ShowSignInMessage("submit a story");
    }

    private void lockOren_Click(object sender, EventArgs e)
    {
      App.DataManager.SettingsMan.ScreenOrenLocked = !App.DataManager.SettingsMan.ScreenOrenLocked;
      if (App.DataManager.SettingsMan.ScreenOrenLocked)
      {
        ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[0]).Text = "unlock screen orientation";
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
      {
        ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[0]).Text = "lock screen orientation";
        this.SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;
      }
    }

    private void gotoImageViewer_Click(object sender, EventArgs e)
    {
      this.NavigationService.Navigate(new Uri("/RedditFlipView.xaml?WhichList=" + (object) this.CurrentShownItem + "&WhichVirtList=" + (object) this.GetVirturalShowing() + "&RedditRName=" + this.LocalSubreddit.RName, UriKind.Relative));
    }

    public double ListVerticalOffsetTop
    {
      get => (double) this.GetValue(this.ListVerticalOffsetPropertyTop);
      set => this.SetValue(this.ListVerticalOffsetPropertyTop, (object) value);
    }

    public double ListVerticalOffsetNew
    {
      get => (double) this.GetValue(this.ListVerticalOffsetPropertyNew);
      set => this.SetValue(this.ListVerticalOffsetPropertyNew, (object) value);
    }

    public double ListVerticalOffsetConv
    {
      get => (double) this.GetValue(this.ListVerticalOffsetPropertyConv);
      set => this.SetValue(this.ListVerticalOffsetPropertyConv, (object) value);
    }

    public double ListVerticalOffsetHot
    {
      get => (double) this.GetValue(this.ListVerticalOffsetPropertyHot);
      set => this.SetValue(this.ListVerticalOffsetPropertyHot, (object) value);
    }

    private static void OnListVerticalOffsetChangedHot(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      RedditsViewer page = obj as RedditsViewer;
      ScrollViewer listScrollViewerHot = page._listScrollViewerHot;
      if (listScrollViewerHot == null || RedditsViewer.Current == null || RedditsViewer.Current.ViewModel == null || listScrollViewerHot.VerticalOffset < listScrollViewerHot.ScrollableHeight - listScrollViewerHot.ViewportHeight * 2.0 || RedditsViewer.Current.ViewModel.LoadingFromWeb[RedditsViewer.Current.CurrentShownItem] || RedditsViewer.Current.ViewModel.HotStories.Count >= 1000)
        return;
      ThreadPool.QueueUserWorkItem((WaitCallback) (obfj =>
      {
        if (page.lastCountHot == RedditsViewer.Current.ViewModel.HotStories.Count || RedditsViewer.Current.CurrentShownItem != 0)
          return;
        page.lastCountHot = RedditsViewer.Current.ViewModel.HotStories.Count;
        Deployment.Current.Dispatcher.BeginInvoke((Action) (() => page.LoadMoreStories_Click((object) null, (EventArgs) null)));
      }));
    }

    private static void OnListVerticalOffsetChangedTop(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      RedditsViewer page = obj as RedditsViewer;
      ScrollViewer listScrollViewerTop = page._listScrollViewerTop;
      if (listScrollViewerTop == null || RedditsViewer.Current == null || RedditsViewer.Current.ViewModel == null || listScrollViewerTop.VerticalOffset < listScrollViewerTop.ScrollableHeight - listScrollViewerTop.ViewportHeight * 2.0 || RedditsViewer.Current.ViewModel.LoadingFromWeb[RedditsViewer.Current.CurrentShownItem] || RedditsViewer.Current.ViewModel.TopStories.Count >= 1000)
        return;
      ThreadPool.QueueUserWorkItem((WaitCallback) (obfj =>
      {
        if (page.lastCountTop == RedditsViewer.Current.ViewModel.TopStories.Count || RedditsViewer.Current.CurrentShownItem != 2)
          return;
        page.lastCountTop = RedditsViewer.Current.ViewModel.TopStories.Count;
        Deployment.Current.Dispatcher.BeginInvoke((Action) (() => page.LoadMoreStories_Click((object) null, (EventArgs) null)));
      }));
    }

    private static void OnListVerticalOffsetChangedNew(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      RedditsViewer page = obj as RedditsViewer;
      ScrollViewer listScrollViewerNew = page._listScrollViewerNew;
      if (listScrollViewerNew == null || RedditsViewer.Current == null || RedditsViewer.Current.ViewModel == null || listScrollViewerNew.VerticalOffset < listScrollViewerNew.ScrollableHeight - listScrollViewerNew.ViewportHeight * 2.0 || RedditsViewer.Current.ViewModel.LoadingFromWeb[RedditsViewer.Current.CurrentShownItem] || RedditsViewer.Current.ViewModel.NewStories.Count >= 1000)
        return;
      ThreadPool.QueueUserWorkItem((WaitCallback) (obfj =>
      {
        if (page.lastCountNew == RedditsViewer.Current.ViewModel.NewStories.Count || RedditsViewer.Current.CurrentShownItem != 1)
          return;
        page.lastCountNew = RedditsViewer.Current.ViewModel.NewStories.Count;
        Deployment.Current.Dispatcher.BeginInvoke((Action) (() => page.LoadMoreStories_Click((object) null, (EventArgs) null)));
      }));
    }

    private static void OnListVerticalOffsetChangedConv(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      RedditsViewer page = obj as RedditsViewer;
      ScrollViewer scrollViewerConv = page._listScrollViewerConv;
      if (scrollViewerConv == null || RedditsViewer.Current == null || RedditsViewer.Current.ViewModel == null || scrollViewerConv.VerticalOffset < scrollViewerConv.ScrollableHeight - scrollViewerConv.ViewportHeight * 2.0 || RedditsViewer.Current.ViewModel.LoadingFromWeb[RedditsViewer.Current.CurrentShownItem] || RedditsViewer.Current.ViewModel.ConvStories.Count >= 1000)
        return;
      ThreadPool.QueueUserWorkItem((WaitCallback) (obfj =>
      {
        if (page.lastCountConv == RedditsViewer.Current.ViewModel.ConvStories.Count || RedditsViewer.Current.CurrentShownItem != 3)
          return;
        page.lastCountConv = RedditsViewer.Current.ViewModel.ConvStories.Count;
        Deployment.Current.Dispatcher.BeginInvoke((Action) (() => page.LoadMoreStories_Click((object) null, (EventArgs) null)));
      }));
    }

    private void hotViewer_Loaded(object sender, RoutedEventArgs e)
    {
      this._listScrollViewerHot = sender as ScrollViewer;
      this.SetBinding(this.ListVerticalOffsetPropertyHot, new Binding()
      {
        Source = (object) this._listScrollViewerHot,
        Path = new PropertyPath("VerticalOffset", new object[0]),
        Mode = BindingMode.OneWay
      });
    }

    private void newViewer_Loaded(object sender, RoutedEventArgs e)
    {
      this._listScrollViewerNew = sender as ScrollViewer;
      this.SetBinding(this.ListVerticalOffsetPropertyNew, new Binding()
      {
        Source = (object) this._listScrollViewerNew,
        Path = new PropertyPath("VerticalOffset", new object[0]),
        Mode = BindingMode.OneWay
      });
    }

    private void topViewer_Loaded(object sender, RoutedEventArgs e)
    {
      this._listScrollViewerTop = sender as ScrollViewer;
      this.SetBinding(this.ListVerticalOffsetPropertyTop, new Binding()
      {
        Source = (object) this._listScrollViewerTop,
        Path = new PropertyPath("VerticalOffset", new object[0]),
        Mode = BindingMode.OneWay
      });
    }

    private void convViewer_Loaded(object sender, RoutedEventArgs e)
    {
      this._listScrollViewerConv = sender as ScrollViewer;
      this.SetBinding(this.ListVerticalOffsetPropertyConv, new Binding()
      {
        Source = (object) this._listScrollViewerConv,
        Path = new PropertyPath("VerticalOffset", new object[0]),
        Mode = BindingMode.OneWay
      });
    }

    private void LoadingFade_Complete(object sender, EventArgs e)
    {
      this.LoadingSubreddit.Visibility = Visibility.Collapsed;
      this.LoadingSubredditLoadingBar.IsIndeterminate = false;
    }

    private void LoadSubreddit_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (((string) e.Result).Equals("valid") && sender != null)
      {
        this.LocalSubreddit = (SubReddit) sender;
        this.isLoadingSubreddit = false;
        this.Dispatcher.BeginInvoke((Action) (() =>
        {
          this.ApplicationBar.IsVisible = true;
          this.FadeOutLoading.Begin();
          this.SecondPartSetup();
          this.RedditsViewer_Loaded((object) null, (RoutedEventArgs) null);
          this.mainPivot_LoadingPivotItem((object) null, (PivotItemEventArgs) null);
        }));
      }
      else
        this.Dispatcher.BeginInvoke((Action) (() =>
        {
          if (((string) e.Result).Equals("notValid"))
          {
            int num1 = (int) MessageBox.Show("The requested subreddit does not exist.", "No Subreddit Found", MessageBoxButton.OK);
          }
          else
          {
            int num2 = (int) MessageBox.Show("A network error has occurred and Baconit can't load this subreddit right. Please try again later.", "Network Error", MessageBoxButton.OK);
          }
          if (!this.NavigationService.CanGoBack)
            return;
          this.NavigationService.GoBack();
        }));
    }

    private void ListFocusFix_GotFocus(object sender, RoutedEventArgs e) => this.mainPivot.Focus();

    private void ShowPinPicker()
    {
      this.FullScreenBlackBackground.Visibility = Visibility.Visible;
      this.TilePicker.Visibility = Visibility.Visible;
      this.TileIcon.ItemsSource = (IEnumerable) new List<LiveTileBackgroundItem>()
      {
        new LiveTileBackgroundItem()
        {
          Title = "Baconit",
          Path = "/Images/LiveTiles/BaconDispenser_m.png"
        },
        new LiveTileBackgroundItem()
        {
          Title = "Reddit Alien",
          Path = "/Images/LiveTiles/RedditAlieanNoBack_m.png"
        },
        new LiveTileBackgroundItem()
        {
          Title = "Reddit Alien Outline",
          Path = "/Images/LiveTiles/RedditAlieanNoFill_m.png"
        },
        new LiveTileBackgroundItem()
        {
          Title = "Reddit Alien Face",
          Path = "/Images/LiveTiles/Face_m.png"
        },
        new LiveTileBackgroundItem()
        {
          Title = "Inbox Message",
          Path = "/Images/LiveTiles/MessageBackground_m.png"
        },
        new LiveTileBackgroundItem()
        {
          Title = "Reddit Alien",
          Path = "/Images/LiveTiles/RedditAliean_m.png"
        },
        new LiveTileBackgroundItem()
        {
          Title = "Old School",
          Path = "/Images/LiveTiles/OldBack_m.png"
        }
      };
      this.TileIcon.SelectedIndex = App.DataManager.SettingsMan.MainTileBackground;
      this.PinSettingsSubName.Text = this.LocalSubreddit.DisplayName;
      this.PinSettingsSubNameImage.Text = this.LocalSubreddit.DisplayName;
      this.ApplicationBar.IsVisible = false;
      this.OpenPinPicker.Begin();
      this.isPinnedOptionsOpened = true;
    }

    private void ClosePinPicker_complete(object sender, EventArgs e)
    {
      this.FullScreenBlackBackground.Visibility = Visibility.Collapsed;
      this.TilePicker.Visibility = Visibility.Collapsed;
      this.ApplicationBar.IsVisible = true;
      this.isPinnedOptionsOpened = false;
    }

    private void ImageTile_Click(object sender, RoutedEventArgs e)
    {
      this.ClosePinPicker.Begin();
      ((ApplicationBarIconButton) this.ApplicationBar.Buttons[3]).IsEnabled = false;
      this.PinToStart(true);
    }

    private void IconicTile_click(object sender, RoutedEventArgs e)
    {
      this.ClosePinPicker.Begin();
      ((ApplicationBarIconButton) this.ApplicationBar.Buttons[3]).IsEnabled = false;
      this.PinToStart(false);
    }

    private void PinToStart(bool isCycle)
    {
      if (this.isPinned || this.LocalSubreddit == null)
        return;
      if (isCycle)
      {
        List<Uri> uriList = new List<Uri>();
        uriList.Add(new Uri("/Images/LiveTiles/DefaultLiveTile1.png", UriKind.Relative));
        uriList.Add(new Uri("/Images/LiveTiles/DefaultLiveTile2.png", UriKind.Relative));
        CycleTileData cycleTileData = new CycleTileData();
        cycleTileData.Title = this.LocalSubreddit.DisplayName;
        cycleTileData.SmallBackgroundImage = new Uri(DataManager.LiveTileBackgrounds[this.TileIcon.SelectedIndex] + "c.png", UriKind.Relative);
        cycleTileData.CycleImages = (IEnumerable<Uri>) uriList;
        ShellTileData initialData = (ShellTileData) cycleTileData;
        App.DataManager.SettingsMan.ForceUpdateTiles = true;
        ShellTile.Create(new Uri("/RedditsViewer.xaml?subredditURL=" + HttpUtility.UrlEncode(this.LocalSubreddit.URL) + "&TileType=cycle", UriKind.Relative), initialData, true);
      }
      else
      {
        string str1 = "";
        string str2 = "";
        string str3 = "";
        if (this.ViewModel != null && this.ViewModel.HotStories != null && this.ViewModel.HotStories.Count > 3)
        {
          str1 = this.ViewModel.HotStories[0].Title;
          str2 = this.ViewModel.HotStories[1].Title;
          str3 = this.ViewModel.HotStories[2].Title;
        }
        IconicTileData iconicTileData = new IconicTileData();
        iconicTileData.Title = this.LocalSubreddit.DisplayName;
        iconicTileData.IconImage = new Uri(DataManager.LiveTileBackgrounds[this.TileIcon.SelectedIndex] + "m.png", UriKind.Relative);
        iconicTileData.SmallIconImage = new Uri(DataManager.LiveTileBackgrounds[this.TileIcon.SelectedIndex] + "s.png", UriKind.Relative);
        iconicTileData.WideContent1 = str1;
        iconicTileData.WideContent2 = str2;
        iconicTileData.WideContent3 = str3;
        ShellTileData initialData = (ShellTileData) iconicTileData;
        ShellTile.Create(new Uri("/RedditsViewer.xaml?subredditURL=" + HttpUtility.UrlEncode(this.LocalSubreddit.URL) + "&TileType=iconic", UriKind.Relative), initialData, true);
      }
    }

    private void TileIcon_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.TileIcon.SelectedIndex <= -1 || this.TileIcon.SelectedIndex >= DataManager.LiveTileBackgrounds.Length)
        return;
      this.PinSettingsSampleImage.Source = (ImageSource) new BitmapImage(new Uri(DataManager.LiveTileBackgrounds[this.TileIcon.SelectedIndex] + "m.png", UriKind.Relative));
    }

    public void ShowTopPicker()
    {
      string[] strArray = new string[6]
      {
        "Today",
        "This Hour",
        "This Week",
        "This Month",
        "This Year",
        "All Time"
      };
      PickerBoxDialog pickerBoxDialog = new PickerBoxDialog();
      pickerBoxDialog.ItemSource = (IEnumerable) strArray;
      pickerBoxDialog.Title = "SORT COMMENTS BY";
      pickerBoxDialog.Closed += new EventHandler(this.TopPicker_Closed);
      switch (this.TypeOfTopShowing)
      {
        case 2:
          pickerBoxDialog.SelectedIndex = 0;
          break;
        case 6:
          pickerBoxDialog.SelectedIndex = 1;
          break;
        case 7:
          pickerBoxDialog.SelectedIndex = 2;
          break;
        case 8:
          pickerBoxDialog.SelectedIndex = 3;
          break;
        case 9:
          pickerBoxDialog.SelectedIndex = 4;
          break;
        case 10:
          pickerBoxDialog.SelectedIndex = 5;
          break;
      }
      pickerBoxDialog.Show();
    }

    private void TopPicker_Closed(object sender, EventArgs e)
    {
      int num = 0;
      switch (((PickerBoxDialog) sender).SelectedIndex)
      {
        case 0:
          num = 2;
          break;
        case 1:
          num = 6;
          break;
        case 2:
          num = 7;
          break;
        case 3:
          num = 8;
          break;
        case 4:
          num = 9;
          break;
        case 5:
          num = 10;
          break;
      }
      if (num == this.TypeOfTopShowing)
        return;
      this.TypeOfTopShowing = num;
      this.lastCountTop = 0;
      this.SetFlipModeButton(2, false);
      this.ViewModel.typeLoaded[2] = false;
      this.TopNoStories.Visibility = Visibility.Collapsed;
      this.ViewModel.TopStories.Clear();
      this.mainPivot_LoadingPivotItem((object) null, (PivotItemEventArgs) null);
    }

    public void ShowNewPicker()
    {
      string[] strArray = new string[2]{ "New", "Rising" };
      PickerBoxDialog pickerBoxDialog = new PickerBoxDialog();
      pickerBoxDialog.ItemSource = (IEnumerable) strArray;
      pickerBoxDialog.Title = "SORT COMMENTS BY";
      pickerBoxDialog.Closed += new EventHandler(this.NewPicker_Closed);
      switch (this.TypeOfNewShowing)
      {
        case 1:
          pickerBoxDialog.SelectedIndex = 0;
          break;
        case 12:
          pickerBoxDialog.SelectedIndex = 1;
          break;
      }
      pickerBoxDialog.Show();
    }

    private void NewPicker_Closed(object sender, EventArgs e)
    {
      int num = 0;
      switch (((PickerBoxDialog) sender).SelectedIndex)
      {
        case 0:
          num = 1;
          break;
        case 1:
          num = 12;
          break;
      }
      if (num == this.TypeOfNewShowing)
        return;
      this.TypeOfNewShowing = num;
      this.lastCountNew = 0;
      this.SetFlipModeButton(1, false);
      this.ViewModel.typeLoaded[1] = false;
      this.NewNoStories.Visibility = Visibility.Collapsed;
      this.ViewModel.NewStories.Clear();
      this.mainPivot_LoadingPivotItem((object) null, (PivotItemEventArgs) null);
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/RedditsViewer.xaml", UriKind.Relative));
      this.FadeOutTips = (Storyboard) this.FindName("FadeOutTips");
      this.FadeInTips = (Storyboard) this.FindName("FadeInTips");
      this.FadeOutLoading = (Storyboard) this.FindName("FadeOutLoading");
      this.OpenPinPicker = (Storyboard) this.FindName("OpenPinPicker");
      this.ClosePinPicker = (Storyboard) this.FindName("ClosePinPicker");
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.mainPivot = (Pivot) this.FindName("mainPivot");
      this.HotLoading = (ProgressBar) this.FindName("HotLoading");
      this.HotList = (ListBox) this.FindName("HotList");
      this.HotNoStories = (TextBlock) this.FindName("HotNoStories");
      this.NewLoading = (ProgressBar) this.FindName("NewLoading");
      this.NewList = (ListBox) this.FindName("NewList");
      this.NewNoStories = (TextBlock) this.FindName("NewNoStories");
      this.TopLoading = (ProgressBar) this.FindName("TopLoading");
      this.TopList = (ListBox) this.FindName("TopList");
      this.TopNoStories = (TextBlock) this.FindName("TopNoStories");
      this.ControversialLoading = (ProgressBar) this.FindName("ControversialLoading");
      this.ControversialList = (ListBox) this.FindName("ControversialList");
      this.ConvNoStories = (TextBlock) this.FindName("ConvNoStories");
      this.StoryHelpOverlay = (Image) this.FindName("StoryHelpOverlay");
      this.LoadingSubreddit = (Grid) this.FindName("LoadingSubreddit");
      this.LoggingInOverlayBack = (Rectangle) this.FindName("LoggingInOverlayBack");
      this.LoadingSubredditLoadingBar = (ProgressBar) this.FindName("LoadingSubredditLoadingBar");
      this.FullScreenBlackBackground = (Grid) this.FindName("FullScreenBlackBackground");
      this.TilePicker = (Grid) this.FindName("TilePicker");
      this.TileIcon = (ListPicker) this.FindName("TileIcon");
      this.PinSettingsSubNameImage = (TextBlock) this.FindName("PinSettingsSubNameImage");
      this.PinSettingsSampleImage = (Image) this.FindName("PinSettingsSampleImage");
      this.PinSettingsSubName = (TextBlock) this.FindName("PinSettingsSubName");
    }
  }
}
