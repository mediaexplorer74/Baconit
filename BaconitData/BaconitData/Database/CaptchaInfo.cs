// Decompiled with JetBrains decompiler
// Type: BaconitData.Database.CaptchaInfo
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

#nullable disable
namespace BaconitData.Database
{
  public class CaptchaInfo
  {
    public const int SUBMIT_LINK = 0;
    public const int MESSAGE_REPLY = 1;
    public const int MESSAGE_COMPOSE = 2;
    public const int NEW_COMMENT = 3;
    public object Info;
    public int type;
    public string CaptchaID;
  }
}
