// Decompiled with JetBrains decompiler
// Type: BaconitData.Libs.TileMan
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using Baconit;
using Baconit.Database;
using Microsoft.Phone.Shell;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Windows.Networking.Connectivity;

#nullable disable
namespace BaconitData.Libs
{
  public class TileMan
  {
    private DataManager DataMan;
    private const int IANA_INTERFACE_TYPE_WIFI = 71;

    public TileMan(DataManager data) => this.DataMan = data;

    public bool CheckForAndDoAllUpdate()
    {
      TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, (int) (BaconitStore.currentTime() - this.DataMan.SettingsMan.TileImageFullUpdate));
      int num1 = Math.Abs((int) timeSpan.TotalMinutes);
      timeSpan = new TimeSpan(0, 0, 0, 0, (int) (BaconitStore.currentTime() - this.DataMan.SettingsMan.TileImageHalfUpdate));
      int num2 = Math.Abs((int) timeSpan.TotalMinutes);
      if (num1 > 1440 || this.DataMan.SettingsMan.ForceUpdateTiles)
      {
        this.UpdateAllTiles();
        return true;
      }
      if (num2 >= 240)
        return false;
      this.DataMan.SettingsMan.TileImageHalfUpdate = BaconitStore.currentTime();
      return true;
    }

    public void UpdateAllTiles()
    {
      try
      {
        int ianaInterfaceType = (int) NetworkInformation.GetInternetConnectionProfile().NetworkAdapter.IanaInterfaceType;
        if (NetworkInformation.GetInternetConnectionProfile().NetworkAdapter.IanaInterfaceType != 71U)
        {
          int num = this.DataMan.SettingsMan.OnlyUpdateWifiTilesImages ? 1 : 0;
        }
      }
      catch
      {
      }
      foreach (ShellTile activeTile in ShellTile.ActiveTiles)
      {
        string str1 = HttpUtility.UrlDecode(activeTile.NavigationUri.OriginalString);
        if (str1.Contains("subredditURL="))
        {
          int startIndex = str1.IndexOf("subredditURL=") + 13;
          int num = str1.IndexOf("&", startIndex);
          if (num == -1)
            num = str1.Length;
          string str2 = str1.Substring(startIndex, num - startIndex);
          List<SubRedditData> stories = this.UpdateSubRedditStories(str2);
          try
          {
            if (stories != null)
              this.UpdateSingleSub(activeTile, str2, stories);
          }
          catch (Exception ex)
          {
            this.DataMan.DebugDia("Can't update tile", ex);
          }
          GC.Collect();
        }
        else if (str1.Contains("StoryDataRedditID="))
          this.UpdateSingleStory(activeTile);
      }
      GC.Collect();
      this.DataMan.SettingsMan.ForceUpdateTiles = false;
      this.DataMan.SettingsMan.TileImageFullUpdate = BaconitStore.currentTime();
      this.DataMan.SettingsMan.TileImageHalfUpdate = BaconitStore.currentTime();
    }

    public void UpdateSingleSub(
      ShellTile SubRedditTileData,
      string subredditURL,
      List<SubRedditData> stories)
    {
      if (SubRedditTileData == null)
        return;
      if (SubRedditTileData.NavigationUri.OriginalString.Contains("TileType=cycle"))
      {
        List<Uri> uriList = new List<Uri>();
        List<string> stringList = new List<string>();
        foreach (SubRedditData storey in stories)
        {
          string imageUrl = DataManager.GetImageURL(storey, false);
          if (imageUrl != null)
            stringList.Add(imageUrl);
        }
        int num = 0;
        foreach (string ImageURL in stringList)
        {
          try
          {
            string str = subredditURL.Replace('/', '0');
            string singleImage = this.DataMan.SpecialImageManager.GetSingleImage(ImageURL, str + "_" + (object) num, true);
            uriList.Add(new Uri("isostore:" + singleImage, UriKind.Absolute));
            ++num;
            if (num > 8)
              break;
          }
          catch
          {
          }
        }
        if (uriList.Count <= 0)
          return;
        ShellTileData data = (ShellTileData) new CycleTileData()
        {
          CycleImages = (IEnumerable<Uri>) uriList,
          SmallBackgroundImage = uriList[0]
        };
        SubRedditTileData.Update(data);
      }
      else
      {
        if (!SubRedditTileData.NavigationUri.OriginalString.Contains("TileType=iconic") || stories == null || stories.Count <= 3 || stories.Count <= 2)
          return;
        ShellTileData data = (ShellTileData) new IconicTileData()
        {
          WideContent1 = stories[0].Title,
          WideContent2 = stories[1].Title,
          WideContent3 = stories[2].Title
        };
        SubRedditTileData.Update(data);
      }
    }

