// Decompiled with JetBrains decompiler
// Type: Baconit.Libs.SubredditDataManager
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using Baconit.Database;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading;

#nullable disable
namespace Baconit.Libs
{
  public class SubredditDataManager
  {
    private DataManager DataMan;
    public bool UpdatingSubReddits;
    private List<SubReddit> _AccountSubList;
    private List<SubReddit> _LocalSubRedditList;
    private List<SubReddit> _SortedCombList;
    private Dictionary<string, bool> _FavoriteSubReddits;

    public event SubredditDataManager.SubredditUpdatedEventHandler SubRedditsUpdated;

    public SubredditDataManager(DataManager data) => this.DataMan = data;

    public void SearchForSubreddits(string query, RunWorkerCompletedEventHandler callback)
    {
      string userCookie = this.DataMan.SettingsMan.UserCookie;
      query = HttpUtility.UrlEncode(query.Trim());
      string str = HttpUtility.UrlEncode(userCookie);
      HttpWebRequest state;
      if (this.DataMan.SettingsMan.IsSignedIn && str != null)
      {
        state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com/reddits/search.json?q=" + query);
        state.CookieContainer = new CookieContainer();
        state.CookieContainer.Add(new Uri("https://api.reddit.com"), new Cookie()
        {
          Domain = ".reddit.com",
          HttpOnly = false,
          Path = "/",
          Secure = false,
          Value = str,
          Name = "reddit_session"
        });
      }
      else
        state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com/reddits/search.json?q=" + query);
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
                JObject jobject = JObject.Parse(streamReader.ReadToEnd());
                string stringInJson;
                if (!(stringInJson = DataManager.FindStringInJson(jobject["jquery"], ".error.")).Equals(""))
                {
                  this.DataMan.MessageManager.showRedditErrorMessage("update the account information", stringInJson);
                }
                else
                {
                  List<SubReddit> sender = new List<SubReddit>();
                  jobject["data"][(object) "children"].Children();
                  foreach (JToken jtoken in (IEnumerable<JToken>) jobject["data"][(object) "children"])
                    sender.Add(new SubReddit()
                    {
                      RName = (string) jtoken[(object) "data"][(object) "name"],
                      DisplayName = (string) jtoken[(object) "data"][(object) "display_name"],
                      created = (float) jtoken[(object) "data"][(object) "created_utc"],
                      URL = (string) jtoken[(object) "data"][(object) "url"],
                      Title = (string) jtoken[(object) "data"][(object) "title"],
                      Subscribers = (int) jtoken[(object) "data"][(object) "subscribers"],
                      PublicDescription = (string) jtoken[(object) "data"][(object) "public_description"],
                      Description = (string) jtoken[(object) "data"][(object) "description"],
                      isAccount = false,
                      isBuiltIn = false,
                      isLocal = false,
                      isGroup = false
                    });
                  callback((object) sender, new RunWorkerCompletedEventArgs((object) true, (Exception) null, false));
                }
              }
            }
          }
          else
          {
            this.DataMan.MessageManager.showWebErrorMessage("searching for subreddits", (Exception) null);
            callback((object) null, new RunWorkerCompletedEventArgs((object) true, (Exception) null, false));
          }
        }
        catch (Exception ex)
        {
          if (!ex.Message.Trim().Equals("Parameter name: index"))
            this.DataMan.MessageManager.showErrorMessage("searching for subreddits", ex);
          callback((object) null, new RunWorkerCompletedEventArgs((object) true, (Exception) null, false));
        }
      }), (object) state);
    }

    public void SubscribeToBaconit()
    {
      this.SubscribeToSubReddit(new SubReddit()
      {
        created = 0.0f,
        Description = "A Metro Reddit App",
        DisplayName = "Baconit",
        PublicDescription = "A Metro Reddit App",
        RName = "t5_2rfk9",
        Subscribers = 10000,
        Title = "Baconit",
        URL = "/r/baconit"
      }, true, true, false);
    }

    public void SubscribeToSubReddit(
      SubReddit subreddit,
      bool subscribe,
      bool UpdateUI,
      bool addToLocal)
    {
      string str = HttpUtility.UrlEncode(this.DataMan.SettingsMan.UserCookie);
      if (!this.DataMan.SettingsMan.IsSignedIn || str == null)
        return;
      Dictionary<string, object> parameters = new Dictionary<string, object>();
      parameters.Add("sr", (object) subreddit.RName);
      if (subscribe)
        parameters.Add("action", (object) "sub");
      else
        parameters.Add("action", (object) "unsub");
      parameters.Add("uh", (object) this.DataMan.SettingsMan.ModHash);
      PostClient postClient = new PostClient((IDictionary<string, object>) parameters);
      postClient.CookieContainer = new CookieContainer();
      Cookie cookie = new Cookie();
      cookie.Domain = ".reddit.com";
      cookie.HttpOnly = false;
      cookie.Path = "/";
      cookie.Secure = false;
      cookie.Value = str;
      cookie.Name = "reddit_session";
      postClient.CookieContainer.Add(new Uri("https://api.reddit.com"), cookie);
      postClient.CookieContainer.Add(new Uri("https://reddit.com"), cookie);
      postClient.CookieContainer.Add(new Uri("https://www.reddit.com"), cookie);
      postClient.DownloadStringCompleted += (PostClient.DownloadStringCompletedHandler) ((sender, e) =>
      {
        try
        {
          if (e.Error == null)
          {
            if (e.Result.Equals("{}"))
            {
              if (subscribe)
              {
                this.AddToAccount(subreddit, false);
                this.RemoveFromLocalSubReddit(subreddit, UpdateUI);
              }
              else
              {
                if (addToLocal)
                  this.AddToLocalSubReddit(subreddit, false);
                this.RemoveFromAccount(subreddit, UpdateUI);
              }
            }
            else
              this.DataMan.MessageManager.showRedditErrorMessage("update the account information", DataManager.FindStringInJson(JObject.Parse(e.Result)["jquery"], ".error."));
          }
          else if (subscribe)
            this.DataMan.MessageManager.showWebErrorMessage("subscribing to the subreddit", e.Error);
          else
            this.DataMan.MessageManager.showWebErrorMessage("unsubscribing to the subreddit", e.Error);
        }
        catch (Exception ex)
        {
          if (subscribe)
            this.DataMan.MessageManager.showErrorMessage("subscribing to the subreddit", ex);
          else
            this.DataMan.MessageManager.showErrorMessage("unsubscribing to the subreddit", ex);
        }
      });
      postClient.DownloadStringAsync(new Uri("https://www.reddit.com/api/subscribe", UriKind.Absolute));
    }

    public void UpdateSubReddits()
    {
      if (this.UpdatingSubReddits)
        return;
      this.UpdatingSubReddits = true;
      new SubredditDataManager.SubredditGetterClass(this).Start();
    }

    private static void AddExtraSubreddits(List<SubReddit> subReddits, bool isSignedIn)
    {
      subReddits.Insert(0, new SubReddit()
      {
        RName = "all",
        DisplayName = "all",
        created = 0.0f,
        URL = "/r/all/",
        Title = "Stories from all subreddits",
        PublicDescription = "Stories from all subreddits",
        Subscribers = 999999999,
        isAccount = false,
        isBuiltIn = true,
        isLocal = false,
        isGroup = false
      });
      subReddits.Insert(0, new SubReddit()
      {
        RName = "frontpage",
        DisplayName = "front page",
        created = 0.0f,
        URL = "/",
        Title = "Stories from the front page",
        PublicDescription = "Stories from the front page",
        Subscribers = 999999999,
        isAccount = false,
        isBuiltIn = true,
        isLocal = false,
        isGroup = false
      });
      if (isSignedIn)
      {
        subReddits.Insert(0, new SubReddit()
        {
          RName = "friends",
          DisplayName = "friends",
          created = 0.0f,
          URL = "/r/friends/",
          Title = "Stories your friends have submitted",
          PublicDescription = "Stories your friends have submitted",
          Subscribers = 99999999,
          isAccount = false,
          isBuiltIn = true,
          isLocal = false,
          isGroup = false
        });
        subReddits.Insert(0, new SubReddit()
        {
          RName = "saved",
          DisplayName = "saved",
          created = 0.0f,
          URL = "/saved/",
          Title = "Stories that have been saved by you",
          PublicDescription = "Stories that have been saved by you",
          Subscribers = 999999999,
          isAccount = false,
          isBuiltIn = true,
          isLocal = false,
          isGroup = false
        });
      }
      else
      {
        subReddits.Insert(0, new SubReddit()
        {
          RName = "t5_2rfk9",
          DisplayName = "baconit",
          created = 1.26288166E+09f,
          URL = "/r/baconit/",
          Title = "Baconit for Windows Phone",
          PublicDescription = "The Official Baconit Subreddit",
          Subscribers = 99999999,
          isAccount = false,
          isBuiltIn = true,
          isLocal = false,
          isGroup = false
        });
        subReddits.Insert(0, new SubReddit()
        {
          RName = "t5_2r71o",
          DisplayName = "windowsphone",
          created = 1.25292762E+09f,
          URL = "/r/windowsphone/",
          Title = "Official  Windows Phone reddit",
          PublicDescription = "Official  Windows Phone reddit",
          Subscribers = 4687,
          isAccount = false,
          isBuiltIn = true,
          isLocal = false,
          isGroup = false
        });
      }
    }

    public void ReSortSubreddits()
    {
      this.Sort(this.SortedCombList);
      this.SaveSortedCombList();
      if (this.SubRedditsUpdated == null)
        return;
      this.SubRedditsUpdated(true, this.SortedCombList, true);
    }

    public void Sort(List<SubReddit> ar)
    {
      if (this.DataMan.SettingsMan.SortSubRedditsBy == 0)
      {
        for (int index1 = 1; index1 < ar.Count; ++index1)
        {
          for (int index2 = 0; index2 < ar.Count - 1; ++index2)
          {
            if (ar[index2].DisplayName.CompareTo(ar[index2 + 1].DisplayName) > 0)
              this.Swap(ar, index2);
          }
        }
      }
      else
      {
        for (int index3 = 1; index3 < ar.Count; ++index3)
        {
          for (int index4 = 0; index4 < ar.Count - 1; ++index4)
          {
            if (ar[index4].Subscribers < ar[index4 + 1].Subscribers)
              this.Swap(ar, index4);
          }
        }
      }
    }

    public void Swap(List<SubReddit> ar, int first)
    {
      SubReddit subReddit = ar[first];
      ar[first] = ar[first + 1];
      ar[first + 1] = subReddit;
    }

    public void SetSubSortedReddits(List<SubReddit> subReddits)
    {
      this.SortedCombList = subReddits;
      this.SaveSortedCombList();
    }

    public List<SubReddit> GetSubSortedReddits() => this.SortedCombList;

    public string GetSubRedditRName(string url)
    {
      foreach (SubReddit sortedComb in this.SortedCombList)
      {
        if (sortedComb.URL.Equals(url))
          return sortedComb.RName;
      }
      return "";
    }

    public void AddToLocalSubReddit(SubReddit subreddit, bool UpdateUI)
    {
      foreach (SubReddit localSubReddit in this.LocalSubRedditList)
      {
        if (localSubReddit.RName.Equals(subreddit.RName))
          return;
      }
      subreddit.isAccount = false;
      subreddit.isLocal = true;
      this.LocalSubRedditList.Add(subreddit);
      this.SaveLocalSubRedditList();
      bool flag = false;
      List<SubReddit> subSortedReddits = this.GetSubSortedReddits();
      for (int index = 0; index < subSortedReddits.Count; ++index)
      {
        if (subSortedReddits[index].DisplayName.CompareTo(subreddit.DisplayName) > 0)
        {
          subSortedReddits.Insert(index, subreddit);
          flag = true;
          break;
        }
      }
      if (!flag)
        subSortedReddits.Add(subreddit);
      this.SaveSortedCombList();
      if (!(this.SubRedditsUpdated != null & UpdateUI))
        return;
      this.SubRedditsUpdated(true, subSortedReddits, true);
    }

    public bool UpdateLocalSubreddit(SubReddit old, SubReddit newSub)
    {
      SubReddit subReddit1 = (SubReddit) null;
      foreach (SubReddit localSubReddit in this.LocalSubRedditList)
      {
        if (localSubReddit.RName.Equals(old.RName))
          subReddit1 = localSubReddit;
      }
      if (subReddit1 == null)
        return false;
      this.LocalSubRedditList.Remove(subReddit1);
      this.LocalSubRedditList.Add(newSub);
      this.SaveLocalSubRedditList();
      SubReddit subReddit2 = (SubReddit) null;
      int index = 0;
      foreach (SubReddit sortedComb in this.SortedCombList)
      {
        if (sortedComb.RName.Equals(old.RName))
        {
          subReddit2 = sortedComb;
          break;
        }
        ++index;
      }
      if (subReddit2 == null)
        return false;
      this.SortedCombList.Remove(subReddit2);
      this.SortedCombList.Insert(index, newSub);
      this.SaveSortedCombList();
      if (this.SubRedditsUpdated != null)
        this.SubRedditsUpdated(true, this.SortedCombList, true);
      return true;
    }

    public void RemoveFromLocalSubReddit(SubReddit subreddit, bool UpdateUI)
    {
      SubReddit subReddit1 = (SubReddit) null;
      foreach (SubReddit localSubReddit in this.LocalSubRedditList)
      {
        if (localSubReddit.RName.Equals(subreddit.RName))
        {
          subReddit1 = localSubReddit;
          break;
        }
      }
      if (subReddit1 != null)
      {
        this.LocalSubRedditList.Remove(subReddit1);
        this.SaveLocalSubRedditList();
        SubReddit subReddit2 = (SubReddit) null;
        foreach (SubReddit sortedComb in this.SortedCombList)
        {
          if (sortedComb.RName.Equals(subreddit.RName))
          {
            subReddit2 = sortedComb;
            break;
          }
        }
        if (subReddit2 != null)
          this.SortedCombList.Remove(subReddit2);
      }
      if (!(this.SubRedditsUpdated != null & UpdateUI))
        return;
      this.SubRedditsUpdated(true, this.SortedCombList, true);
    }

    public void AddToAccount(SubReddit subreddit, bool UpdateUI)
    {
      foreach (SubReddit accountSub in this.AccountSubList)
      {
        if (accountSub.RName.Equals(subreddit.RName))
          return;
      }
      subreddit.isAccount = true;
      subreddit.isLocal = false;
      this.AccountSubList.Add(subreddit);
      this.SaveAccountSubList();
      bool flag = false;
      List<SubReddit> subSortedReddits = this.GetSubSortedReddits();
      for (int index = 0; index < subSortedReddits.Count; ++index)
      {
        if (subSortedReddits[index].DisplayName.CompareTo(subreddit.DisplayName) > 0)
        {
          subSortedReddits.Insert(index, subreddit);
          flag = true;
          break;
        }
      }
      if (!flag)
        subSortedReddits.Add(subreddit);
      this.SaveSortedCombList();
      if (!(this.SubRedditsUpdated != null & UpdateUI))
        return;
      this.SubRedditsUpdated(true, subSortedReddits, true);
    }

    public void RemoveFromAccount(SubReddit subreddit, bool UpdateUI)
    {
      SubReddit subReddit1 = (SubReddit) null;
      foreach (SubReddit accountSub in this.AccountSubList)
      {
        if (accountSub.RName.Equals(subreddit.RName))
        {
          subReddit1 = accountSub;
          break;
        }
      }
      if (subReddit1 != null)
      {
        this.AccountSubList.Remove(subReddit1);
        this.SaveLocalSubRedditList();
        SubReddit subReddit2 = (SubReddit) null;
        foreach (SubReddit sortedComb in this.SortedCombList)
        {
          if (sortedComb.RName.Equals(subreddit.RName))
          {
            subReddit2 = sortedComb;
            break;
          }
        }
        if (subReddit2 != null)
          this.SortedCombList.Remove(subReddit2);
      }
      if (!(this.SubRedditsUpdated != null & UpdateUI))
        return;
      this.SubRedditsUpdated(true, this.SortedCombList, true);
    }

    private List<SubReddit> AccountSubList
    {
      get
      {
        if (this._AccountSubList == null)
        {
          lock (this.DataMan.SettingsMan.settings)
            this._AccountSubList = !this.DataMan.SettingsMan.settings.Contains(nameof (AccountSubList)) ? new List<SubReddit>() : (List<SubReddit>) this.DataMan.SettingsMan.settings[nameof (AccountSubList)];
        }
        return this._AccountSubList;
      }
      set
      {
        lock (this.DataMan.SettingsMan.settings)
        {
          this.DataMan.SettingsMan.settings[nameof (AccountSubList)] = (object) value;
          this._AccountSubList = value;
        }
      }
    }

    private void SaveAccountSubList()
    {
      lock (this.DataMan.SettingsMan.settings)
      {
        this._AccountSubList = this.AccountSubList;
        this.DataMan.SettingsMan.settings["AccountSubList"] = (object) this._AccountSubList;
      }
    }

    private List<SubReddit> LocalSubRedditList
    {
      get
      {
        if (this._LocalSubRedditList == null)
        {
          lock (this.DataMan.SettingsMan.settings)
            this._LocalSubRedditList = !this.DataMan.SettingsMan.settings.Contains("_LocalSubRedditList") ? new List<SubReddit>() : (List<SubReddit>) this.DataMan.SettingsMan.settings["_LocalSubRedditList"];
        }
        return this._LocalSubRedditList;
      }
      set
      {
        lock (this.DataMan.SettingsMan.settings)
        {
          this.DataMan.SettingsMan.settings["_LocalSubRedditList"] = (object) value;
          this._LocalSubRedditList = value;
        }
      }
    }

    private void SaveLocalSubRedditList()
    {
      lock (this.DataMan.SettingsMan.settings)
      {
        this._LocalSubRedditList = this.LocalSubRedditList;
        this.DataMan.SettingsMan.settings["_LocalSubRedditList"] = (object) this._LocalSubRedditList;
      }
    }

    private List<SubReddit> SortedCombList
    {
      get
      {
        if (this._SortedCombList == null)
        {
          lock (this.DataMan.SettingsMan.settings)
            this._SortedCombList = !this.DataMan.SettingsMan.settings.Contains("_SortedCombList") ? new List<SubReddit>() : (List<SubReddit>) this.DataMan.SettingsMan.settings["_SortedCombList"];
        }
        return this._SortedCombList;
      }
      set
      {
        lock (this.DataMan.SettingsMan.settings)
        {
          this.DataMan.SettingsMan.settings["_SortedCombList"] = (object) value;
          this._SortedCombList = value;
        }
      }
    }

    private void SaveSortedCombList()
    {
      lock (this.DataMan.SettingsMan.settings)
      {
        this._SortedCombList = this.SortedCombList;
        this.DataMan.SettingsMan.settings["_SortedCombList"] = (object) this._SortedCombList;
      }
    }

    public void changeFavoriteStatus(string RName, bool isNowFavorite)
    {
      this.FavoriteSubReddits.Remove(RName);
      this.FavoriteSubReddits.Add(RName, isNowFavorite);
      this.SaveFavoriteList();
    }

    public bool checkIfFavorite(string RName)
    {
      return this.FavoriteSubReddits.ContainsKey(RName) && this.FavoriteSubReddits[RName];
    }

    private Dictionary<string, bool> FavoriteSubReddits
    {
      get
      {
        if (this._FavoriteSubReddits == null)
        {
          lock (this.DataMan.SettingsMan.settings)
          {
            if (this.DataMan.SettingsMan.settings.Contains(nameof (FavoriteSubReddits)))
            {
              this._FavoriteSubReddits = (Dictionary<string, bool>) this.DataMan.SettingsMan.settings[nameof (FavoriteSubReddits)];
            }
            else
            {
              this._FavoriteSubReddits = new Dictionary<string, bool>();
              this._FavoriteSubReddits.Add("frontpage", true);
              this._FavoriteSubReddits.Add("t5_2rfk9", true);
              this._FavoriteSubReddits.Add("t5_2r71o", true);
            }
          }
        }
        return this._FavoriteSubReddits;
      }
      set
      {
        lock (this.DataMan.SettingsMan.settings)
        {
          this.DataMan.SettingsMan.settings[nameof (FavoriteSubReddits)] = (object) value;
          this._FavoriteSubReddits = value;
        }
      }
    }

    private void SaveFavoriteList()
    {
      lock (this.DataMan.SettingsMan.settings)
        this.DataMan.SettingsMan.settings["FavoriteSubReddits"] = (object) this.FavoriteSubReddits;
    }

    public delegate void SubredditUpdatedEventHandler(
      bool success,
      List<SubReddit> NewList,
      bool UpdateUI);

    private class SubredditGetterClass
    {
      private SubredditDataManager Host;
      private List<SubReddit> TempHolderSubReddits = new List<SubReddit>();
      private bool TriedToFixList;

      public SubredditGetterClass(SubredditDataManager da) => this.Host = da;

      public void Start() => this.GetPage("");

      public void GetPage(string After)
      {
        string str = HttpUtility.UrlEncode(this.Host.DataMan.SettingsMan.UserCookie);
        if (this.Host.DataMan.SettingsMan.IsSignedIn && str != null)
        {
          HttpWebRequest state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com/reddits/mine.json?after=" + After);
          state.CookieContainer = new CookieContainer();
          state.CookieContainer.Add(new Uri("https://api.reddit.com"), new Cookie()
          {
            Domain = ".reddit.com",
            HttpOnly = false,
            Path = "/",
            Secure = false,
            Value = str,
            Name = "reddit_session"
          });
          state.BeginGetResponse(new AsyncCallback(this.GetPage_Callback), (object) state);
        }
        else
        {
          if (!After.Equals(""))
            return;
          HttpWebRequest state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com/reddits/.json");
          state.BeginGetResponse(new AsyncCallback(this.GetPage_Callback), (object) state);
        }
      }

      public void GetPage_Callback(IAsyncResult result)
      {
        try
        {
          if (result.AsyncState is HttpWebRequest asyncState)
          {
            using (HttpWebResponse response = (HttpWebResponse) asyncState.EndGetResponse(result))
            {
              using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
              {
                JObject jobject = JObject.Parse(streamReader.ReadToEnd());
                List<SubReddit> subRedditList1 = new List<SubReddit>();
                foreach (JToken child in jobject["data"][(object) "children"].Children())
                {
                  SubReddit subReddit = new SubReddit();
                  subReddit.RName = (string) child[(object) "data"][(object) "name"];
                  subReddit.DisplayName = HttpUtility.HtmlDecode((string) child[(object) "data"][(object) "display_name"]);
                  subReddit.created = (float) child[(object) "data"][(object) "created_utc"];
                  subReddit.URL = (string) child[(object) "data"][(object) "url"];
                  subReddit.Title = HttpUtility.HtmlDecode((string) child[(object) "data"][(object) "title"]);
                  subReddit.Subscribers = (int) child[(object) "data"][(object) "subscribers"];
                  subReddit.PublicDescription = HttpUtility.HtmlDecode((string) child[(object) "data"][(object) "public_description"]);
                  subReddit.Description = HttpUtility.HtmlDecode((string) child[(object) "data"][(object) "description"]);
                  if (this.Host.DataMan.SettingsMan.IsSignedIn)
                    subReddit.isAccount = true;
                  else
                    subReddit.isBuiltIn = true;
                  subReddit.isLocal = false;
                  subReddit.isGroup = false;
                  subRedditList1.Add(subReddit);
                }
                string After = "";
                try
                {
                  After = (string) jobject["data"][(object) "after"];
                }
                catch
                {
                }
                if (After == null || After.Equals("") || !this.Host.DataMan.SettingsMan.IsSignedIn)
                {
                  if (subRedditList1.Count == 0 && this.TempHolderSubReddits.Count == 0 && !this.TriedToFixList)
                  {
                    this.TriedToFixList = true;
                    this.Host.SubscribeToBaconit();
                    Thread.Sleep(200);
                    this.GetPage("");
                    return;
                  }
                  foreach (SubReddit tempHolderSubReddit in this.TempHolderSubReddits)
                    subRedditList1.Add(tempHolderSubReddit);
                  bool UpdateUI = false;
                  List<SubReddit> accountSubList = this.Host.AccountSubList;
                  Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
                  List<SubReddit> subRedditList2 = new List<SubReddit>();
                  foreach (SubReddit subReddit in subRedditList1)
                  {
                    if (!dictionary.ContainsKey(subReddit.RName))
                      dictionary.Add(subReddit.RName, false);
                    subRedditList2.Add(subReddit);
                  }
                  int num = 0;
                  foreach (SubReddit subReddit in accountSubList)
                  {
                    if (dictionary.ContainsKey(subReddit.RName))
                      ++num;
                  }
                  if (num != dictionary.Count)
                    UpdateUI = true;
                  if (UpdateUI)
                  {
                    this.Host.AccountSubList = subRedditList2;
                    this.Host.SaveAccountSubList();
                    SubredditDataManager.AddExtraSubreddits(subRedditList1, this.Host.DataMan.SettingsMan.IsSignedIn);
                    foreach (SubReddit localSubReddit in this.Host.LocalSubRedditList)
                      subRedditList1.Add(localSubReddit);
                    this.Host.Sort(subRedditList1);
                    this.Host.SetSubSortedReddits(subRedditList1);
                  }
                  if (this.Host.SubRedditsUpdated != null)
                    this.Host.SubRedditsUpdated(true, subRedditList1, UpdateUI);
                  this.Host.DataMan.BaconitStore.UpdateLastUpdatedTime("!Main_Subreddit", false);
                }
                else
                {
                  foreach (SubReddit subReddit in subRedditList1)
                    this.TempHolderSubReddits.Add(subReddit);
                  this.GetPage(After);
                  return;
                }
              }
            }
          }
          else
          {
            this.Host.DataMan.MessageManager.showWebErrorMessage("loading the subreddits list", (Exception) null);
            if (this.Host.DataMan.SubredditDataManager.SubRedditsUpdated != null)
              this.Host.DataMan.SubredditDataManager.SubRedditsUpdated(false, (List<SubReddit>) null, true);
            this.Host.DataMan.BaconitStore.UpdateLastUpdatedTime("!Main_Subreddit", true);
          }
        }
        catch (Exception ex)
        {
          this.Host.DataMan.MessageManager.showErrorMessage("loading the subreddits list", ex);
          if (this.Host.DataMan.SubredditDataManager.SubRedditsUpdated != null)
            this.Host.DataMan.SubredditDataManager.SubRedditsUpdated(false, (List<SubReddit>) null, true);
          this.Host.DataMan.BaconitStore.UpdateLastUpdatedTime("!Main_Subreddit", true);
        }
        this.Host.UpdatingSubReddits = false;
      }
    }
  }
}
