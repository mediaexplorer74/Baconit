// Decompiled with JetBrains decompiler
// Type: BaconitData.Libs.BaconitUserMessage
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using System.ComponentModel;

#nullable disable
namespace BaconitData.Libs
{
  public class BaconitUserMessage
  {
    public string ToastText;
    public bool hasClick;
    public bool hasToast;
    public string BoxTitle;
    public string BoxMessage;
    public string Button1;
    public string Button2;
    public RunWorkerCompletedEventHandler MessageCallback;

    public BaconitUserMessage(
      string Toasttext,
      bool isToast,
      bool hasclick,
      string Boxtitle,
      string Boxmessage)
    {
      this.ToastText = Toasttext;
      this.hasToast = isToast;
      this.hasClick = hasclick;
      this.BoxTitle = Boxtitle;
      this.BoxMessage = Boxmessage;
    }

    public void SetLongMessage(
      string button1,
      string button2,
      RunWorkerCompletedEventHandler callback)
    {
      this.Button1 = button1;
      this.Button2 = button2;
      this.MessageCallback = callback;
    }
  }
}
