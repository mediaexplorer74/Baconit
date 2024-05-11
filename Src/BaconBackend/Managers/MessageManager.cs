﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace BaconBackend.Managers
{
#pragma warning disable CS4014
    public class MessageManager
    {
        BaconManager m_baconMan;

        public MessageManager(BaconManager baconMan)
        {
            m_baconMan = baconMan;
        }

        public void ShowMessageSimple(string title, string content)
        {
            ShowMessaage(content, title);
        }

        public async void ShowSigninMessage(string toDoWhat)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                // Add the buttons
                bool? response = await ShowYesNoMessage("Login Required", $"You must be logged into to a reddit account to {toDoWhat}. Do you want to login or create a new account now?");

                if(response.HasValue && response.Value)
                {
                    m_baconMan.NavigateToLogin();
                }
            });
        }


        public void DebugDia(string str, Exception ex = null)
        {
            if (m_baconMan.UiSettingsMan.Developer_Debug)
            {
                System.Diagnostics.Debug.WriteLine("Error, " + str + " Message: " + (ex == null ? "" : ex.Message));
                ShowMessaage("DebugDia: str " + str + " \n\nMessage: " + (ex == null ? "" : ex.Message), "DebugDia");
            }
        }

        private async void ShowMessaage(string content, string title)
        {
            // Don't show messages if we are in the background.
            if(m_baconMan.IsBackgroundTask)
            {
                return;
            }

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                MessageDialog message = new MessageDialog(content, title);
                await message.ShowAsync();
            });
        }

        /// <summary>
        /// Shows a yes no dialog with a message.
        /// MUST BE CALLED FROM THE UI THREAD!
        /// </summary>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<bool?> ShowYesNoMessage(string title, string content, string postiveButton = "Yes", string negativeButton = "No")
        {
            // Don't show messages if we are in the background.
            if (m_baconMan.IsBackgroundTask)
            {
                return null;
            }

            bool? response = null;

            // Add the buttons
            MessageDialog message = new MessageDialog(content, title);
            message.Commands.Add(new UICommand(
                postiveButton,
                (IUICommand command)=> {
                    response = true; }));
            message.Commands.Add(new UICommand(
                negativeButton,
                (IUICommand command) => { response = false; }));
            message.DefaultCommandIndex = 0;
            message.CancelCommandIndex = 1;

            // Show the dialog
            await message.ShowAsync();

            return response;
        }
    }
}
