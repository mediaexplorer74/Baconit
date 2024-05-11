// Decompiled with JetBrains decompiler
// Type: BaconitData.Interfaces.BaconitAnalyticsInterface
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

#nullable disable
namespace BaconitData.Interfaces
{
  public interface BaconitAnalyticsInterface
  {
    void LogEvent(string eventName);

    void LogPage(string pageName);

    void LogEvent(string category, string eventName);

    void LogException(string exception, bool fatal);
  }
}
