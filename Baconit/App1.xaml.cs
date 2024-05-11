using Baconit.Database;
using Baconit.Libs;
using BaconitData.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Baconit
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        public const int TOUCH_TOLERANCE = 12;
        private static DataManager _dataManager = (DataManager)null;
        public static SubRedditData ActiveStoryData = (SubRedditData)null;
        private static MessageInboxViewModel _MessageInboxViewModel = (MessageInboxViewModel)null;
        private static SmartBarViewModel _SmartBarViewModel = (SmartBarViewModel)null;
        private static ProfileViewModel _ProfileViewModel = (ProfileViewModel)null;
        //public static NavigationOutTransition SlideOutTransistion;
        //public static NavigationInTransition SlideInTransistion;
        //public static NavigationInTransition FeatherInTransition;
        //public static NavigationInTransition DelayedFeatherInTransition;
        //public static NavigationOutTransition FeatherOutTransition;
        //public static NavigationInTransition TurnstileInTransition;
        //public static NavigationOutTransition TurnstileOutTransition;
        //public static NavigationService navService;
        public static bool DoFastResumeMainPageFix = false;
        public static RedditViewerViewModel ViewModelTransfter;
        public static RedditsViewer RedditViewerTransfter;
        public static string BAR_UP_VOTE_SELECT = "/Images/Arrows/brow_up_black.png";
        public const string BAR_UP_VOTE_NONE = "/Images/Arrows/brow_up_none.png";
        public static string BAR_DOWN_VOTE_SELECT = "/Images/Arrows/brow_down_black.png";
        public const string BAR_DOWN_VOTE_NONE = "/Images/Arrows/brow_down_none.png";
        private static object DataManLock = new object();
        private bool ressetApp;
        private bool clearSubBack;
        private bool phoneApplicationInitialized;



        public static DataManager DataManager
        {
            get
            {
                if (App._dataManager == null)
                {
                    lock (App.DataManLock)
                    {
                        if (App._dataManager == null)
                        {
                            App._dataManager = new DataManager(false);
                            BaconSync BaconSyncInt = new BaconSync(App._dataManager);
                            MessageManager MessageManagerInt = new MessageManager(App._dataManager);
                            AnalyticManager BaconitAnalyticsInt = new AnalyticManager();
                            App._dataManager.InitDataMan((BaconSyncInterface)BaconSyncInt, (MessageManagerInterface)MessageManagerInt, (BaconitAnalyticsInterface)BaconitAnalyticsInt);
                        }
                    }
                }
                return App._dataManager;
            }
        }

        public static MessageInboxViewModel MessageInboxViewModel
        {
            get
            {
                if (App._MessageInboxViewModel == null)
                    App._MessageInboxViewModel = new MessageInboxViewModel();
                return App._MessageInboxViewModel;
            }
        }

        public static SmartBarViewModel SmartBarViewModel
        {
            get
            {
                if (App._SmartBarViewModel == null)
                    App._SmartBarViewModel = new SmartBarViewModel();
                return App._SmartBarViewModel;
            }
        }

        public static ProfileViewModel ProfileViewModel
        {
            get
            {
                if (App._ProfileViewModel == null)
                    App._ProfileViewModel = new ProfileViewModel();
                return App._ProfileViewModel;
            }
        }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}

