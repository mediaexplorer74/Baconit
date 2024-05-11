// Decompiled with JetBrains decompiler
// Type: BaconitData.Libs.StartUpCheckMan
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using Baconit;
using Baconit.Database;
using System;
using System.Threading;

#nullable disable
namespace BaconitData.Libs
{
  public class StartUpCheckMan
  {
    private DataManager DataMan;

    public StartUpCheckMan(DataManager data) => this.DataMan = data;

    public void AppStarted(bool isFreshStart)
    {
      int num = Math.Abs((int) new TimeSpan(0, 0, 0, 0, (int) (BaconitStore.currentTime() - this.DataMan.SettingsMan.LastStartUpRun)).TotalMinutes);
      if (this.DataMan.RunningFromUpdater || num <= 15 && !this.DataMan.SettingsMan.ForceUpdateTiles)
        return;
      ThreadPool.QueueUserWorkItem((WaitCallback) (obj =>
      {
        this.DataMan.BaconSyncObj.Bump();
        Thread.Sleep(700);
        this.CheckForMessage();
        Thread.Sleep(700);
        this.DataMan.SpecialImageManager.CheckForUpdate();
        Thread.Sleep(700);
        this.DataMan.TileMan.CheckForAndDoAllUpdate();
        Thread.Sleep(700);
        this.DataMan.MainLandingImageMan.CheckForUpdate(false);
        this.DataMan.SettingsMan.LastStartUpRun = BaconitStore.currentTime();
      }));
    }

    public void CheckForMessage()
    {
      if (this.DataMan.SettingsMan.BackgroundAgentEnabled == -1 || !this.DataMan.SettingsMan.AdultFilterSet())
        return;
      if (this.DataMan.SettingsMan.OpenedAppRatingCounter == 3)
        this.DataMan.MessageManager.ShowRateMessage();
      else if (this.DataMan.SettingsMan.OpenedAppCounter == 12)
        this.DataMan.MessageManager.ShowRateMessage();
      else if (this.DataMan.SettingsMan.OpenedAppCounter == 40)
        this.DataMan.MessageManager.ShowDonateMessage();
      else
        this.checkMessageOfTheDay();
      ++this.DataMan.SettingsMan.OpenedAppCounter;
      ++this.DataMan.SettingsMan.OpenedAppRatingCounter;
    }

    public void checkMessageOfTheDay()
    {
      if (this.DataMan.BaconitStore.LastUpdatedTime("!MessageOfTheDay") >= BaconitStore.currentTime() - 7200000.0)
        return;
      try
      {
        this.DataMan.GetMessageOfTheDay();
      }
      catch
      {
      }
    }
  }
}
