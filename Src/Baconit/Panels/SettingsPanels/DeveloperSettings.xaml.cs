﻿using Baconit.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Baconit.Panels.SettingsPanels
{
    public sealed partial class DeveloperSettings : UserControl, IPanel
    {
        IPanelHost m_host;
        bool m_takeAction = false;

        public DeveloperSettings()
        {
            this.InitializeComponent();
        }

        public void PanelSetup(IPanelHost host, Dictionary<string, object> arguments)
        {
            m_host = host;
        }

        public void OnNavigatingFrom()
        {
            // Ignore
        }

        public void OnPanelPulledToTop(Dictionary<string, object> arguments)
        {
            // Ignore
        }

        public void OnNavigatingTo()
        {
            m_takeAction = false;
            App.BaconMan.TelemetryMan.ReportEvent(this, "DevSettingsOpened");
            ui_debuggingOn.IsOn = App.BaconMan.UiSettingsMan.Developer_Debug;
            m_takeAction = true;
        }

        private void DebuggingOn_Toggled(object sender, RoutedEventArgs e)
        {
            if(!m_takeAction)
            {
                return;
            }
            App.BaconMan.UiSettingsMan.Developer_Debug = ui_debuggingOn.IsOn;
        }
    }
}
