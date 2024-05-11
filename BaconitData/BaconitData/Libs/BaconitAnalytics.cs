// Decompiled with JetBrains decompiler
// Type: BaconitData.Libs.BaconitAnalytics
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using Baconit;

#nullable disable
namespace BaconitData.Libs
{
  public class BaconitAnalytics
  {
    private DataManager DataMan;

    public BaconitAnalytics(DataManager data) => this.DataMan = data;

    public void LogEvent(string eventName)
    {
      if (this.DataMan.AnalyticsManagerInter == null)
        return;
      this.DataMan.AnalyticsManagerInter.LogEvent(eventName);
    }

    public void LogEvent(string category, string eventName)
    {
      if (this.DataMan.AnalyticsManagerInter == null)
        return;
      this.DataMan.AnalyticsManagerInter.LogEvent(category, eventName);
    }

    public void LogPage(string pageName)
    {
      if (this.DataMan.AnalyticsManagerInter == null)
        return;
      this.DataMan.AnalyticsManagerInter.LogPage(pageName);
    }

    public void LogException(string exception, bool isFatal)
    {
      if (this.DataMan.AnalyticsManagerInter == null)
        return;
      this.DataMan.AnalyticsManagerInter.LogException(exception, isFatal);
    }
  }
}
