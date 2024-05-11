// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebAuthenticationBroker.cs" company="In The Hand Ltd">
//   Copyright (c) 2019 In The Hand Ltd, All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.Web.Http;

namespace InTheHand.Security.Authentication.Web
{
    /// <summary>
    /// Starts the authentication operation. You can call the methods of this class multiple times in a single application or across multiple applications at the same time.
    /// Replaces the UWP equivalent and uses the Edge browser for authentication with apps like GitHub which displays a warning running on Internet Explorer.
    /// </summary>
    public sealed class WebAuthenticationBroker
    {
        private static Uri redirectUri;
        private static ContentDialog dialog;
        private static string code = string.Empty;
        private static uint errorCode = 0;
        public static bool Flag = true;
        static WebView webView;

        public static Task<WebAuthenticationResult> AuthenticateAsync(WebAuthenticationOptions options, 
            Uri requestUri)
        {
            return AuthenticateAsync(options, requestUri, 
                Windows.Security.Authentication.Web.WebAuthenticationBroker.GetCurrentApplicationCallbackUri());
        }

        public static async Task<WebAuthenticationResult> AuthenticateAsync(WebAuthenticationOptions options, 
            Uri requestUri, Uri callbackUri)
        {
            if (options != WebAuthenticationOptions.None)
                throw new ArgumentException("WebAuthenticationOptions.None only!", "options");

            redirectUri = callbackUri;
            dialog = new ContentDialog();

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition()
            { 
                Height = Windows.UI.Xaml.GridLength.Auto })
                ;
            grid.RowDefinitions.Add(new RowDefinition()
            { 
                Height = new Windows.UI.Xaml.GridLength(1, Windows.UI.Xaml.GridUnitType.Star) }
            );

            var label = new TextBlock();
            label.Text = "Connect to a service";
            label.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left;
            label.Margin = new Windows.UI.Xaml.Thickness(0);
            grid.Children.Add(label);

            var closeButton = new Button();
            closeButton.Content = "";
            closeButton.FontFamily = new FontFamily("Segoe UI Symbol");
            closeButton.BorderBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 0, 0, 0));
            closeButton.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(0, 0, 0, 0));
            closeButton.Margin = new Windows.UI.Xaml.Thickness(0);
            closeButton.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Right;
            
            closeButton.Click += (s, e) => 
            { 
                dialog.Hide(); 
            };
            grid.Children.Add(closeButton);

            webView = new WebView(WebViewExecutionMode.SameThread) 
            { 
                Source = /*new Uri("http://sample.com", UriKind.Absolute)*/requestUri 
            };

           
            webView.AllowFocusOnInteraction = true;
            webView.SetValue(Grid.RowProperty, 1);
            webView.NavigationStarting += WebView_NavigationStarting;
            webView.NavigationFailed += WebView_NavigationFailed;
            webView.MinWidth = 768;// 360;//480;
            webView.MinHeight = 1024;// 640;//600;*/
            
            grid.Children.Add(webView);

            dialog.Content = grid;


            //Experimental
            if (1==0)//(Flag)
            {
                Flag = false;

                var requestMsg = new Windows.Web.Http.HttpRequestMessage(HttpMethod.Get, requestUri);
                requestMsg.Headers.Add
                (
                "User-Agent",
                "Mozilla/5.0 (Android 14; Mobile; rv:123.0) Gecko/123.0 Firefox/123.0"//"Mozilla/5.0 (Linux; arm_64; Android 14; SM-G965F) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.6261.119 YaBrowser/21.3.4.59 Mobile Safari/537.36"
                );
                webView.NavigateWithHttpRequestMessage(requestMsg);               
            }



            dialog.GotFocus += (s, e) => 
            { 
                webView.Focus(Windows.UI.Xaml.FocusState.Programmatic); 
            };

            var res = await dialog.ShowAsync();

            return new WebAuthenticationResult(code, errorCode, errorCode > 0
                ? WebAuthenticationStatus.ErrorHttp : string.IsNullOrEmpty(code) 
                ? WebAuthenticationStatus.UserCancel : WebAuthenticationStatus.Success);
        }

        private static void WebView_NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            errorCode = (uint)e.WebErrorStatus;
            dialog.Hide();
        }

        private static void WebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if (1==0)//(Flag)
            {
                Flag = false;
               
                var requestMsg = new Windows.Web.Http.HttpRequestMessage(HttpMethod.Get, args.Uri);
               
                //requestMsg.Headers.Add("User-Agent",
                //    "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.54 Safari/537.36");
               
                requestMsg.Headers.Add("User-Agent",
                   "Mozilla/5.0 (Linux; arm_64; Android 14; SM-G965F) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.6261.119 YaBrowser/21.3.4.59 Mobile Safari/537.36");
               
                webView.NavigateWithHttpRequestMessage(requestMsg);
                
            }

            if (args.Uri.ToString().StartsWith(redirectUri.ToString()))
            {
                var querySegs = args.Uri.Query.Substring(1).Split('&');
                foreach (string seg in querySegs)
                {
                    if (seg.StartsWith("code="))
                    {
                        code = args.Uri.ToString();
                        break;
                    }
                }

                args.Cancel = true;
                dialog.Hide();
            }
        }
    }
}
