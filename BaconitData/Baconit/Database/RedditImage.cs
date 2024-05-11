// Decompiled with JetBrains decompiler
// Type: Baconit.Database.RedditImage
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;

#nullable disable
namespace Baconit.Database
{
  public class RedditImage
  {
    public bool CacheOnly;
    public bool GetLowRes;
    private RunWorkerCompletedEventHandler Handler;
    private DataManager DataMan;
    private bool IsCaptcha;

    public string URL { get; set; }

    public string FileName { get; set; }

    public string SubRedditRName { get; set; }

    public int SubRedditType { get; set; }

    public BitmapSource Image { get; set; }

    public string StoryID { get; set; }

    public bool CallBackStatus { get; set; }

    public bool IsGif { get; set; }

    public RedditImage(DataManager data, bool isCaptcha, RunWorkerCompletedEventHandler han)
    {
      this.DataMan = data;
      this.Handler = han;
      this.IsCaptcha = isCaptcha;
    }

    public void run(object threadContext)
    {
      try
      {
        this.DataMan.RequestImage(this, this.Handler, this.CacheOnly, this.IsCaptcha);
      }
      catch (Exception ex)
      {
      }
    }
  }
}