/*
using Baconit.Database;
using Baconit.Libs;
using BaconitData.Interfaces;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Info;
using Microsoft.Phone.Shell;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;

#nullable disable
namespace Baconit
{
  public class App : Application
  {
    public const int TOUCH_TOLERANCE = 12;
    private static DataManager _dataManager = (DataManager) null;
    public static SubRedditData ActiveStoryData = (SubRedditData) null;
    private static MessageInboxViewModel _MessageInboxViewModel = (MessageInboxViewModel) null;
    private static SmartBarViewModel _SmartBarViewModel = (SmartBarViewModel) null;
    private static ProfileViewModel _ProfileViewModel = (ProfileViewModel) null;
    public static NavigationOutTransition SlideOutTransistion;
    public static NavigationInTransition SlideInTransistion;
    public static NavigationInTransition FeatherInTransition;
    public static NavigationInTransition DelayedFeatherInTransition;
    public static NavigationOutTransition FeatherOutTransition;
    public static NavigationInTransition TurnstileInTransition;
    public static NavigationOutTransition TurnstileOutTransition;
    public static NavigationService navService;
    public static bool DoFastResumeMainPageFix = false;
    public static RedditViewerViewModel ViewModelTransfter;
    public static RedditsViewer RedditViewerTransfter;
    public static string BAR_UP_VOTE_SELECT = "/Images/Arrows/brow_up_black.png";
    public const string BAR_UP_VOTE_NONE = "/Images/Arrows/brow_up_none.png";
    public static string BAR_DOWN_VOTE_SELECT = "/Images/Arrows/brow_down_black.png";
    public const string BAR_DOWN_VOTE_NONE = "/Images/Arrows/brow_down_none.png";
    private static object DataManLock = new object();
    private bool ressetApp;
    private bool clearSubBack;
    private bool phoneApplicationInitialized;
    private bool _contentLoaded;

    public static DataManager DataManager
    {
      get
      {
        if (App._dataManager == null)
        {
          lock (App.DataManLock)
          {
            if (App._dataManager == null)
            {
              App._dataManager = new DataManager(false);
              BaconSync BaconSyncInt = new BaconSync(App._dataManager);
              MessageManager MessageManagerInt = new MessageManager(App._dataManager);
              AnalyticManager BaconitAnalyticsInt = new AnalyticManager();
              App._dataManager.InitDataMan((BaconSyncInterface) BaconSyncInt, (MessageManagerInterface) MessageManagerInt, (BaconitAnalyticsInterface) BaconitAnalyticsInt);
            }
          }
        }
        return App._dataManager;
      }
    }

    public static MessageInboxViewModel MessageInboxViewModel
    {
      get
      {
        if (App._MessageInboxViewModel == null)
          App._MessageInboxViewModel = new MessageInboxViewModel();
        return App._MessageInboxViewModel;
      }
    }

    public static SmartBarViewModel SmartBarViewModel
    {
      get
      {
        if (App._SmartBarViewModel == null)
          App._SmartBarViewModel = new SmartBarViewModel();
        return App._SmartBarViewModel;
      }
    }

    public static ProfileViewModel ProfileViewModel
    {
      get
      {
        if (App._ProfileViewModel == null)
          App._ProfileViewModel = new ProfileViewModel();
        return App._ProfileViewModel;
      }
    }

    public PhoneApplicationFrame RootFrame { get; private set; }

    public App()
    {
      this.UnhandledException += new EventHandler<ApplicationUnhandledExceptionEventArgs>(this.Application_UnhandledException);
      this.InitializeComponent();
      this.InitializePhoneApplication();
      this.RootFrame.Navigating += new NavigatingCancelEventHandler(this.RootFrame_Navigating);
      this.RootFrame.Navigated += new NavigatedEventHandler(this.RootFrame_Navigated);
      NavigationInTransition navigationInTransition1 = new NavigationInTransition();
      navigationInTransition1.Backward = (TransitionElement) new SlideTransition()
      {
        Mode = SlideTransitionMode.SlideUpFadeIn
      };
      navigationInTransition1.Forward = (TransitionElement) new SlideTransition()
      {
        Mode = SlideTransitionMode.SlideUpFadeIn
      };
      App.SlideInTransistion = navigationInTransition1;
      NavigationOutTransition navigationOutTransition1 = new NavigationOutTransition();
      navigationOutTransition1.Backward = (TransitionElement) new SlideTransition()
      {
        Mode = SlideTransitionMode.SlideDownFadeOut
      };
      navigationOutTransition1.Forward = (TransitionElement) new SlideTransition()
      {
        Mode = SlideTransitionMode.SlideDownFadeOut
      };
      App.SlideOutTransistion = navigationOutTransition1;
      NavigationInTransition navigationInTransition2 = new NavigationInTransition();
      navigationInTransition2.Backward = (TransitionElement) new TurnstileFeatherTransition()
      {
        Mode = TurnstileFeatherTransitionMode.BackwardIn
      };
      navigationInTransition2.Forward = (TransitionElement) new TurnstileFeatherTransition()
      {
        Mode = TurnstileFeatherTransitionMode.ForwardIn
      };
      App.FeatherInTransition = navigationInTransition2;
      NavigationInTransition navigationInTransition3 = new NavigationInTransition();
      navigationInTransition3.Backward = (TransitionElement) new TurnstileFeatherTransition()
      {
        Mode = TurnstileFeatherTransitionMode.BackwardIn
      };
      navigationInTransition3.Forward = (TransitionElement) new TurnstileFeatherTransition()
      {
        Mode = TurnstileFeatherTransitionMode.ForwardIn,
        BeginTime = new TimeSpan?(TimeSpan.FromMilliseconds(500.0))
      };
      App.DelayedFeatherInTransition = navigationInTransition3;
      NavigationOutTransition navigationOutTransition2 = new NavigationOutTransition();
      navigationOutTransition2.Backward = (TransitionElement) new TurnstileFeatherTransition()
      {
        Mode = TurnstileFeatherTransitionMode.BackwardOut
      };
      navigationOutTransition2.Forward = (TransitionElement) new TurnstileFeatherTransition()
      {
        Mode = TurnstileFeatherTransitionMode.ForwardOut
      };
      App.FeatherOutTransition = navigationOutTransition2;
      NavigationInTransition navigationInTransition4 = new NavigationInTransition();
      navigationInTransition4.Backward = (TransitionElement) new TurnstileTransition()
      {
        Mode = TurnstileTransitionMode.BackwardIn
      };
      navigationInTransition4.Forward = (TransitionElement) new TurnstileTransition()
      {
        Mode = TurnstileTransitionMode.ForwardIn
      };
      App.TurnstileInTransition = navigationInTransition4;
      NavigationOutTransition navigationOutTransition3 = new NavigationOutTransition();
      navigationOutTransition3.Backward = (TransitionElement) new TurnstileTransition()
      {
        Mode = TurnstileTransitionMode.BackwardOut
      };
      navigationOutTransition3.Forward = (TransitionElement) new TurnstileTransition()
      {
        Mode = TurnstileTransitionMode.ForwardOut
      };
      App.TurnstileOutTransition = navigationOutTransition3;
      DataManager.ACCENT_COLOR_COLOR = (Color) Application.Current.Resources[(object) "PhoneAccentColor"];
      DataManager.ACCENT_COLOR = DataManager.ACCENT_COLOR_COLOR.ToString();
      if (((Visibility) Application.Current.Resources[(object) "PhoneDarkThemeVisibility"]).CompareTo((object) Visibility.Collapsed) == 0)
        DataManager.LIGHT_THEME = true;
      DataManager.IS_LOW_MEMORY_DEVICE = (long) DeviceExtendedProperties.GetValue("DeviceTotalMemory") <= 268435456L;
      if (App.DataManager.SettingsMan.ScaleFactor == -1)
        App.DataManager.SettingsMan.ScaleFactor = Application.Current.Host.Content.ScaleFactor;
      if (!Debugger.IsAttached)
        return;
      App.DataManager.SettingsMan.DEBUGGING = true;
      Application.Current.Host.Settings.EnableFrameRateCounter = true;
      Application.Current.Host.Settings.EnableRedrawRegions = false;
      Application.Current.Host.Settings.EnableCacheVisualization = false;
      PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
    }

    private void RootFrame_Navigating(object sender, NavigatingCancelEventArgs e)
    {
      if (this.ressetApp && e.IsCancelable && e.Uri.OriginalString.Equals("/MainLanding.xaml"))
      {
        this.ressetApp = false;
        if (App.DataManager.SettingsMan.FreshStartFromLiveTile)
        {
          try
          {
            if (App.navService != null)
            {
              while (App.navService.CanGoBack)
                App.navService.RemoveBackEntry();
            }
            App.DoFastResumeMainPageFix = true;
          }
          catch
          {
          }
        }
        else
          e.Cancel = true;
      }
      else if (e.Uri.OriginalString.Contains("TileType") && this.clearSubBack)
      {
        this.clearSubBack = false;
        if (App.navService != null)
        {
          int num = 0;
          foreach (JournalEntry back in App.navService.BackStack)
          {
            if (!back.Source.OriginalString.Contains("MainLanding.xaml"))
              ++num;
          }
          for (int index = 0; index < num && App.navService.CanGoBack; ++index)
            App.navService.RemoveBackEntry();
        }
      }
      if (e.Uri.OriginalString.Contains("external"))
      {
        this.clearSubBack = true;
      }
      else
      {
        if (e.NavigationMode != NavigationMode.Back)
          return;
        this.clearSubBack = false;
      }
    }

    private void RootFrame_Navigated(object sender, NavigationEventArgs e)
    {
      this.ressetApp = e.NavigationMode == NavigationMode.Reset;
    }

    private void Application_Launching(object sender, LaunchingEventArgs e)
    {
      if (App.DataManager.SettingsMan.EnableLogging)
        App.DataManager.LogMan.RestoreLog();
      ThreadPool.QueueUserWorkItem((WaitCallback) (j =>
      {
        try
        {
          if (App.DataManager.SettingsMan.ResetOrenationOnStart)
            App.DataManager.SettingsMan.ScreenOrenLocked = false;
          App.DataManager.SettingsMan.DeactivatedPageObjects.Clear();
          App.DataManager.SettingsMan.SetDeactivatedPageObjects();
        }
        catch
        {
        }
        UpdaterMan.UpdateAgents();
      }));
      App.DataManager.StartUpCheckMan.AppStarted(true);
    }

    private void Application_Activated(object sender, ActivatedEventArgs e)
    {
      DataManager.ACCENT_COLOR_COLOR = (Color) Application.Current.Resources[(object) "PhoneAccentColor"];
      DataManager.ACCENT_COLOR = DataManager.ACCENT_COLOR_COLOR.ToString();
      if (!e.IsApplicationInstancePreserved)
      {
        if (App.DataManager.SettingsMan.DeactivatedActiveSubReddit != null)
          App.DataManager.SettingsMan.DeactivatedActiveSubReddit = (SubReddit) null;
        if (App.DataManager.SettingsMan.DeactivatedActiveStoryData != null)
          App.ActiveStoryData = App.DataManager.SettingsMan.DeactivatedActiveStoryData.Restore();
      }
      ThreadPool.QueueUserWorkItem((WaitCallback) (j =>
      {
        if (e.IsApplicationInstancePreserved)
        {
          App.DataManager.SettingsMan.DeactivatedPageObjects.Clear();
          App.DataManager.SettingsMan.SetDeactivatedPageObjects();
        }
        App.DataManager.StartUpCheckMan.AppStarted(false);
        App.DataManager.MessageManager.SetResumeTime(BaconitStore.currentTime());
      }));
    }

    private void Application_Deactivated(object sender, DeactivatedEventArgs e)
    {
      App.DataManager.LogMan.Info("App Deactivated");
      if (App.ActiveStoryData != null)
        App.DataManager.SettingsMan.DeactivatedActiveStoryData = new SubRedditDataSeralizable(App.ActiveStoryData);
      this.SAVETHESHIT();
      App.DataManager.BaconSyncObj.AppDisactivated();
      if (App.DataManager.SettingsMan.EnableLogging)
        App.DataManager.LogMan.FlushLog();
      Thread.Sleep(500);
    }

    private void Application_Closing(object sender, ClosingEventArgs e)
    {
      App.DataManager.LogMan.Info("App Closing");
      App.DataManager.BaconSyncObj.AppDisactivated();
      this.SAVETHESHIT();
      if (App.DataManager.SettingsMan.EnableLogging)
        App.DataManager.LogMan.FlushLog();
      Thread.Sleep(500);
    }

    private void SAVETHESHIT()
    {
      App.DataManager.VIPDataMan.Save();
      App.DataManager.BaconitStore.Save();
      App.DataManager.SettingsMan.SetReadStories();
      App.DataManager.SettingsMan.SaveMainLandingTiles();
      App.DataManager.SettingsMan.SetDeactivatedPageObjects();
      App.DataManager.SettingsMan.SetOptimizeWebsites();
    }

    private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
    {
      App.DataManager.LogMan.Info("Navigation Failed");
      if (Debugger.IsAttached)
        Debugger.Break();
      App.DataManager.LogMan.FlushLog();
    }

    private void Application_UnhandledException(
      object sender,
      ApplicationUnhandledExceptionEventArgs e)
    {
      if (Debugger.IsAttached)
        Debugger.Break();
      App.DataManager.LogMan.FlushLog();
    }

    private void InitializePhoneApplication()
    {
      if (this.phoneApplicationInitialized)
        return;
      this.RootFrame = (PhoneApplicationFrame) new TransitionFrame();
      this.RootFrame.Navigated += new NavigatedEventHandler(this.CompleteInitializePhoneApplication);
      this.RootFrame.UriMapper = (UriMapperBase) new AssociationUriMapper();
      this.RootFrame.NavigationFailed += new NavigationFailedEventHandler(this.RootFrame_NavigationFailed);
      this.phoneApplicationInitialized = true;
    }

    private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
    {
      if (this.RootVisual != this.RootFrame)
        this.RootVisual = (UIElement) this.RootFrame;
      this.RootFrame.Navigated -= new NavigatedEventHandler(this.CompleteInitializePhoneApplication);
    }
   
  }
}
 
 */
