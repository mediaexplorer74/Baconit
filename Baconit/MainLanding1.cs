// Decompiled with JetBrains decompiler
// Type: Baconit.MainLanding
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Baconit.Database;
using BaconitData.Libs;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

#nullable disable
namespace Baconit
{
  public class MainLanding : PhoneApplicationPage
  {
    public static MainLandingViewModel ViewModel;
    public static MainLanding me;
    private bool RestoreLoadingBar;
    private bool AccountAdded;
    private bool AccountRemoved;
    private bool HasTextLoaded;
    private int MainPanoSelectionChanged;
    internal Storyboard ShowProgressIndicator;
    internal Storyboard HideProgressIndicator;
    internal Storyboard CloseFirstTimeSettings;
    internal Storyboard CloseFirstTimeUse;
    internal Storyboard ShowFirstTimeSettings;
    internal DynamicBackgroundPanorama MainPanorama;
    internal StackPanel HelloHolder;
    internal StackPanel UpdateHolder;
    internal StackPanel TopStoryHolder;
    internal TextBlock TopStoryMessages;
    internal Button TopStory1;
    internal TextBlock TopStoryText1;
    internal Button TopStory2;
    internal TextBlock TopStoryText2;
    internal Button TopStory3;
    internal TextBlock TopStoryText3;
    internal Storyboard FadeInTopStories;
    internal LongListSelector RedditsList;
    internal StackPanel LoggedInHolderUI;
    internal TextBlock MessagesTitleUI;
    internal TextBlock MessagesSubTextUI;
    internal TextBlock AccountSubText;
    internal StackPanel LoggedOutHolderUI;
    internal ItemsControl RecentTiles;
    internal TextBlock RecentNothingText;
    internal ProgressBar LoadingBar;
    internal StackPanel FirstTimeOptions;
    internal StackPanel FirstTimeHolder;
    internal TextBlock FirstTimeTitle;
    internal TextBlock FirstTimeAutoTitle;
    internal TextBlock FistTimeAutoText;
    internal ToggleSwitch AutomaticUpdatingSwitch;
    internal TextBlock FirstTimeContentTitle;
    internal TextBlock FirstTimeConentText;
    internal ToggleSwitch AdultFilterSwitch;
    internal Grid CloseFirstTime;
    internal Grid FirstTimeWait;
    internal Image FirstUseImage;
    internal TextBlock FirstUseText;
    internal ProgressBar FirstUseLoading;
    private bool _contentLoaded;

