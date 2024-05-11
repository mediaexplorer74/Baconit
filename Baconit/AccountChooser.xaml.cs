using BaconitData.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
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
    public sealed partial class AccountChooser : Page
    {
        public bool isOpen = true;
        public static bool AccountsChanged;
        public static AccountChooserViewModel ViewModel;
        private bool isOpening = true;
        private bool showProgressBar;


        public AccountChooser()
        {
            this.InitializeComponent();
            if (AccountChooser.ViewModel == null)
                AccountChooser.ViewModel = new AccountChooserViewModel();
            this.DataContext = (object)AccountChooser.ViewModel;
            //TransitionService.SetNavigationInTransition((UIElement)this, App.FeatherInTransition);
            //TransitionService.SetNavigationOutTransition((UIElement)this, App.FeatherOutTransition);
            this.ApplicationBar = (IApplicationBar)new Microsoft.Phone.Shell.ApplicationBar();
            this.ApplicationBar.IsVisible = true;
            ApplicationBarIconButton applicationBarIconButton = new ApplicationBarIconButton(new Uri("/Images/appbar.add.png", UriKind.Relative));
            applicationBarIconButton.Text = "add";
            applicationBarIconButton.Click += new EventHandler(this.AddAccount_Click);
            this.ApplicationBar.Buttons.Add((object)applicationBarIconButton);
            ApplicationBarMenuItem applicationBarMenuItem = new ApplicationBarMenuItem("log out all accounts");
            applicationBarMenuItem.Click += new EventHandler(this.AppBarItemLogOutAll_Click);
            this.ApplicationBar.MenuItems.Add((object)applicationBarMenuItem);
            this.Loaded += new RoutedEventHandler(this.AccountChooser_Loaded);
            App.DataManager.AccountChanged += new DataManager.AccountChangedEvent(this.AccountUpdated);
        }

        private void AccountChooser_Loaded(object sender, RoutedEventArgs e)
        {
            List<RedditAccount> userAccounts = App.DataManager.SettingsMan.UserAccounts;
            this.isOpening = false;
            bool flag = false;
            foreach (RedditAccount redditAccount in userAccounts)
            {
                if (redditAccount.UserName.ToLower().Equals(App.DataManager.SettingsMan.UserName.ToLower()))
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                userAccounts.Add(new RedditAccount(App.DataManager.SettingsMan.UserName, DateTime.Now, App.DataManager.SettingsMan.ModHash, App.DataManager.SettingsMan.UserCookie));
                App.DataManager.SettingsMan.UserAccounts = userAccounts;
                App.DataManager.SettingsMan.SaveAccounts();
            }
            AccountChooser.ViewModel.setAccounts(userAccounts);
            AccountChooser.AccountsChanged = false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            App.DataManager.BaconitAnalytics.LogPage("Account Chooser");
            this.isOpen = true;
            if (AccountChooser.AccountsChanged)
            {
                List<RedditAccount> userAccounts = App.DataManager.SettingsMan.UserAccounts;
                AccountChooser.ViewModel.setAccounts(userAccounts);
            }
            if (this.showProgressBar)
                this.LoggingOutProgress.IsIndeterminate = true;
            this.showProgressBar = false;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.isOpen = false;
            if (this.LoggingOutProgress.IsIndeterminate)
                this.showProgressBar = true;
            this.LoggingOutProgress.IsIndeterminate = false;
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (this.isOpening)
                e.Cancel = true;
            else
                base.OnBackKeyPress(e);
        }

        private void AccountListUI_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.AccountListUI.SelectedIndex != -1)
            {
                RedditAccount account = AccountChooser.ViewModel.getAccount((AccountChooserViewModel.RedditAccountUI)this.AccountListUI.SelectedItem);
                if (account != null || account.UserName.Equals("") || account.Cookie.Equals("") || account.ModHas.Equals(""))
                {
                    if (!App.DataManager.SettingsMan.UserName.ToLower().Equals(account.UserName.ToLower()))
                        App.DataManager.SwitchUserAccount(account);
                    try
                    {
                        this.NavigationService.GoBack();
                    }
                    catch
                    {
                    }
                }
                else
                {
                    int num = (int)MessageBox.Show("This account data is currpt, please remove the account, restart Baconit, and add the account again.", "Error", MessageBoxButton.OK);
                }
            }
            this.AccountListUI.SelectedIndex = -1;
        }

        private void AddAccount_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            AccountChooserViewModel.RedditAccountUI dataContext = (sender as MenuItem).DataContext as AccountChooserViewModel.RedditAccountUI;
            if (App.DataManager.SettingsMan.UserName.ToLower().Equals(dataContext.UserName.ToLower()))
            {
                int num = (int)MessageBox.Show("The active account can't be removed. If you wish to log out completely, look for the option in the application bar.", "Can't Remove Active Account", MessageBoxButton.OK);
            }
            else
            {
                int index = 0;
                while (index < App.DataManager.SettingsMan.UserAccounts.Count && !App.DataManager.SettingsMan.UserAccounts[index].UserName.ToLower().Equals(dataContext.UserName.ToLower()))
                    ++index;
                if (App.DataManager.SettingsMan.UserAccounts.Count != index)
                {
                    App.DataManager.SettingsMan.UserAccounts.RemoveAt(index);
                    App.DataManager.SettingsMan.SaveAccounts();
                }
                AccountChooser.ViewModel.setAccounts(App.DataManager.SettingsMan.UserAccounts);
            }
        }

        private void AppBarItemLogOutAll_Click(object sender, EventArgs e)
        {
            Guide.BeginShowMessageBox("Log Out", "Are you sure you want to log out all of your accounts?", (IEnumerable<string>)new string[2]
            {
        "yes",
        "no"
            }, 0, MessageBoxIcon.None, (AsyncCallback)(resul =>
            {
                int? nullable = Guide.EndShowMessageBox(resul);
                if (!nullable.HasValue)
                    return;
                if (nullable.GetValueOrDefault() == 0)
                    this.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        this.FullScreenLoading.Visibility = Visibility.Visible;
                        this.LoggingOutProgress.IsIndeterminate = true;
                        this.OpenLoggingOutOverLay.Begin();
                        this.ApplicationBar.IsVisible = false;
                        ThreadPool.QueueUserWorkItem((WaitCallback)(obj => App.DataManager.Logout()));
                    }));
            }), (object)null);
        }

        public void AccountUpdated(bool added, bool logout)
        {
            if (!logout)
                return;
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                this.FullScreenLoading.Visibility = Visibility.Collapsed;
                this.LoggingOutProgress.IsIndeterminate = false;
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
}

