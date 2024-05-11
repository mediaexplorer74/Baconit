// Decompiled with JetBrains decompiler
// Type: Baconit.Libs.UpdaterMan
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using BaconitData.Libs;
using Microsoft.Phone.Scheduler;
using System;

#nullable disable
namespace Baconit.Libs
{
  public class UpdaterMan
  {
    public const string periodicTaskName = "Baconit Updater";
    public const string resourceIntensiveTaskName = "Baconit Nightly Updater";
    private static PeriodicTask periodicTask;
    private static ResourceIntensiveTask resourceIntensiveTask;

    public static void UpdateAgents()
    {
      App.DataManager.LogMan.Info("Update Background Agent");
      try
      {
        bool flag = false;
        UpdaterMan.periodicTask = ScheduledActionService.Find("Baconit Updater") as PeriodicTask;
        UpdaterMan.resourceIntensiveTask = ScheduledActionService.Find("Baconit Nightly Updater") as ResourceIntensiveTask;
        if (UpdaterMan.periodicTask != null && UpdaterMan.periodicTask.LastExitReason != AgentExitReason.Completed)
          App.DataManager.BaconitAnalytics.LogEvent("Perdioc Updater Error - " + UpdaterMan.periodicTask.LastExitReason.ToString());
        if (UpdaterMan.resourceIntensiveTask != null && UpdaterMan.resourceIntensiveTask.LastExitReason != AgentExitReason.Completed && UpdaterMan.resourceIntensiveTask.LastExitReason != AgentExitReason.None)
          App.DataManager.BaconitAnalytics.LogEvent("Resource Updater Error - " + UpdaterMan.resourceIntensiveTask.LastExitReason.ToString());
        if (App.DataManager.SettingsMan.BackgroundAgentEnabled == 0 || App.DataManager.SettingsMan.BackgroundAgentEnabled == -1)
        {
          if (UpdaterMan.periodicTask != null)
            UpdaterMan.RemoveAgent("Baconit Updater");
          if (UpdaterMan.resourceIntensiveTask == null)
            return;
          UpdaterMan.RemoveAgent("Baconit Nightly Updater");
        }
        else
        {
          if (UpdaterMan.periodicTask == null || !UpdaterMan.periodicTask.IsScheduled)
          {
            if (UpdaterMan.periodicTask == null)
              UpdaterMan.periodicTask = new PeriodicTask("Baconit Updater");
            else
              ScheduledActionService.Remove("Baconit Updater");
            UpdaterMan.periodicTask.Description = "Used by Baconit to check for new messages and to update live tiles and the lock screen.";
            try
            {
              ScheduledActionService.Add((ScheduledAction) UpdaterMan.periodicTask);
              App.DataManager.SettingsMan.ShowDisabledBackgroundWarning = true;
            }
            catch (InvalidOperationException ex)
            {
              if (ex.Message.Contains("BNS Error: The action is disabled"))
              {
                if (App.DataManager.SettingsMan.ShowDisabledBackgroundWarning)
                {
                  App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("", false, false, "Updating Disabled", "Background agents are disabled, if you wish to use the background agent please enable them from your phone’s settings."));
                  App.DataManager.SettingsMan.ShowDisabledBackgroundWarning = false;
                  flag = true;
                }
              }
              else if (ex.Message.Contains("The maximum number of ScheduledActions of this type have already been added"))
              {
                if (DataManager.IS_LOW_MEMORY_DEVICE)
                {
                  App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("", false, false, "Updating Disabled", "This device does not support background updating. Turning off background updates."));
                  App.DataManager.SettingsMan.BackgroundAgentEnabled = 0;
                  flag = true;
                }
                else
                {
                  App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("", false, false, "Updating Disabled", "You have too many apps that use background updating. If you want Baconit to use background updating, please disable on of the other apps."));
                  App.DataManager.SettingsMan.ShowDisabledBackgroundWarning = false;
                  flag = true;
                }
              }
            }
          }
          if (UpdaterMan.resourceIntensiveTask != null && UpdaterMan.resourceIntensiveTask.IsScheduled)
            return;
          if (UpdaterMan.resourceIntensiveTask == null)
            UpdaterMan.resourceIntensiveTask = new ResourceIntensiveTask("Baconit Nightly Updater");
          else
            ScheduledActionService.Remove("Baconit Nightly Updater");
          UpdaterMan.resourceIntensiveTask.Description = "Used by Baconit to check for new messages and to update live tiles and the lock screen.";
          try
          {
            ScheduledActionService.Add((ScheduledAction) UpdaterMan.resourceIntensiveTask);
            App.DataManager.SettingsMan.ShowDisabledBackgroundWarning = true;
          }
          catch (InvalidOperationException ex)
          {
            if (ex.Message.Contains("BNS Error: The action is disabled"))
            {
              if (!App.DataManager.SettingsMan.ShowDisabledBackgroundWarning || flag)
                return;
              App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("", false, false, "Updating Disabled", "Background agents are disabled, if you wish to use the background agent please enable them from your phone’s settings."));
              App.DataManager.SettingsMan.ShowDisabledBackgroundWarning = false;
            }
            else
            {
              if (!ex.Message.Contains("The maximum number of ScheduledActions of this type have already been added"))
                return;
              if (DataManager.IS_LOW_MEMORY_DEVICE && !flag)
              {
                App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("", false, false, "Updating Disabled", "This device does not support background updating. Turning off background updates."));
                App.DataManager.SettingsMan.BackgroundAgentEnabled = 0;
              }
              else
              {
                if (!flag)
                  App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("", false, false, "Updating Disabled", "You have too many apps that use background updating. If you want Baconit to use background updating, please disable on of the other apps."));
                App.DataManager.SettingsMan.ShowDisabledBackgroundWarning = false;
              }
            }
          }
        }
      }
      catch (Exception ex)
      {
        App.DataManager.DebugDia("couldn't set background service", ex);
      }
    }

    private static void RemoveAgent(string name)
    {
      try
      {
        ScheduledActionService.Remove(name);
      }
      catch (Exception ex)
      {
      }
    }

    public static void WriteStatus(PeriodicTask per, ResourceIntensiveTask res)
    {
      if (per != null && App.DataManager.SettingsMan.EnableLogging)
      {
        App.DataManager.LogMan.Info("Periodic Task");
        LogMan logMan1 = App.DataManager.LogMan;
        bool flag = per.IsScheduled;
        string text1 = "is scheduled =" + flag.ToString();
        logMan1.Info(text1);
        LogMan logMan2 = App.DataManager.LogMan;
        flag = per.IsEnabled;
        string text2 = "is enabled =" + flag.ToString();
        logMan2.Info(text2);
        App.DataManager.LogMan.Info("is last ran =" + per.LastScheduledTime.ToString());
        App.DataManager.LogMan.Info("is last exit reason =" + (object) per.LastExitReason);
        App.DataManager.LogMan.Info("is expires =" + (object) per.ExpirationTime);
      }
      if (res == null || !App.DataManager.SettingsMan.EnableLogging)
        return;
      App.DataManager.LogMan.Info("Resource Task");
      LogMan logMan3 = App.DataManager.LogMan;
      bool flag1 = res.IsScheduled;
      string text3 = "is scheduled =" + flag1.ToString();
      logMan3.Info(text3);
      LogMan logMan4 = App.DataManager.LogMan;
      flag1 = res.IsEnabled;
      string text4 = "is enabled =" + flag1.ToString();
      logMan4.Info(text4);
      App.DataManager.LogMan.Info("is last ran =" + res.LastScheduledTime.ToString());
      App.DataManager.LogMan.Info("is last exit reason =" + (object) res.LastExitReason);
      App.DataManager.LogMan.Info("is expires =" + (object) res.ExpirationTime);
    }
  }
}
