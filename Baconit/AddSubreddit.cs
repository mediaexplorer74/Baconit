// Decompiled with JetBrains decompiler
// Type: Baconit.AddSubreddit
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Baconit.Database;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

#nullable disable
namespace Baconit
{
  public class AddSubreddit : PhoneApplicationPage
  {
    private bool ListShown;
    private bool isLoaded;
    private bool SubredditViewIsOpen;
    private SubReddit SubredditView;
    internal Storyboard ShowList;
    internal Storyboard ShowSubredditView;
    internal Storyboard HideSubredditView;
    internal Grid LayoutRoot;
    internal StackPanel TitlePanel;
    internal TextBlock ApplicationTitle;
    internal TextBox SearchQuery;
    internal ListBox AddList;
    internal StackPanel TextHolderUI;
    internal Grid BlackBackground;
    internal Grid SubredditOverlay;
    internal TextBlock SubredditViewName;
    internal TextBlock SubredditViewSubs;
    internal ScrollViewer SubredditScoller;
    internal SuperRichTextBox SubredditDesciption;
    internal Grid AccountButton;
    internal TextBlock AccountText;
    internal Grid LocalButton;
    internal TextBlock LocalText;
    private bool _contentLoaded;

    public ObservableCollection<SubredditUI> SubredditUIList { get; private set; }

