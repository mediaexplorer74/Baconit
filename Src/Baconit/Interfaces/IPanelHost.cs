﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baconit.Interfaces
{
    public interface IPanelHost
    {
        /// <summary>
        /// Fired when the screen mode changes
        /// </summary>
        event EventHandler<OnScreenModeChangedArgs> OnScreenModeChanged;

        /// <summary>
        /// Returns the current screen mode
        /// </summary>
        /// <returns></returns>
        ScreenMode CurrentScreenMode();

        /// <summary>
        /// Navigates to a panel. If a panel already exist with the same panelId instead or creating a new
        /// panel the old panel will be shown and passed the new arguments.
        /// </summary>
        /// <param name="panelType">The type of panel to be created</param>
        /// <param name="panelId">A unique identifier for the panel, the id should be able to differeincae between two panels of the same type</param>
        /// <param name="arguments">Arguments to be sent to the panel</param>
        /// <returns></returns>
        bool Navigate(Type panelType, string panelId, Dictionary<string, object> arguments = null);

        /// <summary>
        /// Called to navigate back to the last panel.
        /// </summary>
        bool GoBack();

        /// <summary>
        /// Called to show or hide the menu.
        /// </summary>
        /// <param name="show"></param>
        void ToggleMenu(bool show);
    }
}
