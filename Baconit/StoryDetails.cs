// Decompiled with JetBrains decompiler
// Type: Baconit.StoryDetails
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Baconit.Database;
using Baconit.SettingPages;
using BaconitData.Database;
using BaconitData.Interfaces;
using BaconitData.Libs;
using CollapseControl;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using Microsoft.Xna.Framework.GamerServices;
using Phone.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

#nullable disable
namespace Baconit
{
  public class StoryDetails : PhoneApplicationPage
  {
    public const byte APP_BAR_HEIGHT = 85;
    public StoryDetailsViewModel ViewModel;
    public RedditsViewer SubRedditHost;
    public bool LoadingStoryDataOpen;
    public bool CommentActionPanelOpen;
    public bool TitleActionPanelOpen;
    public bool CommentingPanelOpen;
    public bool FullScreenCommentingPanelOpen;
    public bool SharePickerOpen;
    public bool IngoreNextClick;
    public bool HelpOverlayOpen;
    public bool SelfTextOpen;
    public bool IsPinned;
    public bool NeedsToLoadData;
    public string CommentingOnRName = "";
    public bool CommentingOnIsEdit;
    public bool cancelTitleClick;
    private StoryDetails.CommentTapThread CommentTapThreadObj;
    private CommentData CommentActionOpenedFor;
    public string ShowComment = "";
    public bool isOpen;
    private bool firstOpen_SelfText = true;
    private PageOrientation CurOrientation = PageOrientation.Portrait;
    private bool restoreAppBar;
    public SubRedditData LocalStory;
    private bool isOpening = true;
    private bool isPinnedOptionsOpened;
    private bool isLoaded;
    public int SortShowing;
    private List<CommentData> ProgressBarList = new List<CommentData>();
    private bool RestoreLoadingBar;
    private bool AppBarIsMini;
    private PickerBoxDialog sharePicker;
    private CommentData shareComment;
    private string[] PickerData = new string[5]
    {
      "Social Networks",
      "Tap + Send",
      "SMS",
      "Email",
      "Copy Link"
    };
    public readonly DependencyProperty ListVerticalOffsetProperty = DependencyProperty.Register(nameof (ListVerticalOffset), typeof (double), typeof (StoryDetails), new PropertyMetadata(new PropertyChangedCallback(StoryDetails.OnListVerticalOffsetChanged)));
    private bool barClosed;
    public ScrollViewer _listScrollViewer;
    public double TitleBarHeight = -1.0;
    private bool DontOpenAppBar;
    private string FullScreenCommentingtext = "";
    private string text = "";
    internal Storyboard OpenCommenting;
    internal Storyboard CloseCommenting;
    internal Storyboard FadeOutTips;
    internal Storyboard FadeInTips;
    internal Storyboard FadeOutSelfTips;
    internal Storyboard FadeInSelfTips;
    internal Storyboard OpenCommentActionsAni;
    internal Storyboard CloseCommentActionsAni;
    internal Storyboard OpenFullScreenCommentingAni;
    internal Storyboard CloseFullScreenCommentingAni;
    internal Storyboard OpenStoryActionsAnmi;
    internal Storyboard CloseStoryActionsAnmi;
    internal Storyboard HideViewEntrieThread;
    internal Storyboard FadeOutLoadingOverLay;
    internal Storyboard FadeInLoadingOverLay;
    internal Storyboard ChangeSelfText;
    internal EasingDoubleKeyFrame ChangeSelfTextFrom;
    internal EasingDoubleKeyFrame ChangeSelfTextTo;
    internal Storyboard ExpandTitleBar;
    internal EasingDoubleKeyFrame ExpandTitleBarKeyFrame;
    internal Storyboard CollpaseTitleBar;
    internal EasingDoubleKeyFrame CollpaseTitleBarKeyFrame;
    internal Storyboard OpenPinPicker;
    internal Storyboard ClosePinPicker;
    internal Grid LayoutRoot;
    internal StackPanel TitlePanel;
    internal TextBlock RedditTitle;
    internal TextBlock LineOne;
    internal TextBlock LineTwo;
    internal Grid SelfTextHolder;
    internal ProgressBar SelfTextProgressBar;
    internal ScrollViewer SelfTextScroller;
    internal SuperRichTextBox SelfText;
    internal Rectangle SelfTextDevider;
    internal Grid ViewEntireThread;
    internal ProgressBar LoadingCommentsBar;
    internal ListBox CommentList;
    internal Grid NoComments;
    internal Grid BlackBackground;
    internal Grid CommentActionPanel;
    internal ScrollViewer CommentActionPanelContentScroller;
    internal StackPanel CommentActionPanelContentHolder;
    internal TextBlock CommentActionLineOneOne;
    internal TextBlock CommentActionLineOneTwo;
    internal SuperRichTextBox CommentActionCommentContent;
    internal Grid CommentActionPanelVertHolder;
    internal Grid CommentUpVote;
    internal TextBlock CommentUpVoteText;
    internal Grid CommentDownVote;
    internal TextBlock CommentDownVoteText;
    internal Grid CommentShare;
    internal Grid Profile;
    internal Grid CommentOn;
    internal TextBlock CommentOnText;
    internal StackPanel EditButtons;
    internal Grid EditButton;
    internal Grid DeleteButton;
    internal Grid CommentActionPanelHorzHolder;
    internal Grid CommentUpVoteHorz;
    internal TextBlock CommentUpVoteTextHorz;
    internal Grid CommentDownVoteHorz;
    internal TextBlock CommentDownVoteTextHorz;
    internal Grid CommentOnHorz;
    internal TextBlock CommentOnTextHorz;
    internal Grid EditOnHorz;
    internal Grid CommentShareHorz;
    internal StackPanel TitleActionPanel;
    internal Grid TitleCommentOn;
    internal StackPanel EditButtonsTitle;
    internal Grid EditButtonTitle;
    internal TextBlock EditButtonTitleButton;
    internal Grid DeleteButtonTitle;
    internal Grid TitleOpenLink;
    internal TextBlock TitleOpenLinkText;
    internal Grid TitleShare;
    internal TextBlock TitleShareText;
    internal Image StoryOverlay;
    internal Image SelfTextOverlay;
    internal Grid CommentingDialog;
    internal ScrollViewer CommentingScroller;
    internal TextBox CommentingText;
    internal Button CommentingSubmit;
    internal Button CommentingClose;
    internal Grid TilePicker;
    internal ListPicker TileIcon;
    internal Button ImageTileButton;
    internal TextBlock PinSettingsSubNameImage;
    internal Image PinSettingsSampleImage;
    internal TextBlock PinSettingsSubName;
    internal Grid FullScreenBlackBackground;
    internal Grid FullScreenCommenting;
    internal ScrollViewer TitleHolder;
    internal StackPanel FullScreenCommentingContextHolder;
    internal TextBlock FullScreenCommentingLineOne;
    internal TextBlock FullScreenCommentingLineTwo;
    internal SuperRichTextBox FullScreenCommentingComment;
    internal ScrollViewer FullScreenCommentingScroller;
    internal TextBox FullScreenCommentingBox;
    internal Grid FullScreenSubmit;
    internal Grid FullScreenCancel;
    internal RichTextBox FullScreenCommentingFormattingPreview;
    internal Grid LoadingOverLay;
    internal Rectangle LoadingOverlayBackground;
    internal TextBlock LoadingText;
    internal ProgressBar LoadingOverlayBar;
    private bool _contentLoaded;