    public MainLanding()
    {
      this.InitializeComponent();
      TransitionService.SetNavigationInTransition((UIElement) this, App.TurnstileInTransition);
      TransitionService.SetNavigationOutTransition((UIElement) this, App.TurnstileOutTransition);
      this.ApplicationBar = (IApplicationBar) new Microsoft.Phone.Shell.ApplicationBar();
      this.ApplicationBar.IsVisible = false;
      this.ApplicationBar.Mode = ApplicationBarMode.Minimized;
      if (!DataManager.LIGHT_THEME)
        this.ApplicationBar.BackgroundColor = Color.FromArgb(byte.MaxValue, (byte) 18, (byte) 18, (byte) 18);
      this.ApplicationBar.StateChanged += new EventHandler<ApplicationBarStateChangedEventArgs>(this.ApplicationBar_StateChanged);
      this.ApplicationBar.Opacity = 0.5;
      ApplicationBarIconButton applicationBarIconButton1 = new ApplicationBarIconButton(new Uri("/Images/appbar.sync.rest.png", UriKind.Relative));
      applicationBarIconButton1.Text = "refresh";
      applicationBarIconButton1.Click += new EventHandler(this.refresh_Click);
      this.ApplicationBar.Buttons.Add((object) applicationBarIconButton1);
      ApplicationBarIconButton applicationBarIconButton2 = new ApplicationBarIconButton(new Uri("/Images/appbar.smartBar.png", UriKind.Relative));
      applicationBarIconButton2.Text = "smart bar";
      applicationBarIconButton2.Click += new EventHandler(this.smartBar_Click);
      this.ApplicationBar.Buttons.Add((object) applicationBarIconButton2);
      ApplicationBarIconButton applicationBarIconButton3 = new ApplicationBarIconButton(new Uri("/Images/appbar.add.png", UriKind.Relative));
      applicationBarIconButton3.Text = "submit link";
      applicationBarIconButton3.Click += new EventHandler(this.SubmitLink_Click);
      this.ApplicationBar.Buttons.Add((object) applicationBarIconButton3);
      ApplicationBarMenuItem applicationBarMenuItem1 = new ApplicationBarMenuItem("settings");
      applicationBarMenuItem1.Click += new EventHandler(this.SettingsButton_Click);
      this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem1);
      ApplicationBarMenuItem applicationBarMenuItem2 = new ApplicationBarMenuItem("log out");
      applicationBarMenuItem2.Click += new EventHandler(this.AppBarItemLog_Click);
      this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem2);
      ApplicationBarMenuItem applicationBarMenuItem3 = new ApplicationBarMenuItem("donate");
      applicationBarMenuItem3.Click += new EventHandler(this.DonateButton_Click);
      this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem3);
      this.AccountRemoved = !App.DataManager.SettingsMan.IsSignedIn;
      this.SetAppBarLogin();
      int num = MainLanding.me != null ? 1 : 0;
      MainLanding.me = this;
      if (MainLanding.ViewModel == null)
        MainLanding.ViewModel = new MainLandingViewModel();
      this.DataContext = (object) MainLanding.ViewModel;
      if (num != 0)
      {
        MainLanding.ViewModel.LoadAccountInfo();
        MainLanding.ViewModel.SetRecentTilesText();
      }
      if (DataManager.LIGHT_THEME)
      {
        this.ApplicationBar.BackgroundColor = Color.FromArgb(byte.MaxValue, (byte) 170, (byte) 170, (byte) 170);
        this.MainPanorama.SetToLightTheme();
      }
      this.SetupFirstTimeSetting();
      this.UpdateMainPageLanding();
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        this.HideProgressIndicator.Completed += (EventHandler) ((sender, e) =>
        {
          this.LoadingBar.Visibility = Visibility.Collapsed;
          this.LoadingBar.IsIndeterminate = false;
        });
        TiltEffect.TiltableItems.Add(typeof (DataTemplate));
      }));
      App.DataManager.AccountChanged += new DataManager.AccountChangedEvent(this.AccountUpdated);
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      MainLanding.ViewModel.LoadData();
      App.DataManager.BaconitAnalytics.LogPage("Main Landing");
      if (this.RestoreLoadingBar)
      {
        this.LoadingBar.IsIndeterminate = true;
        this.RestoreLoadingBar = false;
      }
      if (App.navService == null)
        App.navService = this.NavigationService;
      if (!this.State.ContainsKey("MainLandingOpened"))
      {
        this.State["MainLandingOpened"] = (object) true;
        while (this.NavigationService.CanGoBack)
          this.NavigationService.RemoveBackEntry();
        this.CheckQueryParams();
      }
      this.SetAppBarLogin();
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      if (this.LoadingBar.IsIndeterminate)
        this.RestoreLoadingBar = true;
      this.LoadingBar.IsIndeterminate = false;
      this.FirstUseLoading.IsIndeterminate = false;
    }

    private void ApplicationBar_StateChanged(object sender, ApplicationBarStateChangedEventArgs e)
    {
      if (e.IsMenuVisible)
        this.ApplicationBar.Opacity = 0.9;
      else
        this.ApplicationBar.Opacity = 0.5;
    }

    public void AccountUpdated(bool added, bool logout)
    {
      this.AccountAdded = added;
      this.AccountRemoved = logout;
    }

    private void SubReddit_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.NavigationService.Navigate(new Uri("/RedditsViewer.xaml?subredditURL=" + HttpUtility.UrlEncode(((sender as StackPanel).DataContext as ItemViewModel).URL) + "&isFromMain=true", UriKind.Relative));
    }

    private void FavoriteSubreddit_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      ItemViewModel dataContext = (sender as Grid).DataContext as ItemViewModel;
      bool favorite = false;
      if (dataContext.favIcon.Equals("/Images/FavoriteIcon.png") || dataContext.favIcon.Equals("/Images/FavoriteIconLight.png"))
        favorite = true;
      App.DataManager.SubredditDataManager.changeFavoriteStatus(dataContext.RName, !favorite);
      if (!favorite)
      {
        dataContext.favIcon = !DataManager.LIGHT_THEME ? "/Images/FavoriteIcon.png" : "/Images/FavoriteIconLight.png";
        dataContext.LineTwoColor = new SolidColorBrush(DataManager.ACCENT_COLOR_COLOR);
      }
      else
      {
        dataContext.favIcon = "/Images/NotFavoriteIcon.png";
        dataContext.LineTwoColor = (SolidColorBrush) null;
      }
      MainLandingViewModel.UpdateFavorite(dataContext, favorite);
    }

    private void LogIn_Click(object sender, RoutedEventArgs e)
    {
      this.NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
    }

    private void AppBarItemLog_Click(object sender, EventArgs e)
    {
      if (App.DataManager.SettingsMan.IsSignedIn)
        this.NavigationService.Navigate(new Uri("/AccountChooser.xaml", UriKind.Relative));
      else
        this.NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
    }

    private void refresh_Click(object sender, EventArgs e) => MainLanding.ViewModel.UpdateAllData();

    private void smartBar_Click(object sender, EventArgs e)
    {
      this.NavigationService.Navigate(new Uri("/SmartBar.xaml", UriKind.Relative));
    }

    private void ManageSubReddits_Click(object sender, EventArgs e)
    {
      this.NavigationService.Navigate(new Uri("/NewSubredditManager.xaml", UriKind.Relative));
    }

    private void SettingsButton_Click(object sender, EventArgs e)
    {
      this.NavigationService.Navigate(new Uri("/SettingPages/SettingsLanding.xaml", UriKind.Relative));
    }

    private void ViewMyProfile_Click(object sender, RoutedEventArgs e)
    {
      if (App.DataManager.SettingsMan.UserName == null || App.DataManager.SettingsMan.UserName.Equals(""))
        return;
      this.NavigationService.Navigate(new Uri("/ProfileView.xaml?userName=" + App.DataManager.SettingsMan.UserName, UriKind.Relative));
    }

    private void Messages_Click(object sender, RoutedEventArgs e)
    {
      if (!App.DataManager.SettingsMan.IsSignedIn)
        return;
      this.NavigationService.Navigate(new Uri("/MessageInbox.xaml", UriKind.Relative));
    }

    private void ManageSubreddit_Click(object sender, RoutedEventArgs e)
    {
      this.NavigationService.Navigate(new Uri("/NewSubredditManager.xaml", UriKind.Relative));
    }

    private void SubmitLink_Click(object sender, EventArgs e)
    {
      this.NavigationService.Navigate(new Uri("/SubmitLink.xaml", UriKind.Relative));
    }

    private void DonateButton_Click(object sender, EventArgs e)
    {
      this.NavigationService.Navigate(new Uri("/SettingPages/Donate.xaml", UriKind.Relative));
    }

    private void SwitchAccounts_Click(object sender, RoutedEventArgs e)
    {
      this.NavigationService.Navigate(new Uri("/AccountChooser.xaml", UriKind.Relative));
    }

    private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.MainPanorama.SelectedIndex == 0)
      {
        this.ApplicationBar.IsVisible = false;
        this.MainPanorama.ShowImage(true);
      }
      else if (this.MainPanorama.SelectedIndex == 1)
      {
        this.MainPanorama.ShowImage(false);
        this.ApplicationBar.IsVisible = true;
        this.ApplicationBar.Mode = ApplicationBarMode.Minimized;
      }
      else
      {
        this.MainPanorama.ShowImage(false);
        this.ApplicationBar.IsVisible = true;
        this.ApplicationBar.Mode = ApplicationBarMode.Default;
      }
      ++this.MainPanoSelectionChanged;
      if (this.MainPanoSelectionChanged != 35)
        return;
      App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("I <3 Gooby!", true, false, string.Empty, string.Empty));
      App.DataManager.BaconitAnalytics.LogEvent("Easter Egg. Gooby pls");
    }

    private void RecentTile_Click(object sender, RoutedEventArgs e)
    {
      MainLandingTile dataContext = ((FrameworkElement) sender).DataContext as MainLandingTile;
      switch (dataContext.TileKind)
      {
        case 1:
          this.NavigationService.Navigate(new Uri("/ProfileView.xaml?userName=" + dataContext.KeyElement, UriKind.Relative));
          break;
        case 2:
        case 3:
          App.ActiveStoryData = (SubRedditData) null;
          this.NavigationService.Navigate(new Uri("/StoryDetails.xaml?StoryDataRedditID=" + dataContext.KeyElement + "&Recenttile=true", UriKind.Relative));
          break;
      }
      App.DataManager.BaconitAnalytics.LogEvent("Recent Tile Tapped");
    }

    public void SetupFirstTimeSetting()
    {
      if (App.DataManager.SettingsMan.BackgroundAgentEnabled == -1 || !App.DataManager.SettingsMan.AdultFilterSet())
      {
        App.DataManager.BaconitAnalytics.LogPage("First Time Settings");
        if (DataManager.LIGHT_THEME)
          this.FirstTimeOptions.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
        this.FirstUseImage.Source = App.DataManager.SettingsMan.ScaleFactor != 100 ? (App.DataManager.SettingsMan.ScaleFactor != 160 ? (ImageSource) new BitmapImage(new Uri("/SplashScreenImage.Screen-720p.jpg", UriKind.Relative)) : (ImageSource) new BitmapImage(new Uri("/SplashScreenImage.Screen-WXGA.jpg", UriKind.Relative))) : (ImageSource) new BitmapImage(new Uri("/SplashScreenImage.Screen-WVGA.jpg", UriKind.Relative));
        this.FirstTimeOptions.Visibility = Visibility.Visible;
        this.ApplicationBar.IsVisible = false;
        this.FirstTimeWait.Visibility = Visibility.Visible;
        this.FirstUseLoading.IsIndeterminate = true;
        this.AdultFilterSwitch.IsChecked = new bool?(false);
        this.AutomaticUpdatingSwitch.IsChecked = new bool?(true);
        this.CloseFirstTimeSettings.Completed += (EventHandler) ((sefnder, de) => this.FirstTimeOptions.Visibility = Visibility.Collapsed);
        this.ShowLandingPane(0);
        ThreadPool.QueueUserWorkItem((WaitCallback) (obj =>
        {
          Thread.Sleep(5000);
          this.Dispatcher.BeginInvoke((Action) (() => this.CloseFirstTimeUse.Begin()));
        }));
      }
      else
      {
        if (!App.DataManager.FirstRunAfterUpdate)
          return;
        App.DataManager.FirstRunAfterUpdate = false;
        this.ShowLandingPane(1);
      }
    }

    private void CloseFirstTime_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      App.DataManager.SettingsMan.BackgroundAgentEnabled = !this.AutomaticUpdatingSwitch.IsChecked.Value ? 0 : 1;
      App.DataManager.SettingsMan.AdultFilter = this.AdultFilterSwitch.IsChecked.Value;
      try
      {
        App.DataManager.VIPDataMan.Save();
      }
      catch
      {
      }
      this.ApplicationBar.IsVisible = true;
      this.CloseFirstTimeSettings.Begin();
    }

    private void FirstTimeWait_Done(object sender, EventArgs e)
    {
      this.FirstTimeWait.Visibility = Visibility.Collapsed;
      this.FirstUseLoading.IsIndeterminate = false;
      this.ShowFirstTimeSettings.Begin();
    }

    public void UpdateMainPageLanding()
    {
      this.MainPanorama.ImageBackground = (BitmapSource) App.DataManager.MainLandingImageMan.GetImage();
      if (this.HasTextLoaded || this.HelloHolder.Visibility != Visibility.Collapsed)
        return;
      this.HasTextLoaded = true;
      App.DataManager.MainLandingImageMan.GetText(new RunWorkerCompletedEventHandler(this.UpdateTextCallBack));
    }

    public void UpdateTextCallBack(object obj, RunWorkerCompletedEventArgs res)
    {
      if (obj == null)
        return;
      SubRedditData subRedditDataObj = (SubRedditData) obj;
      subRedditDataObj.Title = HttpUtility.HtmlDecode(subRedditDataObj.Title);
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        switch (subRedditDataObj.SubRedditRank)
        {
          case 0:
            this.TopStoryText1.Text = subRedditDataObj.Title;
            break;
          case 1:
            this.TopStoryText2.Text = subRedditDataObj.Title;
            break;
          case 2:
            this.TopStoryText3.Text = subRedditDataObj.Title;
            this.FadeInTopStories.Begin();
            break;
        }
      }));
    }

    private void TopStory_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.NavigationService.Navigate(new Uri("/RedditsViewer.xaml?subredditURL=" + HttpUtility.UrlEncode(App.DataManager.MainLandingImageMan.GetTopSubreddit()) + "&isFromMain=true", UriKind.Relative));
    }

    public void ShowLandingPane(int which)
    {
      this.UpdateHolder.Visibility = Visibility.Collapsed;
      this.HelloHolder.Visibility = Visibility.Collapsed;
      this.TopStoryHolder.Visibility = Visibility.Collapsed;
      switch (which)
      {
        case 0:
          this.HelloHolder.Visibility = Visibility.Visible;
          break;
        case 1:
          this.UpdateHolder.Visibility = Visibility.Visible;
          break;
        case 2:
          this.TopStoryHolder.Visibility = Visibility.Visible;
          break;
      }
    }

    public void SetAppBarLogin()
    {
      if (this.AccountRemoved)
      {
        this.AccountRemoved = false;
        ((ApplicationBarIconButton) this.ApplicationBar.Buttons[2]).IsEnabled = false;
        ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[1]).Text = "log in";
      }
      if (!this.AccountAdded)
        return;
      this.AccountAdded = false;
      ((ApplicationBarIconButton) this.ApplicationBar.Buttons[2]).IsEnabled = true;
      ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[1]).Text = "log out";
    }

    public bool CheckQueryParams()
    {
      bool flag = false;
      IDictionary<string, string> queryString = this.NavigationContext.QueryString;
      if (queryString.ContainsKey("FileId"))
      {
        if (!App.DataManager.SettingsMan.IsSignedIn)
        {
          int num = (int) MessageBox.Show("You must be logged into an account submit images!", "Please Log In", MessageBoxButton.OK);
          return false;
        }
        App.DataManager.BaconitAnalytics.LogEvent("Image app extension used");
        Picture pictureFromToken = new MediaLibrary().GetPictureFromToken(queryString["FileId"]);
        SubmitLink.SelectedImage = new BitmapImage();
        SubmitLink.SelectedImage.CreateOptions = BitmapCreateOptions.None;
        SubmitLink.SelectedImage.SetSource(pictureFromToken.GetImage());
        this.NavigationService.Navigate(new Uri("/SubmitLink.xaml?ImageAlreadyReady=true&RemoveBack=true", UriKind.Relative));
        flag = true;
      }
      if (queryString.ContainsKey("token"))
      {
        if (!App.DataManager.SettingsMan.IsSignedIn)
        {
          int num = (int) MessageBox.Show("You must be logged into submit images!", "Please Log In", MessageBoxButton.OK);
          return false;
        }
        Picture pictureFromToken = new MediaLibrary().GetPictureFromToken(queryString["token"]);
        SubmitLink.SelectedImage = new BitmapImage();
        SubmitLink.SelectedImage.CreateOptions = BitmapCreateOptions.None;
        SubmitLink.SelectedImage.SetSource(pictureFromToken.GetImage());
        App.DataManager.BaconitAnalytics.LogEvent("Image app extension used");
        this.NavigationService.Navigate(new Uri("/SubmitLink.xaml?ImageAlreadyReady=true", UriKind.Relative));
        flag = true;
      }
      if (queryString.ContainsKey("GoToMessages"))
      {
        App.DataManager.BaconitAnalytics.LogEvent("Opened from toast to messages");
        this.NavigationService.Navigate(new Uri("/MessageInbox.xaml?ForceRefresh=True", UriKind.Relative));
        flag = true;
      }
      if (queryString.ContainsKey("WallpaperSettings"))
      {
        App.DataManager.BaconitAnalytics.LogEvent("Opened from wallpaper settings");
        this.NavigationService.Navigate(new Uri("/SettingPages/LiveTiles.xaml", UriKind.Relative));
        flag = true;
      }
      if (queryString.ContainsKey("BaconSyncUrl"))
      {
        App.DataManager.BaconitAnalytics.LogEvent("BaconSync Toast Handle");
        App.DataManager.BaconSyncObj.HandlePushUrl(queryString["BaconSyncUrl"]);
        flag = true;
      }
      return flag;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/MainLanding.xaml", UriKind.Relative));
      this.ShowProgressIndicator = (Storyboard) this.FindName("ShowProgressIndicator");
      this.HideProgressIndicator = (Storyboard) this.FindName("HideProgressIndicator");
      this.CloseFirstTimeSettings = (Storyboard) this.FindName("CloseFirstTimeSettings");
      this.CloseFirstTimeUse = (Storyboard) this.FindName("CloseFirstTimeUse");
      this.ShowFirstTimeSettings = (Storyboard) this.FindName("ShowFirstTimeSettings");
      this.MainPanorama = (DynamicBackgroundPanorama) this.FindName("MainPanorama");
      this.HelloHolder = (StackPanel) this.FindName("HelloHolder");
      this.UpdateHolder = (StackPanel) this.FindName("UpdateHolder");
      this.TopStoryHolder = (StackPanel) this.FindName("TopStoryHolder");
      this.TopStoryMessages = (TextBlock) this.FindName("TopStoryMessages");
      this.TopStory1 = (Button) this.FindName("TopStory1");
      this.TopStoryText1 = (TextBlock) this.FindName("TopStoryText1");
      this.TopStory2 = (Button) this.FindName("TopStory2");
      this.TopStoryText2 = (TextBlock) this.FindName("TopStoryText2");
      this.TopStory3 = (Button) this.FindName("TopStory3");
      this.TopStoryText3 = (TextBlock) this.FindName("TopStoryText3");
      this.FadeInTopStories = (Storyboard) this.FindName("FadeInTopStories");
      this.RedditsList = (LongListSelector) this.FindName("RedditsList");
      this.LoggedInHolderUI = (StackPanel) this.FindName("LoggedInHolderUI");
      this.MessagesTitleUI = (TextBlock) this.FindName("MessagesTitleUI");
      this.MessagesSubTextUI = (TextBlock) this.FindName("MessagesSubTextUI");
      this.AccountSubText = (TextBlock) this.FindName("AccountSubText");
      this.LoggedOutHolderUI = (StackPanel) this.FindName("LoggedOutHolderUI");
      this.RecentTiles = (ItemsControl) this.FindName("RecentTiles");
      this.RecentNothingText = (TextBlock) this.FindName("RecentNothingText");
      this.LoadingBar = (ProgressBar) this.FindName("LoadingBar");
      this.FirstTimeOptions = (StackPanel) this.FindName("FirstTimeOptions");
      this.FirstTimeHolder = (StackPanel) this.FindName("FirstTimeHolder");
      this.FirstTimeTitle = (TextBlock) this.FindName("FirstTimeTitle");
      this.FirstTimeAutoTitle = (TextBlock) this.FindName("FirstTimeAutoTitle");
      this.FistTimeAutoText = (TextBlock) this.FindName("FistTimeAutoText");
      this.AutomaticUpdatingSwitch = (ToggleSwitch) this.FindName("AutomaticUpdatingSwitch");
      this.FirstTimeContentTitle = (TextBlock) this.FindName("FirstTimeContentTitle");
      this.FirstTimeConentText = (TextBlock) this.FindName("FirstTimeConentText");
      this.AdultFilterSwitch = (ToggleSwitch) this.FindName("AdultFilterSwitch");
      this.CloseFirstTime = (Grid) this.FindName("CloseFirstTime");
      this.FirstTimeWait = (Grid) this.FindName("FirstTimeWait");
      this.FirstUseImage = (Image) this.FindName("FirstUseImage");
      this.FirstUseText = (TextBlock) this.FindName("FirstUseText");
      this.FirstUseLoading = (ProgressBar) this.FindName("FirstUseLoading");
    }
  }
}