    public AddSubreddit()
    {
      this.InitializeComponent();
      this.SubredditUIList = new ObservableCollection<SubredditUI>();
      this.Loaded += new RoutedEventHandler(this.AddSubreddit_Loaded);
      this.DataContext = (object) this;
      PhoneApplicationService.Current.Deactivated += new EventHandler<DeactivatedEventArgs>(this.Current_Deactivated);
      TransitionService.SetNavigationInTransition((UIElement) this, App.SlideInTransistion);
      TransitionService.SetNavigationOutTransition((UIElement) this, App.SlideOutTransistion);
      this.HideSubredditView.Completed += (EventHandler) delegate
      {
        this.BlackBackground.Visibility = Visibility.Collapsed;
        this.SubredditOverlay.Visibility = Visibility.Collapsed;
      };
      this.ApplicationBar = (IApplicationBar) new Microsoft.Phone.Shell.ApplicationBar();
      this.ApplicationBar.IsVisible = true;
      ApplicationBarIconButton applicationBarIconButton = new ApplicationBarIconButton(new Uri("/Images/appbar.search.png", UriKind.Relative));
      applicationBarIconButton.Text = "search";
      applicationBarIconButton.Click += new EventHandler(this.Search_Click);
      this.ApplicationBar.Buttons.Add((object) applicationBarIconButton);
      if (!DataManager.LIGHT_THEME)
        return;
      this.BlackBackground.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue));
    }

    private void AddSubreddit_Loaded(object sender, RoutedEventArgs e)
    {
      if (this.isLoaded)
        return;
      this.isLoaded = true;
      this.SearchQuery.Focus();
    }

    private void Current_Deactivated(object sender, DeactivatedEventArgs e)
    {
      App.DataManager.SettingsMan.DeactivatedPageObjects["AddSubSearchQuery"] = (object) this.SearchQuery.Text;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      App.DataManager.BaconitAnalytics.LogPage("Add Subreddit");
      if (!App.DataManager.SettingsMan.DeactivatedPageObjects.ContainsKey("AddSubSearchQuery"))
        return;
      this.SearchQuery.Text = (string) App.DataManager.SettingsMan.DeactivatedPageObjects["AddSubSearchQuery"];
      App.DataManager.SettingsMan.DeactivatedPageObjects.Remove("AddSubSearchQuery");
    }

    protected override void OnBackKeyPress(CancelEventArgs e)
    {
      if (!this.SubredditViewIsOpen)
        return;
      this.CloseSubredditView();
      e.Cancel = true;
    }

    private void AddList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.AddList.SelectedIndex != -1)
        this.OpenSubredditView(this.SubredditUIList[this.AddList.SelectedIndex].subreddit);
      this.AddList.SelectedIndex = -1;
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
    }

    private void SearchQuery_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Enter)
        return;
      this.Search();
    }

    private void Search_Click(object sender, EventArgs e) => this.Search();

    private void Search()
    {
      string query = this.SearchQuery.Text;
      ThreadPool.QueueUserWorkItem((WaitCallback) (obj => App.DataManager.SubredditDataManager.SearchForSubreddits(query, new RunWorkerCompletedEventHandler(this.SearchReturnHandler))));
      SystemTray.SetProgressIndicator((DependencyObject) this, new ProgressIndicator()
      {
        IsIndeterminate = true,
        IsVisible = true,
        Text = "Searching..."
      });
      this.Focus();
      App.DataManager.BaconitAnalytics.LogEvent("Add Subreddit Search");
    }

    public void SearchReturnHandler(object sender, RunWorkerCompletedEventArgs e)
    {
      if ((bool) e.Result && sender != null)
      {
        List<SubReddit> subRedditList = (List<SubReddit>) sender;
        List<SubredditUI> UIList = new List<SubredditUI>();
        if (subRedditList.Count == 0)
        {
          this.Dispatcher.BeginInvoke((Action) (() =>
          {
            this.AddList.Visibility = Visibility.Collapsed;
            this.TextHolderUI.Visibility = Visibility.Visible;
          }));
        }
        else
        {
          foreach (SubReddit subReddit in subRedditList)
          {
            SubredditUI subredditUi1 = new SubredditUI();
            subredditUi1.SubredditTitle = subReddit.DisplayName;
            subredditUi1.SecondLineText = DataManager.SimpleFormatString(subReddit.PublicDescription);
            if (subredditUi1.SecondLineText == null || subredditUi1.SecondLineText.Length < 5)
              subredditUi1.SecondLineText = DataManager.SimpleFormatString(subReddit.Description);
            if (subredditUi1.SecondLineText != null)
            {
              SubredditUI subredditUi2 = subredditUi1;
              subredditUi2.SecondLineText = subredditUi2.SecondLineText.Replace('\n', ' ');
            }
            SubredditUI subredditUi3 = subredditUi1;
            subredditUi3.ThirdLine = subredditUi3.ThirdLine + string.Format("{0:#,###0}", (object) subReddit.Subscribers) + " subscribers";
            subredditUi1.subreddit = subReddit;
            UIList.Add(subredditUi1);
          }
          this.Dispatcher.BeginInvoke((Action) (() =>
          {
            this.SubredditUIList.Clear();
            foreach (SubredditUI subredditUi in UIList)
              this.SubredditUIList.Add(subredditUi);
            this.AddList.Visibility = Visibility.Visible;
            this.TextHolderUI.Visibility = Visibility.Collapsed;
            if (this.ListShown)
              return;
            this.ListShown = true;
            this.ShowList.Begin();
          }));
        }
      }
      this.Dispatcher.BeginInvoke((Action) (() => SystemTray.SetProgressIndicator((DependencyObject) this, new ProgressIndicator()
      {
        IsVisible = false
      })));
    }

    public void OpenSubredditView(SubReddit openTo)
    {
      if (openTo == null)
        return;
      this.SubredditViewIsOpen = true;
      this.SubredditView = openTo;
      this.SubredditViewName.Text = openTo.DisplayName;
      this.SubredditDesciption.ClearText();
      this.SubredditScoller.ScrollToVerticalOffset(0.0);
      if (openTo.Description != null && openTo.Description.Length != 0)
        this.SubredditDesciption.SetText(openTo.Description);
      else if (openTo.PublicDescription != null)
        this.SubredditDesciption.SetText(openTo.PublicDescription);
      this.SubredditViewSubs.Text = openTo.Subscribers.ToString() + " subscribers";
      if (!App.DataManager.SubredditDataManager.GetSubRedditRName(this.SubredditView.URL).Equals(""))
        this.SubredditView.isAccount = true;
      if (this.SubredditView.isLocal || this.SubredditView.isAccount)
      {
        SolidColorBrush solidColorBrush1 = new SolidColorBrush(Color.FromArgb((byte) 150, (byte) 0, (byte) 0, (byte) 0));
        SolidColorBrush solidColorBrush2 = new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 100, (byte) 100, (byte) 100));
        this.AccountButton.Background = (Brush) solidColorBrush2;
        this.AccountText.Foreground = (Brush) solidColorBrush1;
        this.LocalButton.Background = (Brush) solidColorBrush2;
        this.LocalText.Foreground = (Brush) solidColorBrush1;
      }
      if (!App.DataManager.SettingsMan.IsSignedIn)
      {
        SolidColorBrush solidColorBrush = new SolidColorBrush(Color.FromArgb((byte) 150, (byte) 0, (byte) 0, (byte) 0));
        this.AccountButton.Background = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 100, (byte) 100, (byte) 100));
        this.AccountText.Foreground = (Brush) solidColorBrush;
      }
      this.ApplicationBar.IsVisible = false;
      this.BlackBackground.Visibility = Visibility.Visible;
      this.SubredditOverlay.Visibility = Visibility.Visible;
      this.ShowSubredditView.Begin();
    }

    public void CloseSubredditView()
    {
      this.SubredditViewIsOpen = false;
      this.HideSubredditView.Begin();
      this.ApplicationBar.IsVisible = true;
    }

    private void SubAccount_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      if (this.SubredditView.isAccount || this.SubredditView.isLocal || !App.DataManager.SettingsMan.IsSignedIn)
        return;
      this.SubredditView.isAccount = true;
      App.DataManager.SubredditDataManager.SubscribeToSubReddit(this.SubredditView, true, true, false);
      this.CloseSubredditView();
    }

    private void SubLocal_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      if (this.SubredditView.isLocal || this.SubredditView.isAccount)
        return;
      this.SubredditView.isLocal = true;
      App.DataManager.SubredditDataManager.AddToLocalSubReddit(this.SubredditView, true);
      this.CloseSubredditView();
    }

    private void ViewSubreddit_Tap(object sender, System.Windows.Input.GestureEventArgs e)
    {
      this.NavigationService.Navigate(new Uri("/RedditsViewer.xaml?subredditURL=" + this.SubredditView.URL, UriKind.Relative));
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/AddSubreddit.xaml", UriKind.Relative));
      this.ShowList = (Storyboard) this.FindName("ShowList");
      this.ShowSubredditView = (Storyboard) this.FindName("ShowSubredditView");
      this.HideSubredditView = (Storyboard) this.FindName("HideSubredditView");
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TitlePanel = (StackPanel) this.FindName("TitlePanel");
      this.ApplicationTitle = (TextBlock) this.FindName("ApplicationTitle");
      this.SearchQuery = (TextBox) this.FindName("SearchQuery");
      this.AddList = (ListBox) this.FindName("AddList");
      this.TextHolderUI = (StackPanel) this.FindName("TextHolderUI");
      this.BlackBackground = (Grid) this.FindName("BlackBackground");
      this.SubredditOverlay = (Grid) this.FindName("SubredditOverlay");
      this.SubredditViewName = (TextBlock) this.FindName("SubredditViewName");
      this.SubredditViewSubs = (TextBlock) this.FindName("SubredditViewSubs");
      this.SubredditScoller = (ScrollViewer) this.FindName("SubredditScoller");
      this.SubredditDesciption = (SuperRichTextBox) this.FindName("SubredditDesciption");
      this.AccountButton = (Grid) this.FindName("AccountButton");
      this.AccountText = (TextBlock) this.FindName("AccountText");
      this.LocalButton = (Grid) this.FindName("LocalButton");
      this.LocalText = (TextBlock) this.FindName("LocalText");
    }
  }
}
