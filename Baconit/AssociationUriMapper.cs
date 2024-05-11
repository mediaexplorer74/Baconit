// Decompiled with JetBrains decompiler
// Type: Baconit.AssociationUriMapper
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using System;
using System.Net;
using System.Windows.Navigation;

#nullable disable
namespace Baconit
{
  internal class AssociationUriMapper : UriMapperBase
  {
    private string tempUri;

    public override Uri MapUri(Uri uri)
    {
      this.tempUri = HttpUtility.UrlDecode(uri.ToString());
      return this.tempUri.Contains("baconit:StoryDetails?StoryDataRedditID=") ? new Uri("/StoryDetails.xaml?StoryDataRedditID=" + this.tempUri.Substring(this.tempUri.IndexOf("StoryDataRedditID=") + 18), UriKind.Relative) : uri;
    }
  }
}