    public List<SubRedditData> UpdateSubRedditStories(string redditUrl)
    {
      string str = HttpUtility.UrlEncode(this.DataMan.SettingsMan.UserCookie);
      HttpWebRequest state;
      if (this.DataMan.SettingsMan.IsSignedIn && str != null)
      {
        state = !redditUrl.Equals("/saved/") ? (HttpWebRequest) WebRequest.Create("https://www.reddit.com" + redditUrl + "hot/.json") : (HttpWebRequest) WebRequest.Create("https://www.reddit.com/user/" + this.DataMan.SettingsMan.UserName + "/saved/.json");
        state.CookieContainer = new CookieContainer();
        Cookie cookie = new Cookie();
        cookie.Domain = ".reddit.com";
        cookie.HttpOnly = false;
        cookie.Path = "/";
        cookie.Secure = false;
        cookie.Value = str;
        cookie.Name = "reddit_session";
        state.CookieContainer.Add(new Uri("https://api.reddit.com"), cookie);
        state.CookieContainer.Add(new Uri("https://www.reddit.com"), cookie);
        state.CookieContainer.Add(new Uri("https://reddit.com"), cookie);
      }
      else
        state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com" + redditUrl + "hot/.json");
      List<SubRedditData> SubRedditDataList = new List<SubRedditData>();
      using (AutoResetEvent are = new AutoResetEvent(false))
      {
        state.BeginGetResponse((AsyncCallback) (result =>
        {
          try
          {
            if (result.AsyncState is HttpWebRequest asyncState2)
            {
              using (HttpWebResponse response = (HttpWebResponse) asyncState2.EndGetResponse(result))
              {
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                  JToken jtoken1 = JObject.Parse(streamReader.ReadToEnd())["data"][(object) "children"];
                  int num = 0;
                  foreach (JToken jtoken2 in (IEnumerable<JToken>) jtoken1)
                  {
                    SubRedditData story = new SubRedditData();
                    story.URL = (string) jtoken2[(object) "data"][(object) "url"];
                    story.Title = (string) jtoken2[(object) "data"][(object) "title"];
                    try
                    {
                      story.isNotSafeForWork = (bool) jtoken2[(object) "data"][(object) "over_18"];
                      if (story.isNotSafeForWork)
                        continue;
                    }
                    catch
                    {
                      story.isNotSafeForWork = false;
                    }
                    if (this.DataMan.SettingsMan.AdultFilter)
                    {
                      story.Title = DataManager.FilterText(story.Title);
                      story.SelfText = DataManager.FilterText(story.SelfText);
                    }
                    if (DataManager.GetImageURL(story, false) != null)
                      ++num;
                    SubRedditDataList.Add(story);
                    if (num > 3)
                      break;
                  }
                }
              }
            }
          }
          catch (Exception ex)
          {
            this.DataMan.LogMan.Error("Updating Subreddit " + redditUrl, ex);
          }
          are.Set();
        }), (object) state);
        are.WaitOne();
      }
      return SubRedditDataList;
    }

    public void UpdateSingleStory(SubRedditData story)
    {
      if (story == null)
        return;
      ShellTile tile = ShellTile.ActiveTiles.FirstOrDefault<ShellTile>((Func<ShellTile, bool>) (x => x.NavigationUri.ToString().Contains("StoryDataRedditID=" + story.RedditID)));
      if (tile == null)
        return;
      this.UpdateSingleStory(tile, story);
    }

    public void UpdateSingleStory(ShellTile tileData)
    {
      if (tileData == null)
        return;
      int startIndex = tileData.NavigationUri.OriginalString.IndexOf("StoryDataRedditID=") + 18;
      int num = tileData.NavigationUri.OriginalString.IndexOf("&", startIndex);
      if (num == -1)
        num = tileData.NavigationUri.OriginalString.Length;
      SubRedditData singleStory = this.GetSingleStory(tileData.NavigationUri.OriginalString.Substring(startIndex, num - startIndex));
      this.UpdateSingleStory(tileData, singleStory);
    }

    public void UpdateSingleStory(ShellTile tile, SubRedditData story)
    {
      if (tile == null || story == null || story.RedditID == null)
        return;
      int num = 0;
      if (this.DataMan.SettingsMan.ReadStories.ContainsKey(story.RedditID))
      {
        num = this.DataMan.SettingsMan.ReadStories[story.RedditID] - story.comments;
        if (num < 0)
          num = 0;
      }
      else
        this.DataMan.SettingsMan.ReadStories[story.RedditID] = story.comments;
      if (story == null)
        return;
      if (tile.NavigationUri.OriginalString.Contains("TileType=cycle"))
      {
        string imageUrl = DataManager.GetImageURL(story, false);
        if (imageUrl == null)
          return;
        string singleImage = this.DataMan.SpecialImageManager.GetSingleImage(imageUrl, story.RedditID, true);
        if (singleImage == null)
          return;
        ShellTileData data = (ShellTileData) new CycleTileData()
        {
          CycleImages = (IEnumerable<Uri>) new List<Uri>()
          {
            new Uri("isostore:" + singleImage, UriKind.Absolute)
          },
          Count = new int?(num)
        };
        tile.Update(data);
      }
      else
      {
        if (!tile.NavigationUri.OriginalString.Contains("TileType=iconic"))
          return;
        string title = story.Title;
        string str1 = (story.ups - story.downs).ToString() + " points; " + (object) story.comments + " comments";
        string str2 = "By " + story.Author;
        IconicTileData iconicTileData = new IconicTileData();
        iconicTileData.Title = story.Title;
        iconicTileData.WideContent1 = title;
        iconicTileData.WideContent2 = str1;
        iconicTileData.WideContent3 = str2;
        iconicTileData.Count = new int?(num);
        ShellTileData data = (ShellTileData) iconicTileData;
        tile.Update(data);
      }
    }

    public SubRedditData GetSingleStory(string StoryRID)
    {
      SubRedditData story = new SubRedditData();
      string str1 = HttpUtility.UrlEncode(this.DataMan.SettingsMan.UserCookie);
      if (StoryRID.Contains("_"))
      {
        string str2 = StoryRID;
        StoryRID = str2.Substring(str2.IndexOf("_") + 1);
      }
      HttpWebRequest state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com/comments/" + StoryRID + ".json");
      if (this.DataMan.SettingsMan.IsSignedIn && str1 != null)
      {
        state.CookieContainer = new CookieContainer();
        Cookie cookie = new Cookie();
        cookie.Domain = ".reddit.com";
        cookie.HttpOnly = false;
        cookie.Path = "/";
        cookie.Secure = false;
        cookie.Value = str1;
        cookie.Name = "reddit_session";
        state.CookieContainer.Add(new Uri("https://api.reddit.com"), cookie);
        state.CookieContainer.Add(new Uri("https://www.reddit.com"), cookie);
        state.CookieContainer.Add(new Uri("https://reddit.com"), cookie);
      }
      using (AutoResetEvent are = new AutoResetEvent(false))
      {
        state.BeginGetResponse((AsyncCallback) (result =>
        {
          try
          {
            if (result.AsyncState is HttpWebRequest asyncState2)
            {
              using (HttpWebResponse response = (HttpWebResponse) asyncState2.EndGetResponse(result))
              {
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                  string end = streamReader.ReadToEnd();
                  int num1 = end.IndexOf("\"kind\": \"Listing\",");
                  int num2 = end.IndexOf("\"kind\": \"Listing\",", num1 + 1);
                  JToken jtoken = JObject.Parse(end.Substring(num1 - 1, num2 - num1 - 2))["data"][(object) "children"][(object) 0][(object) "data"];
                  story.RedditID = (string) jtoken[(object) "name"];
                  story.Author = (string) jtoken[(object) "author"];
                  story.score = (int) jtoken[(object) "score"];
                  story.isSelf = (bool) jtoken[(object) "is_self"];
                  story.created = (float) jtoken[(object) "created_utc"];
                  story.URL = (string) jtoken[(object) "url"];
                  story.Title = HttpUtility.HtmlDecode((string) jtoken[(object) "title"]);
                  story.comments = (int) jtoken[(object) "num_comments"];
                  story.ups = (int) jtoken[(object) "ups"];
                  story.RedditRealID = (string) jtoken[(object) "id"];
                  if (this.DataMan.SettingsMan.AdultFilter)
                    story.Title = DataManager.FilterText(story.Title);
                }
              }
            }
          }
          catch
          {
          }
          are.Set();
        }), (object) state);
        are.WaitOne();
      }
      return story;
    }
  }
}
