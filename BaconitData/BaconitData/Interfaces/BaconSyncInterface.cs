// Decompiled with JetBrains decompiler
// Type: BaconitData.Interfaces.BaconSyncInterface
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using System.ComponentModel;

#nullable disable
namespace BaconitData.Interfaces
{
  public interface BaconSyncInterface
  {
    void Bump();

    void HandlePushUrl(string url);

    void AddToReadList(string redditid);

    void SetNewPassCode(string NewPassword, RunWorkerCompletedEventHandler callback);

    void SignOutOfBaconSync();

    void StartUpPush();

    void CheckAccount(RunWorkerCompletedEventHandler callback);

    void AppDisactivated();
  }
}
