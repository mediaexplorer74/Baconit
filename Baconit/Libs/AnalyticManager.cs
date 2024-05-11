// Decompiled with JetBrains decompiler
// Type: Baconit.Libs.AnalyticManager
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using BaconitData.Interfaces;
using GoogleAnalytics;
using System;
using System.Windows;

#nullable disable
namespace Baconit.Libs
{
  internal class AnalyticManager : BaconitAnalyticsInterface
  {
    public void LogEvent(string eventName)
    {
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() => EasyTracker.GetTracker().SendEvent("Event", eventName, (string) null, 0)));
    }

    public void LogEvent(string category, string eventName)
    {
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() => EasyTracker.GetTracker().SendEvent(category, eventName, (string) null, 0)));
    }

    public void LogPage(string pageName)
    {
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() => EasyTracker.GetTracker().SendView(pageName)));
    }

    public void LogException(string exception, bool fatal)
    {
      Deployment.Current.Dispatcher.BeginInvoke((Action) (() => EasyTracker.GetTracker().SendException(exception, fatal)));
    }
  }
}
