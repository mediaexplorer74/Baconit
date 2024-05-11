﻿using BaconBackend.DataObjects;
using Baconit.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace Baconit.FlipViewControls
{
    public sealed partial class WebPageFlipControl : UserControl, IFlipViewContentControl
    {
        /// <summary>
        /// Reference to the host
        /// </summary>
        IFlipViewContentHost m_host;

        /// <summary>
        /// Holds a reference to the webview.
        /// </summary>
        WebView m_webView;

        /// <summary>
        /// Indicates if we have called hide or not.
        /// </summary>
        bool m_loadingHidden;

        /// <summary>
        /// Indicates if we should be destoryed
        /// </summary>
        bool m_isDestroyed = false;

        public WebPageFlipControl(IFlipViewContentHost host)
        {
            this.InitializeComponent();
            m_host = host;
        }

        /// <summary>
        /// Called by the host when it queries if we can handle a post.
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        static public bool CanHandlePost(Post post)
        {
            // Web view is the fall back, we should be handle just about anything.
            return true;
        }

        /// <summary>
        /// Called by the host when we should show content.
        /// </summary>
        /// <param name="post"></param>
        public async void OnPrepareContent(Post post)
        {
            // So the loading UI
            m_host.ShowLoading();
            m_loadingHidden = false;

            // Since some of this can be costly, delay the work load until we aren't animating.
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                lock(this)
                {
                    if(m_isDestroyed)
                    {
                        return;
                    }

                    // Make the webview
                    m_webView = new WebView(WebViewExecutionMode.SeparateThread);

                    // Setup the listeners, we need all of these because some web pages don't trigger
                    // some of them.
                    m_webView.FrameNavigationCompleted += NavigationCompleted;
                    m_webView.NavigationFailed += NavigationFailed;
                    m_webView.DOMContentLoaded += DOMContentLoaded;
                    m_webView.ContentLoading += ContentLoading;

                    // Navigate
                    m_webView.Navigate(new Uri(post.Url, UriKind.Absolute));
                    ui_contentRoot.Children.Add(m_webView);
                }
            });
        }

        /// <summary>
        /// Called when the  post actually becomes visible
        /// </summary>
        public void OnVisible()
        {
            // Ignore for now
        }

        public void OnDestroyContent()
        {
            lock(this)
            {
                m_isDestroyed = true;

                if (m_webView != null)
                {

                    // Clear handlers
                    m_webView.FrameNavigationCompleted -= NavigationCompleted;
                    m_webView.NavigationFailed -= NavigationFailed;
                    m_webView.DOMContentLoaded -= DOMContentLoaded;
                    m_webView.ContentLoading -= ContentLoading;

                    // Clear the webview
                    m_webView.NavigateToString("");
                }
                m_webView = null;
            }

            // Clear the UI
            ui_contentRoot.Children.Clear();
        }


        private void DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            HideLoading();
        }

        private void ContentLoading(WebView sender, WebViewContentLoadingEventArgs args)
        {
            HideLoading();
        }

        private void NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            HideLoading();
        }
        private void NavigationFailed(object sender, WebViewNavigationFailedEventArgs e)
        {
            m_host.ShowError();
        }

        /// <summary>
        /// Calls hide loading on the host if we haven't already.
        /// </summary>
        private void HideLoading()
        {
            // Make sure we haven't been called before.
            lock(m_host)
            {
                if(m_loadingHidden)
                {
                    return;
                }
                m_loadingHidden = true;
            }

            // Hide it.
            m_host.HideLoading();
        }
    }
}