    public StoryDetails()
    {
      this.InitializeComponent();
      this.ViewModel = new StoryDetailsViewModel(this);
      this.DataContext = (object) this.ViewModel;
      this.Loaded += new RoutedEventHandler(this.StoryDetails_Loaded);
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        this.sharePicker = new PickerBoxDialog();
        this.sharePicker.ItemSource = (IEnumerable) this.PickerData;
        this.sharePicker.Title = "SHARE VIA";
        this.sharePicker.Closed += new EventHandler(this.sharePicker_closed);
      }));
      TransitionService.SetNavigationInTransition((UIElement) this, App.SlideInTransistion);
      TransitionService.SetNavigationOutTransition((UIElement) this, App.SlideOutTransistion);
      this.ApplicationBar = (IApplicationBar) new Microsoft.Phone.Shell.ApplicationBar();
      if (App.DataManager.SettingsMan.ShowStoryOverLay)
        this.ApplicationBar.IsVisible = false;
      else
        this.ApplicationBar.IsVisible = true;
      ApplicationBarIconButton applicationBarIconButton1 = new ApplicationBarIconButton(new Uri("/Images/AppBar.List.png", UriKind.Relative));
      applicationBarIconButton1.Text = "sort";
      applicationBarIconButton1.Click += new EventHandler(this.ChangeSort_Click);
      this.ApplicationBar.Buttons.Add((object) applicationBarIconButton1);
      ApplicationBarIconButton applicationBarIconButton2 = new ApplicationBarIconButton(new Uri("/Images/Arrows/brow_up_none.png", UriKind.Relative));
      applicationBarIconButton2.Text = "upvote";
      applicationBarIconButton2.Click += new EventHandler(this.UpVote_Click);
      this.ApplicationBar.Buttons.Add((object) applicationBarIconButton2);
      ApplicationBarIconButton applicationBarIconButton3 = new ApplicationBarIconButton(new Uri("/Images/Arrows/brow_down_none.png", UriKind.Relative));
      applicationBarIconButton3.Text = "downvote";
      applicationBarIconButton3.Click += new EventHandler(this.DownVote_Click);
      this.ApplicationBar.Buttons.Add((object) applicationBarIconButton3);
      ApplicationBarIconButton applicationBarIconButton4 = new ApplicationBarIconButton(new Uri("/Images/pushpin.png", UriKind.Relative));
      applicationBarIconButton4.Text = "pin to start";
      applicationBarIconButton4.Click += new EventHandler(this.Pin_Click);
      this.ApplicationBar.Buttons.Add((object) applicationBarIconButton4);
      ApplicationBarMenuItem applicationBarMenuItem1 = new ApplicationBarMenuItem("share comments...");
      applicationBarMenuItem1.Click += new EventHandler(this.Share_Click);
      this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem1);
      ApplicationBarMenuItem applicationBarMenuItem2 = new ApplicationBarMenuItem("refresh");
      applicationBarMenuItem2.Click += new EventHandler(this.refresh_Click);
      this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem2);
      ApplicationBarMenuItem applicationBarMenuItem3 = new ApplicationBarMenuItem("save story");
      applicationBarMenuItem3.Click += new EventHandler(this.Save_Click);
      this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem3);
      ApplicationBarMenuItem applicationBarMenuItem4 = new ApplicationBarMenuItem("hide story");
      applicationBarMenuItem4.Click += new EventHandler(this.Hide_Click);
      this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem4);
      ApplicationBarMenuItem applicationBarMenuItem5 = new ApplicationBarMenuItem("lock screen orientation");
      applicationBarMenuItem5.Click += new EventHandler(this.lockOren_Click);
      this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem5);
      ApplicationBarMenuItem applicationBarMenuItem6 = new ApplicationBarMenuItem("show help tips");
      applicationBarMenuItem6.Click += new EventHandler(this.ShowTips_Click);
      this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem6);
      this.ApplicationBar.StateChanged += new EventHandler<ApplicationBarStateChangedEventArgs>(this.ApplicationBar_StateChanged);
      this.CloseFullScreenCommentingAni.Completed += new EventHandler(this.CloseFullScreenColsed_Completed);
      this.FadeOutLoadingOverLay.Completed += new EventHandler(this.CloseLoadingDataDone);
      this.FadeOutTips.Completed += new EventHandler(this.FadeOutTipsComplete);
      this.FadeInSelfTips.Completed += new EventHandler(this.SelfTextOverlay_Opened);
      this.FadeOutSelfTips.Completed += new EventHandler(this.FadeOutSelfTipsComplete);
      this.CloseCommentActionsAni.Completed += new EventHandler(this.CommentActionClosed);
      this.CloseStoryActionsAnmi.Completed += new EventHandler(this.TitleActionClosed);
      this.HideViewEntrieThread.Completed += (EventHandler) ((sender, e) => this.ViewEntireThread.Visibility = Visibility.Collapsed);
      if (DataManager.LIGHT_THEME)
      {
        this.LoadingOverlayBackground.Fill = (Brush) new SolidColorBrush(Color.FromArgb((byte) 207, byte.MaxValue, byte.MaxValue, byte.MaxValue));
        this.BlackBackground.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
        this.FullScreenBlackBackground.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
        this.CommentActionPanel.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
        this.CommentActionPanelContentHolder.Background = (Brush) new SolidColorBrush(Color.FromArgb((byte) 207, byte.MaxValue, byte.MaxValue, byte.MaxValue));
        this.CommentActionCommentContent.Foreground = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, (byte) 0));
        this.CommentingDialog.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 200, (byte) 200, (byte) 200));
        this.SelfTextHolder.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 208, (byte) 208, (byte) 208));
        this.SelfTextDevider.Fill = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 222, (byte) 222, (byte) 222));
        this.TitlePanel.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 222, (byte) 222, (byte) 222));
        this.ViewEntireThread.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 180, (byte) 180, (byte) 180));
        this.FullScreenBlackBackground.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
        this.FullScreenCommentingComment.Foreground = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, (byte) 0));
        this.FullScreenCommentingFormattingPreview.Foreground = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, (byte) 0));
        this.FullScreenCommentingContextHolder.Background = (Brush) new SolidColorBrush(Color.FromArgb((byte) 207, byte.MaxValue, byte.MaxValue, byte.MaxValue));
      }
      this.SortShowing = App.DataManager.SettingsMan.DefaultCommentSort;
      this.SelfTextScroller.AddHandler(UIElement.ManipulationCompletedEvent, (Delegate) new EventHandler<ManipulationCompletedEventArgs>(this.SelfTextHolder_ManipulationCompleted), true);
    }

    private void StoryDetails_Loaded(object sender, RoutedEventArgs e)
    {
      this.isOpening = false;
      if (App.navService == null)
        App.navService = this.NavigationService;
      if (this.LocalStory == null)
        return;
      if (!this.ViewModel.IsDataLoaded)
        ThreadPool.QueueUserWorkItem((WaitCallback) (obj => this.ViewModel.LoadData()));
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        if (App.DataManager.SettingsMan.ShowStoryOverLay)
        {
          this.HelpOverlayOpen = true;
          this.ApplicationBar.IsVisible = false;
          this.StoryOverlay.Source = (ImageSource) new BitmapImage(new Uri("/Images/StoryOverlay-" + (object) App.DataManager.SettingsMan.ScaleFactor + ".png", UriKind.Relative));
          this.StoryOverlay.Visibility = Visibility.Visible;
          this.SupportedOrientations = SupportedPageOrientation.Portrait;
        }
        else if (App.DataManager.SettingsMan.ShowStorySelfOverLay && this.LocalStory.isSelf && this.LocalStory.hasSelfText)
        {
          this.HelpOverlayOpen = true;
          this.ApplicationBar.IsVisible = false;
          this.SelfTextOverlay.Source = (ImageSource) new BitmapImage(new Uri("/Images/StorySelfOverlay-" + (object) App.DataManager.SettingsMan.ScaleFactor + ".png", UriKind.Relative));
          this.SelfTextOverlay.Visibility = Visibility.Visible;
          this.SupportedOrientations = SupportedPageOrientation.Portrait;
        }
        if (this.LocalStory.isHidden)
          ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[3]).Text = "unhide story";
        else
          ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[3]).Text = "hide story";
        if (this.LocalStory.isSaved)
          ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[2]).Text = "unsave story";
        else
          ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[2]).Text = "save story";
      }));
      ThreadPool.QueueUserWorkItem((WaitCallback) (obj =>
      {
        if (App.DataManager.SettingsMan.OpenedCommentsCounter == 14)
        {
          if (!this.LocalStory.isSelf)
            return;
          this.Dispatcher.BeginInvoke((Action) (() =>
          {
            App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("New tip! Tap here", true, true, "Pro Tip", "You can expand the self-text box to fill the entire screen. Place your finger on the self-text and drag to the bottom of the screen."));
            ++App.DataManager.SettingsMan.OpenedCommentsCounter;
          }));
        }
        else if (App.DataManager.SettingsMan.OpenedCommentsCounter == 10)
        {
          ++App.DataManager.SettingsMan.OpenedCommentsCounter;
          this.Dispatcher.BeginInvoke((Action) (() => App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("New tip! Tap here", true, true, "Did You Know?", "You can hide a comment and all of its children by double tapping or swiping over it? Try it!"))));
        }
        else if (App.DataManager.SettingsMan.OpenedCommentsCounter == 6)
        {
          if (!this.LocalStory.isSelf)
            return;
          this.Dispatcher.BeginInvoke((Action) (() => App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("New tip! Tap here", true, true, "Did You Know?", "You can expand the self text box by double tapping it? Try it!"))));
          ++App.DataManager.SettingsMan.OpenedCommentsCounter;
        }
        else
          ++App.DataManager.SettingsMan.OpenedCommentsCounter;
      }));
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      if (e.Uri.OriginalString.StartsWith("/StoryDetails.xaml"))
      {
        if (this.CommentActionPanelOpen)
        {
          this.CloseCommentActions(true);
          this.CommentActionPanelOpen = false;
        }
        if (this.TitleActionPanelOpen)
        {
          this.TitleActionPanel.Visibility = Visibility.Collapsed;
          this.BlackBackground.Visibility = Visibility.Collapsed;
          this.TitleActionPanelOpen = false;
        }
        if (this.CommentingPanelOpen && this.CommentingDialog != null)
        {
          this.CommentingText.Text = "";
          this.CommentingDialog.Visibility = Visibility.Collapsed;
          this.CommentingDialog.MaxHeight = 0.0;
          this.CommentingPanelOpen = false;
        }
      }
      this.isOpen = false;
      this.SelfTextProgressBar.IsIndeterminate = false;
      this.LoadingOverlayBar.IsIndeterminate = false;
      this.RestoreLoadingBar = this.LoadingCommentsBar.IsIndeterminate;
      this.LoadingCommentsBar.IsIndeterminate = false;
      foreach (CommentData progressBar in this.ProgressBarList)
        progressBar.SendingProgressBarIsInd = false;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      this.isOpen = true;
      App.DataManager.BaconitAnalytics.LogPage("Story Details");
      if (this.SubRedditHost == null)
        this.SubRedditHost = App.RedditViewerTransfter;
      if (App.navService == null)
        App.navService = this.NavigationService;
      if (this.RestoreLoadingBar)
      {
        this.RestoreLoadingBar = false;
        this.LoadingCommentsBar.IsIndeterminate = true;
      }
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        SystemTray.IsVisible = App.DataManager.SettingsMan.ShowSystemBar;
        if (this.restoreAppBar)
          this.ApplicationBar.IsVisible = true;
        if (App.DataManager.SettingsMan.ScreenOrenLocked)
        {
          if (App.DataManager.SettingsMan.ScreenOrenLockedtoPortrait)
            this.SupportedOrientations = SupportedPageOrientation.Portrait;
          else
            this.SupportedOrientations = SupportedPageOrientation.Landscape;
          ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[4]).Text = "unlock screen orientation";
        }
        else
        {
          this.SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;
          ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[4]).Text = "lock screen orientation";
        }
      }));
      if (this.isLoaded)
        return;
      if (this.NavigationContext.QueryString.ContainsKey("ShowComment"))
      {
        this.ShowComment = this.NavigationContext.QueryString["ShowComment"];
        this.SortShowing = 6;
        this.ViewEntireThread.Visibility = Visibility.Visible;
        this.SelfTextDevider.Visibility = Visibility.Collapsed;
      }
      if (this.NavigationContext.QueryString.ContainsKey("ClearBack"))
      {
        try
        {
          this.NavigationService.RemoveBackEntry();
        }
        catch
        {
        }
      }
      string str = "";
      if (this.NavigationContext.QueryString.ContainsKey("StoryDataRedditID"))
      {
        str = this.NavigationContext.QueryString["StoryDataRedditID"];
        if (App.ActiveStoryData != null && App.ActiveStoryData.RedditID != null && App.ActiveStoryData.RedditID.Equals(str))
          this.LocalStory = App.ActiveStoryData;
      }
      if (this.LocalStory == null)
      {
        if (this.NavigationContext.QueryString.ContainsKey("TileType"))
        {
          if (this.NavigationService.CanGoBack)
            this.NavigationService.RemoveBackEntry();
          this.IsPinned = true;
          ((ApplicationBarIconButton) this.ApplicationBar.Buttons[3]).IsEnabled = false;
        }
        if (str != null && !str.Equals(""))
        {
          this.LocalStory = App.DataManager.BaconitStore.GetStory(str);
          if (this.LocalStory == null)
            this.LocalStory = App.DataManager.BaconitStore.GetPinnedStory(str);
        }
      }
      if (this.LocalStory != null)
      {
        ThreadPool.QueueUserWorkItem((WaitCallback) (obj =>
        {
          ShellTile TileToFind = ShellTile.ActiveTiles.FirstOrDefault<ShellTile>((Func<ShellTile, bool>) (x => x.NavigationUri.ToString().Contains("StoryDataRedditID=" + this.LocalStory.RedditID)));
          this.IsPinned = TileToFind != null;
          this.Dispatcher.BeginInvoke((Action) (() => ((ApplicationBarIconButton) this.ApplicationBar.Buttons[3]).IsEnabled = TileToFind == null));
        }));
        this.SetStoryData(this.LocalStory, true);
        ThreadPool.QueueUserWorkItem((WaitCallback) (obj => App.DataManager.SettingsMan.AddMainLandingTile(new MainLandingTile(2, this.LocalStory.RedditID, this.LocalStory.Title))));
      }
      else
      {
        try
        {
          this.ApplicationBar.IsVisible = false;
          this.NeedsToLoadData = true;
          string temp = str.Substring(3);
          ThreadPool.QueueUserWorkItem((WaitCallback) (obj => App.DataManager.UpdateStoryComments((CommentDetailsViewModelInterface) this.ViewModel, temp, "LoadingOnTheFly", true, this.SortShowing, this.ShowComment)));
          this.LoadingStoryDataOpen = true;
          this.LoadingOverLay.Visibility = Visibility.Visible;
          this.FadeInLoadingOverLay.Begin();
          this.LoadingOverlayBar.Visibility = Visibility.Visible;
          this.LoadingOverlayBar.IsIndeterminate = true;
          this.NoComments.Visibility = Visibility.Collapsed;
          this.CommentList.Visibility = Visibility.Collapsed;
        }
        catch
        {
          int num = (int) MessageBox.Show("Sorry, we couldn't load the story. Try again later.", "Failed to Load", MessageBoxButton.OK);
          this.Dispatcher.BeginInvoke((Action) (() =>
          {
            try
            {
              this.NavigationService.GoBack();
            }
            catch
            {
            }
          }));
        }
      }
      this.isLoaded = true;
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
        if (this.CommentActionPanelOpen)
        {
          this.CloseCommentActions(true);
          e.Cancel = true;
        }
        else if (this.TitleActionPanelOpen)
        {
          this.CloseTitleActions();
          e.Cancel = true;
        }
        else if (this.CommentingPanelOpen)
        {
          this.CommentingText.Text = "";
          this.CloseCommenting.Begin();
          e.Cancel = true;
        }
        else if (this.FullScreenCommentingPanelOpen)
        {
          this.CloseFullScreenCommentingAni.Begin();
          e.Cancel = true;
        }
        else if (this.SharePickerOpen)
          this.SharePickerOpen = false;
        else if (this.isPinnedOptionsOpened)
        {
          e.Cancel = true;
          this.ClosePinPicker.Begin();
          this.isPinnedOptionsOpened = false;
        }
        else
        {
          if (this.NavigationService.CanGoBack)
            return;
          this.NavigationService.Navigate(new Uri("/MainLanding.xaml?RemoveBack=true", UriKind.Relative));
          e.Cancel = true;
        }
      }
    }

    public void LoadingStoryFailed()
    {
      if (this.LoadingOverLay.Visibility != Visibility.Visible)
        return;
      try
      {
        this.NavigationService.GoBack();
      }
      catch
      {
      }
    }

    public void SetStoryData(SubRedditData stroy, bool isUpdate)
    {
      if (this.NeedsToLoadData)
      {
        this.LocalStory = stroy;
        this.NeedsToLoadData = false;
        this.FadeOutLoadingOverLay.Begin();
        this.LoadingStoryDataOpen = false;
        ((ApplicationBarIconButton) this.ApplicationBar.Buttons[3]).IsEnabled = ShellTile.ActiveTiles.FirstOrDefault<ShellTile>((Func<ShellTile, bool>) (x => x.NavigationUri.ToString().Contains("StoryDataRedditID=" + this.LocalStory.RedditID))) == null;
        this.ApplicationBar.IsVisible = true;
        this.ViewModel.SetLoadingBar(true);
        ThreadPool.QueueUserWorkItem((WaitCallback) (obj => App.DataManager.SettingsMan.AddMainLandingTile(new MainLandingTile(2, this.LocalStory.RedditID, this.LocalStory.Title))));
      }
      if (isUpdate)
      {
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds((double) stroy.created);
        TimeSpan timeSpan = DateTime.Now.ToUniversalTime().Subtract(dateTime);
        this.LocalStory.LineOne = Math.Abs(timeSpan.Days) <= 0 ? (Math.Abs(timeSpan.Hours) <= 0 ? (Math.Abs(timeSpan.Minutes) <= 0 ? (Math.Abs(timeSpan.Seconds) != 1 ? Math.Abs(timeSpan.Seconds).ToString() + " secs" : "1 sec") : (Math.Abs(timeSpan.Minutes) != 1 ? Math.Abs(timeSpan.Minutes).ToString() + " mins" : "1 min")) : (Math.Abs(timeSpan.Hours) != 1 ? Math.Abs(timeSpan.Hours).ToString() + " hrs" : "1 hr")) : (Math.Abs(timeSpan.Days) != 1 ? Math.Abs(timeSpan.Days).ToString() + " days" : "1 day");
        if (this.LocalStory.SubRedditURL.Equals("/") || this.LocalStory.SubRedditURL.Equals("/r/all/") || this.LocalStory.SubRedditURL.Equals("/saved/"))
        {
          SubRedditData localStory = this.LocalStory;
          localStory.LineOne = localStory.LineOne + " ago to " + this.LocalStory.SubReddit;
        }
        else
        {
          SubRedditData localStory = this.LocalStory;
          localStory.LineOne = localStory.LineOne + " ago by " + this.LocalStory.Author;
        }
        this.LocalStory.LineTwo = this.LocalStory.comments.ToString() + " comments; " + this.LocalStory.Domain;
        ThreadPool.QueueUserWorkItem((WaitCallback) (obj => App.DataManager.TileMan.UpdateSingleStory(this.LocalStory)));
      }
      if (this.LocalStory.Like == 1)
      {
        ((ApplicationBarIconButton) this.ApplicationBar.Buttons[1]).IconUri = new Uri(App.BAR_UP_VOTE_SELECT, UriKind.Relative);
        ((ApplicationBarIconButton) this.ApplicationBar.Buttons[2]).IconUri = new Uri("/Images/Arrows/brow_down_none.png", UriKind.Relative);
      }
      else if (this.LocalStory.Like == -1)
      {
        ((ApplicationBarIconButton) this.ApplicationBar.Buttons[1]).IconUri = new Uri("/Images/Arrows/brow_up_none.png", UriKind.Relative);
        ((ApplicationBarIconButton) this.ApplicationBar.Buttons[2]).IconUri = new Uri(App.BAR_DOWN_VOTE_SELECT, UriKind.Relative);
      }
      else
      {
        ((ApplicationBarIconButton) this.ApplicationBar.Buttons[1]).IconUri = new Uri("/Images/Arrows/brow_up_none.png", UriKind.Relative);
        ((ApplicationBarIconButton) this.ApplicationBar.Buttons[2]).IconUri = new Uri("/Images/Arrows/brow_down_none.png", UriKind.Relative);
      }
      App.DataManager.SettingsMan.ReadStories[this.LocalStory.RedditID] = this.LocalStory.comments;
      this.RedditTitle.Text = HttpUtility.HtmlDecode(this.LocalStory.Title.Trim());
      this.LineOne.Text = this.LocalStory.LineOne;
      this.LineTwo.Text = "(" + (object) this.LocalStory.ups + "," + (object) this.LocalStory.downs + ") points; " + (object) this.LocalStory.comments + " comments";
      if (!this.LocalStory.hasSelfText)
      {
        this.SelfTextDevider.Visibility = Visibility.Collapsed;
        this.SelfTextHolder.Visibility = Visibility.Collapsed;
        this.SelfTextOpen = false;
      }
      else
      {
        this.SelfTextDevider.Visibility = Visibility.Visible;
        this.SelfTextHolder.Visibility = Visibility.Visible;
        if (this.firstOpen_SelfText)
        {
          this.firstOpen_SelfText = false;
          this.SelfTextOpen = App.DataManager.SettingsMan.ShowExpandedSelfText;
        }
        this.setSelfTextUI(false, false);
        if (this.LocalStory.SelfTextLoaded)
        {
          this.SelfText.SetText(this.LocalStory.SelfText);
        }
        else
        {
          if (this.LocalStory.SelfTextLoading)
            return;
          this.LocalStory.SelfTextLoading = true;
          ThreadPool.QueueUserWorkItem((WaitCallback) (obj => this.LoadSelfText()));
        }
      }
    }

    public double ListVerticalOffset
    {
      get => (double) this.GetValue(this.ListVerticalOffsetProperty);
      set => this.SetValue(this.ListVerticalOffsetProperty, (object) value);
    }

    private void CommentScrollViewer_Loaded(object sender, RoutedEventArgs e)
    {
      this._listScrollViewer = sender as ScrollViewer;
      this.SetBinding(this.ListVerticalOffsetProperty, new Binding()
      {
        Source = (object) this._listScrollViewer,
        Path = new PropertyPath("VerticalOffset", new object[0]),
        Mode = BindingMode.OneWay
      });
    }

    private static void OnListVerticalOffsetChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs e)
    {
      StoryDetails storyDetails1 = obj as StoryDetails;
      ScrollViewer listScrollViewer = storyDetails1._listScrollViewer;
      if (listScrollViewer == null)
        return;
      if (listScrollViewer.VerticalOffset < 0.001)
      {
        if (!storyDetails1.barClosed)
          return;
        storyDetails1.ExpandTitleBarKeyFrame.Value = storyDetails1.TitleBarHeight;
        storyDetails1.ExpandTitleBar.Begin();
        storyDetails1.barClosed = false;
        storyDetails1.ApplicationBar.Mode = ApplicationBarMode.Default;
        storyDetails1.ApplicationBar.Opacity = 1.0;
        storyDetails1.AppBarIsMini = false;
      }
      else
      {
        if (storyDetails1.barClosed || !App.DataManager.SettingsMan.ShrinkStoryHeaderOnScroll)
          return;
        if (storyDetails1.TitleBarHeight == -1.0)
        {
          StoryDetails storyDetails2 = storyDetails1;
          storyDetails2.TitleBarHeight = storyDetails2.TitlePanel.DesiredSize.Height;
        }
        storyDetails1.CollpaseTitleBarKeyFrame.Value = storyDetails1.TitleBarHeight;
        storyDetails1.CollpaseTitleBar.Begin();
        storyDetails1.barClosed = true;
        storyDetails1.ApplicationBar.Mode = ApplicationBarMode.Minimized;
        storyDetails1.ApplicationBar.Opacity = 0.0;
        storyDetails1.AppBarIsMini = true;
      }
    }

    private void CloseLoadingDataDone(object sender, EventArgs e)
    {
      this.LoadingOverLay.Visibility = Visibility.Collapsed;
    }

    private void LoadSelfText()
    {
      if (this.LocalStory == null)
        return;
      this.LocalStory.SelfText = "";
      this.LocalStory.SelfText = App.DataManager.BaconitStore.GetSingleLongText(this.LocalStory.RedditID, this.LocalStory.SubRedditURL);
      this.LocalStory.SelfTextLoaded = true;
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() => this.SetStoryData(this.LocalStory, false)));
    }

    private void setSelfTextUI(bool showAnimation, bool wasFlick)
    {
      if (this.SelfTextOpen)
      {
        if (showAnimation)
        {
          this.ChangeSelfTextFrom.Value = this.SelfTextScroller.ActualHeight;
          if (wasFlick)
          {
            if (this.CurOrientation == PageOrientation.Portrait || this.CurOrientation == PageOrientation.PortraitUp)
              this.ChangeSelfTextTo.Value = 800.0 - this.TitlePanel.ActualHeight - 85.0;
            else
              this.ChangeSelfTextTo.Value = 465.0 - this.TitlePanel.ActualHeight;
          }
          else if (this.CurOrientation == PageOrientation.Portrait || this.CurOrientation == PageOrientation.PortraitUp)
            this.ChangeSelfTextTo.Value = 300.0;
          else
            this.ChangeSelfTextTo.Value = 130.0;
        }
        else
        {
          if (wasFlick)
          {
            if (this.CurOrientation == PageOrientation.Portrait || this.CurOrientation == PageOrientation.PortraitUp)
            {
              this.SelfTextScroller.MaxHeight = 800.0 - this.TitlePanel.ActualHeight - 85.0;
              return;
            }
            this.SelfTextScroller.MaxHeight = 465.0 - this.TitlePanel.ActualHeight;
            return;
          }
          if (this.CurOrientation == PageOrientation.Portrait || this.CurOrientation == PageOrientation.PortraitUp)
          {
            this.SelfTextScroller.MaxHeight = 300.0;
            return;
          }
          this.SelfTextScroller.MaxHeight = 130.0;
          return;
        }
      }
      else if (showAnimation)
      {
        this.ChangeSelfTextTo.Value = 54.0;
        this.ChangeSelfTextFrom.Value = this.SelfTextScroller.ActualHeight;
      }
      else
      {
        this.SelfTextScroller.MaxHeight = 54.0;
        return;
      }
      if (!showAnimation)
        return;
      this.ChangeSelfText.Begin();
    }

    private void UpVote_Click(object sender, EventArgs e)
    {
      if (this.NeedsToLoadData)
        return;
      if (App.DataManager.SettingsMan.IsSignedIn)
      {
        int vote = 0;
        switch (this.LocalStory.Like)
        {
          case -1:
            --this.LocalStory.downs;
            ++this.LocalStory.score;
            this.LocalStory.DownVoteColor = RedditViewerViewModel.ARROW_NO_SELECT;
            ((ApplicationBarIconButton) this.ApplicationBar.Buttons[2]).IconUri = new Uri("/Images/Arrows/brow_down_none.png", UriKind.Relative);
            this.LocalStory.UpVoteColor = DataManager.ACCENT_COLOR;
            ((ApplicationBarIconButton) this.ApplicationBar.Buttons[1]).IconUri = new Uri(App.BAR_UP_VOTE_SELECT, UriKind.Relative);
            this.LocalStory.Like = 1;
            ++this.LocalStory.ups;
            ++this.LocalStory.score;
            vote = 1;
            break;
          case 0:
            this.LocalStory.UpVoteColor = DataManager.ACCENT_COLOR;
            ((ApplicationBarIconButton) this.ApplicationBar.Buttons[1]).IconUri = new Uri(App.BAR_UP_VOTE_SELECT, UriKind.Relative);
            this.LocalStory.Like = 1;
            ++this.LocalStory.ups;
            ++this.LocalStory.score;
            vote = 1;
            break;
          case 1:
            this.LocalStory.Like = 0;
            --this.LocalStory.ups;
            --this.LocalStory.score;
            this.LocalStory.UpVoteColor = RedditViewerViewModel.ARROW_NO_SELECT;
            ((ApplicationBarIconButton) this.ApplicationBar.Buttons[1]).IconUri = new Uri("/Images/Arrows/brow_up_none.png", UriKind.Relative);
            vote = 0;
            break;
        }
        if (this.SubRedditHost != null)
          this.SubRedditHost.refreshUI(this.LocalStory);
        ThreadPool.QueueUserWorkItem((WaitCallback) (obj => App.DataManager.Vote(this.LocalStory.RedditID, vote)));
      }
      else
        App.DataManager.MessageManager.ShowSignInMessage("up vote");
    }

    private void DownVote_Click(object sender, EventArgs e)
    {
      if (this.NeedsToLoadData)
        return;
      if (App.DataManager.SettingsMan.IsSignedIn)
      {
        int vote = 0;
        switch (this.LocalStory.Like)
        {
          case -1:
            this.LocalStory.Like = 0;
            --this.LocalStory.downs;
            ++this.LocalStory.score;
            this.LocalStory.DownVoteColor = RedditViewerViewModel.ARROW_NO_SELECT;
            ((ApplicationBarIconButton) this.ApplicationBar.Buttons[2]).IconUri = new Uri("/Images/Arrows/brow_down_none.png", UriKind.Relative);
            vote = 0;
            break;
          case 0:
            this.LocalStory.DownVoteColor = DataManager.ACCENT_COLOR;
            ((ApplicationBarIconButton) this.ApplicationBar.Buttons[2]).IconUri = new Uri(App.BAR_DOWN_VOTE_SELECT, UriKind.Relative);
            this.LocalStory.Like = -1;
            ++this.LocalStory.downs;
            --this.LocalStory.score;
            vote = -1;
            break;
          case 1:
            --this.LocalStory.ups;
            --this.LocalStory.score;
            this.LocalStory.UpVoteColor = RedditViewerViewModel.ARROW_NO_SELECT;
            ((ApplicationBarIconButton) this.ApplicationBar.Buttons[1]).IconUri = new Uri("/Images/Arrows/brow_up_none.png", UriKind.Relative);
            this.LocalStory.DownVoteColor = DataManager.ACCENT_COLOR;
            ((ApplicationBarIconButton) this.ApplicationBar.Buttons[2]).IconUri = new Uri(App.BAR_DOWN_VOTE_SELECT, UriKind.Relative);
            this.LocalStory.Like = -1;
            ++this.LocalStory.downs;
            --this.LocalStory.score;
            vote = -1;
            break;
        }
        if (this.SubRedditHost != null)
          this.SubRedditHost.refreshUI(this.LocalStory);
        ThreadPool.QueueUserWorkItem((WaitCallback) (obj => App.DataManager.Vote(this.LocalStory.RedditID, vote)));
      }
      else
        App.DataManager.MessageManager.ShowSignInMessage("down vote");
    }

    private void SelfTextHolder_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      if (this.ingoreClick((Microsoft.Phone.Controls.GestureEventArgs) null) || !this.LocalStory.isSelf)
        return;
      this.SelfTextOpen = !this.SelfTextOpen;
      this.setSelfTextUI(true, false);
    }

    private void SelfTextHolder_ManipulationCompleted(
      object sender,
      ManipulationCompletedEventArgs e)
    {
      if (this.ingoreClick((Microsoft.Phone.Controls.GestureEventArgs) null))
        return;
      if (this.CurOrientation == PageOrientation.Portrait || this.CurOrientation == PageOrientation.PortraitUp)
      {
        if (!this.LocalStory.isSelf || e.TotalManipulation.Translation.Y <= 300.0 || this.SelfTextScroller.MaxHeight != 54.0 && this.SelfTextScroller.MaxHeight != 300.0)
          return;
        this.SelfTextOpen = true;
        this.setSelfTextUI(true, true);
      }
      else
      {
        if (!this.LocalStory.isSelf || e.TotalManipulation.Translation.X <= 150.0 || this.SelfTextScroller.MaxHeight != 54.0 && this.SelfTextScroller.MaxHeight != 154.0)
          return;
        this.SelfTextOpen = true;
        this.setSelfTextUI(true, true);
      }
    }

    private void refresh_Click(object sender, EventArgs e)
    {
      if (this.NeedsToLoadData || !App.DataManager.UpdateStoryComments((CommentDetailsViewModelInterface) this.ViewModel, this.LocalStory.RedditRealID, this.LocalStory.SubRedditURL, true, this.SortShowing, this.ShowComment))
        return;
      this.ViewModel.LoadingFromWeb = true;
      this.ViewModel.SetLoadingBar(true);
    }

    private void Pin_Click(object sender, EventArgs e)
    {
      if (this.NeedsToLoadData)
        return;
      this.ShowPinPicker();
    }

    private void Share_Click(object sender, EventArgs e)
    {
      if (this.NeedsToLoadData)
        return;
      this.shareComment = (CommentData) null;
      this.SharePickerOpen = true;
      this.sharePicker.Show();
    }

    private void sharePicker_closed(object sender, EventArgs e)
    {
      if (!this.SharePickerOpen)
        return;
      if (this.shareComment == null)
      {
        switch (this.sharePicker.SelectedIndex)
        {
          case 0:
            ShareLinkTask shareLinkTask1 = new ShareLinkTask();
            shareLinkTask1.Title = "Comments on \"" + this.LocalStory.Title + "\"";
            shareLinkTask1.LinkUri = new Uri("https://reddit.com" + this.LocalStory.Permalink, UriKind.Absolute);
            shareLinkTask1.Message = this.LocalStory.Title;
            try
            {
              shareLinkTask1.Show();
              break;
            }
            catch
            {
              break;
            }
          case 1:
            App.DataManager.ShareHelper.ShareNFC("baconit:StoryDetails?StoryDataRedditID=" + this.LocalStory.RedditID);
            break;
          case 2:
            SmsComposeTask smsComposeTask1 = new SmsComposeTask();
            smsComposeTask1.Body = "\"" + this.LocalStory.Title + "\" https://reddit.com" + this.LocalStory.Permalink;
            try
            {
              smsComposeTask1.Show();
              break;
            }
            catch
            {
              break;
            }
          case 3:
            EmailComposeTask emailComposeTask1 = new EmailComposeTask();
            emailComposeTask1.Body = "Reddit comments on \"" + this.LocalStory.Title + "\"\n\rhttps://reddit.com" + this.LocalStory.Permalink;
            emailComposeTask1.Subject = "Reddit Comments on \"" + this.LocalStory.Title + "\"";
            try
            {
              emailComposeTask1.Show();
              break;
            }
            catch
            {
              break;
            }
          case 4:
            Clipboard.SetText("https://reddit.com" + this.LocalStory.Permalink);
            App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("link copied", true, false, string.Empty, string.Empty));
            break;
        }
      }
      else
      {
        switch (this.sharePicker.SelectedIndex)
        {
          case 0:
            ShareLinkTask shareLinkTask2 = new ShareLinkTask();
            shareLinkTask2.Title = "Reddit Comment on " + this.LocalStory.Title;
            shareLinkTask2.LinkUri = new Uri("https://reddit.com" + this.LocalStory.Permalink + this.CommentActionOpenedFor.RName.Substring(3), UriKind.Absolute);
            shareLinkTask2.Message = "\"" + this.shareComment.Body + this.shareComment.Body2 + "\"";
            try
            {
              shareLinkTask2.Show();
              break;
            }
            catch
            {
              break;
            }
          case 1:
            App.DataManager.ShareHelper.ShareNFC("baconit:StoryDetails?StoryDataRedditID=" + this.LocalStory.RedditID);
            break;
          case 2:
            SmsComposeTask smsComposeTask2 = new SmsComposeTask();
            smsComposeTask2.Body = "https://reddit.com" + this.LocalStory.Permalink + this.CommentActionOpenedFor.RName.Substring(3);
            try
            {
              smsComposeTask2.Show();
              break;
            }
            catch
            {
              break;
            }
          case 3:
            EmailComposeTask emailComposeTask2 = new EmailComposeTask();
            emailComposeTask2.Body = "\"" + this.CommentActionOpenedFor.Body + this.CommentActionOpenedFor.Body2 + "\"\n\rhttps://reddit.com" + this.LocalStory.Permalink + this.CommentActionOpenedFor.RName.Substring(3);
            emailComposeTask2.Subject = "Reddit Comment on " + this.LocalStory.Title;
            try
            {
              emailComposeTask2.Show();
              break;
            }
            catch
            {
              break;
            }
          case 4:
            Clipboard.SetText("https://reddit.com" + this.LocalStory.Permalink + this.CommentActionOpenedFor.RName.Substring(3));
            App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("link copied", true, false, string.Empty, string.Empty));
            break;
        }
      }
      this.SharePickerOpen = false;
      this.IngoreNextClick = true;
    }

    public void ChangeCommentView(int SelectedIndex)
    {
      if (SelectedIndex < 0)
        return;
      CollapseBox.TapTime = BaconitStore.currentTime();
      int StartingIndexToClose = SelectedIndex + 1;
      int indentLevel = this.ViewModel.Comments[SelectedIndex].IndentLevel;
      int LastIndexToClose = StartingIndexToClose;
      while (LastIndexToClose < this.ViewModel.Comments.Count && this.ViewModel.Comments[LastIndexToClose].IndentLevel > indentLevel)
        LastIndexToClose++;
      bool ShouldCollpase = this.ViewModel.Comments[SelectedIndex].IsCommentVisible;
      Visibility ShouldBeVisisble = ShouldCollpase ? Visibility.Collapsed : Visibility.Visible;
      int numToClose = LastIndexToClose - StartingIndexToClose;
      ThreadPool.QueueUserWorkItem((WaitCallback) (obje =>
      {
        if (!ShouldCollpase)
          Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
          {
            this.ViewModel.Comments[SelectedIndex].IsCommentVisible = true;
            this.ViewModel.Comments[SelectedIndex].MoreCommentsText = "";
            this.ViewModel.Comments[SelectedIndex].TitleHeight = 31;
          }));
        if (numToClose < 10)
        {
          for (int curIndex = LastIndexToClose - 1; curIndex >= StartingIndexToClose; curIndex--)
          {
            if (curIndex - StartingIndexToClose < 5)
            {
              AutoResetEvent are = new AutoResetEvent(false);
              this.Dispatcher.BeginInvoke((Action) (() =>
              {
                this.ViewModel.Comments[curIndex].isCollapsed = ShouldCollpase;
                are.Set();
              }));
              are.WaitOne();
            }
            else
            {
              AutoResetEvent are = new AutoResetEvent(false);
              this.Dispatcher.BeginInvoke((Action) (() =>
              {
                this.ViewModel.Comments[curIndex].isVisible = ShouldBeVisisble;
                are.Set();
              }));
              are.WaitOne();
            }
          }
        }
        else if (ShouldCollpase)
        {
          ThreadPool.QueueUserWorkItem((WaitCallback) (objec => this.Dispatcher.BeginInvoke((Action) (() =>
          {
            for (int index = StartingIndexToClose + 11; index < LastIndexToClose; ++index)
              this.ViewModel.Comments[index].isVisible = Visibility.Collapsed;
          }))));
          for (int curIndexSecond = StartingIndexToClose + 10; curIndexSecond >= StartingIndexToClose; curIndexSecond--)
          {
            AutoResetEvent are = new AutoResetEvent(false);
            this.Dispatcher.BeginInvoke((Action) (() =>
            {
              this.ViewModel.Comments[curIndexSecond].isCollapsed = true;
              are.Set();
            }));
            are.WaitOne();
          }
        }
        else
        {
          for (int curIndex = StartingIndexToClose; curIndex < LastIndexToClose; curIndex++)
          {
            AutoResetEvent are = new AutoResetEvent(false);
            this.Dispatcher.BeginInvoke((Action) (() =>
            {
              if (curIndex > StartingIndexToClose + 10)
                this.ViewModel.Comments[curIndex].isVisible = Visibility.Visible;
              else
                this.ViewModel.Comments[curIndex].isCollapsed = false;
              are.Set();
            }));
            are.WaitOne();
          }
        }
        if (!ShouldCollpase)
          return;
        Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
        {
          this.ViewModel.Comments[SelectedIndex].MoreCommentsText = "+" + (object) (LastIndexToClose - StartingIndexToClose);
          this.ViewModel.Comments[SelectedIndex].IsCommentVisible = false;
          this.ViewModel.Comments[SelectedIndex].TitleHeight = 50;
        }));
      }));
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
      if (this.CommentActionPanelOpen)
      {
        if (this.CurOrientation == PageOrientation.Portrait || this.CurOrientation == PageOrientation.PortraitUp)
        {
          this.CommentActionPanelVertHolder.Visibility = Visibility.Visible;
          this.CommentActionPanelHorzHolder.Visibility = Visibility.Collapsed;
          this.CommentActionPanel.Height = 850.0;
        }
        else
        {
          this.CommentActionPanelVertHolder.Visibility = Visibility.Collapsed;
          this.CommentActionPanelHorzHolder.Visibility = Visibility.Visible;
          this.CommentActionPanel.Height = 530.0;
        }
        this.CommentActionPanel.InvalidateArrange();
        this.CommentActionPanel.InvalidateMeasure();
      }
      if (this.CurOrientation == PageOrientation.Portrait || this.CurOrientation == PageOrientation.PortraitUp)
      {
        this.CommentingScroller.MaxHeight = 182.0;
        if (this.SelfTextScroller.MaxHeight == 130.0)
        {
          this.SelfTextScroller.MaxHeight = 300.0;
        }
        else
        {
          if (this.SelfTextScroller.MaxHeight == 54.0)
            return;
          this.SelfTextScroller.MaxHeight = 800.0 - this.TitlePanel.ActualHeight - 85.0;
        }
      }
      else
      {
        this.CommentingScroller.MaxHeight = 90.0;
        if (this.SelfTextScroller.MaxHeight == 300.0)
        {
          this.SelfTextScroller.MaxHeight = 130.0;
        }
        else
        {
          if (this.SelfTextScroller.MaxHeight == 54.0)
            return;
          this.SelfTextScroller.MaxHeight = 465.0 - this.TitlePanel.ActualHeight;
        }
      }
    }

    private void StoryOverlay_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.FadeOutTips.Begin();
      App.DataManager.SettingsMan.ShowStoryOverLay = false;
      if (App.DataManager.SettingsMan.ShowStorySelfOverLay && this.LocalStory.isSelf && this.LocalStory.hasSelfText)
      {
        this.HelpOverlayOpen = true;
        this.ApplicationBar.IsVisible = false;
        this.SelfTextOverlay.Source = (ImageSource) new BitmapImage(new Uri("/Images/StorySelfOverlay-" + (object) App.DataManager.SettingsMan.ScaleFactor + ".png", UriKind.Relative));
        this.SelfTextOverlay.Visibility = Visibility.Visible;
        this.FadeInSelfTips.Begin();
      }
      else if (App.DataManager.SettingsMan.ScreenOrenLocked)
      {
        if (App.DataManager.SettingsMan.ScreenOrenLockedtoPortrait)
          this.SupportedOrientations = SupportedPageOrientation.Portrait;
        else
          this.SupportedOrientations = SupportedPageOrientation.Landscape;
        ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[4]).Text = "unlock screen orientation";
      }
      else
      {
        this.SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;
        ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[4]).Text = "lock screen orientation";
      }
    }

    private void SelfTextOverlay_Opened(object sender, EventArgs e) => this.HelpOverlayOpen = true;

    private void FadeOutTipsComplete(object sender, EventArgs e)
    {
      this.StoryOverlay.Visibility = Visibility.Collapsed;
      this.StoryOverlay.Opacity = 1.0;
      this.HelpOverlayOpen = false;
      if (this.SelfTextOverlay.Visibility == Visibility.Visible)
        return;
      this.ApplicationBar.IsVisible = true;
      if (App.DataManager.SettingsMan.ScreenOrenLocked)
      {
        if (App.DataManager.SettingsMan.ScreenOrenLockedtoPortrait)
          this.SupportedOrientations = SupportedPageOrientation.Portrait;
        else
          this.SupportedOrientations = SupportedPageOrientation.Landscape;
        ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[4]).Text = "unlock screen orientation";
      }
      else
      {
        this.SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;
        ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[4]).Text = "lock screen orientation";
      }
    }

    private void SelfTextOverlay_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.FadeOutSelfTips.Begin();
      App.DataManager.SettingsMan.ShowStorySelfOverLay = false;
    }

    private void FadeOutSelfTipsComplete(object sender, EventArgs e)
    {
      this.SelfTextOverlay.Visibility = Visibility.Collapsed;
      this.SelfTextOverlay.Opacity = 1.0;
      this.HelpOverlayOpen = false;
      this.ApplicationBar.IsVisible = true;
      if (App.DataManager.SettingsMan.ScreenOrenLocked)
      {
        if (App.DataManager.SettingsMan.ScreenOrenLockedtoPortrait)
          this.SupportedOrientations = SupportedPageOrientation.Portrait;
        else
          this.SupportedOrientations = SupportedPageOrientation.Landscape;
        ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[4]).Text = "unlock screen orientation";
      }
      else
      {
        this.SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;
        ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[4]).Text = "lock screen orientation";
      }
    }

    private void ShowTips_Click(object sender, EventArgs e)
    {
      if (this.NeedsToLoadData)
        return;
      this.ApplicationBar.IsVisible = false;
      this.HelpOverlayOpen = true;
      this.StoryOverlay.Source = (ImageSource) new BitmapImage(new Uri("/Images/StoryOverlay-" + (object) App.DataManager.SettingsMan.ScaleFactor + ".png", UriKind.Relative));
      this.StoryOverlay.Visibility = Visibility.Visible;
      this.FadeInTips.Begin();
      App.DataManager.SettingsMan.ShowStorySelfOverLay = true;
      this.SupportedOrientations = SupportedPageOrientation.Portrait;
    }

    private void CommentingClose_Click(object sender, RoutedEventArgs e)
    {
      this.CommentingText.Text = "";
      this.CloseCommenting.Begin();
    }

    private void CommentingSubmit_Click(object sender, RoutedEventArgs e)
    {
      if (App.DataManager.SettingsMan.IsSignedIn)
      {
        string lastEditText = "";
        bool isComment = false;
        this.Focus();
        this.CloseCommenting.Begin();
        if (this.CommentingOnRName.StartsWith("t3_") && this.CommentingOnIsEdit)
        {
          this.SelfTextProgressBar.Visibility = Visibility.Visible;
          this.SelfTextProgressBar.IsIndeterminate = true;
          lastEditText = this.LocalStory.SelfText;
          this.LocalStory.SelfText = this.CommentingText.Text;
          this.SetStoryData(this.LocalStory, false);
          isComment = false;
        }
        else
        {
          isComment = true;
          if (this.CommentingOnRName.StartsWith("t3_"))
          {
            CommentData commentData1 = new CommentData();
            commentData1.IndentLevel = 0;
            commentData1.Content = this.CommentingText.Text;
            commentData1.Body = this.CommentingText.Text;
            commentData1.Author = App.DataManager.SettingsMan.UserName;
            CommentData commentData2 = commentData1;
            commentData2.LineOne = commentData2.Author;
            commentData1.Up = 1;
            commentData1.Downs = 0;
            commentData1.SendingProgressBar = Visibility.Visible;
            commentData1.SendingProgressBarIsInd = true;
            commentData1.LineOnePartTwo = " " + (object) (commentData1.Up - commentData1.Downs) + " points ";
            commentData1.LineOnePartTwo += "just now";
            commentData1.RName = "unkown";
            CommentData commentData3 = commentData1;
            commentData3.Padding = new Thickness((double) (commentData3.IndentLevel * 10), 0.0, 0.0, 0.0);
            commentData1.PanelWidth = 455 - 15 * commentData1.IndentLevel;
            commentData1.DownVoteStatus = true;
            commentData1.UpVoteStatus = true;
            commentData1.UpVoteIcon = Visibility.Visible;
            commentData1.DownVoteStatus = false;
            this.ProgressBarList.Add(commentData1);
            Color color = new Color();
            if (commentData1.IndentLevel == 0)
            {
              color.A = byte.MaxValue;
              color.B = (byte) Math.Min((int) DataManager.ACCENT_COLOR_COLOR.B + 35, (int) byte.MaxValue);
              color.G = (byte) Math.Min((int) DataManager.ACCENT_COLOR_COLOR.G + 35, (int) byte.MaxValue);
              color.R = (byte) Math.Min((int) DataManager.ACCENT_COLOR_COLOR.R + 35, (int) byte.MaxValue);
            }
            else
            {
              color.A = byte.MaxValue;
              color.B = (byte) Math.Max((int) DataManager.ACCENT_COLOR_COLOR.B - (commentData1.IndentLevel + 1) * 25, 0);
              color.G = (byte) Math.Max((int) DataManager.ACCENT_COLOR_COLOR.G - (commentData1.IndentLevel + 1) * 25, 0);
              color.R = (byte) Math.Max((int) DataManager.ACCENT_COLOR_COLOR.R - (commentData1.IndentLevel + 1) * 25, 0);
            }
            commentData1.Color = new SolidColorBrush(color);
            commentData1.Openable = false;
            this.ViewModel.Comments.Insert(0, commentData1);
            this.Dispatcher.BeginInvoke((Action) (() =>
            {
              this.CommentList.UpdateLayout();
              this.CommentList.SetVerticalScrollOffset(0.0);
            }));
            if (this.LocalStory != null)
              App.DataManager.SettingsMan.ReadStories[this.LocalStory.RedditID]++;
          }
          else
          {
            int num = 0;
            foreach (CommentData comment in (Collection<CommentData>) this.ViewModel.Comments)
            {
              if (comment.RName.Equals(this.CommentingOnRName))
              {
                if (this.CommentingOnIsEdit)
                {
                  lastEditText = comment.Body + comment.Body2;
                  comment.Content = this.CommentingText.Text;
                  comment.Body = this.CommentingText.Text;
                  comment.SendingProgressBar = Visibility.Visible;
                  comment.SendingProgressBarIsInd = true;
                  this.ProgressBarList.Add(comment);
                  break;
                }
                CommentData commentData4 = new CommentData();
                commentData4.IndentLevel = comment.IndentLevel + 1;
                commentData4.Content = this.CommentingText.Text;
                commentData4.Body = this.CommentingText.Text;
                commentData4.Author = App.DataManager.SettingsMan.UserName;
                CommentData commentData5 = commentData4;
                commentData5.LineOne = commentData5.Author;
                commentData4.Up = 1;
                commentData4.Downs = 0;
                commentData4.SendingProgressBar = Visibility.Visible;
                commentData4.SendingProgressBarIsInd = true;
                commentData4.LineOnePartTwo = " " + (object) (commentData4.Up - commentData4.Downs) + " points ";
                commentData4.LineOnePartTwo += "just now";
                commentData4.RName = "unkown";
                commentData4.SubRedditURL = comment.SubRedditURL;
                commentData4.SubRedditRName = comment.SubRedditRName;
                CommentData commentData6 = commentData4;
                commentData6.Padding = new Thickness((double) (commentData6.IndentLevel * 10), 0.0, 0.0, 0.0);
                commentData4.PanelWidth = 455 - 15 * commentData4.IndentLevel;
                commentData4.DownVoteStatus = true;
                commentData4.UpVoteStatus = true;
                commentData4.UpVoteIcon = Visibility.Visible;
                commentData4.DownVoteStatus = false;
                this.ProgressBarList.Add(commentData4);
                Color resource = (Color) Application.Current.Resources[(object) "PhoneAccentColor"];
                commentData4.Color = new SolidColorBrush(new Color()
                {
                  A = byte.MaxValue,
                  B = (byte) Math.Max((int) resource.B - (commentData4.IndentLevel + 1) * 20, 0),
                  G = (byte) Math.Max((int) resource.G - (commentData4.IndentLevel + 1) * 20, 0),
                  R = (byte) Math.Max((int) resource.R - (commentData4.IndentLevel + 1) * 20, 0)
                });
                commentData4.Openable = false;
                this.ViewModel.Comments.Insert(num + 1, commentData4);
                this.CommentList.ScrollIntoView((object) this.ViewModel.Comments.Last<CommentData>());
                this.CommentList.UpdateLayout();
                this.CommentList.ScrollIntoView((object) commentData4);
                break;
              }
              ++num;
            }
          }
          this.NoComments.Visibility = Visibility.Collapsed;
          this.CommentList.Visibility = Visibility.Visible;
        }
        if (this.CommentingOnIsEdit)
        {
          this.CommentingOnIsEdit = false;
          string text = this.CommentingText.Text;
          ThreadPool.QueueUserWorkItem((WaitCallback) (obj => App.DataManager.EditComment(new RunWorkerCompletedEventHandler(this.EditSubmitCallback), new SubmitCommentData()
          {
            CommentingOnRName = this.CommentingOnRName,
            text = text,
            lastEditText = lastEditText,
            isComment = isComment
          })));
        }
        else
        {
          string text = this.CommentingText.Text;
          ThreadPool.QueueUserWorkItem((WaitCallback) (obj => App.DataManager.CommentOn(new RunWorkerCompletedEventHandler(this.CommentSubmitCallback), new SubmitCommentData()
          {
            CommentingOnRName = this.CommentingOnRName,
            text = text
          })));
        }
      }
      else
        App.DataManager.MessageManager.ShowSignInMessage("comment");
      this.CommentingText.Text = "";
    }

    private void FullScreenCommenting_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.CloseCommentActionsAni.Begin();
      this.OpenFullScreenCommenting();
    }

    public void CommentSubmitCallback(object obj, RunWorkerCompletedEventArgs e)
    {
      SubmitCommentData data = (SubmitCommentData) obj;
      string newID = data.NewID;
      if (newID == null)
        newID = "";
      if (!newID.Equals(""))
        App.DataManager.BaconitAnalytics.LogEvent("Comment Submitted");
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        string commentingOnRname = data.CommentingOnRName;
        int num = 0;
        foreach (CommentData comment in (Collection<CommentData>) this.ViewModel.Comments)
        {
          if (comment.SendingProgressBarIsInd && (num == 0 || this.ViewModel.Comments[num - 1].RName.Equals(commentingOnRname)))
          {
            if (!newID.Equals(""))
            {
              comment.SendingProgressBarIsInd = false;
              comment.SendingProgressBar = Visibility.Collapsed;
              comment.RName = newID;
              comment.Openable = true;
              App.DataManager.BaconitStore.UpdateLastUpdatedTime(this.LocalStory.RedditID + (object) this.SortShowing + "comments", true);
              break;
            }
            comment.isVisible = Visibility.Collapsed;
            break;
          }
          ++num;
        }
        if (data.WasSuccessful || this.CommentingPanelOpen)
          return;
        this.CommentingOnRName = data.CommentingOnRName;
        this.CommentingText.Text = data.text;
        this.CommentingOnIsEdit = true;
        this.ApplicationBar.IsVisible = false;
        this.CommentingPanelOpen = true;
        this.CommentingDialog.Visibility = Visibility.Visible;
        this.CommentingScroller.ScrollToVerticalOffset(0.0);
        this.OpenCommenting.Begin();
      }));
    }

    public void EditSubmitCallback(object obj, RunWorkerCompletedEventArgs e)
    {
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        SubmitCommentData submitCommentData = (SubmitCommentData) obj;
        if (submitCommentData.CommentingOnRName.StartsWith("t1_"))
        {
          int index = 0;
          foreach (CommentData comment in (Collection<CommentData>) this.ViewModel.Comments)
          {
            if (comment.SendingProgressBarIsInd && (index == 0 || this.ViewModel.Comments[index].RName.Equals(submitCommentData.CommentingOnRName)))
            {
              comment.SendingProgressBarIsInd = false;
              comment.SendingProgressBar = Visibility.Collapsed;
              comment.Openable = true;
              App.DataManager.BaconitStore.UpdateLastUpdatedTime(this.LocalStory.RedditID + (object) this.SortShowing + "comments", true);
              if (submitCommentData.WasSuccessful)
              {
                App.DataManager.BaconitStore.UpdateComment(comment);
                break;
              }
              comment.Content = submitCommentData.lastEditText;
              if (this.CommentingPanelOpen)
                break;
              this.CommentingOnRName = submitCommentData.CommentingOnRName;
              this.CommentingText.Text = submitCommentData.text;
              this.CommentingOnIsEdit = true;
              this.ApplicationBar.IsVisible = false;
              this.CommentingPanelOpen = true;
              this.CommentingDialog.Visibility = Visibility.Visible;
              this.CommentingScroller.ScrollToVerticalOffset(0.0);
              this.OpenCommenting.Begin();
              break;
            }
            ++index;
          }
        }
        else
        {
          this.SelfTextProgressBar.Visibility = Visibility.Collapsed;
          this.SelfTextProgressBar.IsIndeterminate = false;
          if (submitCommentData.WasSuccessful)
          {
            this.refresh_Click((object) null, (EventArgs) null);
            if (this.SubRedditHost == null)
              return;
            this.SubRedditHost.refreshUI(this.LocalStory);
          }
          else
          {
            this.LocalStory.SelfText = submitCommentData.lastEditText;
            this.SetStoryData(this.LocalStory, false);
            if (this.CommentingPanelOpen)
              return;
            this.CommentingOnRName = submitCommentData.CommentingOnRName;
            this.CommentingText.Text = submitCommentData.text;
            this.CommentingOnIsEdit = true;
            this.ApplicationBar.IsVisible = false;
            this.CommentingPanelOpen = true;
            this.CommentingDialog.Visibility = Visibility.Visible;
            this.CommentingScroller.ScrollToVerticalOffset(0.0);
            this.OpenCommenting.Begin();
          }
        }
      }));
    }

    public void DeleteSubmitCallback(object obj, RunWorkerCompletedEventArgs e)
    {
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        string str = (string) obj;
        if (str.StartsWith("t1_"))
        {
          int index = 0;
          foreach (CommentData comment in (Collection<CommentData>) this.ViewModel.Comments)
          {
            if (comment.SendingProgressBarIsInd && (index == 0 || this.ViewModel.Comments[index].RName.Equals(str)))
            {
              comment.SendingProgressBarIsInd = false;
              comment.SendingProgressBar = Visibility.Collapsed;
              comment.Openable = false;
              App.DataManager.BaconitStore.UpdateLastUpdatedTime(this.LocalStory.RedditID + "comments", true);
              if (!(bool) e.Result)
                break;
              comment.Author = "[deleted]";
              comment.Content = "[deleted]";
              App.DataManager.BaconitStore.UpdateComment(comment);
              break;
            }
            ++index;
          }
        }
        else
        {
          this.LocalStory.Title = "[deleted]";
          if (this.SubRedditHost != null)
            this.SubRedditHost.refreshUI(this.LocalStory);
          App.DataManager.BaconitStore.UpdateStory(this.LocalStory);
          this.NavigationService.GoBack();
        }
      }));
    }

    public void OpenFullScreenCommenting()
    {
      if (this.CommentActionOpenedFor != null && this.CommentActionOpenedFor.RName.Equals(this.CommentingOnRName))
      {
        this.TitleHolder.Visibility = Visibility.Visible;
        this.TitleHolder.ScrollToVerticalOffset(0.0);
        this.FullScreenCommentingLineOne.Text = this.CommentActionOpenedFor.LineOne;
        this.FullScreenCommentingLineTwo.Text = this.CommentActionOpenedFor.LineOnePartTwo;
        this.FullScreenCommentingComment.SetText(this.CommentActionOpenedFor.Body + this.CommentActionOpenedFor.Body2 + this.CommentActionOpenedFor.Body3);
      }
      else
        this.TitleHolder.Visibility = Visibility.Collapsed;
      this.FullScreenCommentingPanelOpen = true;
      this.FullScreenCommenting.Visibility = Visibility.Visible;
      this.FullScreenBlackBackground.Visibility = Visibility.Visible;
      this.ApplicationBar.IsVisible = false;
      this.FullScreenCommentingFormattingPreview.Blocks.Clear();
      this.OpenFullScreenCommentingAni.Begin();
      this.DontOpenAppBar = true;
      this.CloseCommenting.Begin();
      this.FullScreenCommentingBox.Text = this.CommentingText.Text;
      App.DataManager.FormatText(this.FullScreenCommentingFormattingPreview.Blocks, HttpUtility.HtmlDecode(this.CommentingText.Text));
      this.FullScreenCommentingBox.SelectionStart = this.CommentingText.Text.Length;
      this.Dispatcher.BeginInvoke((Action) (() => this.FullScreenCommentingBox.Focus()));
      App.DataManager.BaconitAnalytics.LogEvent("Full Screen Commenting Open");
    }

    private void CloseFullScreenColsed_Completed(object sender, EventArgs e)
    {
      this.FullScreenCommentingPanelOpen = false;
      this.FullScreenCommenting.Visibility = Visibility.Collapsed;
      this.FullScreenBlackBackground.Visibility = Visibility.Collapsed;
      this.FullScreenCommentingComment.ClearText();
      this.ApplicationBar.IsVisible = true;
    }

    private void FullScreenSubmit_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.CloseFullScreenCommentingAni.Begin();
      this.CommentingText.Text = this.FullScreenCommentingBox.Text;
      this.CommentingSubmit_Click((object) null, (RoutedEventArgs) null);
    }

    private void FullScreenCancel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.CloseFullScreenCommentingAni.Begin();
    }

    private void FullScreenCommentingBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.FullScreenCommentingtext.Length == 0 || this.FullScreenCommentingBox.Text.Length == 0)
      {
        this.FullScreenCommentingtext = this.FullScreenCommentingBox.Text;
      }
      else
      {
        if ((int) this.FullScreenCommentingtext[this.FullScreenCommentingtext.Length - 1] == (int) this.FullScreenCommentingBox.Text[this.FullScreenCommentingBox.Text.Length - 1])
          return;
        this.FullScreenCommentingScroller.ScrollToVerticalOffset(5000000.0);
        this.FullScreenCommentingtext = this.FullScreenCommentingBox.Text;
        this.FullScreenCommentingFormattingPreview.Blocks.Clear();
        App.DataManager.FormatText(this.FullScreenCommentingFormattingPreview.Blocks, HttpUtility.HtmlDecode(this.FullScreenCommentingtext));
      }
    }

    private void CommentingOnClosed_Completed(object sender, EventArgs e)
    {
      this.CommentingPanelOpen = false;
      this.CommentingDialog.Visibility = Visibility.Collapsed;
      if (this.DontOpenAppBar)
        this.DontOpenAppBar = false;
      else
        this.ApplicationBar.IsVisible = true;
    }

    private void AppBarMenuViewStory_Click(object sender, EventArgs e)
    {
      if (this.NeedsToLoadData)
        return;
      this.TitleOpenLink_Tap((object) null, (System.Windows.Input.GestureEventArgs) null);
    }

    private void CommentList_GotFocus(object sender, RoutedEventArgs e) => this.Focus();

    private void Comment_DragCompleted(object sender, DragCompletedGestureEventArgs e)
    {
      if (Math.Abs(e.HorizontalChange) <= 100.0 || Math.Abs(e.VerticalChange) >= 100.0)
        return;
      int SelectedIndex = this.ViewModel.Comments.IndexOf((sender as CollapseBox).DataContext as CommentData);
      if (SelectedIndex == -1)
        return;
      this.ChangeCommentView(SelectedIndex);
    }

    private void Comment_Tap(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
    {
      if (this.ingoreClick(e) || !((sender as CollapseBox).DataContext as CommentData).Openable)
        return;
      if (!((sender as CollapseBox).DataContext as CommentData).IsCommentVisible)
      {
        if (this.CommentList.SelectedIndex <= -1)
          return;
        this.ChangeCommentView(this.CommentList.SelectedIndex);
        this.CommentList.SelectedIndex = -1;
      }
      else
      {
        if (this.CommentTapThreadObj != null || this.CommentList.SelectedIndex == -1)
          return;
        this.CommentTapThreadObj = new StoryDetails.CommentTapThread(this.CommentList.SelectedIndex, (sender as CollapseBox).DataContext as CommentData, this);
        new Thread(new ThreadStart(this.CommentTapThreadObj.run)).Start();
      }
    }

    private void Comment_DoubleTap(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
    {
      if (this.ingoreClick(e) || this.CommentList.SelectedIndex < 0 || this.ViewModel.Comments.Count < this.CommentList.SelectedIndex || !this.ViewModel.Comments[this.CommentList.SelectedIndex].Openable || this.CommentTapThreadObj == null)
        return;
      this.CommentTapThreadObj.Cancel();
      if (this.CommentList.SelectedIndex <= -1)
        return;
      this.ChangeCommentView(this.CommentList.SelectedIndex);
      this.CommentList.SelectedIndex = -1;
    }

    public void OpenCommentActions()
    {
      SystemTray.IsVisible = false;
      SolidColorBrush solidColorBrush1 = new SolidColorBrush(Color.FromArgb((byte) 150, (byte) 0, (byte) 0, (byte) 0));
      SolidColorBrush solidColorBrush2 = new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 100, (byte) 100, (byte) 100));
      SolidColorBrush solidColorBrush3 = new SolidColorBrush((Color) Application.Current.Resources[(object) "PhoneAccentColor"]);
      SolidColorBrush solidColorBrush4 = new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
      if (this.CurOrientation == PageOrientation.Portrait || this.CurOrientation == PageOrientation.PortraitUp)
      {
        this.CommentActionPanelVertHolder.Visibility = Visibility.Visible;
        this.CommentActionPanelHorzHolder.Visibility = Visibility.Collapsed;
        this.CommentActionPanel.Height = 850.0;
      }
      else
      {
        this.CommentActionPanelVertHolder.Visibility = Visibility.Collapsed;
        this.CommentActionPanelHorzHolder.Visibility = Visibility.Visible;
        this.CommentActionPanel.Height = 530.0;
      }
      if (this.CommentActionOpenedFor.UpVoteStatus)
      {
        this.CommentUpVote.Background = (Brush) solidColorBrush3;
        this.CommentUpVoteText.Foreground = (Brush) solidColorBrush4;
        this.CommentUpVoteHorz.Background = (Brush) solidColorBrush3;
        this.CommentUpVoteTextHorz.Foreground = (Brush) solidColorBrush4;
      }
      else
      {
        this.CommentUpVote.Background = (Brush) solidColorBrush2;
        this.CommentUpVoteText.Foreground = (Brush) solidColorBrush1;
        this.CommentUpVoteHorz.Background = (Brush) solidColorBrush2;
        this.CommentUpVoteTextHorz.Foreground = (Brush) solidColorBrush1;
      }
      if (this.CommentActionOpenedFor.DownVoteStatus)
      {
        this.CommentDownVote.Background = (Brush) solidColorBrush3;
        this.CommentDownVoteText.Foreground = (Brush) solidColorBrush4;
        this.CommentDownVoteHorz.Background = (Brush) solidColorBrush3;
        this.CommentDownVoteTextHorz.Foreground = (Brush) solidColorBrush4;
      }
      else
      {
        this.CommentDownVote.Background = (Brush) solidColorBrush2;
        this.CommentDownVoteText.Foreground = (Brush) solidColorBrush1;
        this.CommentDownVoteHorz.Background = (Brush) solidColorBrush2;
        this.CommentDownVoteTextHorz.Foreground = (Brush) solidColorBrush1;
      }
      this.CommentActionLineOneOne.Text = this.CommentActionOpenedFor.LineOne;
      this.CommentActionLineOneTwo.Text = this.CommentActionOpenedFor.LineOnePartTwo;
      this.CommentActionPanelContentScroller.ScrollToVerticalOffset(0.0);
      this.CommentActionCommentContent.SetText(this.CommentActionOpenedFor.Body + this.CommentActionOpenedFor.Body2 + this.CommentActionOpenedFor.Body3);
      if (this.CommentActionOpenedFor.Author.Equals(App.DataManager.SettingsMan.UserName))
      {
        this.CommentOn.Visibility = Visibility.Collapsed;
        this.EditButtons.Visibility = Visibility.Visible;
        this.CommentOnHorz.Visibility = Visibility.Collapsed;
        this.EditOnHorz.Visibility = Visibility.Visible;
      }
      else
      {
        this.CommentOn.Visibility = Visibility.Visible;
        this.EditButtons.Visibility = Visibility.Collapsed;
        this.CommentOnHorz.Visibility = Visibility.Visible;
        this.EditOnHorz.Visibility = Visibility.Collapsed;
      }
      ((ApplicationBarIconButton) this.ApplicationBar.Buttons[0]).IsEnabled = false;
      ((ApplicationBarIconButton) this.ApplicationBar.Buttons[1]).IsEnabled = false;
      ((ApplicationBarIconButton) this.ApplicationBar.Buttons[2]).IsEnabled = false;
      ((ApplicationBarIconButton) this.ApplicationBar.Buttons[3]).IsEnabled = false;
      this.CommentActionPanel.Visibility = Visibility.Visible;
      this.BlackBackground.Visibility = Visibility.Visible;
      this.ApplicationBar.IsVisible = false;
      this.OpenCommentActionsAni.Begin();
      this.CommentActionPanelOpen = true;
    }

    public void CloseCommentActions(bool showAppBar)
    {
      this.ApplicationBar.IsVisible = showAppBar;
      ((ApplicationBarIconButton) this.ApplicationBar.Buttons[0]).IsEnabled = true;
      ((ApplicationBarIconButton) this.ApplicationBar.Buttons[1]).IsEnabled = true;
      ((ApplicationBarIconButton) this.ApplicationBar.Buttons[2]).IsEnabled = true;
      ((ApplicationBarIconButton) this.ApplicationBar.Buttons[3]).IsEnabled = true;
      this.CloseCommentActionsAni.Begin();
    }

    private void CommentActionClosed(object sender, EventArgs e)
    {
      this.CommentActionPanel.Visibility = Visibility.Collapsed;
      this.BlackBackground.Visibility = Visibility.Collapsed;
      this.CommentActionCommentContent.ClearText();
      SystemTray.IsVisible = App.DataManager.SettingsMan.ShowSystemBar;
      this.CommentActionPanelOpen = false;
    }

    private void CopyCommentText_Click(object sender, RoutedEventArgs e)
    {
      if (this.CommentActionOpenedFor == null)
        return;
      Clipboard.SetText(this.CommentActionOpenedFor.Body + this.CommentActionOpenedFor.Body2 + this.CommentActionOpenedFor.Body3);
    }

    private void CommentUpVote_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      e.Handled = true;
      this.CloseCommentActions(true);
      if (App.DataManager.SettingsMan.IsSignedIn)
      {
        int vote = 0;
        switch (this.CommentActionOpenedFor.Likes)
        {
          case -1:
          case 0:
            this.CommentActionOpenedFor.DownVoteStatus = true;
            this.CommentActionOpenedFor.UpVoteStatus = false;
            this.CommentActionOpenedFor.UpVoteIcon = Visibility.Visible;
            this.CommentActionOpenedFor.DownVoteIcon = Visibility.Collapsed;
            this.CommentActionOpenedFor.Likes = 1;
            ++this.CommentActionOpenedFor.Up;
            vote = 1;
            break;
          case 1:
            this.CommentActionOpenedFor.DownVoteStatus = true;
            this.CommentActionOpenedFor.UpVoteStatus = true;
            this.CommentActionOpenedFor.UpVoteIcon = Visibility.Collapsed;
            this.CommentActionOpenedFor.UpVoteIcon = Visibility.Collapsed;
            this.CommentActionOpenedFor.Likes = 0;
            --this.CommentActionOpenedFor.Up;
            vote = 0;
            break;
        }
        int startIndex = this.CommentActionOpenedFor.LineOnePartTwo.IndexOf(' ', 1);
        if (startIndex != -1)
          this.CommentActionOpenedFor.LineOnePartTwo = " " + (object) (this.CommentActionOpenedFor.Up - this.CommentActionOpenedFor.Downs) + this.CommentActionOpenedFor.LineOnePartTwo.Substring(startIndex);
        ThreadPool.QueueUserWorkItem((WaitCallback) (obj => App.DataManager.Vote(this.CommentActionOpenedFor.RName, vote)));
      }
      else
        App.DataManager.MessageManager.ShowSignInMessage("vote");
    }

    private void CommentDownVote_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      e.Handled = true;
      this.CloseCommentActions(true);
      if (App.DataManager.SettingsMan.IsSignedIn)
      {
        int vote = 0;
        switch (this.CommentActionOpenedFor.Likes)
        {
          case -1:
            this.CommentActionOpenedFor.DownVoteStatus = true;
            this.CommentActionOpenedFor.UpVoteStatus = true;
            this.CommentActionOpenedFor.DownVoteIcon = Visibility.Collapsed;
            this.CommentActionOpenedFor.UpVoteIcon = Visibility.Collapsed;
            this.CommentActionOpenedFor.Likes = 0;
            --this.CommentActionOpenedFor.Downs;
            vote = 0;
            break;
          case 0:
          case 1:
            this.CommentActionOpenedFor.DownVoteStatus = false;
            this.CommentActionOpenedFor.UpVoteStatus = true;
            this.CommentActionOpenedFor.DownVoteIcon = Visibility.Visible;
            this.CommentActionOpenedFor.UpVoteIcon = Visibility.Collapsed;
            this.CommentActionOpenedFor.Likes = -1;
            ++this.CommentActionOpenedFor.Downs;
            vote = -1;
            break;
        }
        int startIndex = this.CommentActionOpenedFor.LineOnePartTwo.IndexOf(' ', 1);
        if (startIndex != -1)
          this.CommentActionOpenedFor.LineOnePartTwo = " " + (object) (this.CommentActionOpenedFor.Up - this.CommentActionOpenedFor.Downs) + this.CommentActionOpenedFor.LineOnePartTwo.Substring(startIndex);
        ThreadPool.QueueUserWorkItem((WaitCallback) (obj => App.DataManager.Vote(this.CommentActionOpenedFor.RName, vote)));
      }
      else
        App.DataManager.MessageManager.ShowSignInMessage("vote");
    }

    private void CommentOn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.CloseCommentActions(true);
      e.Handled = true;
      if (App.DataManager.SettingsMan.IsSignedIn)
      {
        this.CommentingOnRName = this.CommentActionOpenedFor.RName;
        if (this.CommentActionOpenedFor.Author.Equals(App.DataManager.SettingsMan.UserName))
        {
          this.CommentingText.Text = this.CommentActionOpenedFor.Body + this.CommentActionOpenedFor.Body2;
          this.CommentingOnIsEdit = true;
        }
        else
        {
          this.CommentingText.Text = "";
          this.CommentingOnIsEdit = false;
        }
        this.ApplicationBar.IsVisible = false;
        this.CommentingPanelOpen = true;
        this.CommentingDialog.Visibility = Visibility.Visible;
        this.CommentingScroller.ScrollToVerticalOffset(0.0);
        this.OpenCommenting.Begin();
      }
      else
      {
        App.DataManager.MessageManager.ShowSignInMessage("comment");
        this.CommentingPanelOpen = false;
      }
    }

    private void CommentShare_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.CloseCommentActions(true);
      e.Handled = true;
      this.shareComment = this.CommentActionOpenedFor;
      this.SharePickerOpen = true;
      this.sharePicker.Show();
    }

    private void EditButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.CloseCommentActions(true);
      e.Handled = true;
      if (App.DataManager.SettingsMan.IsSignedIn)
      {
        this.CommentingOnRName = this.CommentActionOpenedFor.RName;
        if (this.CommentActionOpenedFor.Author.Equals(App.DataManager.SettingsMan.UserName))
        {
          this.CommentingText.Text = this.CommentActionOpenedFor.Body + this.CommentActionOpenedFor.Body2;
          this.CommentingOnIsEdit = true;
        }
        else
        {
          this.CommentingText.Text = "";
          this.CommentingOnIsEdit = false;
        }
        this.CommentingPanelOpen = true;
        this.CommentingDialog.Visibility = Visibility.Visible;
        this.OpenCommenting.Begin();
      }
      else
      {
        App.DataManager.MessageManager.ShowSignInMessage("edit");
        this.CommentingPanelOpen = false;
      }
    }

    private void DeleteButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.CloseCommentActions(true);
      e.Handled = true;
      if (App.DataManager.SettingsMan.IsSignedIn)
        Guide.BeginShowMessageBox("Confirm Deletion", "Deleting a comment cannot be undone, are you sure you want to delete this comment?", (IEnumerable<string>) new string[2]
        {
          "delete",
          "cancel"
        }, 0, MessageBoxIcon.None, (AsyncCallback) (resul =>
        {
          int? nullable = Guide.EndShowMessageBox(resul);
          if (!nullable.HasValue)
            return;
          if (nullable.GetValueOrDefault() == 0)
            this.Dispatcher.BeginInvoke((Action) (() =>
            {
              this.CommentActionOpenedFor.SendingProgressBarIsInd = true;
              this.CommentActionOpenedFor.SendingProgressBar = Visibility.Visible;
              string rname = this.CommentActionOpenedFor.RName;
              ThreadPool.QueueUserWorkItem((WaitCallback) (obj =>
              {
                App.DataManager.DeleteComment(new RunWorkerCompletedEventHandler(this.DeleteSubmitCallback), rname);
                if (this.LocalStory == null)
                  return;
                App.DataManager.SettingsMan.ReadStories[this.LocalStory.RedditID]--;
              }));
            }));
        }), (object) null);
      else
        App.DataManager.MessageManager.ShowSignInMessage("delete");
    }

    private void Profile_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.CloseCommentActions(false);
      e.Handled = true;
      if (this.CommentActionOpenedFor == null || this.CommentActionOpenedFor.Author == null)
        return;
      this.restoreAppBar = true;
      this.NavigationService.Navigate(new Uri("/ProfileView.xaml?userName=" + this.CommentActionOpenedFor.Author, UriKind.Relative));
    }

    private void TitlePanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      if (this.ingoreClick((Microsoft.Phone.Controls.GestureEventArgs) null))
        return;
      SolidColorBrush solidColorBrush1 = new SolidColorBrush(Color.FromArgb((byte) 150, (byte) 0, (byte) 0, (byte) 0));
      SolidColorBrush solidColorBrush2 = new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 100, (byte) 100, (byte) 100));
      SolidColorBrush solidColorBrush3 = new SolidColorBrush((Color) Application.Current.Resources[(object) "PhoneAccentColor"]);
      SolidColorBrush solidColorBrush4 = new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
      if (!this.LocalStory.isSelf)
      {
        this.TitleOpenLink.Background = (Brush) solidColorBrush3;
        this.TitleOpenLinkText.Foreground = (Brush) solidColorBrush4;
        this.EditButtonTitle.Background = (Brush) solidColorBrush2;
        this.EditButtonTitleButton.Foreground = (Brush) solidColorBrush1;
      }
      else
      {
        this.TitleOpenLink.Background = (Brush) solidColorBrush2;
        this.TitleOpenLinkText.Foreground = (Brush) solidColorBrush1;
        this.EditButtonTitle.Background = (Brush) solidColorBrush3;
        this.EditButtonTitleButton.Foreground = (Brush) solidColorBrush4;
      }
      if (this.LocalStory.Author.Equals(App.DataManager.SettingsMan.UserName))
        this.EditButtonsTitle.Visibility = Visibility.Visible;
      else
        this.EditButtonsTitle.Visibility = Visibility.Collapsed;
      this.OpenTitleActions();
      e.Handled = true;
    }

    public void OpenTitleActions()
    {
      this.TitleActionPanel.Visibility = Visibility.Visible;
      this.BlackBackground.Visibility = Visibility.Visible;
      this.OpenStoryActionsAnmi.Begin();
      this.TitleActionPanelOpen = true;
      this.ApplicationBar.Opacity = 0.0;
      this.ApplicationBar.Mode = ApplicationBarMode.Default;
    }

    public void CloseTitleActions()
    {
      this.CloseStoryActionsAnmi.Begin();
      if (this.AppBarIsMini)
      {
        this.ApplicationBar.Opacity = 0.0;
        this.ApplicationBar.Mode = ApplicationBarMode.Minimized;
      }
      else
      {
        this.ApplicationBar.Opacity = 1.0;
        this.ApplicationBar.Mode = ApplicationBarMode.Default;
      }
    }

    private void TitleActionClosed(object sender, EventArgs e)
    {
      this.TitleActionPanel.Visibility = Visibility.Collapsed;
      this.BlackBackground.Visibility = Visibility.Collapsed;
      this.TitleActionPanelOpen = false;
    }

    private void TitleCommentOn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.CloseTitleActions();
      if (App.DataManager.SettingsMan.IsSignedIn)
      {
        this.CommentingOnRName = this.LocalStory.RedditID;
        this.CommentingOnIsEdit = false;
        this.CommentingText.Text = "";
        this.ApplicationBar.IsVisible = false;
        this.CommentingPanelOpen = true;
        this.CommentingDialog.Visibility = Visibility.Visible;
        this.OpenCommenting.Begin();
      }
      else
      {
        App.DataManager.MessageManager.ShowSignInMessage("comment");
        this.CommentingPanelOpen = false;
      }
      e.Handled = true;
    }

    private void TitleOpenLink_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      if (this.LocalStory.isSelf)
        return;
      this.CloseTitleActions();
      if (App.DataManager.SettingsMan.UseInAppBroser)
      {
        this.NavigationService.Navigate(new Uri("/InAppWebBrowser.xaml?Url=" + HttpUtility.UrlEncode(this.LocalStory.URL), UriKind.Relative));
      }
      else
      {
        WebBrowserTask webBrowserTask = new WebBrowserTask();
        webBrowserTask.Uri = new Uri(this.LocalStory.URL, UriKind.Absolute);
        try
        {
          webBrowserTask.Show();
        }
        catch
        {
        }
      }
    }

    private void TitleShare_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.CloseTitleActions();
      this.SharePickerOpen = true;
      this.sharePicker.Show();
      e.Handled = true;
    }

    private bool ingoreClick(Microsoft.Phone.Controls.GestureEventArgs e)
    {
      if (this.IngoreNextClick)
      {
        this.IngoreNextClick = false;
        return true;
      }
      return e != null && e.Handled || this.CommentActionPanelOpen || this.TitleActionPanelOpen || this.CommentingPanelOpen || this.SharePickerOpen || this.HelpOverlayOpen || this.LoadingStoryDataOpen || this.FullScreenCommentingPanelOpen || this.isPinnedOptionsOpened;
    }

    private void EditButtonTitle_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      if (!this.LocalStory.hasSelfText)
        return;
      this.CloseTitleActions();
      if (App.DataManager.SettingsMan.IsSignedIn)
      {
        this.CommentingOnRName = this.LocalStory.RedditID;
        this.CommentingOnIsEdit = true;
        this.CommentingText.Text = this.LocalStory.SelfText;
        this.CommentingPanelOpen = true;
        this.CommentingDialog.Visibility = Visibility.Visible;
        this.OpenCommenting.Begin();
      }
      else
      {
        App.DataManager.MessageManager.ShowSignInMessage("comment");
        this.CommentingPanelOpen = false;
      }
      e.Handled = true;
    }

    private void DeleteButtonTitle_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.CloseTitleActions();
      e.Handled = true;
      if (App.DataManager.SettingsMan.IsSignedIn)
        Guide.BeginShowMessageBox("Confirm Deletion", "Deleting a post cannot be undone, are you sure you want to delete this post?", (IEnumerable<string>) new string[2]
        {
          "delete",
          "cancel"
        }, 0, MessageBoxIcon.None, (AsyncCallback) (resul =>
        {
          int? nullable = Guide.EndShowMessageBox(resul);
          if (!nullable.HasValue)
            return;
          if (nullable.GetValueOrDefault() == 0)
            this.Dispatcher.BeginInvoke((Action) (() =>
            {
              ThreadPool.QueueUserWorkItem((WaitCallback) (obj => App.DataManager.DeleteComment(new RunWorkerCompletedEventHandler(this.DeleteSubmitCallback), this.LocalStory.RedditID)));
              this.LoadingOverLay.Visibility = Visibility.Visible;
              this.LoadingText.Text = "Deleting Post...";
            }));
        }), (object) null);
      else
        App.DataManager.MessageManager.ShowSignInMessage("delete");
    }

    private void ProfileTitle_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.CloseTitleActions();
      e.Handled = true;
      this.NavigationService.Navigate(new Uri("/ProfileView.xaml?userName=" + this.LocalStory.Author, UriKind.Relative));
    }

    private void GoToSubreddit_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.CloseTitleActions();
      e.Handled = true;
      this.NavigationService.Navigate(new Uri("/RedditsViewer.xaml?subredditURL=" + HttpUtility.UrlEncode("/r/" + this.LocalStory.SubReddit + "/"), UriKind.Relative));
    }

    private void CommentActionPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      e.Handled = true;
    }

    private void TitleActionPanel_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      e.Handled = true;
    }

    private void CommentActionPanel_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      e.Handled = true;
    }

    private void TitleActionPanel_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      e.Handled = true;
    }

    private void CommentingText_TextChanged(object sender, TextChangedEventArgs e)
    {
      if (this.text.Length == 0 || this.CommentingText.Text.Length == 0)
      {
        this.text = this.CommentingText.Text;
      }
      else
      {
        if ((int) this.text[this.text.Length - 1] == (int) this.CommentingText.Text[this.CommentingText.Text.Length - 1])
          return;
        this.CommentingScroller.ScrollToVerticalOffset(5000000.0);
        this.text = this.CommentingText.Text;
      }
    }

    private void ChangeSort_Click(object sender, EventArgs e)
    {
      string[] strArray = new string[6]
      {
        "Best",
        "Top",
        "New",
        "Hot",
        "Controversial",
        "Old"
      };
      PickerBoxDialog pickerBoxDialog = new PickerBoxDialog();
      pickerBoxDialog.ItemSource = (IEnumerable) strArray;
      pickerBoxDialog.Title = "SORT COMMENTS BY";
      pickerBoxDialog.Closed += new EventHandler(this.ChangeSort_Closed);
      pickerBoxDialog.SelectedIndex = this.SortShowing;
      pickerBoxDialog.Show();
    }

    private void ChangeSort_Closed(object sender, EventArgs e)
    {
      if (((PickerBoxDialog) sender).SelectedIndex == this.SortShowing)
        return;
      this.SortShowing = ((PickerBoxDialog) sender).SelectedIndex;
      App.DataManager.SettingsMan.DefaultCommentSort = this.SortShowing;
      this.ChangeSort();
    }

    public void ChangeSort()
    {
      if (this.ViewEntireThread.Visibility == Visibility.Visible)
        this.HideViewEntrieThread.Begin();
      this.ViewModel.Comments.Clear();
      this.ViewModel.HasCommentsFromWeb = false;
      ThreadPool.QueueUserWorkItem((WaitCallback) (obj =>
      {
        Thread.Sleep(400);
        this.ViewModel.LoadData();
      }));
    }

    private void Save_Click(object sender, EventArgs e)
    {
      if (!App.DataManager.SettingsMan.IsSignedIn)
      {
        App.DataManager.MessageManager.ShowSignInMessage("save a story");
      }
      else
      {
        this.LocalStory.isSaved = !this.LocalStory.isSaved;
        ThreadPool.QueueUserWorkItem((WaitCallback) (obj => App.DataManager.SaveStory(this.LocalStory.RedditID, this.LocalStory.isSaved)));
        if (App.DataManager.SettingsMan.ShowSaveNote)
        {
          App.DataManager.SettingsMan.ShowSaveNote = false;
          int num = (int) MessageBox.Show("Saving a story will keep it in a saved list on reddit, look for the “saved” subreddit in this app to access these stories later.", "Saving A Story", MessageBoxButton.OK);
        }
        App.DataManager.BaconitStore.UpdateStory(this.LocalStory);
        if (this.SubRedditHost != null)
          this.SubRedditHost.refreshUI(this.LocalStory);
        if (this.LocalStory.isSaved)
          ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[2]).Text = "unsave story";
        else
          ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[2]).Text = "save story";
      }
    }

    private void Hide_Click(object sender, EventArgs e)
    {
      if (!App.DataManager.SettingsMan.IsSignedIn)
      {
        App.DataManager.MessageManager.ShowSignInMessage("hide a story");
      }
      else
      {
        this.LocalStory.isHidden = !this.LocalStory.isHidden;
        ThreadPool.QueueUserWorkItem((WaitCallback) (obj => App.DataManager.HideStory(this.LocalStory.RedditID, this.LocalStory.isHidden)));
        App.DataManager.BaconitStore.UpdateStory(this.LocalStory);
        if (this.SubRedditHost != null)
          this.SubRedditHost.refreshUI(this.LocalStory);
        if (this.LocalStory.isHidden)
          ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[3]).Text = "unhide story";
        else
          ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[3]).Text = "hide story";
      }
    }

    private void lockOren_Click(object sender, EventArgs e)
    {
      App.DataManager.SettingsMan.ScreenOrenLocked = !App.DataManager.SettingsMan.ScreenOrenLocked;
      if (App.DataManager.SettingsMan.ScreenOrenLocked)
      {
        ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[4]).Text = "unlock screen orientation";
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
        ((ApplicationBarMenuItem) this.ApplicationBar.MenuItems[4]).Text = "lock screen orientation";
        this.SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;
      }
    }

    private void ApplicationBar_StateChanged(object sender, ApplicationBarStateChangedEventArgs e)
    {
      if (e.IsMenuVisible)
      {
        SystemTray.IsVisible = true;
        SystemTray.Opacity = 0.8;
      }
      else
        SystemTray.IsVisible = false;
      if (this.AppBarIsMini)
      {
        if (e.IsMenuVisible)
          this.ApplicationBar.Opacity = 1.0;
        else
          this.ApplicationBar.Opacity = 0.0;
      }
      else
        this.ApplicationBar.Opacity = 1.0;
    }

    private void ShowPinPicker()
    {
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
      string imageUrl = DataManager.GetImageURL(this.LocalStory, false);
      if (imageUrl == null || imageUrl.Equals(""))
        this.ImageTileButton.Visibility = Visibility.Collapsed;
      else
        this.ImageTileButton.Visibility = Visibility.Visible;
      this.PinSettingsSubName.Text = this.LocalStory.Title;
      this.PinSettingsSubNameImage.Text = this.LocalStory.Title;
      this.BlackBackground.Visibility = Visibility.Visible;
      this.TilePicker.Visibility = Visibility.Visible;
      this.ApplicationBar.IsVisible = false;
      this.OpenPinPicker.Begin();
      this.isPinnedOptionsOpened = true;
    }

    private void ClosePinPicker_complete(object sender, EventArgs e)
    {
      this.BlackBackground.Visibility = Visibility.Collapsed;
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
      if (this.LocalStory == null)
        return;
      if (isCycle)
      {
        List<Uri> uriList = new List<Uri>();
        uriList.Add(new Uri("/Images/LiveTiles/DefaultLiveTile1.png", UriKind.Relative));
        CycleTileData cycleTileData = new CycleTileData();
        cycleTileData.Title = this.LocalStory.Title;
        cycleTileData.SmallBackgroundImage = new Uri(DataManager.LiveTileBackgrounds[this.TileIcon.SelectedIndex] + "c.png", UriKind.Relative);
        cycleTileData.CycleImages = (IEnumerable<Uri>) uriList;
        ShellTileData initialData = (ShellTileData) cycleTileData;
        App.DataManager.SettingsMan.ForceUpdateTiles = true;
        ShellTile.Create(new Uri("/StoryDetails.xaml?StoryDataRedditID=" + this.LocalStory.RedditID + "&TileType=cycle", UriKind.Relative), initialData, true);
      }
      else
      {
        string str1 = "";
        string str2 = "";
        string str3 = "";
        if (this.LocalStory != null)
        {
          str1 = this.LocalStory.Title;
          str2 = (this.LocalStory.ups - this.LocalStory.downs).ToString() + " points; " + (object) this.LocalStory.comments + " comments";
          str3 = "By " + this.LocalStory.Author;
        }
        IconicTileData initialData = new IconicTileData();
        initialData.Title = this.LocalStory.Title;
        initialData.IconImage = new Uri(DataManager.LiveTileBackgrounds[this.TileIcon.SelectedIndex] + "m.png", UriKind.Relative);
        initialData.SmallIconImage = new Uri(DataManager.LiveTileBackgrounds[this.TileIcon.SelectedIndex] + "s.png", UriKind.Relative);
        initialData.WideContent1 = str1;
        initialData.WideContent2 = str2;
        initialData.WideContent3 = str3;
        ShellTile.Create(new Uri("/StoryDetails.xaml?StoryDataRedditID=" + this.LocalStory.RedditID + "&TileType=iconic", UriKind.Relative), (ShellTileData) initialData, true);
      }
    }

    private void TileIcon_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.TileIcon.SelectedIndex <= -1 || this.TileIcon.SelectedIndex >= DataManager.LiveTileBackgrounds.Length)
        return;
      this.PinSettingsSampleImage.Source = (ImageSource) new BitmapImage(new Uri(DataManager.LiveTileBackgrounds[this.TileIcon.SelectedIndex] + "m.png", UriKind.Relative));
    }

    private void ViewEntireThread_Tap_1(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.SelfTextDevider.Visibility = Visibility.Visible;
      this.SortShowing = 0;
      this.ChangeSort();
    }

    private void CopyStoryTitle_Click_1(object sender, RoutedEventArgs e)
    {
      if (this.LocalStory == null)
        return;
      Clipboard.SetText(this.LocalStory.Title);
      App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("Text copied", true, false, "", ""));
    }

    private void CopySelfText_Click_1(object sender, RoutedEventArgs e)
    {
      if (this.LocalStory == null)
        return;
      try
      {
        Clipboard.SetText(this.LocalStory.SelfText);
        App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("Text copied", true, false, "", ""));
      }
      catch
      {
      }
    }

    private void CopyCommentText_Click_1(object sender, RoutedEventArgs e)
    {
      if (!((sender as MenuItem).DataContext is CommentData dataContext))
        return;
      try
      {
        Clipboard.SetText(dataContext.Body + dataContext.Body2 + dataContext.Body3);
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
      Application.LoadComponent((object) this, new Uri("/Baconit;component/StoryDetails.xaml", UriKind.Relative));
      this.OpenCommenting = (Storyboard) this.FindName("OpenCommenting");
      this.CloseCommenting = (Storyboard) this.FindName("CloseCommenting");
      this.FadeOutTips = (Storyboard) this.FindName("FadeOutTips");
      this.FadeInTips = (Storyboard) this.FindName("FadeInTips");
      this.FadeOutSelfTips = (Storyboard) this.FindName("FadeOutSelfTips");
      this.FadeInSelfTips = (Storyboard) this.FindName("FadeInSelfTips");
      this.OpenCommentActionsAni = (Storyboard) this.FindName("OpenCommentActionsAni");
      this.CloseCommentActionsAni = (Storyboard) this.FindName("CloseCommentActionsAni");
      this.OpenFullScreenCommentingAni = (Storyboard) this.FindName("OpenFullScreenCommentingAni");
      this.CloseFullScreenCommentingAni = (Storyboard) this.FindName("CloseFullScreenCommentingAni");
      this.OpenStoryActionsAnmi = (Storyboard) this.FindName("OpenStoryActionsAnmi");
      this.CloseStoryActionsAnmi = (Storyboard) this.FindName("CloseStoryActionsAnmi");
      this.HideViewEntrieThread = (Storyboard) this.FindName("HideViewEntrieThread");
      this.FadeOutLoadingOverLay = (Storyboard) this.FindName("FadeOutLoadingOverLay");
      this.FadeInLoadingOverLay = (Storyboard) this.FindName("FadeInLoadingOverLay");
      this.ChangeSelfText = (Storyboard) this.FindName("ChangeSelfText");
      this.ChangeSelfTextFrom = (EasingDoubleKeyFrame) this.FindName("ChangeSelfTextFrom");
      this.ChangeSelfTextTo = (EasingDoubleKeyFrame) this.FindName("ChangeSelfTextTo");
      this.ExpandTitleBar = (Storyboard) this.FindName("ExpandTitleBar");
      this.ExpandTitleBarKeyFrame = (EasingDoubleKeyFrame) this.FindName("ExpandTitleBarKeyFrame");
      this.CollpaseTitleBar = (Storyboard) this.FindName("CollpaseTitleBar");
      this.CollpaseTitleBarKeyFrame = (EasingDoubleKeyFrame) this.FindName("CollpaseTitleBarKeyFrame");
      this.OpenPinPicker = (Storyboard) this.FindName("OpenPinPicker");
      this.ClosePinPicker = (Storyboard) this.FindName("ClosePinPicker");
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TitlePanel = (StackPanel) this.FindName("TitlePanel");
      this.RedditTitle = (TextBlock) this.FindName("RedditTitle");
      this.LineOne = (TextBlock) this.FindName("LineOne");
      this.LineTwo = (TextBlock) this.FindName("LineTwo");
      this.SelfTextHolder = (Grid) this.FindName("SelfTextHolder");
      this.SelfTextProgressBar = (ProgressBar) this.FindName("SelfTextProgressBar");
      this.SelfTextScroller = (ScrollViewer) this.FindName("SelfTextScroller");
      this.SelfText = (SuperRichTextBox) this.FindName("SelfText");
      this.SelfTextDevider = (Rectangle) this.FindName("SelfTextDevider");
      this.ViewEntireThread = (Grid) this.FindName("ViewEntireThread");
      this.LoadingCommentsBar = (ProgressBar) this.FindName("LoadingCommentsBar");
      this.CommentList = (ListBox) this.FindName("CommentList");
      this.NoComments = (Grid) this.FindName("NoComments");
      this.BlackBackground = (Grid) this.FindName("BlackBackground");
      this.CommentActionPanel = (Grid) this.FindName("CommentActionPanel");
      this.CommentActionPanelContentScroller = (ScrollViewer) this.FindName("CommentActionPanelContentScroller");
      this.CommentActionPanelContentHolder = (StackPanel) this.FindName("CommentActionPanelContentHolder");
      this.CommentActionLineOneOne = (TextBlock) this.FindName("CommentActionLineOneOne");
      this.CommentActionLineOneTwo = (TextBlock) this.FindName("CommentActionLineOneTwo");
      this.CommentActionCommentContent = (SuperRichTextBox) this.FindName("CommentActionCommentContent");
      this.CommentActionPanelVertHolder = (Grid) this.FindName("CommentActionPanelVertHolder");
      this.CommentUpVote = (Grid) this.FindName("CommentUpVote");
      this.CommentUpVoteText = (TextBlock) this.FindName("CommentUpVoteText");
      this.CommentDownVote = (Grid) this.FindName("CommentDownVote");
      this.CommentDownVoteText = (TextBlock) this.FindName("CommentDownVoteText");
      this.CommentShare = (Grid) this.FindName("CommentShare");
      this.Profile = (Grid) this.FindName("Profile");
      this.CommentOn = (Grid) this.FindName("CommentOn");
      this.CommentOnText = (TextBlock) this.FindName("CommentOnText");
      this.EditButtons = (StackPanel) this.FindName("EditButtons");
      this.EditButton = (Grid) this.FindName("EditButton");
      this.DeleteButton = (Grid) this.FindName("DeleteButton");
      this.CommentActionPanelHorzHolder = (Grid) this.FindName("CommentActionPanelHorzHolder");
      this.CommentUpVoteHorz = (Grid) this.FindName("CommentUpVoteHorz");
      this.CommentUpVoteTextHorz = (TextBlock) this.FindName("CommentUpVoteTextHorz");
      this.CommentDownVoteHorz = (Grid) this.FindName("CommentDownVoteHorz");
      this.CommentDownVoteTextHorz = (TextBlock) this.FindName("CommentDownVoteTextHorz");
      this.CommentOnHorz = (Grid) this.FindName("CommentOnHorz");
      this.CommentOnTextHorz = (TextBlock) this.FindName("CommentOnTextHorz");
      this.EditOnHorz = (Grid) this.FindName("EditOnHorz");
      this.CommentShareHorz = (Grid) this.FindName("CommentShareHorz");
      this.TitleActionPanel = (StackPanel) this.FindName("TitleActionPanel");
      this.TitleCommentOn = (Grid) this.FindName("TitleCommentOn");
      this.EditButtonsTitle = (StackPanel) this.FindName("EditButtonsTitle");
      this.EditButtonTitle = (Grid) this.FindName("EditButtonTitle");
      this.EditButtonTitleButton = (TextBlock) this.FindName("EditButtonTitleButton");
      this.DeleteButtonTitle = (Grid) this.FindName("DeleteButtonTitle");
      this.TitleOpenLink = (Grid) this.FindName("TitleOpenLink");
      this.TitleOpenLinkText = (TextBlock) this.FindName("TitleOpenLinkText");
      this.TitleShare = (Grid) this.FindName("TitleShare");
      this.TitleShareText = (TextBlock) this.FindName("TitleShareText");
      this.StoryOverlay = (Image) this.FindName("StoryOverlay");
      this.SelfTextOverlay = (Image) this.FindName("SelfTextOverlay");
      this.CommentingDialog = (Grid) this.FindName("CommentingDialog");
      this.CommentingScroller = (ScrollViewer) this.FindName("CommentingScroller");
      this.CommentingText = (TextBox) this.FindName("CommentingText");
      this.CommentingSubmit = (Button) this.FindName("CommentingSubmit");
      this.CommentingClose = (Button) this.FindName("CommentingClose");
      this.TilePicker = (Grid) this.FindName("TilePicker");
      this.TileIcon = (ListPicker) this.FindName("TileIcon");
      this.ImageTileButton = (Button) this.FindName("ImageTileButton");
      this.PinSettingsSubNameImage = (TextBlock) this.FindName("PinSettingsSubNameImage");
      this.PinSettingsSampleImage = (Image) this.FindName("PinSettingsSampleImage");
      this.PinSettingsSubName = (TextBlock) this.FindName("PinSettingsSubName");
      this.FullScreenBlackBackground = (Grid) this.FindName("FullScreenBlackBackground");
      this.FullScreenCommenting = (Grid) this.FindName("FullScreenCommenting");
      this.TitleHolder = (ScrollViewer) this.FindName("TitleHolder");
      this.FullScreenCommentingContextHolder = (StackPanel) this.FindName("FullScreenCommentingContextHolder");
      this.FullScreenCommentingLineOne = (TextBlock) this.FindName("FullScreenCommentingLineOne");
      this.FullScreenCommentingLineTwo = (TextBlock) this.FindName("FullScreenCommentingLineTwo");
      this.FullScreenCommentingComment = (SuperRichTextBox) this.FindName("FullScreenCommentingComment");
      this.FullScreenCommentingScroller = (ScrollViewer) this.FindName("FullScreenCommentingScroller");
      this.FullScreenCommentingBox = (TextBox) this.FindName("FullScreenCommentingBox");
      this.FullScreenSubmit = (Grid) this.FindName("FullScreenSubmit");
      this.FullScreenCancel = (Grid) this.FindName("FullScreenCancel");
      this.FullScreenCommentingFormattingPreview = (RichTextBox) this.FindName("FullScreenCommentingFormattingPreview");
      this.LoadingOverLay = (Grid) this.FindName("LoadingOverLay");
      this.LoadingOverlayBackground = (Rectangle) this.FindName("LoadingOverlayBackground");
      this.LoadingText = (TextBlock) this.FindName("LoadingText");
      this.LoadingOverlayBar = (ProgressBar) this.FindName("LoadingOverlayBar");
    }

    private class CommentTapThread
    {
      private bool Canceled;
      private object lockObj = new object();
      private CommentData OpenedWith;
      private int SelectedIndex;
      private StoryDetails Host;

      public CommentTapThread(int index, CommentData open, StoryDetails me)
      {
        this.SelectedIndex = index;
        this.OpenedWith = open;
        this.Host = me;
      }

      public void run()
      {
        Thread.Sleep(190);
        lock (this.lockObj)
        {
          if (!this.Canceled)
          {
            this.Host.CommentActionOpenedFor = this.OpenedWith;
            Deployment.Current.Dispatcher.BeginInvoke(new Action(this.Host.OpenCommentActions));
          }
        }
        this.Host.CommentTapThreadObj = (StoryDetails.CommentTapThread) null;
      }

      public void Cancel()
      {
        lock (this.lockObj)
        {
          this.Canceled = true;
          this.Host.CommentTapThreadObj = (StoryDetails.CommentTapThread) null;
        }
      }
    }
  }
}
