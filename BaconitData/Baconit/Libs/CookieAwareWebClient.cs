// Decompiled with JetBrains decompiler
// Type: Baconit.Libs.CookieAwareWebClient
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using System;
using System.Net;
using System.Security;

#nullable disable
namespace Baconit.Libs
{
  [SecuritySafeCritical]
  internal class CookieAwareWebClient : WebClient
  {
    private CookieContainer CookieContainer = new CookieContainer();
    private string lastPage;

    [SecuritySafeCritical]
    public CookieAwareWebClient()
    {
    }

    protected override WebRequest GetWebRequest(Uri address)
    {
      WebRequest webRequest = base.GetWebRequest(address);
      if (webRequest is HttpWebRequest)
      {
        ((HttpWebRequest) webRequest).CookieContainer = this.CookieContainer;
        string lastPage = this.lastPage;
      }
      this.lastPage = address.ToString();
      return webRequest;
    }

    public void setCookieConainer(CookieContainer cc) => this.CookieContainer = cc;
  }
}
