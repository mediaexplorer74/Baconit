using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Web;
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
    public sealed partial class LoginPage : Page
    {
        private bool userHasText;
        private bool passHasText;
        private bool isOpening = true;
        private string user;
        private string password;

        /*internal Grid LayoutRoot;
        internal StackPanel TitlePanel;
        internal TextBlock PageTitle;
        internal Grid ContentPanel;
        internal TextBox userBox;
        internal PasswordBox passwordBox;
        internal Button LoginButton;
        private bool _contentLoaded;*/

       
        public LoginPage()
        {
            this.InitializeComponent();
            //TransitionService.SetNavigationInTransition((UIElement)this, App.SlideInTransistion);
            //TransitionService.SetNavigationOutTransition((UIElement)this, App.SlideOutTransistion);
            //PhoneApplicationService.Current.Deactivated += new EventHandler<DeactivatedEventArgs>(this.Current_Deactivated);
            //this.Loaded += (RoutedEventHandler)((sender, e) => this.isOpening = false);
        }

        private void Current_Deactivated(object sender, EventArgs e)
        {
            App.DataManager.SettingsMan.DeactivatedPageObjects["LoginUserBox"] = this.userBox.Text;
            App.DataManager.SettingsMan.DeactivatedPageObjects["LoginPasswordBox"] = this.passwordBox.Password;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            App.DataManager.BaconitAnalytics.LogPage("Log In");
            if (string.IsNullOrWhiteSpace(userBox.Text) && string.IsNullOrWhiteSpace(this.passwordBox.Password))
                this.LoginButton.IsEnabled = false;
            else
                this.LoginButton.IsEnabled = true;
            if (App.DataManager.SettingsMan.DeactivatedPageObjects.ContainsKey("LoginUserBox"))
            {
                this.userBox.Text = (string)App.DataManager.SettingsMan.DeactivatedPageObjects["LoginUserBox"];
                this.passwordBox.Password = (string)App.DataManager.SettingsMan.DeactivatedPageObjects["LoginPasswordBox"];
                App.DataManager.SettingsMan.DeactivatedPageObjects.Remove("LoginUserBox");
                this.LoginButton.IsEnabled = true;
            }
            if (App.navService != null)
                return;
            App.navService = this.NavigationService;
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (this.isOpening)
                e.Cancel = true;
            else
                base.OnBackKeyPress(e);
        }

        private void LogIn_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            this.IsTabStop = true;
            this.Focus();
            /*
            SystemTray.SetProgressIndicator((DependencyObject)this, new ProgressIndicator()
            {
                IsVisible = true,
                IsIndeterminate = true,
                Text = "Logging in..."
            });
            */
            this.userBox.IsEnabled = false;
            this.passwordBox.IsEnabled = false;
            this.LoginButton.IsEnabled = false;
            this.user = this.userBox.Text;
            this.password = this.passwordBox.Password;
            ThreadPool.QueueUserWorkItem((WaitCallback)(obj => this.Login()));
        }

        public void Login()
        {
            App.DataManager.SignIn(this.user, this.password, 
                new RunWorkerCompletedEventHandler(this.loginComplete_RunWorkerCompleted));
        }

        private void loginComplete_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result.Equals((object)"success"))
            {
                App.DataManager.BaconitAnalytics.LogEvent("User Logged In");
                if (App.DataManager.SettingsMan.ShowBaconSyncSiginMessage)
                    Deployment.Current.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        Guide.BeginShowMessageBox("Try BaconSync?", 
                            "BaconSync is a free service that synchronizes your reddit visited links between all of your devices and also allows you to send web pages directly to your phone. BaconSync can be found in the app settings.", (IEnumerable<string>)new string[2]
                        {
              "learn more",
              "later"
                        }, 0, MessageBoxIcon.None, (AsyncCallback)(resul =>
                        {
                            int? nullable = Guide.EndShowMessageBox(resul);
                            if (!nullable.HasValue)
                                return;
                            switch (nullable.GetValueOrDefault())
                            {
                                case 0:
                                    this.Dispatcher.BeginInvoke((Action)(() =>
                                    {
                                        try
                                        {
                                            this.NavigationService.Navigate(
                                                new Uri("/SettingPages/BaconSync/BaconSyncLanding.xaml?RemoveBack=true", 
                                                UriKind.Relative));
                                        }
                                        catch
                                        {
                                        }
                                    }));
                                    break;
                                case 1:
                                    Deployment.Current.Dispatcher.BeginInvoke((Action)(() =>
                                    {
                                        try
                                        {
                                            this.NavigationService.GoBack();
                                        }
                                        catch
                                        {
                                        }
                                    }));
                                    break;
                            }
                        }), (object)null);
                        App.DataManager.SettingsMan.ShowBaconSyncSiginMessage = false;
                    }));
                else
                    this.Dispatcher.BeginInvoke((Action)(() => this.NavigationService.GoBack()));
            }
            else
                this.Dispatcher.BeginInvoke((Action)(() =>
                {
                    SystemTray.SetProgressIndicator((DependencyObject)this, new ProgressIndicator()
                    {
                        IsVisible = false,
                        IsIndeterminate = false
                    });
                    this.userBox.IsEnabled = true;
                    this.passwordBox.IsEnabled = true;
                    this.LoginButton.IsEnabled = true;
                    if (e.Result.ToString().Equals("invalid password"))
                    {
                        int num1 = (int)MessageBox.Show("Invalid user name or password, try again.", 
                            "Signin Error", MessageBoxButton.OK);
                    }
                    else if (e.Result.ToString().Contains("you are doing that too much."))
                    {
                        string str = e.Result.ToString();
                        if (str.Length > 0)
                            str = (str[0].ToString() + string.Empty).ToUpper() + str.Substring(1);
                        int num2 = (int)MessageBox.Show(str.Replace(". ", ", "), "Error", MessageBoxButton.OK);
                    }
                    else
                    {
                        int num3 = (int)MessageBox.Show("Reddit an unknown returned an error.\n\nError: " 
                            + e.Result, 
                            "Error",
                            MessageBoxButton.OK);
                    }
                }));
        }

        private void passwordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;
            this.LogIn_Tap((object)null, (System.Windows.Input.GestureEventArgs)null);
        }

        private void userBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.userHasText = this.userBox.Text.Length > 0;
            if (this.passHasText && this.userHasText)
                this.LoginButton.IsEnabled = true;
            else
                this.LoginButton.IsEnabled = false;
        }

        private void NeedAccount_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            if (!this.userBox.IsEnabled)
                return;
            this.NavigationService.Navigate(new Uri("/InAppWebBrowser.xaml?Url=" 
                + HttpUtility.UrlEncode("https://www.reddit.com/register.compact"), UriKind.Relative));
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.passHasText = this.passwordBox.Password.Length > 0;
            if (this.passHasText && this.userHasText)
                this.LoginButton.IsEnabled = true;
            else
                this.LoginButton.IsEnabled = false;
        }

       
    }
}
