// Decompiled with JetBrains decompiler
// Type: BaconitData.Interfaces.MessageManagerInterface
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using BaconitData.Libs;
using System;
using System.Windows;

#nullable disable
namespace BaconitData.Interfaces
{
  public interface MessageManagerInterface
  {
    void showRedditErrorMessage(string doingWhat, string redditError, bool ShowRedditError);

    void showRedditErrorMessage(string str, string error);

    void showErrorMessage(string doingWhat, Exception e);

    void showWebErrorMessage(string str, Exception e);

    void showWebErrorMessage(string str, Exception e, bool forceMessageBox);

    void QueueMessage(BaconitUserMessage message);

    void DebugDia(string str, Exception e);

    void SetResumeTime(double time);

    void ShowSignInMessage(string action);

    void ShowRateMessage();

    void ShowSubMessage();

    void ShowDonateMessage();

    MessageBoxResult AskAboutCrash();

    void ShowCrashSent(string crashEx);

    void ShowAprilError();
  }
}
