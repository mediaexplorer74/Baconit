// Decompiled with JetBrains decompiler
// Type: BaconitData.Libs.ShareHelper
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using Baconit;
using System;
using System.Threading;
using Windows.Networking.Proximity;

#nullable disable
namespace BaconitData.Libs
{
  public class ShareHelper
  {
    private DataManager DataMan;
    private ProximityDevice device;
    private long lastMessage;

    public ShareHelper(DataManager data) => this.DataMan = data;

    public void ShareNFC(string uri)
    {
      if (this.device == null)
        this.device = ProximityDevice.GetDefault();
      if (this.lastMessage != 0L)
      {
        if (this.device != null)
          this.device.StopPublishingMessage(this.lastMessage);
        this.lastMessage = 0L;
      }
      if (this.device == null)
      {
        this.DataMan.MessageManager.QueueMessage(new BaconitUserMessage("Your device doesn't support NFC", true, false, "", ""));
      }
      else
      {
        if (this.device != null)
          this.lastMessage = this.device.PublishUriMessage(new Uri(uri, UriKind.Absolute));
        this.DataMan.MessageManager.QueueMessage(new BaconitUserMessage("Tap your phone with another to share", true, false, "", ""));
        new Thread((ThreadStart) (() =>
        {
          Thread.Sleep(30000);
          if (this.device == null || this.lastMessage == 0L)
            return;
          this.device.StopPublishingMessage(this.lastMessage);
          this.lastMessage = 0L;
        })).Start();
      }
    }
  }
}
