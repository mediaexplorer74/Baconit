// Decompiled with JetBrains decompiler
// Type: BaconitData.Database.RedditAccount
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Threading;

#nullable disable
namespace BaconitData.Database
{
  [DataContract(Name = "RedditAccount")]
  public class RedditAccount
  {
    [DataMember]
    public string UserName;
    [DataMember]
    public string DateString;
    [DataMember]
    public string ModHas;
    [DataMember]
    public string Cookie;

    public RedditAccount(string user, DateTime added, string mod, string cookie)
    {
      this.UserName = user;
      this.ModHas = mod;
      this.Cookie = cookie;
      CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
      this.DateString = "Added " + added.ToString(currentCulture.DateTimeFormat.ShortDatePattern.ToString());
    }
  }
}
