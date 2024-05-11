// Decompiled with JetBrains decompiler
// Type: Baconit.SmartBar
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Baconit.Database;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

#nullable disable
namespace Baconit
{
  public class SmartBar : PhoneApplicationPage
  {
    public static SmartBar me;
    public bool PendingSubRedditValidation;
    private bool SearchIsGoing;
    private bool SubredditSearchIsGoing;
    private bool SearchFailed;
    private bool SubredditLookupFailed;
    private bool UserSearchGoing;
    private bool UserSearchFailed;
    private bool WasSearch;
    private bool HelpOpen;
    private bool isOpening = true;
    private bool RestoreProgressBar;
    internal Storyboard CloseHelpAnmi;
    internal Storyboard OpenHelpAnmi;
    internal Grid LayoutRoot;
    internal StackPanel TitlePanel;
    internal TextBlock ApplicationTitle;
    internal AutoCompleteBox SmartBarBox;
    internal Grid ContentPanel;
    internal StackPanel TextHolderUI;
    internal TextBlock TextHolderTextUI;
    internal ListBox SearchList;
    internal StackPanel SmartBarHelp;
    internal Grid CloseHelp;
    internal ProgressBar LoadingBar;
    private bool _contentLoaded;

    public SmartBar()
    {
      this.InitializeComponent();
      SmartBar.me = this;
      TransitionService.SetNavigationInTransition((UIElement) this, App.SlideInTransistion);
      TransitionService.SetNavigationOutTransition((UIElement) this, App.SlideOutTransistion);
      this.ApplicationBar = (IApplicationBar) new Microsoft.Phone.Shell.ApplicationBar();
      this.ApplicationBar.IsVisible = true;
      ApplicationBarIconButton applicationBarIconButton = new ApplicationBarIconButton(new Uri("/Images/appbar.search.png", UriKind.Relative));
      applicationBarIconButton.Text = "search";
      applicationBarIconButton.Click += new EventHandler(this.Search_Click);
      this.ApplicationBar.Buttons.Add((object) applicationBarIconButton);
      ApplicationBarMenuItem applicationBarMenuItem = new ApplicationBarMenuItem("show help");
      applicationBarMenuItem.Click += new EventHandler(this.ShowHelp_Click);
      this.ApplicationBar.MenuItems.Add((object) applicationBarMenuItem);
      this.DataContext = (object) App.SmartBarViewModel;
      this.Loaded += new RoutedEventHandler(this.SmartBar_Loaded);
      this.CloseHelpAnmi.Completed += (EventHandler) ((sender, e) => this.SmartBarHelp.Visibility = Visibility.Collapsed);
      if (DataManager.LIGHT_THEME)
        this.SmartBarHelp.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
      PhoneApplicationService.Current.Deactivated += new EventHandler<DeactivatedEventArgs>(this.Current_Deactivated);
    }

    private void SmartBar_Loaded(object sender, RoutedEventArgs e)
    {
      this.isOpening = false;
      if (App.DataManager.SettingsMan.ShowSmartBarHelp)
      {
        this.SmartBarHelp.Visibility = Visibility.Visible;
        this.OpenHelpAnmi.Begin();
        this.ApplicationBar.IsVisible = false;
        App.DataManager.SettingsMan.ShowSmartBarHelp = false;
        this.HelpOpen = true;
      }
      else if (SmartBarViewModel.SearchResults.Count == 0)
        this.SmartBarBox.Focus();
      if (App.navService == null)
        App.navService = this.NavigationService;
      this.SmartBarBox.ItemFilter = new AutoCompleteFilterPredicate<object>(this.SmartBarAutoCompleteSearch);
    }

    private void Current_Deactivated(object sender, DeactivatedEventArgs e)
    {
      App.DataManager.SettingsMan.DeactivatedPageObjects["SmartBarBox"] = (object) this.SmartBarBox.Text;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      App.DataManager.BaconitAnalytics.LogPage("Smart Bar");
      SystemTray.IsVisible = App.DataManager.SettingsMan.ShowSystemBar;
      if (App.DataManager.SettingsMan.DeactivatedPageObjects.ContainsKey("SmartBarBox"))
      {
        this.SmartBarBox.Text = (string) App.DataManager.SettingsMan.DeactivatedPageObjects["SmartBarBox"];
        App.DataManager.SettingsMan.DeactivatedPageObjects.Remove("SmartBarBox");
      }
      if (this.RestoreProgressBar)
        this.LoadingBar.IsIndeterminate = true;
      this.RestoreProgressBar = false;
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
      if (this.LoadingBar.IsIndeterminate)
        this.RestoreProgressBar = true;
      this.LoadingBar.IsIndeterminate = false;
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
        if (!this.HelpOpen)
          return;
        this.CloseHelp_Tap((object) null, (System.Windows.Input.GestureEventArgs) null);
        e.Cancel = true;
      }
    }

