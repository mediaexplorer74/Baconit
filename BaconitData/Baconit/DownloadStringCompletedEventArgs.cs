// Decompiled with JetBrains decompiler
// Type: Baconit.DownloadStringCompletedEventArgs
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using System;

#nullable disable
namespace Baconit
{
  public class DownloadStringCompletedEventArgs : EventArgs
  {
    public string Result { get; private set; }

    public Exception Error { get; private set; }

    public DownloadStringCompletedEventArgs(string result) => this.Result = result;

    public DownloadStringCompletedEventArgs(Exception ex) => this.Error = ex;
  }
}