    private bool SmartBarAutoCompleteSearch(string search, object value) => value != null;

    private void SmartBarBox_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
        return;
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        List<SmartBar.AutoCompleteItem> autoCompleteItemList = new List<SmartBar.AutoCompleteItem>();
        string text = this.SmartBarBox.Text;
        string str = !"/r/".StartsWith(text) || text.Length >= 4 ? (!text.StartsWith("/r/") ? "/r/" + text : text) : "/r/";
        if (!text.Contains<char>(' ') && text.LastIndexOf('/') < 3)
          autoCompleteItemList.Add(new SmartBar.AutoCompleteItem()
          {
            Text = str,
            isSearch = false,
            isSubreddit = true
          });
        autoCompleteItemList.Add(new SmartBar.AutoCompleteItem()
        {
          Text = "Search " + this.SmartBarBox.Text,
          isSearch = true,
          isSubreddit = false
        });
        this.SmartBarBox.ItemsSource = (IEnumerable) autoCompleteItemList;
      }));
    }

    private void CloseHelp_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.CloseHelpAnmi.Begin();
      this.ApplicationBar.IsVisible = true;
      this.HelpOpen = false;
      this.SmartBarBox.Focus();
    }

    private void ShowHelp_Click(object sender, EventArgs e)
    {
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        this.IsTabStop = true;
        this.Focus();
      }));
      this.HelpOpen = true;
      this.SmartBarHelp.Visibility = Visibility.Visible;
      this.OpenHelpAnmi.Begin();
      this.ApplicationBar.IsVisible = false;
    }

    private void SmartBarBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.SmartBarBox.SelectedItem == null || this.SmartBarBox.Text.Equals("/r/"))
        return;
      this.SmartBoxSubmit(this.SmartBarBox.Text);
    }

    private void SmartBoxSubmit(string text)
    {
      if (text.Equals(""))
        return;
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        this.IsTabStop = true;
        this.Focus();
      }));
      App.SmartBarViewModel.ClearResults();
      this.WasSearch = false;
      text = text.ToLower();
      string stringFromRedditUrl = DataManager.GetLocalNavStringFromRedditURL(text);
      if (stringFromRedditUrl != null && stringFromRedditUrl.Length > 0 && !stringFromRedditUrl.Contains("RedditsViewer"))
      {
        App.DataManager.BaconitAnalytics.LogEvent("Smart Bar - Reddit URL Open Directly");
        this.NavigationService.Navigate(new Uri(stringFromRedditUrl + "&FromSmartBar=true", UriKind.Relative));
      }
      else
      {
        if (text.Trim().StartsWith("/r/") || text.Contains("/r/") && text.Contains("reddit.com/"))
        {
          int num1 = text.IndexOf("/r/");
          if (num1 != -1)
          {
            int startIndex = num1 + 3;
            int num2 = text.IndexOf("/", startIndex);
            if (num2 == -1)
              num2 = text.Length;
            string str = text.Substring(startIndex, num2 - startIndex);
            this.PendingSubRedditValidation = true;
            App.DataManager.CheckIfValidSubreddit("/r/" + str + "/", true, new RunWorkerCompletedEventHandler(this.LoadSubreddit_RunWorkerCompleted));
            this.SetLoadingBar(true);
            this.SetInformation("searching for subreddit...");
            return;
          }
        }
        this.SetInformation("searching...");
        this.SetLoadingBar(true);
        App.DataManager.CheckIfValidSubreddit("/r/" + HttpUtility.UrlEncode(text) + "/", true, new RunWorkerCompletedEventHandler(this.LoadSubreddit_RunWorkerCompleted));
        App.DataManager.CheckIfValidUser(HttpUtility.UrlEncode(text), true, new RunWorkerCompletedEventHandler(this.UserSearch_CallBack));
        App.DataManager.SearchReddit(HttpUtility.UrlEncode(text), new RunWorkerCompletedEventHandler(this.Search_Callback));
        this.SearchIsGoing = true;
        this.SubredditSearchIsGoing = true;
        this.SearchFailed = false;
        this.SubredditLookupFailed = false;
        this.UserSearchGoing = true;
        this.UserSearchFailed = false;
        this.WasSearch = true;
        App.DataManager.BaconitAnalytics.LogEvent("Smart Bar - Search");
      }
    }

    public void LoadSubreddit_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Result.Equals((object) "NetworkError"))
      {
        this.SubredditSearchIsGoing = false;
        this.SubredditLookupFailed = true;
        if (this.SubredditSearchIsGoing || this.SearchIsGoing || this.UserSearchGoing)
          return;
        this.SetLoadingBar(false);
        if (this.SubredditLookupFailed && this.SearchFailed && this.UserSearchFailed)
          this.SetInformation("network error");
        if (this.WasSearch)
          return;
        this.SetInformation("network error");
      }
      else if (e.Result.Equals((object) "valid") && sender != null)
      {
        if (this.PendingSubRedditValidation)
        {
          SubReddit subreddit = (SubReddit) sender;
          this.SetLoadingBar(false);
          this.SetInformation("");
          this.PendingSubRedditValidation = false;
          this.Dispatcher.BeginInvoke((Action) (() =>
          {
            this.SmartBarBox.Text = "";
            this.NavigationService.Navigate(new Uri("/RedditsViewer.xaml?subredditURL=" + subreddit.URL, UriKind.Relative));
            App.DataManager.BaconitAnalytics.LogEvent("Smart Bar - Subreddit Shown");
          }));
        }
        else
        {
          this.SubredditSearchIsGoing = false;
          this.SubredditLookupFailed = false;
          App.SmartBarViewModel.SetSubReddit((SubReddit) sender);
          this.showSearchRes();
          if (this.SubredditSearchIsGoing || this.SearchIsGoing || this.UserSearchGoing)
            return;
          this.SetLoadingBar(false);
        }
      }
      else if (this.PendingSubRedditValidation)
      {
        this.PendingSubRedditValidation = false;
        this.SetLoadingBar(false);
        this.SetInformation("subreddit not found");
      }
      else
      {
        this.SubredditSearchIsGoing = false;
        this.SubredditLookupFailed = true;
        if (this.SubredditSearchIsGoing || this.SearchIsGoing || this.UserSearchGoing)
          return;
        this.SetLoadingBar(false);
        if (!this.SubredditLookupFailed || !this.SearchFailed || !this.UserSearchFailed)
          return;
        this.SetInformation("no results");
      }
    }

    public void Search_Callback(object subreddits, RunWorkerCompletedEventArgs e)
    {
      if (e.Cancelled)
      {
        this.SearchIsGoing = false;
        this.SearchFailed = true;
        if (this.SubredditSearchIsGoing || this.SearchIsGoing || this.UserSearchGoing)
          return;
        this.SetLoadingBar(false);
        if (!this.SubredditLookupFailed || !this.SearchFailed || !this.UserSearchFailed)
          return;
        this.SetInformation("no results");
      }
      else if ((bool) e.Result && subreddits != null)
      {
        this.SearchIsGoing = false;
        this.SearchFailed = false;
        App.SmartBarViewModel.FormatAndSetResults((List<SubRedditData>) subreddits);
        this.showSearchRes();
        if (this.SubredditSearchIsGoing || this.SearchIsGoing || this.UserSearchGoing)
          return;
        this.SetLoadingBar(false);
      }
      else
      {
        this.SearchIsGoing = false;
        this.SearchFailed = true;
        if (this.SubredditSearchIsGoing || this.SearchIsGoing || this.UserSearchGoing)
          return;
        this.SetLoadingBar(false);
        if (!this.SubredditLookupFailed || !this.SearchFailed || !this.UserSearchFailed)
          return;
        this.SetInformation("no results");
      }
    }

    public void UserSearch_CallBack(object user, RunWorkerCompletedEventArgs e)
    {
      if (e.Cancelled)
      {
        this.UserSearchGoing = false;
        this.UserSearchFailed = true;
        if (this.SubredditSearchIsGoing || this.SearchIsGoing || this.UserSearchGoing)
          return;
        this.SetLoadingBar(false);
        if (!this.SubredditLookupFailed || !this.SearchFailed || !this.UserSearchFailed)
          return;
        this.SetInformation("no results");
      }
      else if (((string) e.Result).Equals("valid") && user != null)
      {
        this.UserSearchGoing = false;
        this.UserSearchFailed = false;
        App.SmartBarViewModel.SetUser((UserAccountInformation) user);
        this.showSearchRes();
        if (this.SubredditSearchIsGoing || this.SearchIsGoing || this.UserSearchGoing)
          return;
        this.SetLoadingBar(false);
      }
      else
      {
        this.UserSearchGoing = false;
        this.UserSearchFailed = true;
        if (this.SubredditSearchIsGoing || this.SearchIsGoing || this.UserSearchGoing)
          return;
        this.SetLoadingBar(false);
        if (!this.SubredditLookupFailed || !this.SearchFailed || !this.UserSearchFailed)
          return;
        this.SetInformation("no results");
      }
    }

    public void SetLoadingBar(bool enabled)
    {
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        if (enabled)
        {
          this.LoadingBar.Visibility = Visibility.Visible;
          this.LoadingBar.IsIndeterminate = true;
        }
        else
        {
          this.LoadingBar.Visibility = Visibility.Collapsed;
          this.LoadingBar.IsIndeterminate = false;
        }
      }));
    }

    public void SetInformation(string one)
    {
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        this.TextHolderTextUI.Text = one == null || one.Equals("") ? "enter a search term" : one;
        this.TextHolderUI.Visibility = Visibility.Visible;
        this.SearchList.Visibility = Visibility.Collapsed;
      }));
    }

    public void showSearchRes()
    {
      this.Dispatcher.BeginInvoke((Action) (() =>
      {
        this.TextHolderUI.Visibility = Visibility.Collapsed;
        this.SearchList.Visibility = Visibility.Visible;
      }));
    }

    private void Search_Click(object sender, EventArgs e)
    {
      this.SmartBoxSubmit(this.SmartBarBox.Text);
    }

    private void SearchList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.SearchList.SelectedIndex == -1)
        return;
      SmartBarListItem selectedItem = (SmartBarListItem) this.SearchList.SelectedItem;
      if (selectedItem.ShowSubReddit == Visibility.Visible)
      {
        if (selectedItem.subreddit != null)
        {
          this.NavigationService.Navigate(new Uri("/RedditsViewer.xaml?subredditURL=" + selectedItem.subreddit.URL, UriKind.Relative));
          App.DataManager.BaconitAnalytics.LogEvent("Smart Bar - Subreddit Shown");
        }
        else
        {
          this.NavigationService.Navigate(new Uri("/ProfileView.xaml?userName=" + selectedItem.UserName, UriKind.Relative));
          App.DataManager.BaconitAnalytics.LogEvent("Smart Bar - User Shown");
        }
      }
      else if (selectedItem.ShowSearchResult == Visibility.Visible && selectedItem.storyName != null)
      {
        App.ActiveStoryData = (SubRedditData) null;
        this.NavigationService.Navigate(new Uri("/StoryDetails.xaml?StoryDataRedditID=" + selectedItem.storyName + "&FromSmartBar=true", UriKind.Relative));
        App.DataManager.BaconitAnalytics.LogEvent("Smart Bar - Search Story Shown");
      }
      this.SearchList.SelectedIndex = -1;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/SmartBar.xaml", UriKind.Relative));
      this.CloseHelpAnmi = (Storyboard) this.FindName("CloseHelpAnmi");
      this.OpenHelpAnmi = (Storyboard) this.FindName("OpenHelpAnmi");
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TitlePanel = (StackPanel) this.FindName("TitlePanel");
      this.ApplicationTitle = (TextBlock) this.FindName("ApplicationTitle");
      this.SmartBarBox = (AutoCompleteBox) this.FindName("SmartBarBox");
      this.ContentPanel = (Grid) this.FindName("ContentPanel");
      this.TextHolderUI = (StackPanel) this.FindName("TextHolderUI");
      this.TextHolderTextUI = (TextBlock) this.FindName("TextHolderTextUI");
      this.SearchList = (ListBox) this.FindName("SearchList");
      this.SmartBarHelp = (StackPanel) this.FindName("SmartBarHelp");
      this.CloseHelp = (Grid) this.FindName("CloseHelp");
      this.LoadingBar = (ProgressBar) this.FindName("LoadingBar");
    }

    public class AutoCompleteItem
    {
      public string Text { get; set; }

      public bool isSearch { get; set; }

      public bool isSubreddit { get; set; }

      public override string ToString()
      {
        return this.isSearch && this.Text.Length > 7 ? this.Text.Substring(7) : this.Text;
      }
    }
  }
}
