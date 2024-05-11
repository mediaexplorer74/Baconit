// Type: Baconit.DataManager
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

using Baconit.Database;
using Baconit.Libs;
using BaconitData.Database;
using BaconitData.Interfaces;
using BaconitData.Libs;
using Newtonsoft.Json.Linq;

//using Coding4Fun.Toolkit.Net;
//using Microsoft.Phone.Info;
//using Microsoft.Phone.Shell;
//using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Baconit
{
  public class DataManager
  {
    public static bool IS_BETA = false;
    public static bool IS_DONATE = false;
    public static double VERSION = 3.01;
    public static string PUSH_CHANNEL = "BaconSync";
    public static bool LIGHT_THEME = false;
    public static bool IS_LOW_MEMORY_DEVICE = false;
    public static Color ACCENT_COLOR_COLOR;
    public static string ACCENT_COLOR = "";
    public static double DEBUG_TIME = 0.0;
    public bool RunningFromUpdater;
    public bool FirstRunAfterUpdate;
    public IsolatedStorageFile FileStorage;
    public BaconSyncInterface BaconSyncObj;
    public SubredditDataManager SubredditDataManager;
    public MessageManagerInterface MessageManager;
    public SpecialImageManager SpecialImageManager;
    public BaconitStore BaconitStore;
    public BaconitAnalytics BaconitAnalytics;
    public ShareHelper ShareHelper;
    public TileMan TileMan;
    public StartUpCheckMan StartUpCheckMan;
    public LogMan LogMan;
    public MainLandingImageManager MainLandingImageMan;
    public CacheManager CacheMan;
    public VIPDataManager VIPDataMan;
    public BaconitAnalyticsInterface AnalyticsManagerInter;
    public SettingsManager SettingsMan;
    public long TotalMemory;
    public int ShowMemoryWarning;
    public const int REDDIT_TYPE_HOT = 0;
    public const int REDDIT_TYPE_NEW = 1;
    public const int REDDIT_TYPE_TOP = 2;
    public const int REDDIT_TYPE_CONT = 3;
    public const int TOP_TODAY = 2;
    public const int TOP_HOUR = 6;
    public const int TOP_WEEK = 7;
    public const int TOP_MONTH = 8;
    public const int TOP_YEAR = 9;
    public const int TOP_ALLTIME = 10;
    public const int NEW_NEW = 1;
    public const int NEW_RISING = 12;
    public const int COMMENT_SORT_BEST = 0;
    public const int COMMENT_SORT_TOP = 1;
    public const int COMMENT_SORT_NEW = 2;
    public const int COMMENT_SORT_HOT = 3;
    public const int COMMENT_SORT_CONV = 4;
    public const int COMMENT_SORT_OLD = 5;
    public const int COMMENT_SORT_SUBSET = 6;
    public static Dictionary<string, DateTime> BeingUpdated = new Dictionary<string, DateTime>();
    public Dictionary<string, KeyValuePair<string, bool>> CommentSubRedditMap = new Dictionary<string, KeyValuePair<string, bool>>();
    public CaptchaInfo CaptchaInfoHolder;
    private Random random = new Random((int) DateTime.Now.Ticks);
    public static string[] LiveTileBackgrounds = new string[7];

    public DataManager(bool isUpdater)
    {
      this.RunningFromUpdater = isUpdater;
      this.FileStorage = IsolatedStorageFile.GetUserStoreForApplication();
      lock (this.FileStorage)
      {
        if (this.FileStorage.GetDirectoryNames("Thumbs").Length == 0)
          this.FileStorage.CreateDirectory("Thumbs");
      }
      DataManager.LiveTileBackgrounds[0] = "/Images/LiveTiles/BaconDispenser_";
      DataManager.LiveTileBackgrounds[1] = "/Images/LiveTiles/RedditAlieanNoBack_";
      DataManager.LiveTileBackgrounds[2] = "/Images/LiveTiles/RedditAlieanNoFill_";
      DataManager.LiveTileBackgrounds[3] = "/Images/LiveTiles/Face_";
      DataManager.LiveTileBackgrounds[4] = "/Images/LiveTiles/MessageBackground_";
      DataManager.LiveTileBackgrounds[5] = "/Images/LiveTiles/RedditAliean_";
      DataManager.LiveTileBackgrounds[6] = "/Images/LiveTiles/OldBack_";
    }

    public void InitDataMan(
      BaconSyncInterface BaconSyncInt,
      MessageManagerInterface MessageManagerInt,
      BaconitAnalyticsInterface BaconitAnalyticsInt)
    {
      this.SettingsMan = new SettingsManager(this);
      this.BaconitStore = new BaconitStore(this);
      if (!this.RunningFromUpdater)
      {
        this.BaconSyncObj = BaconSyncInt;
        this.SubredditDataManager = new SubredditDataManager(this);
        this.MessageManager = MessageManagerInt;
        this.AnalyticsManagerInter = BaconitAnalyticsInt;
        this.BaconitAnalytics = new BaconitAnalytics(this);
        this.ShareHelper = new ShareHelper(this);
        this.StartUpCheckMan = new StartUpCheckMan(this);
        this.MainLandingImageMan = new MainLandingImageManager(this);
        this.CacheMan = new CacheManager();
        this.BaconitStore.Initialize();
      }
      this.VIPDataMan = new VIPDataManager(this);
      this.SpecialImageManager = new SpecialImageManager(this);
      this.TileMan = new TileMan(this);
      this.LogMan = new LogMan(this);
      WebRequest.RegisterPrefix("http://", WebRequestCreator.Gzip);
      WebRequest.RegisterPrefix("https://", WebRequestCreator.Gzip);
      if (this.SettingsMan.WasAppRest && this.SettingsMan.AppVersion != 0.0)
      {
        this.SettingsMan.WasAppRest = false;
        this.BaconitStore.ResetAllTimes();
      }
      if (this.SettingsMan.AppVersion != DataManager.VERSION)
      {
        if (this.SettingsMan.AppVersion <= 0.0)
          this.SettingsMan.OpenedAppCounter = 0;
        if (this.SettingsMan.AppVersion <= 3.0)
        {
          this.SetIfExists("userName");
          this.SetIfExists("_SettingsMan.ModHash");
          this.SetIfExists("_SettingsMan.UserCookie");
          this.SetIfExists("_SettingsMan.UserAccountCreated");
          this.SetIfExists("_SettingsMan.IsSignedIn");
          this.SetIfExists("_BackgroundAgentEnabled");
          this.SetIfExists("_SettingsMan.AdultFilter");
          this.SetIfExists("_ShowStoryListOverLay");
          this.SetIfExists("_ShowImageViewerHelpOverlay");
          this.SetIfExists("_showStoryOverLay");
          this.SetIfExists("_showStorySelfOverLay");
        }
        this.SettingsMan.AppVersion = DataManager.VERSION;
        this.SettingsMan.DEBUGGING = false;
        this.FirstRunAfterUpdate = true;
      }
      try
      {
        this.TotalMemory = DeviceStatus.ApplicationMemoryUsageLimit;
      }
      catch
      {
        this.TotalMemory = 203079680L;
      }
      if (this.TotalMemory != 0L)
        return;
      this.TotalMemory = 203079680L;
    }

    public void GFYCatGif(string gifUrl, RunWorkerCompletedEventHandler callback)
    {
      string str = "";
      for (int index = 0; index < 10; ++index)
        str += Convert.ToChar(Convert.ToInt32(Math.Floor(26.0 * this.random.NextDouble() + 65.0))).ToString();
      HttpWebRequest state = (HttpWebRequest) WebRequest.Create("http://upload.gfycat.com/transcode/" 
          + str + "?fetchUrl=" + HttpUtility.UrlEncode(gifUrl));
      state.BeginGetResponse((AsyncCallback) (result =>
      {
        try
        {
          if (result.AsyncState is HttpWebRequest asyncState2)
          {
            using (HttpWebResponse response = (HttpWebResponse) asyncState2.EndGetResponse(result))
            {
              using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                callback((object) gifUrl, new RunWorkerCompletedEventArgs((object) (string) JObject.Parse(streamReader.ReadToEnd())["mp4Url"], (Exception) null, false));
            }
          }
          else
            callback((object) gifUrl, new RunWorkerCompletedEventArgs((object) null, (Exception) null, false));
        }
        catch (Exception ex)
        {
          callback((object) gifUrl, new RunWorkerCompletedEventArgs((object) null, ex, false));
        }
      }), (object) state);
    }

    public void GetNewCaptcha(RunWorkerCompletedEventHandler callback)
    {
      string userCookie = this.SettingsMan.UserCookie;
      HttpWebRequest state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com/api/new_captcha/");
      state.CookieContainer = new CookieContainer();
      if (this.SettingsMan.IsSignedIn && userCookie != null)
      {
        string str = HttpUtility.UrlEncode(userCookie);
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
                if (!DataManager.FindStringInJson(jobject["jquery"], ".error.").Equals(""))
                  callback((object) null, new RunWorkerCompletedEventArgs((object) false, (Exception) null, false));
                UserAccountInformation sender = new UserAccountInformation();
                string str = (string) jobject["data"][(object) "name"];
                if (str == null || str.Equals(""))
                  throw new Exception();
                sender.Name = (string) jobject["data"][(object) "name"];
                sender.IsGold = (bool) jobject["data"][(object) "is_gold"];
                sender.Created = (double) jobject["data"][(object) "created_utc"];
                sender.LinkKarma = (int) jobject["data"][(object) "link_karma"];
                sender.CommentKarma = (int) jobject["data"][(object) "comment_karma"];
                callback((object) sender, new RunWorkerCompletedEventArgs((object) true, (Exception) null, false));
              }
            }
          }
          else
            callback((object) null, new RunWorkerCompletedEventArgs((object) false, (Exception) null, false));
        }
        catch
        {
          callback((object) null, new RunWorkerCompletedEventArgs((object) false, (Exception) null, false));
        }
      }), (object) state);
    }

    public static string CheckForCaptchaError(JToken response)
    {
      if (response == null)
        return "";
      try
      {
        if (!response.ToString().Contains(".error.BAD_CAPTCHA.field-captcha"))
          return "";
        foreach (JToken source in (IEnumerable<JToken>) response)
        {
          if (source.Count<JToken>() > 2)
          {
            JToken jtoken = source[(object) 3];
            if (jtoken.Type == JTokenType.Array && jtoken.HasValues)
            {
              string str = (string) jtoken[(object) 0];
              if (str.Length > 30 && !str.Contains(".") && !str.Contains("*") 
                                && !str.Contains("value") && !str.Contains(" "))
                return str;
            }
          }
        }
      }
      catch
      {
      }
      return "";
    }

    public void GetAccountInformation(string userName, RunWorkerCompletedEventHandler callback)
    {
      if (userName == null)
        return;
      string userCookie = this.SettingsMan.UserCookie;
      userName = HttpUtility.UrlEncode(userName);
      HttpWebRequest state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com/user/" 
          + userName + "/about.json");
      state.CookieContainer = new CookieContainer();
      if (this.SettingsMan.IsSignedIn && userCookie != null)
      {
        string str = HttpUtility.UrlEncode(userCookie);
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
                if (!DataManager.FindStringInJson(jobject["jquery"], ".error.").Equals(""))
                  callback((object) null, new RunWorkerCompletedEventArgs((object) false, (Exception) null, false));
                UserAccountInformation sender = new UserAccountInformation();
                string str = (string) jobject["data"][(object) "name"];
                if (str == null || str.Equals(""))
                  throw new Exception();
                sender.Name = (string) jobject["data"][(object) "name"];
                sender.IsGold = (bool) jobject["data"][(object) "is_gold"];
                sender.Created = (double) jobject["data"][(object) "created_utc"];
                sender.LinkKarma = (int) jobject["data"][(object) "link_karma"];
                sender.CommentKarma = (int) jobject["data"][(object) "comment_karma"];
                callback((object) sender, new RunWorkerCompletedEventArgs((object) true, (Exception) null, false));
              }
            }
          }
          else
            callback((object) null, new RunWorkerCompletedEventArgs((object) false, (Exception) null, false));
        }
        catch
        {
          callback((object) null, new RunWorkerCompletedEventArgs((object) false, (Exception) null, false));
        }
      }), (object) state);
    }

    public void GetAccountComments(string userName, RunWorkerCompletedEventHandler callback)
    {
      if (userName == null)
        return;
      string userCookie = this.SettingsMan.UserCookie;
      userName = HttpUtility.UrlEncode(userName);
      HttpWebRequest state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com/user/"
          + userName + "/comments/.json");
      state.CookieContainer = new CookieContainer();
      if (this.SettingsMan.IsSignedIn && userCookie != null)
      {
        string str = HttpUtility.UrlEncode(userCookie);
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
                  this.MessageManager.showRedditErrorMessage("get this account's comments", stringInJson);
                List<CommentData> sender = new List<CommentData>();
                foreach (JToken jtoken1 in (IEnumerable<JToken>) jobject["data"][(object) "children"])
                {
                  CommentData commentData = new CommentData();
                  commentData.Body = (string) jtoken1[(object) "data"][(object) "body"];
                  commentData.SubRedditRName = (string) jtoken1[(object) "data"][(object) "subreddit_id"];
                  commentData.Created = (double) jtoken1[(object) "data"][(object) "created_utc"];
                  commentData.Downs = (int) jtoken1[(object) "data"][(object) "downs"];
                  commentData.Up = (int) jtoken1[(object) "data"][(object) "ups"];
                  commentData.RName = (string) jtoken1[(object) "data"][(object) "name"];
                  commentData.LinkRID = (string) jtoken1[(object) "data"][(object) "link_id"];
                  JToken jtoken2 = jtoken1[(object) "data"][(object) "likes"];
                  commentData.Likes = jtoken2 == null || jtoken2.Type != JTokenType.Boolean ? 0 : (!(bool) jtoken2 ? -1 : 1);
                  commentData.Body2 = (string) jtoken1[(object) "data"][(object) "link_title"];
                  commentData.SubRedditURL = (string) jtoken1[(object) "data"][(object) "subreddit"];
                  sender.Add(commentData);
                }
                callback((object) sender, new RunWorkerCompletedEventArgs((object) true, (Exception) null, false));
              }
            }
          }
          else
            this.MessageManager.showWebErrorMessage("getting this account's comments", (Exception) null);
        }
        catch (Exception ex)
        {
          this.MessageManager.showWebErrorMessage("getting this account's comments", ex);
        }
      }), (object) state);
    }

    public void GetAccountSubmitted(string userName, RunWorkerCompletedEventHandler callback)
    {
      if (userName == null)
        return;
      string userCookie = this.SettingsMan.UserCookie;
      userName = HttpUtility.UrlEncode(userName);
      HttpWebRequest state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com/user/" 
          + userName + "/submitted/.json");
      state.CookieContainer = new CookieContainer();
      if (this.SettingsMan.IsSignedIn && userCookie != null)
      {
        string str = HttpUtility.UrlEncode(userCookie);
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
                  this.MessageManager.showRedditErrorMessage("get this account's submitted stories", stringInJson);
                List<SubRedditData> sender = new List<SubRedditData>();
                foreach (JToken jtoken1 in (IEnumerable<JToken>) jobject["data"][(object) "children"])
                {
                  SubRedditData subRedditData = new SubRedditData();
                  subRedditData.Title = (string) jtoken1[(object) "data"][(object) "title"];
                  subRedditData.Domain = (string) jtoken1[(object) "data"][(object) "domain"];
                  subRedditData.created = (float) jtoken1[(object) "data"][(object) "created_utc"];
                  subRedditData.score = (int) jtoken1[(object) "data"][(object) "score"];
                  subRedditData.comments = (int) jtoken1[(object) "data"][(object) "num_comments"];
                  subRedditData.Author = (string) jtoken1[(object) "data"][(object) "author"];
                  subRedditData.RedditID = (string) jtoken1[(object) "data"][(object) "name"];
                  subRedditData.SubReddit = (string) jtoken1[(object) "data"][(object) "subreddit"];
                  JToken jtoken2 = jtoken1[(object) "data"][(object) "likes"];
                  subRedditData.Like = jtoken2 == null || jtoken2.Type != JTokenType.Boolean ? 0 : (!(bool) jtoken2 ? -1 : 1);
                  sender.Add(subRedditData);
                }
                callback((object) sender, new RunWorkerCompletedEventArgs((object) true, (Exception) null, false));
              }
            }
          }
          else
            this.MessageManager.showWebErrorMessage("getting this account's submitted stories", (Exception) null);
        }
        catch (Exception ex)
        {
          this.MessageManager.showWebErrorMessage("getting this account's submitted stories", ex);
        }
      }), (object) state);
    }

    public void GetAccountLiked(string userName, RunWorkerCompletedEventHandler callback)
    {
      if (userName == null)
        return;
      string userCookie = this.SettingsMan.UserCookie;
      userName = HttpUtility.UrlEncode(userName);
      HttpWebRequest state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com/user/" 
          + userName + "/liked/.json");
      state.CookieContainer = new CookieContainer();
      if (this.SettingsMan.IsSignedIn && userCookie != null)
      {
        string str = HttpUtility.UrlEncode(userCookie);
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
                  this.MessageManager.showRedditErrorMessage("get this account's liked stories", stringInJson);
                List<SubRedditData> sender = new List<SubRedditData>();
                foreach (JToken jtoken1 in (IEnumerable<JToken>) jobject["data"][(object) "children"])
                {
                  SubRedditData subRedditData = new SubRedditData();
                  subRedditData.Title = (string) jtoken1[(object) "data"][(object) "title"];
                  subRedditData.Domain = (string) jtoken1[(object) "data"][(object) "domain"];
                  subRedditData.created = (float) jtoken1[(object) "data"][(object) "created_utc"];
                  subRedditData.score = (int) jtoken1[(object) "data"][(object) "score"];
                  subRedditData.comments = (int) jtoken1[(object) "data"][(object) "num_comments"];
                  subRedditData.Author = (string) jtoken1[(object) "data"][(object) "author"];
                  subRedditData.RedditID = (string) jtoken1[(object) "data"][(object) "name"];
                  subRedditData.SubReddit = (string) jtoken1[(object) "data"][(object) "subreddit"];
                  JToken jtoken2 = jtoken1[(object) "data"][(object) "likes"];
                  subRedditData.Like = jtoken2 == null || jtoken2.Type != JTokenType.Boolean ? 0 : (!(bool) jtoken2 ? -1 : 1);
                  sender.Add(subRedditData);
                }
                callback((object) sender, new RunWorkerCompletedEventArgs((object) true, (Exception) null, false));
              }
            }
          }
          else
            this.MessageManager.showWebErrorMessage("getting this account's liked stories", (Exception) null);
        }
        catch (Exception ex)
        {
          this.MessageManager.showWebErrorMessage("getting this account's liked stories", ex);
        }
      }), (object) state);
    }

    public void GetAccountDisliked(string userName, RunWorkerCompletedEventHandler callback)
    {
      if (userName == null)
        return;
      string userCookie = this.SettingsMan.UserCookie;
      userName = HttpUtility.UrlEncode(userName);
      HttpWebRequest state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com/user/" 
          + userName + "/disliked/.json");
      state.CookieContainer = new CookieContainer();
      if (this.SettingsMan.IsSignedIn && userCookie != null)
      {
        string str = HttpUtility.UrlEncode(userCookie);
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
                  this.MessageManager.showRedditErrorMessage("get this account's disliked stories", stringInJson);
                List<SubRedditData> sender = new List<SubRedditData>();
                foreach (JToken jtoken1 in (IEnumerable<JToken>) jobject["data"][(object) "children"])
                {
                  SubRedditData subRedditData = new SubRedditData();
                  subRedditData.Title = (string) jtoken1[(object) "data"][(object) "title"];
                  subRedditData.Domain = (string) jtoken1[(object) "data"][(object) "domain"];
                  subRedditData.created = (float) jtoken1[(object) "data"][(object) "created_utc"];
                  subRedditData.score = (int) jtoken1[(object) "data"][(object) "score"];
                  subRedditData.comments = (int) jtoken1[(object) "data"][(object) "num_comments"];
                  subRedditData.Author = (string) jtoken1[(object) "data"][(object) "author"];
                  subRedditData.RedditID = (string) jtoken1[(object) "data"][(object) "name"];
                  subRedditData.SubReddit = (string) jtoken1[(object) "data"][(object) "subreddit"];
                  JToken jtoken2 = jtoken1[(object) "data"][(object) "likes"];
                  subRedditData.Like = jtoken2 == null || jtoken2.Type != JTokenType.Boolean ? 0 : (!(bool) jtoken2 ? -1 : 1);
                  sender.Add(subRedditData);
                }
                callback((object) sender, new RunWorkerCompletedEventArgs((object) true, (Exception) null, false));
              }
            }
          }
          else
            this.MessageManager.showWebErrorMessage("getting this account's disliked stories", (Exception) null);
        }
        catch (Exception ex)
        {
          this.MessageManager.showWebErrorMessage("getting this account's disliked stories", ex);
        }
      }), (object) state);
    }

    public void GetAccountHidden(string userName, RunWorkerCompletedEventHandler callback)
    {
      if (userName == null)
        return;
      string userCookie = this.SettingsMan.UserCookie;
      userName = HttpUtility.UrlEncode(userName);
      HttpWebRequest state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com/user/" 
          + userName + "/hidden/.json");
      state.CookieContainer = new CookieContainer();
      if (this.SettingsMan.IsSignedIn && userCookie != null)
      {
        string str = HttpUtility.UrlEncode(userCookie);
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
                  this.MessageManager.showRedditErrorMessage("get this account's hidden stories", stringInJson);
                List<SubRedditData> sender = new List<SubRedditData>();
                foreach (JToken jtoken1 in (IEnumerable<JToken>) jobject["data"][(object) "children"])
                {
                  SubRedditData subRedditData = new SubRedditData();
                  subRedditData.Title = (string) jtoken1[(object) "data"][(object) "title"];
                  subRedditData.Domain = (string) jtoken1[(object) "data"][(object) "domain"];
                  subRedditData.created = (float) jtoken1[(object) "data"][(object) "created_utc"];
                  subRedditData.score = (int) jtoken1[(object) "data"][(object) "score"];
                  subRedditData.comments = (int) jtoken1[(object) "data"][(object) "num_comments"];
                  subRedditData.Author = (string) jtoken1[(object) "data"][(object) "author"];
                  subRedditData.RedditID = (string) jtoken1[(object) "data"][(object) "name"];
                  subRedditData.SubReddit = (string) jtoken1[(object) "data"][(object) "subreddit"];
                  JToken jtoken2 = jtoken1[(object) "data"][(object) "likes"];
                  subRedditData.Like = jtoken2 == null || jtoken2.Type != JTokenType.Boolean ? 0 : (!(bool) jtoken2 ? -1 : 1);
                  sender.Add(subRedditData);
                }
                callback((object) sender, new RunWorkerCompletedEventArgs((object) true, (Exception) null, false));
              }
            }
          }
          else
            this.MessageManager.showWebErrorMessage("getting this account's hidden stories", (Exception) null);
        }
        catch (Exception ex)
        {
          this.MessageManager.showWebErrorMessage("getting this account's hidden stories", ex);
        }
      }), (object) state);
    }

    public void SendMessage(MessageComposed message, RunWorkerCompletedEventHandler callback)
    {
      if (message.userName == null)
        return;
      string str1 = HttpUtility.UrlEncode(this.SettingsMan.UserCookie);
      bool isInCaptchaState = false;
      if (!string.IsNullOrWhiteSpace(message.CapResponse) && !string.IsNullOrWhiteSpace(message.CapID))
        isInCaptchaState = true;
      if (!this.SettingsMan.IsSignedIn || str1 == null)
        return;
      Dictionary<string, object> parameters = new Dictionary<string, object>();
      parameters.Add("subject", (object) message.subject);
      parameters.Add("text", (object) message.text);
      parameters.Add("thing_id", (object) "");
      parameters.Add("to", (object) message.userName);
      parameters.Add("uh", (object) this.SettingsMan.ModHash);
      if (isInCaptchaState)
      {
        parameters.Add("iden", (object) HttpUtility.UrlEncode(message.CapID));
        parameters.Add("captcha", (object) HttpUtility.UrlEncode(message.CapResponse));
      }
      PostClient postClient = new PostClient((IDictionary<string, object>) parameters);
      postClient.CookieContainer = new CookieContainer();
      Cookie cookie = new Cookie();
      cookie.Domain = ".reddit.com";
      cookie.HttpOnly = false;
      cookie.Path = "/";
      cookie.Secure = false;
      cookie.Value = str1;
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
            JObject jobject = JObject.Parse(e.Result);
            string str2 = DataManager.CheckForCaptchaError(jobject["jquery"]);
            if (!string.IsNullOrEmpty(str2))
            {
              if (!isInCaptchaState)
              {
                this.CaptchaInfoHolder = new CaptchaInfo()
                {
                  Info = (object) message,
                  type = 2,
                  CaptchaID = str2
                };
                callback((object) "captcha", new RunWorkerCompletedEventArgs((object) false, 
                    (Exception) new CaptchaException(), false));
              }
              else
                callback((object) "captcha-failed", new RunWorkerCompletedEventArgs((object) false, 
                    (Exception) new CaptchaException()
                {
                  NewCaptchaID = str2
                }, false));
            }
            else
            {
              string stringInJson;
              if (!(stringInJson = DataManager.FindStringInJson(jobject["jquery"], ".error.")).Equals(""))
              {
                if (!isInCaptchaState)
                  callback((object) stringInJson, new RunWorkerCompletedEventArgs((object) false, 
                      (Exception) null, false));
                else
                  callback((object) stringInJson, new RunWorkerCompletedEventArgs((object) false, 
                      (Exception) new CaptchaException(), false));
              }
              else
              {
                try
                {
                  JToken jtoken = jobject["jquery"][(object) 0];
                  callback((object) null, new RunWorkerCompletedEventArgs((object) true, 
                      (Exception) null, false));
                }
                catch
                {
                  if (isInCaptchaState)
                    throw new Exception();
                  callback((object) stringInJson, new RunWorkerCompletedEventArgs((object) false, (Exception) null, 
                      false));
                }
              }
            }
          }
          else
          {
            if (isInCaptchaState)
              throw new Exception();
            callback((object) "", new RunWorkerCompletedEventArgs((object) false, (Exception) null, false));
          }
        }
        catch
        {
          if (!isInCaptchaState)
            callback((object) "", new RunWorkerCompletedEventArgs((object) false, (Exception) null, false));
          else
            callback((object) "captcha-failed-net-error", new RunWorkerCompletedEventArgs((object) false, 
                (Exception) new CaptchaException(), false));
        }
      });
      postClient.DownloadStringAsync(new Uri("https://www.reddit.com/api/compose", UriKind.Absolute));
    }

    public void CheckIfValidSubreddit(
      string url,
      bool reportError,
      RunWorkerCompletedEventHandler callback)
    {
      if (!url.EndsWith("/"))
        url += "/";
      HttpWebRequest state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com" + url + "about.json");
      state.CookieContainer = new CookieContainer();
      state.BeginGetResponse((AsyncCallback) (result =>
      {
        try
        {
          if (result.AsyncState is HttpWebRequest asyncState2)
          {
            using (WebResponse response = asyncState2.EndGetResponse(result))
            {
              using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
              {
                try
                {
                  JToken jtoken = JObject.Parse(streamReader.ReadToEnd())["data"];
                  callback((object) new SubReddit()
                  {
                    DisplayName = (string) jtoken[(object) "display_name"],
                    isAccount = false,
                    isLocal = false,
                    RName = ("t5_" + (string) jtoken[(object) "id"]),
                    Title = (string) jtoken[(object) "title"],
                    URL = (string) jtoken[(object) nameof (url)]
                  }, new RunWorkerCompletedEventArgs((object) "valid", (Exception) null, false));
                }
                catch (Exception ex)
                {
                  callback((object) null, new RunWorkerCompletedEventArgs((object) "notValid", 
                      (Exception) null, false));
                }
              }
            }
          }
          else
          {
            this.MessageManager.showWebErrorMessage("validating the subreddit", (Exception) null);
            callback((object) null, new RunWorkerCompletedEventArgs((object) "NetworkError",
                (Exception) null, false));
          }
        }
        catch (Exception ex)
        {
          bool flag = true;
          try
          {
            if (ex.Message.Equals("The remote server returned an error: NotFound."))
            {
              callback((object) null, new RunWorkerCompletedEventArgs((object) "notValid", (Exception) null, false));
              flag = false;
            }
          }
          catch
          {
          }
          if (!flag)
            return;
          if (reportError)
            this.MessageManager.showErrorMessage("validating the subreddit", ex);
          callback((object) null, new RunWorkerCompletedEventArgs((object) "NetworkError", (Exception) null, false));
        }
      }), (object) state);
    }

    public void CheckIfValidUser(
      string user,
      bool reportError,
      RunWorkerCompletedEventHandler callback)
    {
      HttpWebRequest state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com/user/" 
          + user + "/about.json");
      state.CookieContainer = new CookieContainer();
      state.BeginGetResponse((AsyncCallback) (result =>
      {
        try
        {
          if (result.AsyncState is HttpWebRequest asyncState2)
          {
            using (WebResponse response = asyncState2.EndGetResponse(result))
            {
              using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
              {
                try
                {
                  JToken jtoken = JObject.Parse(streamReader.ReadToEnd())["data"];
                  callback((object) new UserAccountInformation()
                  {
                    Name = (string) jtoken[(object) "name"],
                    CommentKarma = (int) jtoken[(object) "comment_karma"],
                    LinkKarma = (int) jtoken[(object) "link_karma"]
                  }, new RunWorkerCompletedEventArgs((object) "valid", (Exception) null, false));
                }
                catch (Exception ex)
                {
                  callback((object) null, new RunWorkerCompletedEventArgs((object) "notValid",
                      (Exception) null, false));
                }
              }
            }
          }
          else
          {
            this.MessageManager.showWebErrorMessage("validating the subreddit", (Exception) null);
            callback((object) null, new RunWorkerCompletedEventArgs((object) "NetworkError", (Exception) null, false));
          }
        }
        catch (Exception ex)
        {
          bool flag = true;
          try
          {
            if (ex.Message.Equals("The remote server returned an error: NotFound."))
            {
              callback((object) null, new RunWorkerCompletedEventArgs((object) "notValid", (Exception) null, false));
              flag = false;
            }
          }
          catch
          {
          }
          if (!flag)
            return;
          if (reportError)
            this.MessageManager.showErrorMessage("validating the subreddit", ex);
          callback((object) null, new RunWorkerCompletedEventArgs((object) "NetworkError", (Exception) null, false));
        }
      }), (object) state);
    }

    public void SearchReddit(string term, RunWorkerCompletedEventHandler callback)
    {
      HttpWebRequest state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com/search.json?q=" + term);
      state.CookieContainer = new CookieContainer();
      state.BeginGetResponse((AsyncCallback) (result =>
      {
        try
        {
          if (result.AsyncState is HttpWebRequest asyncState2)
          {
            using (WebResponse response = asyncState2.EndGetResponse(result))
            {
              using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
              {
                try
                {
                  JObject jobject = JObject.Parse(streamReader.ReadToEnd());
                  List<SubRedditData> sender = new List<SubRedditData>();
                  List<LongTextData> longTextData1 = new List<LongTextData>();
                  JToken jtoken1 = jobject["data"][(object) "children"][(object) 0];
                  int num1 = 0;
                  int key = 0;
                  string subReddit = "searchResults";
                  int type = 0;
                  while (jtoken1 != null)
                  {
                    SubRedditData subRedditData1 = new SubRedditData();
                    subRedditData1.ID = num1;
                    subRedditData1.SubRedditURL = subReddit;
                    subRedditData1.SubTypeOfSubReddit = type;
                    subRedditData1.SubRedditRank = num1;
                    subRedditData1.Domain = (string) jtoken1[(object) "data"][(object) "domain"];
                    subRedditData1.SubReddit = (string) jtoken1[(object) "data"][(object) "subreddit"];
                    subRedditData1.RedditID = (string) jtoken1[(object) "data"][(object) "name"];
                    subRedditData1.Author = (string) jtoken1[(object) "data"][(object) "author"];
                    subRedditData1.score = (int) jtoken1[(object) "data"][(object) "score"];
                    subRedditData1.thumbnail = (string) jtoken1[(object) "data"][(object) "thumbnail"];
                    subRedditData1.downs = (int) jtoken1[(object) "data"][(object) "downs"];
                    subRedditData1.isSelf = (bool) jtoken1[(object) "data"][(object) "is_self"];
                    subRedditData1.created = (float) jtoken1[(object) "data"][(object) "created_utc"];
                    subRedditData1.URL = (string) jtoken1[(object) "data"][(object) "url"];
                    subRedditData1.Title = (string) jtoken1[(object) "data"][(object) "title"];
                    subRedditData1.comments = (int) jtoken1[(object) "data"][(object) "num_comments"];
                    subRedditData1.ups = (int) jtoken1[(object) "data"][(object) "ups"];
                    subRedditData1.Permalink = (string) jtoken1[(object) "data"][(object) "permalink"];
                    subRedditData1.SelfText = (string) jtoken1[(object) "data"][(object) "selftext"];
                    subRedditData1.RedditRealID = (string) jtoken1[(object) "data"][(object) "id"];
                    subRedditData1.isPinned = false;
                    try
                    {
                      subRedditData1.isNotSafeForWork = (bool) jtoken1[(object) "data"][(object) "over_18"];
                    }
                    catch
                    {
                      subRedditData1.isNotSafeForWork = false;
                    }
                    bool flag;
                    if (this.SettingsMan.AdultFilter)
                    {
                      subRedditData1.Title = DataManager.FilterText(subRedditData1.Title);
                      subRedditData1.SelfText = DataManager.FilterText(subRedditData1.SelfText);
                      flag = subRedditData1.isNotSafeForWork;
                    }
                    else
                      flag = false;
                    if (subRedditData1.SelfText.Length > 0)
                    {
                      subRedditData1.hasSelfText = true;
                      int num2 = 0;
                      while (subRedditData1.SelfText.Length > 0)
                      {
                        LongTextData longTextData2 = new LongTextData();
                        longTextData2.RStoryName = subRedditData1.RedditID;
                        longTextData2.PartNum = num2;
                        longTextData2.SubRedditUrl = subReddit;
                        longTextData2.isPinned = false;
                        longTextData2.SubType = type;
                        if (subRedditData1.SelfText.Length > 4000)
                        {
                          longTextData2.text = subRedditData1.SelfText.Substring(0, 4000);
                          SubRedditData subRedditData2 = subRedditData1;
                          subRedditData2.SelfText = subRedditData2.SelfText.Substring(4000);
                        }
                        else
                        {
                          longTextData2.text = subRedditData1.SelfText.Substring(0, subRedditData1.SelfText.Length);
                          subRedditData1.SelfText = "";
                        }
                        longTextData1.Add(longTextData2);
                        ++num2;
                      }
                    }
                    else
                      subRedditData1.hasSelfText = false;
                    JToken jtoken2 = jtoken1[(object) "data"][(object) "likes"];
                    subRedditData1.Like = jtoken2 == null || jtoken2.Type != JTokenType.Boolean ? 0 : (!(bool) jtoken2 ? -1 : 1);
                    try
                    {
                      subRedditData1.isSaved = (bool) jtoken1[(object) "data"][(object) "saved"];
                    }
                    catch
                    {
                      subRedditData1.isSaved = false;
                    }
                    try
                    {
                      subRedditData1.isHidden = (bool) jtoken1[(object) "data"][(object) "hidden"];
                    }
                    catch
                    {
                      subRedditData1.isHidden = false;
                    }
                    if (!flag)
                    {
                      sender.Add(subRedditData1);
                      ++num1;
                    }
                    ++key;
                    try
                    {
                      jtoken1 = jobject["data"][(object) "children"][(object) key];
                    }
                    catch
                    {
                      break;
                    }
                  }
                  callback((object) sender, new RunWorkerCompletedEventArgs((object) true, (Exception) null, false));
                  this.BaconitStore.SetLongText(longTextData1, subReddit, type, false);
                }
                catch
                {
                  callback((object) null, new RunWorkerCompletedEventArgs((object) false, (Exception) null, false));
                }
              }
            }
          }
          else
          {
            this.MessageManager.showWebErrorMessage("searching reddit", (Exception) null);
            callback((object) null, new RunWorkerCompletedEventArgs((object) false, (Exception) null, true));
          }
        }
        catch (Exception ex)
        {
          this.MessageManager.showErrorMessage("searching reddit", ex);
          callback((object) null, new RunWorkerCompletedEventArgs((object) false, (Exception) null, true));
        }
      }), (object) state);
    }

    public void SendErrorReport(string exception)
    {
      PostClient postClient = new PostClient((IDictionary<string, object>) new Dictionary<string, object>()
      {
        [nameof (exception)] = (object) HttpUtility.UrlEncode(exception),
        ["AppVersion"] = (object) HttpUtility.UrlEncode(string.Concat((object) DataManager.VERSION))
      });
      postClient.CookieContainer = new CookieContainer();
      postClient.DownloadStringCompleted += (PostClient.DownloadStringCompletedHandler) ((sender, e) =>
      {
        try
        {
          if (e.Result.Equals("good"))
            return;
          this.DebugDia("bad resonse sending send crash report", e.Error);
        }
        catch (Exception ex)
        {
          this.DebugDia("send crash report", ex);
        }
      });
      postClient.DownloadStringAsync(new Uri("http://www.quinndamerell.com/Baconit/reportError.php", UriKind.Absolute));
    }

    public void GetMessageOfTheDay()
    {
      PostClient postClient = new PostClient((IDictionary<string, object>) new Dictionary<string, object>());
      postClient.CookieContainer = new CookieContainer();
      postClient.DownloadStringCompleted += (PostClient.DownloadStringCompletedHandler) ((sender, e) =>
      {
        try
        {
          bool flag1 = false;
          bool flag2 = false;
          bool flag3 = false;
          if (e.Error != null)
            return;
          JObject jobject = JObject.Parse(e.Result);
          string title = (string) jobject["Title"];
          string message = (string) jobject["Message"];
          string url = (string) jobject["URL"];
          int num = int.Parse((string) jobject["OpenedTimes"]);
          if (((string) jobject["Critical"]).Equals("yes"))
            flag1 = true;
          if (((string) jobject["BetaOnly"]).Equals("yes"))
            flag2 = true;
          if (((string) jobject["Ingnore"]).Equals("yes"))
            flag3 = true;
          this.BaconitStore.UpdateLastUpdatedTime("!MessageOfTheDay", false);
          if (this.SettingsMan.MessageOfTheDay.Equals(title + message + flag1.ToString()))
            return;
          if (this.SettingsMan.OpenedAppCounter < num)
            this.SettingsMan.MessageOfTheDay = "notOpenedEnough";
          else if (this.SettingsMan.MessageOfTheDay.Equals(""))
          {
            this.SettingsMan.MessageOfTheDay = "nextTime";
            this.BaconitStore.UpdateLastUpdatedTime("!MessageOfTheDay", true);
          }
          else
          {
            bool flag4 = true;
            if (flag3)
              flag4 = false;
            if (flag2 && !DataManager.IS_BETA)
              flag4 = false;
            if (flag4)
              Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
              {
                if (url != null && !url.Equals(""))
                {
                  BaconitUserMessage message1 = new BaconitUserMessage("", false, false, title, message);
                  message1.SetLongMessage("ok", "cancel", (RunWorkerCompletedEventHandler) ((o, arg) =>
                  {
                    if (arg.Result != null)
                    {
                      switch ((int) arg.Result)
                      {
                        case 0:
                          this.BaconitAnalytics.LogEvent("Message Of The Day - Accepted");
                          Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
                          {
                            try
                            {
                              this.BaconSyncObj.HandlePushUrl(url);
                            }
                            catch
                            {
                            }
                          }));
                          break;
                        case 1:
                          this.BaconitAnalytics.LogEvent("Message Of The Day - Ingored");
                          break;
                      }
                    }
                    else
                      this.BaconitAnalytics.LogEvent("Message Of The Day - Ingored");
                  }));
                  this.MessageManager.QueueMessage(message1);
                }
                else
                  this.MessageManager.QueueMessage(new BaconitUserMessage("", false, false, title, message));
              }));
            this.SettingsMan.MessageOfTheDay = title + message + flag1.ToString();
          }
        }
        catch (Exception ex)
        {
          this.DebugDia("motd", ex);
        }
      });
      postClient.DownloadStringAsync(new Uri("http://dl.dropbox.com/u/451896/motd.txt", UriKind.Absolute));
    }

    public void PostImageToImagur(
      BitmapImage image,
      string title,
      RunWorkerCompletedEventHandler callback)
    {
      Dictionary<string, object> parameters = new Dictionary<string, object>();
      parameters.Add("key", (object) "1a507266cc9ac194b56e2700a67185e4");
      if (title != null && !title.Equals(""))
        parameters.Add(nameof (title), (object) "1a507266cc9ac194b56e2700a67185e4");
      byte[] imageData = (byte[]) null;
      using (AutoResetEvent are = new AutoResetEvent(false))
      {
        Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
        {
          try
          {
            using (MemoryStream targetStream = new MemoryStream())
            {
              new WriteableBitmap((BitmapSource) image).SaveJpeg((Stream) targetStream, image.PixelWidth, image.PixelHeight, 0, 100);
              targetStream.Seek(0L, SeekOrigin.Begin);
              imageData = new byte[targetStream.Length];
              targetStream.Read(imageData, 0, imageData.Length);
            }
            are.Set();
          }
          catch
          {
            are.Set();
          }
        }));
        are.WaitOne();
      }
      parameters.Add(nameof (image), (object) HttpUtility.UrlEncode(Convert.ToBase64String(imageData)));
      PostClient postClient = new PostClient((IDictionary<string, object>) parameters);
      postClient.CookieContainer = new CookieContainer();
      Cookie cookie = new Cookie();
      cookie.Domain = ".reddit.com";
      cookie.HttpOnly = false;
      cookie.Path = "/";
      cookie.Secure = false;
      cookie.Value = "fdjkaf";
      cookie.Name = "reddit_session";
      postClient.CookieContainer.Add(new Uri("https://api.reddit.com"), cookie);
      postClient.CookieContainer.Add(new Uri("https://reddit.com"), cookie);
      postClient.CookieContainer.Add(new Uri("https://www.reddit.com"), cookie);
      postClient.DownloadStringCompleted += (PostClient.DownloadStringCompletedHandler) ((sender, e) =>
      {
        try
        {
          if (e.Error == null)
            callback((object) (string) JObject.Parse(e.Result)["upload"][(object) "links"][(object) "original"], new RunWorkerCompletedEventArgs((object) true, (Exception) null, false));
          else
            callback((object) e.Error, new RunWorkerCompletedEventArgs((object) false, (Exception) null, false));
        }
        catch (Exception ex)
        {
          callback((object) ex, new RunWorkerCompletedEventArgs((object) false, (Exception) null, false));
        }
      });
      postClient.DownloadStringAsync(new Uri("http://api.imgur.com/2/upload.json", UriKind.Absolute));
    }

    public void SubmitRedditStory(SubmitALinkInfo info, RunWorkerCompletedEventHandler callback)
    {
      string str1 = HttpUtility.UrlEncode(this.SettingsMan.UserCookie);
      bool isInCaptchaState = false;
      if (!string.IsNullOrWhiteSpace(info.CapResponse) && !string.IsNullOrWhiteSpace(info.CapID))
        isInCaptchaState = true;
      if (!this.SettingsMan.IsSignedIn || str1 == null)
        return;
      Dictionary<string, object> parameters = new Dictionary<string, object>();
      parameters.Add("title", (object) HttpUtility.UrlEncode(info.title));
      parameters.Add("sr", (object) HttpUtility.UrlEncode(info.subreddit));
      if (info.isSelf)
      {
        parameters.Add("text", (object) HttpUtility.UrlEncode(info.urlOrSelf));
        parameters.Add("kind", (object) "self");
      }
      else
      {
        parameters.Add("url", (object) HttpUtility.UrlEncode(info.urlOrSelf));
        parameters.Add("kind", (object) "link");
      }
      parameters.Add("uh", (object) this.SettingsMan.ModHash);
      if (isInCaptchaState)
      {
        parameters.Add("iden", (object) HttpUtility.UrlEncode(info.CapID));
        parameters.Add("captcha", (object) HttpUtility.UrlEncode(info.CapResponse));
      }
      PostClient postClient = new PostClient((IDictionary<string, object>) parameters);
      postClient.CookieContainer = new CookieContainer();
      Cookie cookie = new Cookie();
      cookie.Domain = ".reddit.com";
      cookie.HttpOnly = false;
      cookie.Path = "/";
      cookie.Secure = false;
      cookie.Value = str1;
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
            JObject jobject = JObject.Parse(e.Result);
            string str2 = DataManager.CheckForCaptchaError(jobject["jquery"]);
            if (!string.IsNullOrEmpty(str2))
            {
              if (!isInCaptchaState)
              {
                this.CaptchaInfoHolder = new CaptchaInfo()
                {
                  Info = (object) info,
                  type = 0,
                  CaptchaID = str2
                };
                callback((object) "captcha", new RunWorkerCompletedEventArgs((object) false, (Exception) new CaptchaException(), false));
              }
              else
                callback((object) "captcha-failed", new RunWorkerCompletedEventArgs((object) false, (Exception) new CaptchaException()
                {
                  NewCaptchaID = str2
                }, false));
            }
            else
            {
              string stringInJson = DataManager.FindStringInJson(jobject["jquery"], "https://");
              if (stringInJson.StartsWith("https://"))
              {
                callback((object) stringInJson, new RunWorkerCompletedEventArgs((object) true, (Exception) null, false));
              }
              else
              {
                if (!isInCaptchaState)
                {
                  this.MessageManager.showRedditErrorMessage("submit your link", DataManager.FindStringInJson(jobject["jquery"], ".error."), true);
                  throw new Exception("");
                }
                callback((object) DataManager.FindStringInJson(jobject["jquery"], ".error."), new RunWorkerCompletedEventArgs((object) "", (Exception) new CaptchaException(), false));
              }
            }
          }
          else
          {
            if (!isInCaptchaState)
              this.MessageManager.showWebErrorMessage("submitting your link", e.Error);
            throw new Exception("Couln't post link");
          }
        }
        catch (Exception ex)
        {
          if (isInCaptchaState)
            callback((object) "captcha-failed-net-error", new RunWorkerCompletedEventArgs((object) false, (Exception) new CaptchaException(), false));
          else
            callback((object) ex, new RunWorkerCompletedEventArgs((object) false, (Exception) null, false));
        }
      });
      postClient.DownloadStringAsync(new Uri("https://www.reddit.com/api/submit", UriKind.Absolute));
    }

    public void SaveStory(string RName, bool saveNow)
    {
      string str = HttpUtility.UrlEncode(this.SettingsMan.UserCookie);
      if (!this.SettingsMan.IsSignedIn || str == null)
        return;
      PostClient postClient = new PostClient((IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "id",
          (object) RName
        },
        {
          "uh",
          (object) this.SettingsMan.ModHash
        }
      });
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
              return;
            JObject jobject = JObject.Parse(e.Result);
            if (saveNow)
              this.MessageManager.showRedditErrorMessage("save the story", DataManager.FindStringInJson(jobject["jquery"], ".error."));
            else
              this.MessageManager.showRedditErrorMessage("unsave the story", DataManager.FindStringInJson(jobject["jquery"], ".error."));
          }
          else if (saveNow)
            this.MessageManager.showWebErrorMessage("saving the story", e.Error);
          else
            this.MessageManager.showWebErrorMessage("unsaving the story", e.Error);
        }
        catch (Exception ex)
        {
          if (saveNow)
            this.MessageManager.showErrorMessage("saving the story", ex);
          else
            this.MessageManager.showErrorMessage("unsaving the story", ex);
        }
      });
      if (saveNow)
        postClient.DownloadStringAsync(new Uri("https://www.reddit.com/api/save", UriKind.Absolute));
      else
        postClient.DownloadStringAsync(new Uri("https://www.reddit.com/api/unsave", UriKind.Absolute));
    }

    public void HideStory(string RName, bool hideNow)
    {
      string str = HttpUtility.UrlEncode(this.SettingsMan.UserCookie);
      if (!this.SettingsMan.IsSignedIn || str == null)
        return;
      PostClient postClient = new PostClient((IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "id",
          (object) RName
        },
        {
          "uh",
          (object) this.SettingsMan.ModHash
        }
      });
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
              return;
            JObject jobject = JObject.Parse(e.Result);
            if (hideNow)
              this.MessageManager.showRedditErrorMessage("hide the story", DataManager.FindStringInJson(jobject["jquery"], ".error."));
            else
              this.MessageManager.showRedditErrorMessage("unhide the story", DataManager.FindStringInJson(jobject["jquery"], ".error."));
          }
          else if (hideNow)
            this.MessageManager.showWebErrorMessage("hiding the story", e.Error);
          else
            this.MessageManager.showWebErrorMessage("unhiding the story", e.Error);
        }
        catch (Exception ex)
        {
          if (hideNow)
            this.MessageManager.showErrorMessage("hiding the story", ex);
          else
            this.MessageManager.showErrorMessage("unhiding the story", ex);
        }
      });
      if (hideNow)
        postClient.DownloadStringAsync(new Uri("https://www.reddit.com/api/hide", UriKind.Absolute));
      else
        postClient.DownloadStringAsync(new Uri("https://www.reddit.com/api/unhide", UriKind.Absolute));
    }

    public event DataManager.MessageInboxUpdatedEventHandler MessageInboxUpdated;

    public List<Message> GetInboxMessages() => this.BaconitStore.GetInboxMessages();

    public void UpdateInboxMessages(bool loadingMore)
    {
      string str = HttpUtility.UrlEncode(this.SettingsMan.UserCookie);
      if (!this.SettingsMan.IsSignedIn || str == null)
        return;
      HttpWebRequest state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com/message/inbox/.json");
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
      state.BeginGetResponse(new AsyncCallback(this.InboxMessageUpdate_Callback), (object) state);
    }

    private void InboxMessageUpdate_Callback(IAsyncResult result)
    {
      long millisecond = (long) DateTime.Now.Millisecond;
      try
      {
        if (result.AsyncState is HttpWebRequest asyncState)
        {
          using (HttpWebResponse response = (HttpWebResponse) asyncState.EndGetResponse(result))
          {
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
              JObject jobject = JObject.Parse(streamReader.ReadToEnd());
              string stringInJson;
              if (!(stringInJson = DataManager.FindStringInJson(jobject["jquery"], ".error.")).Equals(""))
              {
                if (this.MessageManager == null)
                  return;
                this.MessageManager.showRedditErrorMessage("get the inbox messages", stringInJson);
              }
              else
              {
                List<Message> messageList = new List<Message>();
                int num1 = 0;
                int num2 = 0;
                int num3 = 0;
                if (jobject["data"][(object) "children"].HasValues)
                {
                  foreach (JToken jtoken in (IEnumerable<JToken>) jobject["data"][(object) "children"])
                  {
                    Message message1 = new Message();
                    message1.ID = num2;
                    message1.body = (string) jtoken[(object) "data"][(object) "body"];
                    message1.Author = (string) jtoken[(object) "data"][(object) "author"];
                    message1.Context = (string) jtoken[(object) "data"][(object) "context"];
                    message1.Created = (float) jtoken[(object) "data"][(object) "created_utc"];
                    message1.RName = (string) jtoken[(object) "data"][(object) "name"];
                    message1.Subject = (string) jtoken[(object) "data"][(object) "subject"];
                    message1.Subreddit = (string) jtoken[(object) "data"][(object) "subreddit"];
                    if (this.SettingsMan.AdultFilter)
                    {
                      message1.body = DataManager.FilterText(message1.body);
                      message1.Subject = DataManager.FilterText(message1.Subject);
                    }
                    message1.PrimeKey = message1.RName + (object) millisecond;
                    if (message1.body.Length > 4000)
                    {
                      Message message2 = message1;
                      message2.bodyOver = message2.body.Substring(4000);
                      Message message3 = message1;
                      message3.body = message3.body.Substring(0, 4000);
                      if (message1.bodyOver.Length > 3000)
                      {
                        Message message4 = message1;
                        message4.bodyOver = message4.bodyOver.Substring(0, 3000);
                      }
                    }
                    try
                    {
                      message1.wasComment = !(bool) jtoken[(object) "data"][(object) "was_comment"] ? 0 : 1;
                    }
                    catch
                    {
                      message1.wasComment = 0;
                    }
                    try
                    {
                      if ((bool) jtoken[(object) "data"][(object) "new"])
                      {
                        message1.isNew = 1;
                        ++num3;
                      }
                      else
                        message1.isNew = 0;
                    }
                    catch
                    {
                      message1.isNew = 0;
                    }
                    messageList.Add(message1);
                    ++num2;
                    ++num1;
                  }
                }
                this.SettingsMan.UnreadInboxMessageCount = num3;
                if (this.SettingsMan.UnreadInboxMessageCount > 0)
                {
                  this.SettingsMan.HasMail = true;
                  foreach (Message message in messageList)
                  {
                    if (message.isNew == 1)
                    {
                      this.SettingsMan.MostRecentInboxMessageTextAuthor = HttpUtility.HtmlDecode(messageList[0].Author);
                      string str = HttpUtility.HtmlDecode(messageList[0].body);
                      if (str.Length > 50)
                        str = str.Substring(0, 50);
                      this.SettingsMan.MostRecentInboxMessageTextBody = str;
                      break;
                    }
                  }
                }
                else
                  this.SettingsMan.HasMail = false;
                this.UpdateApplicationTile();
                if (this.MessageInboxUpdated != null)
                  this.MessageInboxUpdated(true, messageList, true);
                if (this.BaconitStore == null || this.RunningFromUpdater)
                  return;
                this.BaconitStore.SetInboxMessages(messageList);
                this.BaconitStore.UpdateLastUpdatedTime("!InboxMessages", false);
              }
            }
          }
        }
        else
        {
          if (this.MessageManager != null)
            this.MessageManager.showWebErrorMessage("geting the inbox messages", (Exception) null);
          if (this.BaconitStore != null)
            this.BaconitStore.UpdateLastUpdatedTime("!InboxMessages", true);
          if (this.MessageInboxUpdated == null)
            return;
          this.MessageInboxUpdated(false, (List<Message>) null, false);
        }
      }
      catch (Exception ex)
      {
        if (this.MessageManager != null)
          this.MessageManager.showErrorMessage("getting the inbox messages", ex);
        if (this.BaconitStore != null)
          this.BaconitStore.UpdateLastUpdatedTime("!InboxMessages", true);
        if (this.MessageInboxUpdated == null)
          return;
        this.MessageInboxUpdated(false, (List<Message>) null, false);
      }
    }

    public void ChangeMessageReadStatus(string RName, int isNew)
    {
      string str = HttpUtility.UrlEncode(this.SettingsMan.UserCookie);
      if (!this.SettingsMan.IsSignedIn || str == null)
        return;
      this.BaconitStore.UpdateMessageNewStatus(RName, isNew);
      if (isNew == 0)
      {
        --this.SettingsMan.UnreadInboxMessageCount;
        if (this.SettingsMan.UnreadInboxMessageCount < 0)
          this.SettingsMan.UnreadInboxMessageCount = 0;
        this.SettingsMan.HasMail = this.SettingsMan.UnreadInboxMessageCount > 0;
      }
      PostClient postClient = new PostClient((IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "id",
          (object) RName
        },
        {
          "uh",
          (object) this.SettingsMan.ModHash
        }
      });
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
              if (this.SettingsMan.UnreadInboxMessageCount != 0)
                return;
              try
              {
                this.OpenInboxMarkAsRead();
              }
              catch
              {
              }
            }
            else
              this.MessageManager.showRedditErrorMessage("change the message read status", DataManager.FindStringInJson(JObject.Parse(e.Result)["jquery"], ".error."));
          }
          else
            this.MessageManager.showWebErrorMessage("changing the message read status", e.Error);
        }
        catch (Exception ex)
        {
          this.MessageManager.showErrorMessage("changing the message read status", ex);
        }
      });
      if (isNew == 1)
        postClient.DownloadStringAsync(new Uri("https://www.reddit.com/api/unread_message", UriKind.Absolute));
      else
        postClient.DownloadStringAsync(new Uri("https://www.reddit.com/api/read_message", UriKind.Absolute));
      this.UpdateApplicationTile();
      if (this.MessageInboxUpdated == null)
        return;
      this.MessageInboxUpdated(true, (List<Message>) null, true);
    }

    public void OpenInboxMarkAsRead()
    {
      string str = HttpUtility.UrlEncode(this.SettingsMan.UserCookie);
      if (!this.SettingsMan.IsSignedIn || str == null)
        return;
      CookieAwareWebClient cookieAwareWebClient = new CookieAwareWebClient();
      CookieContainer cc = new CookieContainer();
      Cookie cookie = new Cookie();
      cookie.Domain = ".reddit.com";
      cookie.HttpOnly = false;
      cookie.Path = "/";
      cookie.Secure = false;
      cookie.Value = str;
      cookie.Name = "reddit_session";
      cc.Add(new Uri("https://api.reddit.com"), cookie);
      cc.Add(new Uri("https://reddit.com"), cookie);
      cc.Add(new Uri("https://www.reddit.com"), cookie);
      cookieAwareWebClient.setCookieConainer(cc);
      cookieAwareWebClient.OpenReadCompleted += (OpenReadCompletedEventHandler) ((sender, e) =>
      {
        try
        {
        }
        catch
        {
        }
      });
      cookieAwareWebClient.DownloadStringAsync(new Uri("https://www.reddit.com/message/unread/", UriKind.Absolute));
    }

    public void PostMessageReply(MessageReply message, RunWorkerCompletedEventHandler callback)
    {
      string str1 = HttpUtility.UrlEncode(this.SettingsMan.UserCookie);
      bool isInCaptchaState = false;
      if (!string.IsNullOrWhiteSpace(message.CapResponse) && !string.IsNullOrWhiteSpace(message.CapID))
        isInCaptchaState = true;
      if (this.SettingsMan.IsSignedIn && str1 != null)
      {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("thing_id", (object) message.RName);
        parameters.Add("text", (object) HttpUtility.UrlEncode(message.text));
        parameters.Add("uh", (object) this.SettingsMan.ModHash);
        if (isInCaptchaState)
        {
          parameters.Add("iden", (object) HttpUtility.UrlEncode(message.CapID));
          parameters.Add("captcha", (object) HttpUtility.UrlEncode(message.CapResponse));
        }
        PostClient postClient = new PostClient((IDictionary<string, object>) parameters);
        postClient.CookieContainer = new CookieContainer();
        Cookie cookie = new Cookie();
        cookie.Domain = ".reddit.com";
        cookie.HttpOnly = false;
        cookie.Path = "/";
        cookie.Secure = false;
        cookie.Value = str1;
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
              JObject jobject = JObject.Parse(e.Result);
              string str2 = DataManager.CheckForCaptchaError(jobject["jquery"]);
              if (!string.IsNullOrEmpty(str2))
              {
                if (!isInCaptchaState)
                {
                  this.CaptchaInfoHolder = new CaptchaInfo()
                  {
                    Info = (object) message,
                    type = 1,
                    CaptchaID = str2
                  };
                  callback((object) "captcha", new RunWorkerCompletedEventArgs((object) false, (Exception) new CaptchaException(), false));
                }
                else
                  callback((object) "captcha-failed", new RunWorkerCompletedEventArgs((object) false, (Exception) new CaptchaException()
                  {
                    NewCaptchaID = str2
                  }, false));
              }
              else
              {
                string stringInJson;
                if (!(stringInJson = DataManager.FindStringInJson(jobject["jquery"], ".error.")).Equals(""))
                {
                  if (isInCaptchaState)
                  {
                    callback((object) stringInJson, new RunWorkerCompletedEventArgs((object) false, (Exception) new CaptchaException(), false));
                  }
                  else
                  {
                    this.MessageManager.showRedditErrorMessage("post the message reply", stringInJson);
                    callback((object) stringInJson, new RunWorkerCompletedEventArgs((object) false, (Exception) null, false));
                  }
                }
                else
                {
                  try
                  {
                    JToken jtoken = jobject["jquery"][(object) 0];
                    if (!isInCaptchaState)
                      callback((object) stringInJson, new RunWorkerCompletedEventArgs((object) true, (Exception) null, true));
                    else
                      callback((object) "good", new RunWorkerCompletedEventArgs((object) null, (Exception) null, true));
                  }
                  catch
                  {
                    if (isInCaptchaState)
                      throw new Exception();
                    this.MessageManager.showRedditErrorMessage("post the message reply", "");
                    callback((object) stringInJson, new RunWorkerCompletedEventArgs((object) true, (Exception) null, false));
                  }
                }
              }
            }
            else
            {
              if (isInCaptchaState)
                throw new Exception();
              this.MessageManager.showWebErrorMessage("posting the message reply", e.Error);
              callback((object) null, new RunWorkerCompletedEventArgs((object) false, (Exception) null, false));
            }
          }
          catch (Exception ex)
          {
            if (isInCaptchaState)
              callback((object) "captcha-failed-net-error", new RunWorkerCompletedEventArgs((object) false, (Exception) new CaptchaException(), false));
            this.MessageManager.showErrorMessage("posting the message reply", ex);
            callback((object) null, new RunWorkerCompletedEventArgs((object) false, (Exception) null, false));
          }
        });
        postClient.DownloadStringAsync(new Uri("https://www.reddit.com/api/comment", UriKind.Absolute));
      }
      else
        callback((object) null, new RunWorkerCompletedEventArgs((object) false, (Exception) null, false));
    }

    public void DeleteComment(RunWorkerCompletedEventHandler callback, string RName)
    {
      string str = HttpUtility.UrlEncode(this.SettingsMan.UserCookie);
      if (!this.SettingsMan.IsSignedIn || str == null)
        return;
      PostClient postClient = new PostClient((IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "id",
          (object) RName
        },
        {
          "executed",
          (object) "deleted"
        },
        {
          "uh",
          (object) this.SettingsMan.ModHash
        }
      });
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
            JObject jobject = JObject.Parse(e.Result);
            string stringInJson;
            if (!(stringInJson = DataManager.FindStringInJson(jobject["jquery"], ".error.")).Equals(""))
            {
              if (RName.StartsWith("t1_"))
                this.MessageManager.showRedditErrorMessage("delete your comment", stringInJson);
              else
                this.MessageManager.showRedditErrorMessage("delete your story", stringInJson);
              callback((object) RName, new RunWorkerCompletedEventArgs((object) false, (Exception) null, false));
            }
            else
              callback((object) RName, new RunWorkerCompletedEventArgs((object) true, (Exception) null, false));
          }
          else
          {
            if (RName.StartsWith("t1_"))
              this.MessageManager.showWebErrorMessage("deleting your comment", e.Error);
            else
              this.MessageManager.showWebErrorMessage("deleting your story", e.Error);
            callback((object) RName, new RunWorkerCompletedEventArgs((object) false, (Exception) null, false));
          }
        }
        catch (Exception ex)
        {
          if (RName.StartsWith("t1_"))
            this.MessageManager.showErrorMessage("deleting your comment", ex);
          else
            this.MessageManager.showErrorMessage("deleting your story", ex);
          callback((object) RName, new RunWorkerCompletedEventArgs((object) false, (Exception) null, false));
        }
      });
      postClient.DownloadStringAsync(new Uri("https://www.reddit.com/api/del", UriKind.Absolute));
    }

    public void EditComment(RunWorkerCompletedEventHandler callback, SubmitCommentData data)
    {
      string str = HttpUtility.UrlEncode(this.SettingsMan.UserCookie);
      if (!this.SettingsMan.IsSignedIn || str == null)
        return;
      PostClient postClient = new PostClient((IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "thing_id",
          (object) data.CommentingOnRName
        },
        {
          "text",
          (object) HttpUtility.UrlEncode(data.text)
        },
        {
          "uh",
          (object) this.SettingsMan.ModHash
        }
      });
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
            JObject jobject = JObject.Parse(e.Result);
            string stringInJson;
            if (!(stringInJson = DataManager.FindStringInJson(jobject["jquery"], ".error.")).Equals(""))
            {
              if (data.isComment)
                this.MessageManager.showRedditErrorMessage("edit your comment", stringInJson);
              else
                this.MessageManager.showRedditErrorMessage("edit your post", stringInJson);
              data.WasSuccessful = false;
              callback((object) data, new RunWorkerCompletedEventArgs((object) null, (Exception) null, false));
            }
            else
            {
              data.WasSuccessful = true;
              callback((object) data, new RunWorkerCompletedEventArgs((object) null, (Exception) null, true));
            }
          }
          else
          {
            if (data.isComment)
              this.MessageManager.showWebErrorMessage("editing your comment", e.Error);
            else
              this.MessageManager.showWebErrorMessage("editing your story", e.Error);
            data.WasSuccessful = false;
            callback((object) data, new RunWorkerCompletedEventArgs((object) null, (Exception) null, false));
          }
        }
        catch (Exception ex)
        {
          if (data.isComment)
            this.MessageManager.showErrorMessage("editing your comment", ex);
          else
            this.MessageManager.showErrorMessage("editing your story", ex);
          data.WasSuccessful = false;
          callback((object) data, new RunWorkerCompletedEventArgs((object) null, (Exception) null, false));
        }
      });
      postClient.DownloadStringAsync(new Uri("https://www.reddit.com/api/editusertext", UriKind.Absolute));
    }

    public void CommentOn(RunWorkerCompletedEventHandler callback, SubmitCommentData data)
    {
      string str1 = HttpUtility.UrlEncode(this.SettingsMan.UserCookie);
      if (!this.SettingsMan.IsSignedIn || str1 == null)
        return;
      PostClient postClient = new PostClient((IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "thing_id",
          (object) data.CommentingOnRName
        },
        {
          "text",
          (object) HttpUtility.UrlEncode(data.text)
        },
        {
          "uh",
          (object) this.SettingsMan.ModHash
        }
      });
      postClient.CookieContainer = new CookieContainer();
      Cookie cookie = new Cookie();
      cookie.Domain = ".reddit.com";
      cookie.HttpOnly = false;
      cookie.Path = "/";
      cookie.Secure = false;
      cookie.Value = str1;
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
            JObject jobject = JObject.Parse(e.Result);
            string stringInJson;
            if (!(stringInJson = DataManager.FindStringInJson(jobject["jquery"], ".error.")).Equals(""))
            {
              this.MessageManager.showRedditErrorMessage("submit the comment", stringInJson);
              data.WasSuccessful = false;
              callback((object) data, new RunWorkerCompletedEventArgs((object) "", (Exception) null, false));
            }
            else
            {
              bool flag = false;
              try
              {
                JToken jtoken = jobject["jquery"][(object) 30][(object) 3][(object) 0][(object) 0];
                string str2 = (string) jobject["jquery"][(object) 30][(object) 3][(object) 0][(object) 0][(object) nameof (data)][(object) "id"];
                data.WasSuccessful = true;
                data.NewID = str2;
                callback((object) data, new RunWorkerCompletedEventArgs((object) null, (Exception) null, false));
                flag = true;
              }
              catch
              {
              }
              if (!flag)
              {
                JToken jtoken = jobject["jquery"][(object) 18][(object) 3][(object) 0][(object) 0];
                string str3 = (string) jobject["jquery"][(object) 18][(object) 3][(object) 0][(object) 0][(object) nameof (data)][(object) "id"];
                data.WasSuccessful = true;
                data.NewID = str3;
                callback((object) data, new RunWorkerCompletedEventArgs((object) null, (Exception) null, false));
                flag = true;
              }
              if (!flag)
                throw new Exception("fail");
            }
          }
          else
          {
            this.MessageManager.showWebErrorMessage("submiting the comment", e.Error);
            data.WasSuccessful = false;
            callback((object) data, new RunWorkerCompletedEventArgs((object) "", (Exception) null, false));
          }
        }
        catch (Exception ex)
        {
          this.MessageManager.showErrorMessage("submiting the comment", ex);
          data.WasSuccessful = false;
          callback((object) data, new RunWorkerCompletedEventArgs((object) "", (Exception) null, false));
        }
      });
      postClient.DownloadStringAsync(new Uri("https://www.reddit.com/api/comment", UriKind.Absolute));
    }

    public List<CommentData> GetStoryComments(string storyID, string subRedditURL, int sort)
    {
      return sort == 6 ? new List<CommentData>() : this.BaconitStore.GetComments(storyID, subRedditURL, sort);
    }

    public bool UpdateStoryComments(
      CommentDetailsViewModelInterface callback,
      string StoryRID,
      string subRedditRName,
      bool updateStory,
      int sort,
      string ShowComment)
    {
      string str1 = HttpUtility.UrlEncode(this.SettingsMan.UserCookie);
      if (this.CommentSubRedditMap.ContainsKey(StoryRID + (object) sort))
        return false;
      this.CommentSubRedditMap.Add(StoryRID + (object) sort, new KeyValuePair<string, bool>(subRedditRName, updateStory));
      string str2;
      switch (sort)
      {
        case 0:
          str2 = "?sort=confidence";
          break;
        case 1:
          str2 = "?sort=top";
          break;
        case 2:
          str2 = "?sort=new";
          break;
        case 3:
          str2 = "?sort=hot";
          break;
        case 4:
          str2 = "?sort=controversial";
          break;
        case 5:
          str2 = "?sort=old";
          break;
        default:
          str2 = "?sort=top";
          break;
      }
      HttpWebRequest state;
      if (sort != 6 || string.IsNullOrWhiteSpace(ShowComment))
      {
        state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com/comments/" + StoryRID + ".json" + str2);
      }
      else
      {
        if (ShowComment.StartsWith("t1_"))
          ShowComment = ShowComment.Substring(3);
        state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com/comments/" + StoryRID + "/whatIsThisTextFor/" + ShowComment + "/.json?context=3");
      }
      if (this.SettingsMan.IsSignedIn && str1 != null)
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
                string str3 = end.Substring(num2 - 1);
                JObject jobject = JObject.Parse(str3.Substring(0, str3.Length - 1));
                List<CommentData> commentData1 = new List<CommentData>();
                JToken key1 = jobject["data"][(object) "children"];
                string key2 = this.CommentSubRedditMap[StoryRID + (object) sort].Key;
                int num3 = 0;
                if (this.CommentSubRedditMap[StoryRID + (object) sort].Value)
                {
                  int num4 = end.IndexOf("\"kind\": \"Listing\",");
                  int num5 = end.IndexOf("\"kind\": \"Listing\",", num4 + 1);
                  JToken jtoken1 = JObject.Parse(end.Substring(num4 - 1, num5 - num4 - 2))["data"][(object) "children"][(object) 0];
                  SubRedditData subRedditDataObj = new SubRedditData();
                  subRedditDataObj.ID = 0;
                  subRedditDataObj.SubRedditRank = 0;
                  subRedditDataObj.Domain = (string) jtoken1[(object) "data"][(object) "domain"];
                  subRedditDataObj.SubReddit = (string) jtoken1[(object) "data"][(object) "subreddit"];
                  string str4 = (string) jtoken1[(object) "data"][(object) "permalink"];
                  subRedditDataObj.SubRedditURL = str4.Substring(0, str4.IndexOf('/', 4) + 1);
                  subRedditDataObj.RedditID = (string) jtoken1[(object) "data"][(object) "name"];
                  subRedditDataObj.Author = (string) jtoken1[(object) "data"][(object) "author"];
                  subRedditDataObj.score = (int) jtoken1[(object) "data"][(object) "score"];
                  subRedditDataObj.thumbnail = (string) jtoken1[(object) "data"][(object) "thumbnail"];
                  subRedditDataObj.downs = (int) jtoken1[(object) "data"][(object) "downs"];
                  subRedditDataObj.isSelf = (bool) jtoken1[(object) "data"][(object) "is_self"];
                  subRedditDataObj.created = (float) jtoken1[(object) "data"][(object) "created_utc"];
                  subRedditDataObj.URL = HttpUtility.HtmlDecode((string) jtoken1[(object) "data"][(object) "url"]);
                  subRedditDataObj.Title = HttpUtility.HtmlDecode((string) jtoken1[(object) "data"][(object) "title"]);
                  subRedditDataObj.comments = (int) jtoken1[(object) "data"][(object) "num_comments"];
                  subRedditDataObj.ups = (int) jtoken1[(object) "data"][(object) "ups"];
                  subRedditDataObj.Permalink = (string) jtoken1[(object) "data"][(object) "permalink"];
                  subRedditDataObj.SelfText = (string) jtoken1[(object) "data"][(object) "selftext"];
                  subRedditDataObj.RedditRealID = (string) jtoken1[(object) "data"][(object) "id"];
                  if (this.SettingsMan.AdultFilter)
                  {
                    subRedditDataObj.Title = DataManager.FilterText(subRedditDataObj.Title);
                    subRedditDataObj.SelfText = DataManager.FilterText(subRedditDataObj.SelfText);
                  }
                  List<LongTextData> longTextData1 = new List<LongTextData>();
                  if (subRedditDataObj.SelfText.Length > 0)
                  {
                    subRedditDataObj.hasSelfText = true;
                    int num6 = 0;
                    while (subRedditDataObj.SelfText.Length > 0)
                    {
                      LongTextData longTextData2 = new LongTextData();
                      longTextData2.RStoryName = subRedditDataObj.RedditID;
                      longTextData2.PartNum = num6;
                      longTextData2.SubRedditUrl = subRedditDataObj.SubRedditURL;
                      longTextData2.isPinned = callback.GetisPinned();
                      longTextData2.SubType = 0;
                      if (subRedditDataObj.SelfText.Length > 4000)
                      {
                        longTextData2.text = subRedditDataObj.SelfText.Substring(0, 4000);
                        subRedditDataObj.SelfText = subRedditDataObj.SelfText.Substring(4000);
                      }
                      else
                      {
                        longTextData2.text = subRedditDataObj.SelfText.Substring(0, subRedditDataObj.SelfText.Length);
                        subRedditDataObj.SelfText = "";
                      }
                      longTextData1.Add(longTextData2);
                      ++num6;
                    }
                  }
                  else
                    subRedditDataObj.hasSelfText = false;
                  JToken jtoken2 = jtoken1[(object) "data"][(object) "likes"];
                  subRedditDataObj.Like = jtoken2 == null || jtoken2.Type != JTokenType.Boolean ? 0 : (!(bool) jtoken2 ? -1 : 1);
                  this.BaconitStore.SetLongText(longTextData1, "", 0, true);
                  Deployment.Current.Dispatcher.BeginInvoke((Action) (() => callback.SetStoryData(subRedditDataObj, true)));
                  if (callback.GetisPinned())
                    this.BaconitStore.SetPinnedStory(subRedditDataObj);
                }
                callback.CommentFeeder_Start(true, sort);
                int num7 = 0;
                int num8 = 0;
                int key3 = 0;
                Stack<KeyValuePair<JToken, int>> keyValuePairStack = new Stack<KeyValuePair<JToken, int>>();
                if (key1.HasValues)
                {
                  while (key1 != null)
                  {
                    if (((string) key1[(object) key3][(object) "kind"]).Equals("t1"))
                    {
                      CommentData Comment = new CommentData();
                      Comment.Order = num8;
                      Comment.SubRedditURL = key2;
                      Comment.StoryRID = StoryRID;
                      Comment.Sort = sort;
                      Comment.Author = (string) key1[(object) key3][(object) "data"][(object) "author"];
                      Comment.Body = HttpUtility.HtmlDecode(HttpUtility.HtmlDecode((string) key1[(object) key3][(object) "data"][(object) "body"]));
                      Comment.Created = (double) key1[(object) key3][(object) "data"][(object) "created_utc"];
                      Comment.Downs = (int) key1[(object) key3][(object) "data"][(object) "downs"];
                      Comment.LinkRID = (string) key1[(object) key3][(object) "data"][(object) "link_id"];
                      Comment.RID = (string) key1[(object) key3][(object) "data"][(object) "id"];
                      Comment.RName = (string) key1[(object) key3][(object) "data"][(object) "name"];
                      Comment.SubRedditRName = (string) key1[(object) key3][(object) "data"][(object) "subreddit_id"];
                      Comment.Up = (int) key1[(object) key3][(object) "data"][(object) "ups"];
                      Comment.IndentLevel = num7;
                      Comment.Permalink = (string) key1[(object) key3][(object) "data"][(object) "permalink"];
                      JToken jtoken = key1[(object) key3][(object) "data"][(object) "likes"];
                      Comment.Likes = jtoken == null || jtoken.Type != JTokenType.Boolean ? 0 : (!(bool) jtoken ? -1 : 1);
                      if (this.SettingsMan.AdultFilter)
                        Comment.Body = DataManager.FilterText(Comment.Body);
                      if (Comment.Body.Length > 4000)
                      {
                        CommentData commentData2 = Comment;
                        commentData2.Body2 = commentData2.Body.Substring(4000);
                        CommentData commentData3 = Comment;
                        commentData3.Body = commentData3.Body.Substring(0, 4000);
                        if (Comment.Body2.Length > 4000)
                        {
                          CommentData commentData4 = Comment;
                          commentData4.Body3 = commentData4.Body2.Substring(4001);
                          CommentData commentData5 = Comment;
                          commentData5.Body2 = commentData5.Body2.Substring(0, 4000);
                        }
                      }
                      commentData1.Add(Comment);
                      callback.CommentFeeder_Feed(Comment, num3, sort);
                      ++num3;
                      ++num8;
                    }
                    JToken jtoken3 = key1[(object) key3][(object) "data"][(object) "replies"];
                    ++key3;
                    try
                    {
                      if (((string) jtoken3[(object) "kind"]).Equals("Listing"))
                      {
                        keyValuePairStack.Push(new KeyValuePair<JToken, int>(key1, key3));
                        key1 = jtoken3[(object) "data"][(object) "children"];
                        key3 = 0;
                        ++num7;
                        continue;
                      }
                    }
                    catch
                    {
                    }
                    try
                    {
                      ((string) key1[(object) key3][(object) "kind"]).Equals("t1");
                    }
                    catch
                    {
                      bool flag = true;
                      while (flag)
                      {
                        if (keyValuePairStack.Count == 0)
                        {
                          key1 = (JToken) null;
                          break;
                        }
                        KeyValuePair<JToken, int> keyValuePair = keyValuePairStack.Pop();
                        key1 = keyValuePair.Key;
                        key3 = keyValuePair.Value;
                        --num7;
                        try
                        {
                          if (((string) key1[(object) key3][(object) "kind"]).Equals("t1"))
                            flag = false;
                        }
                        catch
                        {
                        }
                      }
                    }
                  }
                }
                callback.CommentFeeder_Done(num3, sort);
                callback.LoadingFromWebDone(commentData1.Count != 0);
                if (this.BaconitStore.SetComments(commentData1, StoryRID, sort))
                  this.BaconitStore.UpdateLastUpdatedTime("t3_" + StoryRID + (object) sort + "comments", false);
                else
                  this.BaconitStore.UpdateLastUpdatedTime("t3_" + StoryRID + (object) sort + "comments", true);
              }
            }
          }
          else
          {
            if (callback.GetisOpen())
              this.MessageManager.showWebErrorMessage("getting the comments", (Exception) null);
            Deployment.Current.Dispatcher.BeginInvoke((Action) (() => callback.LoadingStoryFailed()));
          }
        }
        catch (Exception ex)
        {
          if (callback.GetisOpen())
            this.MessageManager.showErrorMessage("getting the comments", ex);
          Deployment.Current.Dispatcher.BeginInvoke((Action) (() => callback.LoadingStoryFailed()));
        }
        this.CommentSubRedditMap.Remove(StoryRID + (object) sort);
      }), (object) state);
      return true;
    }

    public void RequestImage(
      RedditImage img,
      RunWorkerCompletedEventHandler callback,
      bool CacheOnly,
      bool IsCaptcha)
    {
      if (this.TotalMemory != 0L)
      {
        double d = (double) (long) DeviceExtendedProperties.GetValue("ApplicationCurrentMemoryUsage") * 1.0 / ((double) this.TotalMemory * 1.0);
        if (d > 0.9 && !double.IsInfinity(d))
        {
          this.BaconitAnalytics.LogEvent("Low Memory Warning");
          img.CallBackStatus = false;
          callback((object) null, new RunWorkerCompletedEventArgs((object) img, (Exception) null, false));
          return;
        }
      }
      img.FileName = DataManager.normalizeString(img.URL);
      bool flag = false;
      lock (this.FileStorage)
      {
        if (this.FileStorage.DirectoryExists("Thumbs\\" + img.SubRedditRName + (object) img.SubRedditType))
        {
          if (this.FileStorage.FileExists("Thumbs\\" + img.SubRedditRName + (object) img.SubRedditType + "\\" + img.FileName))
            flag = true;
        }
        else
          this.FileStorage.CreateDirectory("Thumbs\\" + img.SubRedditRName + (object) img.SubRedditType);
      }
      if (!flag | IsCaptcha)
      {
        WebClient userToken = new WebClient();
        userToken.OpenReadCompleted += (OpenReadCompletedEventHandler) ((sender, e) =>
        {
          string url = img.URL;
          if (e.Error == null)
          {
            if (!e.Cancelled)
            {
              try
              {
                WriteableBitmap writeableBitmap = (WriteableBitmap) null;
                BinaryReader binaryReader = new BinaryReader(e.Result);
                char[] chArray = binaryReader.ReadChars(3);
                binaryReader.BaseStream.Seek(0L, SeekOrigin.Begin);
                if (chArray[0] == 'G' && chArray[1] == 'I' && chArray[2] == 'F')
                {
                  img.CallBackStatus = false;
                  img.IsGif = true;
                  binaryReader.Dispose();
                  callback((object) null, new RunWorkerCompletedEventArgs((object) img, (Exception) null, false));
                  return;
                }
                using (AutoResetEvent are = new AutoResetEvent(false))
                {
                  Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
                  {
                    try
                    {
                      BitmapImage source = new BitmapImage();
                      if (IsCaptcha)
                        source.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                      source.SetSource(e.Result);
                      writeableBitmap = new WriteableBitmap((BitmapSource) source);
                      if (img.GetLowRes)
                      {
                        int pixelWidth = writeableBitmap.PixelWidth;
                        int pixelHeight = writeableBitmap.PixelHeight;
                        int num3 = this.SettingsMan.ScaleFactor != 100 ? (this.SettingsMan.ScaleFactor != 160 ? 1080 : 768) : 480;
                        if (pixelWidth > num3)
                        {
                          double num4 = (double) num3 / (double) pixelWidth;
                          writeableBitmap = writeableBitmap.Resize((int) Math.Floor((double) pixelWidth * num4), (int) Math.Floor((double) pixelHeight * num4), WriteableBitmapExtensions.Interpolation.Bilinear);
                        }
                      }
                      img.Image = (BitmapSource) writeableBitmap;
                    }
                    catch
                    {
                      writeableBitmap = (WriteableBitmap) null;
                      img.Image = (BitmapSource) null;
                    }
                    are.Set();
                  }));
                  are.WaitOne();
                }
                binaryReader.Dispose();
                if (img.Image != null)
                {
                  img.CallBackStatus = true;
                  if (!CacheOnly)
                    callback((object) null, new RunWorkerCompletedEventArgs((object) img, (Exception) null, false));
                  if (writeableBitmap == null)
                    return;
                  try
                  {
                    lock (this.FileStorage)
                    {
                      using (IsolatedStorageFileStream targetStream = new IsolatedStorageFileStream("Thumbs\\" + img.SubRedditRName + (object) img.SubRedditType + "\\" + img.FileName, FileMode.OpenOrCreate, this.FileStorage))
                        writeableBitmap.SaveJpeg((Stream) targetStream, writeableBitmap.PixelWidth, writeableBitmap.PixelHeight, 0, 100);
                    }
                  }
                  catch (Exception ex)
                  {
                    this.DebugDia("Failed to write image to file", ex);
                  }
                  if (writeableBitmap == null)
                    return;
                  writeableBitmap = (WriteableBitmap) null;
                  return;
                }
                this.DebugDia("Failed to get image with GZip Webclient", (Exception) null);
                img.CallBackStatus = false;
                callback((object) null, new RunWorkerCompletedEventArgs((object) img, (Exception) null, false));
                return;
              }
              catch (Exception ex)
              {
                this.DebugDia("Error in request web image", ex);
                img.CallBackStatus = false;
                callback((object) null, new RunWorkerCompletedEventArgs((object) img, (Exception) null, false));
                return;
              }
            }
          }
          this.DebugDia("Error in request web image", (Exception) null);
          img.CallBackStatus = false;
          callback((object) null, new RunWorkerCompletedEventArgs((object) img, (Exception) null, false));
        });
        userToken.OpenReadAsync(new Uri(img.URL), (object) userToken);
      }
      else
      {
        if (CacheOnly)
          return;
        try
        {
          img.FileName = DataManager.normalizeString(img.URL);
          lock (this.FileStorage)
          {
            using (IsolatedStorageFileStream isoFileStream = new IsolatedStorageFileStream("Thumbs\\" + img.SubRedditRName + (object) img.SubRedditType + "\\" + img.FileName, FileMode.Open, this.FileStorage))
            {
              using (AutoResetEvent are = new AutoResetEvent(false))
              {
                Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
                {
                  try
                  {
                    img.Image = (BitmapSource) new BitmapImage();
                    img.Image.SetSource((Stream) isoFileStream);
                    img.CallBackStatus = true;
                    are.Set();
                  }
                  catch (OutOfMemoryException ex)
                  {
                  }
                  catch
                  {
                    img.CallBackStatus = false;
                    are.Set();
                  }
                }));
                are.WaitOne();
              }
            }
          }
          callback((object) null, new RunWorkerCompletedEventArgs((object) img, (Exception) null, false));
        }
        catch (Exception ex)
        {
          this.DebugDia("Error in request image from file", ex);
          img.CallBackStatus = false;
          callback((object) null, new RunWorkerCompletedEventArgs((object) img, (Exception) null, false));
        }
      }
    }

    public static string normalizeString(string normal)
    {
      normal = normal.Replace('\\', '.');
      normal = normal.Replace('/', '.');
      normal = normal.Replace(':', '.');
      normal = normal.Replace('?', '.');
      normal = normal.Replace('>', '.');
      normal = normal.Replace('<', '.');
      return normal;
    }

    public bool ClearImageCache(string RName, int type)
    {
      lock (this.FileStorage)
      {
        try
        {
          if (!this.FileStorage.DirectoryExists("Thumbs\\" + RName + (object) type))
            return false;
          IsolatedStorageFile fileStorage = this.FileStorage;
          string searchPattern = "Thumbs\\" + RName + (object) type + "\\*";
          foreach (string fileName in fileStorage.GetFileNames(searchPattern))
            this.FileStorage.DeleteFile("Thumbs\\" + RName + (object) type + "\\" + fileName);
          this.FileStorage.DeleteDirectory("Thumbs\\" + RName + (object) type);
          return true;
        }
        catch
        {
          return false;
        }
      }
    }

    public void Vote(string RName, int vote)
    {
      ThreadPool.QueueUserWorkItem((WaitCallback) (obj =>
      {
        string str = HttpUtility.UrlEncode(this.SettingsMan.UserCookie);
        if (!this.SettingsMan.IsSignedIn || str == null)
          return;
        PostClient postClient = new PostClient((IDictionary<string, object>) new Dictionary<string, object>()
        {
          {
            "id",
            (object) RName
          },
          {
            "dir",
            (object) vote
          },
          {
            "uh",
            (object) this.SettingsMan.ModHash
          }
        });
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
              JObject jobject = JObject.Parse(e.Result);
              try
              {
                string stringInJson;
                if ((stringInJson = DataManager.FindStringInJson(jobject["jquery"], ".error.")).Equals(""))
                  return;
                this.MessageManager.showRedditErrorMessage("update the account information", stringInJson);
              }
              catch
              {
              }
            }
            else
              this.MessageManager.showWebErrorMessage("sending the new vote", e.Error);
          }
          catch (Exception ex)
          {
            this.MessageManager.showErrorMessage("sending the new vote", ex);
          }
        });
        postClient.DownloadStringAsync(new Uri("https://www.reddit.com/api/vote", UriKind.Absolute));
        try
        {
          if (RName.StartsWith("t1"))
            this.BaconitStore.UpdateCommentLikeStatus(RName, vote);
          else
            this.BaconitStore.UpdateSubRedditData(RName, vote);
        }
        catch (Exception ex)
        {
          this.DebugDia("failed to set in database", ex);
        }
      }));
    }

    public List<SubRedditData> GetSubRedditStories(string redditUrl, int type)
    {
      return this.BaconitStore.GetStories(redditUrl, type);
    }

    public SubRedditData GetStory(string RedditID) => this.BaconitStore.GetStory(RedditID);

    public bool UpdateSubRedditStories(
      RedditViewerViewModelInterface callback,
      string redditUrl,
      int type,
      int VirturalType,
      int pos)
    {
      if (DataManager.BeingUpdated.ContainsKey(redditUrl + (object) VirturalType) && DateTime.Now.Subtract(DataManager.BeingUpdated[redditUrl + (object) VirturalType]).TotalSeconds < 120.0)
        return false;
      DataManager.BeingUpdated[redditUrl + (object) VirturalType] = DateTime.Now;
      callback.SetLoadingFromWeb(type, true);
      string str1 = HttpUtility.UrlEncode(this.SettingsMan.UserCookie);
      string str2 = "";
      switch (VirturalType)
      {
        case 0:
          str2 = "hot/";
          break;
        case 1:
          str2 = "new/.json?sort=new";
          break;
        case 2:
          str2 = "top/.json?sort=top&t=day";
          break;
        case 3:
          str2 = "controversial/";
          break;
        case 6:
          str2 = "top/.json?sort=top&t=hour";
          break;
        case 7:
          str2 = "top/.json?sort=top&t=week";
          break;
        case 8:
          str2 = "top/.json?sort=top&t=month";
          break;
        case 9:
          str2 = "top/.json?sort=top&t=year";
          break;
        case 10:
          str2 = "top/.json?sort=top&t=all";
          break;
        case 12:
          str2 = "new/.json?sort=rising";
          break;
      }
      string str3 = "";
      if (pos >= 0)
      {
        SubRedditData storyInSubreddit = this.BaconitStore.GetLastStoryInSubreddit(redditUrl, VirturalType);
        if (storyInSubreddit != null)
          str3 = storyInSubreddit.RedditID;
      }
      HttpWebRequest state;
      if (this.SettingsMan.IsSignedIn && str1 != null)
      {
        if (redditUrl.Equals("/saved/"))
          state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com/user/" + this.SettingsMan.UserName + "/saved/.json?after=" + str3);
        else if (VirturalType == 3 || VirturalType == 0)
          state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com" + redditUrl + str2 + ".json?after=" + str3);
        else
          state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com" + redditUrl + str2 + "&after=" + str3);
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
      else if (VirturalType == 3 || VirturalType == 0)
        state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com" + redditUrl + str2 + ".json?after=" + str3);
      else
        state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com" + redditUrl + str2 + "&after=" + str3);
      state.BeginGetResponse((AsyncCallback) (result =>
      {
        int millisecond = DateTime.Now.Millisecond;
        string str4 = "";
        try
        {
          if (result.AsyncState is HttpWebRequest asyncState2)
          {
            using (HttpWebResponse response = (HttpWebResponse) asyncState2.EndGetResponse(result))
            {
              using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
              {
                string absolutePath = asyncState2.RequestUri.AbsolutePath;
                if (absolutePath.Contains("/saved/"))
                {
                  str4 = "/saved/";
                  type = 0;
                }
                else
                {
                  int num1 = absolutePath.LastIndexOf("/");
                  int num2 = absolutePath.LastIndexOf("/", num1 - 1);
                  str4 = absolutePath.Substring(0, num2 + 1);
                }
                int num3 = 0;
                DataManager.CountHolder countHolder = new DataManager.CountHolder();
                bool LoadingMore;
                if ((countHolder.StoryCount = pos) != -1)
                {
                  num3 = this.BaconitStore.GetLastStoryInSubreddit(str4, type).SubRedditRank + 1;
                  LoadingMore = true;
                }
                else
                  LoadingMore = false;
                if (countHolder.StoryCount < 0)
                  countHolder.StoryCount = 0;
                if (countHolder.StoryCount == 0)
                  callback.SubRedditDataFeeder_Start(type, VirturalType, true);
                if (!LoadingMore)
                  this.BaconitStore.ClearLongText(str4, VirturalType);
                JObject jobject = JObject.Parse(streamReader.ReadToEnd());
                List<SubRedditData> subRedditData1 = new List<SubRedditData>();
                List<LongTextData> longTextData1 = new List<LongTextData>();
                JToken jtoken1 = jobject["data"][(object) "children"];
                int num4 = 0;
                foreach (JToken jtoken2 in (IEnumerable<JToken>) jtoken1)
                {
                  SubRedditData subRedditDataObj = new SubRedditData();
                  subRedditDataObj.ID = num3;
                  subRedditDataObj.SubRedditURL = str4;
                  subRedditDataObj.SubTypeOfSubReddit = VirturalType;
                  subRedditDataObj.SubRedditRank = num3;
                  subRedditDataObj.Domain = (string) jtoken2[(object) "data"][(object) "domain"];
                  subRedditDataObj.SubReddit = (string) jtoken2[(object) "data"][(object) "subreddit"];
                  subRedditDataObj.RedditID = (string) jtoken2[(object) "data"][(object) "name"];
                  subRedditDataObj.Author = (string) jtoken2[(object) "data"][(object) "author"];
                  subRedditDataObj.score = (int) jtoken2[(object) "data"][(object) "score"];
                  subRedditDataObj.thumbnail = (string) jtoken2[(object) "data"][(object) "thumbnail"];
                  subRedditDataObj.downs = (int) jtoken2[(object) "data"][(object) "downs"];
                  subRedditDataObj.isSelf = (bool) jtoken2[(object) "data"][(object) "is_self"];
                  subRedditDataObj.created = (float) jtoken2[(object) "data"][(object) "created_utc"];
                  subRedditDataObj.URL = (string) jtoken2[(object) "data"][(object) "url"];
                  subRedditDataObj.Title = (string) jtoken2[(object) "data"][(object) "title"];
                  subRedditDataObj.comments = (int) jtoken2[(object) "data"][(object) "num_comments"];
                  subRedditDataObj.ups = (int) jtoken2[(object) "data"][(object) "ups"];
                  subRedditDataObj.Permalink = (string) jtoken2[(object) "data"][(object) "permalink"];
                  subRedditDataObj.SelfText = (string) jtoken2[(object) "data"][(object) "selftext"];
                  subRedditDataObj.RedditRealID = (string) jtoken2[(object) "data"][(object) "id"];
                  subRedditDataObj.isPinned = false;
                  try
                  {
                    subRedditDataObj.isNotSafeForWork = (bool) jtoken2[(object) "data"][(object) "over_18"];
                  }
                  catch
                  {
                    subRedditDataObj.isNotSafeForWork = false;
                  }
                  bool flag;
                  if (this.SettingsMan.AdultFilter)
                  {
                    subRedditDataObj.Title = DataManager.FilterText(subRedditDataObj.Title);
                    subRedditDataObj.SelfText = DataManager.FilterText(subRedditDataObj.SelfText);
                    flag = subRedditDataObj.isNotSafeForWork;
                  }
                  else
                    flag = false;
                  if (subRedditDataObj.SelfText.Length > 0)
                  {
                    subRedditDataObj.hasSelfText = true;
                    int num5 = 0;
                    while (subRedditDataObj.SelfText.Length > 0)
                    {
                      LongTextData longTextData2 = new LongTextData();
                      longTextData2.RStoryName = subRedditDataObj.RedditID;
                      longTextData2.PartNum = num5;
                      longTextData2.SubRedditUrl = str4;
                      longTextData2.isPinned = false;
                      longTextData2.SubType = VirturalType;
                      if (subRedditDataObj.SelfText.Length > 4000)
                      {
                        longTextData2.text = subRedditDataObj.SelfText.Substring(0, 4000);
                        SubRedditData subRedditData2 = subRedditDataObj;
                        subRedditData2.SelfText = subRedditData2.SelfText.Substring(4000);
                      }
                      else
                      {
                        longTextData2.text = subRedditDataObj.SelfText.Substring(0, subRedditDataObj.SelfText.Length);
                        subRedditDataObj.SelfText = "";
                      }
                      longTextData1.Add(longTextData2);
                      ++num5;
                    }
                    this.BaconitStore.SetLongText(longTextData1, str4, VirturalType, LoadingMore);
                    longTextData1.Clear();
                  }
                  else
                    subRedditDataObj.hasSelfText = false;
                  JToken jtoken3 = jtoken2[(object) "data"][(object) "likes"];
                  subRedditDataObj.Like = jtoken3 == null || jtoken3.Type != JTokenType.Boolean ? 0 : (!(bool) jtoken3 ? -1 : 1);
                  JToken jtoken4 = jtoken2[(object) "data"][(object) "saved"];
                  subRedditDataObj.isSaved = jtoken4 != null && jtoken4.Type == JTokenType.Boolean && (bool) jtoken4;
                  JToken jtoken5 = jtoken2[(object) "data"][(object) "hidden"];
                  subRedditDataObj.isHidden = jtoken5 != null && jtoken5.Type == JTokenType.Boolean && (bool) jtoken5;
                  if (!flag)
                  {
                    callback.SubRedditDataFeeder_Feed(subRedditDataObj, countHolder, type, VirturalType, true);
                    subRedditData1.Add(subRedditDataObj);
                    ++num3;
                  }
                  ++num4;
                }
                this.BaconitStore.SetStories(subRedditData1, str4, VirturalType, LoadingMore);
                callback.SubRedditDataFeeder_Done(type, VirturalType, countHolder, true);
                callback.SetLoadingBar(type, false);
                callback.SetLoadingFromWeb(type, false);
                string subRedditRname = this.SubredditDataManager.GetSubRedditRName(str4);
                if (!LoadingMore)
                  this.ClearImageCache(subRedditRname, VirturalType);
                this.BaconitStore.UpdateLastUpdatedTime(subRedditRname + (object) VirturalType, false);
              }
            }
          }
          else if (callback.GetisOpen())
          {
            this.MessageManager.showWebErrorMessage("getting subreddit stories", (Exception) null);
            callback.SetLoadingBar(type, false);
          }
        }
        catch (Exception ex)
        {
          if (callback.GetisOpen())
          {
            if (!ex.Message.Trim().Equals("Parameter name: index") && !ex.Message.Trim().Equals("SubredditDoesNotExist"))
              this.MessageManager.showErrorMessage("getting subreddit stories", ex);
            callback.SetLoadingBar(type, false);
          }
        }
        callback.SetLoadingFromWeb(type, false);
        DataManager.BeingUpdated.Remove(str4 + (object) VirturalType);
      }), (object) state);
      return true;
    }

    public void GetSubredditTopImages(
      string redditUrl,
      int max,
      RunWorkerCompletedEventHandler callback)
    {
      HttpWebRequest state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com" + redditUrl + "hot/.json");
      if (this.SettingsMan.IsSignedIn && this.SettingsMan.UserCookie != null)
      {
        string str = HttpUtility.UrlEncode(this.SettingsMan.UserCookie);
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
                List<string> sender = new List<string>();
                JToken jtoken1 = jobject["data"][(object) "children"];
                int num = 0;
                foreach (JToken jtoken2 in (IEnumerable<JToken>) jtoken1)
                {
                  SubRedditData story = new SubRedditData();
                  story.URL = (string) jtoken2[(object) "data"][(object) "url"];
                  try
                  {
                    story.isNotSafeForWork = (bool) jtoken2[(object) "data"][(object) "over_18"];
                    if (story.isNotSafeForWork)
                      continue;
                  }
                  catch
                  {
                  }
                  string imageUrl = DataManager.GetImageURL(story, false);
                  if (imageUrl != null)
                  {
                    sender.Add(imageUrl);
                    ++num;
                    if (num > max)
                      break;
                  }
                }
                callback((object) sender, new RunWorkerCompletedEventArgs((object) true, (Exception) null, false));
              }
            }
          }
          else
            callback((object) null, new RunWorkerCompletedEventArgs((object) false, (Exception) null, false));
        }
        catch (Exception ex)
        {
          this.DebugDia("Counldn't update wallpaper images", ex);
          callback((object) null, new RunWorkerCompletedEventArgs((object) false, (Exception) null, false));
        }
      }), (object) state);
    }

    public event DataManager.AccountChangedEvent AccountChanged;

    public void UpdateAccountInformation(RunWorkerCompletedEventHandler callback)
    {
      string str1 = HttpUtility.UrlEncode(this.SettingsMan.UserCookie);
      if (this.SettingsMan.IsSignedIn && str1 != null && !str1.Equals("") && !this.SettingsMan.ModHash.Equals(""))
      {
        HttpWebRequest state = (HttpWebRequest) WebRequest.Create("https://www.reddit.com/api/me.json");
        state.CookieContainer = new CookieContainer();
        state.CookieContainer.Add(new Uri("https://api.reddit.com"), new Cookie()
        {
          Domain = ".reddit.com",
          HttpOnly = false,
          Path = "/",
          Secure = false,
          Value = str1,
          Name = "reddit_session"
        });
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
                    if (this.MessageManager != null)
                      this.MessageManager.showRedditErrorMessage("update the account information", stringInJson);
                    throw new Exception("json error");
                  }
                  if (!this.SettingsMan.HasMail)
                    this.SettingsMan.UnreadInboxMessageCount = 0;
                  this.SettingsMan.HasMail = false;
                  string str2 = (string) jobject["data"][(object) "name"];
                  if (str2 == null || str2.Equals(""))
                    throw new Exception();
                  this.SettingsMan.UserName = (string) jobject["data"][(object) "name"];
                  this.SettingsMan.HasMail = (bool) jobject["data"][(object) "has_mail"];
                  this.SettingsMan.LinkKarma = (int) jobject["data"][(object) "link_karma"];
                  this.SettingsMan.CommentKarma = (int) jobject["data"][(object) "comment_karma"];
                  this.SettingsMan.UserAccountCreated = (double) jobject["data"][(object) "created_utc"];
                  if (this.SettingsMan.HasMail)
                  {
                    this.UpdateInboxMessages(false);
                  }
                  else
                  {
                    this.SettingsMan.UnreadInboxMessageCount = 0;
                    this.UpdateApplicationTile();
                  }
                  callback((object) true, new RunWorkerCompletedEventArgs((object) true, (Exception) null, false));
                  if (this.BaconitStore == null || this.RunningFromUpdater)
                    return;
                  this.BaconitStore.UpdateLastUpdatedTime("!Main_Account", false);
                }
              }
            }
            else
            {
              if (this.MessageManager != null)
                this.MessageManager.showWebErrorMessage("updating the account information", (Exception) null);
              callback((object) false, new RunWorkerCompletedEventArgs((object) false, (Exception) null, false));
              if (this.BaconitStore == null)
                return;
              this.BaconitStore.UpdateLastUpdatedTime("!Main_Account", true);
            }
          }
          catch (Exception ex)
          {
            if (this.MessageManager != null)
              this.MessageManager.showErrorMessage("updating the account information", ex);
            callback((object) false, new RunWorkerCompletedEventArgs((object) false, (Exception) null, false));
            if (this.BaconitStore == null)
              return;
            this.BaconitStore.UpdateLastUpdatedTime("!Main_Account", true);
          }
        }), (object) state);
      }
      else
      {
        this.SettingsMan.UserName = "";
        this.SettingsMan.HasMail = false;
        this.SettingsMan.LinkKarma = 0;
        this.SettingsMan.CommentKarma = 0;
        this.SettingsMan.UserCookie = "";
        this.SettingsMan.ModHash = "";
        this.SettingsMan.UserAccountCreated = -1.0;
        this.UpdateApplicationTile();
        callback((object) false, new RunWorkerCompletedEventArgs((object) false, (Exception) null, false));
      }
    }

    public void SwitchUserAccount(RedditAccount account)
    {
      this.SettingsMan.UserName = account.UserName;
      this.SettingsMan.ModHash = account.ModHas;
      this.SettingsMan.UserCookie = account.Cookie;
      this.SettingsMan.UserAccountCreated = -1.0;
      try
      {
        this.VIPDataMan.Save();
      }
      catch
      {
      }
      ThreadPool.QueueUserWorkItem((WaitCallback) (o => this.BaconitStore.ClearData()));
      this.SubredditDataManager.UpdateSubReddits();
      if (this.AccountChanged != null)
        this.AccountChanged(true, false);
      this.BaconitAnalytics.LogEvent("User Account Switched");
    }

    public void SignIn(string user, string password, RunWorkerCompletedEventHandler callback)
    {
      PostClient postClient = new PostClient((IDictionary<string, object>) new Dictionary<string, object>()
      {
        {
          "api_type",
          (object) "json"
        },
        {
          nameof (user),
          (object) HttpUtility.UrlEncode(user)
        },
        {
          "passwd",
          (object) HttpUtility.UrlEncode(password)
        }
      });
      postClient.DownloadStringCompleted += (PostClient.DownloadStringCompletedHandler) ((sender, e) =>
      {
        if (e.Error == null)
        {
          try
          {
            JObject jobject = JObject.Parse(e.Result);
            if (jobject["json"][(object) "errors"].HasValues)
            {
              callback((object) null, new RunWorkerCompletedEventArgs((object) jobject["json"][(object) "errors"][(object) 0][(object) 1], (Exception) null, false));
            }
            else
            {
              this.SettingsMan.ModHash = (string) jobject["json"][(object) "data"][(object) "modhash"];
              this.SettingsMan.UserCookie = (string) jobject["json"][(object) "data"][(object) "cookie"];
              this.SettingsMan.UserName = user;
              try
              {
                this.VIPDataMan.Save();
              }
              catch
              {
              }
              int index = 0;
              bool flag = true;
              for (; index < this.SettingsMan.UserAccounts.Count; ++index)
              {
                int num = this.SettingsMan.UserAccounts[index].UserName.CompareTo(this.SettingsMan.UserName);
                if (num == 0)
                {
                  flag = false;
                  break;
                }
                if (num > 0)
                  break;
              }
              if (flag)
              {
                this.SettingsMan.UserAccounts.Insert(index, new RedditAccount(this.SettingsMan.UserName, DateTime.Now, this.SettingsMan.ModHash, this.SettingsMan.UserCookie));
                this.SettingsMan.Save();
              }
              if (this.AccountChanged != null)
                this.AccountChanged(true, false);
              this.BaconitStore.ClearData();
              callback((object) null, new RunWorkerCompletedEventArgs((object) "success", (Exception) null, false));
            }
          }
          catch
          {
            callback((object) null, new RunWorkerCompletedEventArgs((object) "Reddit Error", (Exception) null, false));
          }
        }
        else
          callback((object) null, new RunWorkerCompletedEventArgs((object) e.Error.Message, (Exception) null, false));
      });
      postClient.DownloadStringAsync(new Uri("https://ssl.reddit.com/api/login/" + user, UriKind.Absolute));
    }

    public void Logout()
    {
      this.SettingsMan.ModHash = "";
      this.SettingsMan.UserCookie = "";
      this.SettingsMan.UserName = "";
      this.SettingsMan.UserAccountCreated = -1.0;
      this.SettingsMan.LinkKarma = 0;
      this.SettingsMan.CommentKarma = 0;
      this.SettingsMan.UnreadInboxMessageCount = 0;
      this.SettingsMan.HasMail = false;
      this.SettingsMan.UserAccounts = new List<RedditAccount>();
      this.SettingsMan.Save();
      this.UpdateApplicationTile();
      this.SubredditDataManager.UpdateSubReddits();
      this.BaconitStore.ClearData();
      if (this.AccountChanged == null)
        return;
      this.AccountChanged(false, true);
    }

    public void SetIfExists(string key)
    {
      if (!this.SettingsMan.settings.Contains(key))
        return;
      this.VIPDataMan.SetValue(key, this.SettingsMan.settings[key]);
    }

    public static string GetImageURL(SubRedditData story, bool getSmallerIfPossible)
    {
      string imageUrl = (string) null;
      if (story.URL.EndsWith(".png") || story.URL.EndsWith(".jpg") || story.URL.EndsWith(".bmp") || story.URL.EndsWith(".gif") || story.URL.EndsWith(".gifv"))
      {
        imageUrl = story.URL;
        if (imageUrl.Contains("imgur.com/"))
        {
          if (getSmallerIfPossible)
          {
            try
            {
              int startIndex = imageUrl.LastIndexOf(".png");
              if (startIndex != -1)
                imageUrl = imageUrl.Insert(startIndex, "l");
            }
            catch
            {
              imageUrl = story.URL;
            }
          }
        }
      }
      else if (story.URL.Contains("imgur.com/"))
      {
        if (!story.URL.Contains("imgur.com/a/") && !story.URL.Contains(".gif"))
        {
          int num = story.URL.LastIndexOf('/');
          if (num != -1)
            imageUrl = "http://i.imgur.com/" + story.URL.Substring(num + 1) + ".jpg";
        }
      }
      else if (story.URL.Contains("qkme.me/"))
      {
        int num1 = story.URL.LastIndexOf('/');
        int num2 = story.URL.LastIndexOf('?');
        if (num1 != -1 && num2 != -1)
          imageUrl = "http://i.qkme.me/" + story.URL.Substring(num1 + 1, num2 - num1 - 1) + ".jpg";
        else if (num1 != -1)
          imageUrl = "http://i.qkme.me/" + story.URL.Substring(num1 + 1) + ".jpg";
      }
      else if (story.URL.Contains("quickmeme.com/"))
      {
        int num3 = story.URL.LastIndexOf('/');
        if (num3 > 0)
        {
          int num4 = story.URL.LastIndexOf('/', num3 - 1);
          if (num3 != -1 && num4 != -1)
            imageUrl = "http://i.qkme.me/" + story.URL.Substring(num4 + 1, num3 - num4 - 1) + ".jpg";
          else if (num3 != -1)
            imageUrl = "http://i.qkme.me/" + story.URL.Substring(num3 + 1) + ".jpg";
        }
      }
      return imageUrl;
    }

    public static string FindStringInJson(JToken response, string toFind)
    {
      if (response == null)
        return "";
      try
      {
        foreach (JToken source in (IEnumerable<JToken>) response)
        {
          if (source.Count<JToken>() > 2)
          {
            JToken jtoken = source[(object) 3];
            if (jtoken.Type == JTokenType.Array && jtoken.HasValues && ((string) jtoken[(object) 0]).StartsWith(toFind))
              return (string) jtoken[(object) 0];
          }
        }
      }
      catch
      {
      }
      return "";
    }

    public void FormatText(BlockCollection AddTo, string text)
    {
      text = text.Replace('\r', '\n');
      Paragraph paragraph1 = new Paragraph();
      int startIndex1 = 0;
      try
      {
        while (startIndex1 < text.Length)
        {
          int startIndex2 = text.IndexOf("](", startIndex1);
          if (startIndex2 == -1)
            startIndex2 = text.IndexOf("] (", startIndex1);
          int num1 = -1;
          if (startIndex2 != -1)
          {
            num1 = text.LastIndexOf('[', startIndex2);
            if (text.IndexOf(')', startIndex2 + 1) == -1 || text.IndexOf('[') > startIndex2 + 1)
              startIndex2 = -1;
          }
          int val1_1 = text.IndexOf("http://", startIndex1);
          int val2_1 = text.IndexOf("https://", startIndex1);
          if (val1_1 == -1)
            val1_1 = val2_1;
          else if (val2_1 != -1)
            val1_1 = Math.Min(val1_1, val2_1);
          int startIndex3 = text.IndexOf("/r/", startIndex1);
          int startIndex4 = text.IndexOf("/u/", startIndex1);
          int num2 = text.IndexOf("**", startIndex1);
          if (num2 != -1 && text.IndexOf("**", num2 + 2) == -1)
            num2 = -1;
          int startIndex5;
          for (int startIndex6 = startIndex1; (startIndex5 = text.IndexOf('*', startIndex6)) != -1; startIndex6 = startIndex5 + 1)
          {
            int num3 = text.IndexOf('*', startIndex5 + 1);
            if (num3 == startIndex5 + 1)
              ++startIndex5;
            else if ((startIndex5 == 0 || text.IndexOf('\n', startIndex5 - 1) == startIndex5 - 1) && text.IndexOf(' ', startIndex5) == startIndex5 + 1)
            {
              text = text.Remove(startIndex5, 1);
              text = text.Insert(startIndex5, "•");
            }
            else if (num3 != -1 && num3 < text.IndexOf('\n', startIndex5 + 1) || num3 != -1 && text.IndexOf('\n', startIndex5 + 1) == -1)
              break;
          }
          int startIndex7 = text.IndexOf("zune://", startIndex1);
          Run run1 = new Run();
          if (startIndex2 == -1 && num2 == -1 && val1_1 == -1 && startIndex5 == -1 && startIndex7 == -1 && startIndex3 == -1 && startIndex4 == -1)
          {
            run1.Text = text.Substring(startIndex1, text.Length - startIndex1);
            paragraph1.Inlines.Add((Inline) run1);
            int length = text.Length;
            break;
          }
          if (num2 != -1 && (val1_1 != -1 || startIndex2 != -1))
          {
            int num4 = text.IndexOf("**", num2 + 2);
            if (val1_1 != -1 && num4 > val1_1)
              num2 = -1;
            else if (startIndex2 != -1 && num4 > startIndex2)
              num2 = -1;
          }
          int val1_2 = num1;
          if (val1_2 == -1)
            val1_2 = 9999999;
          int val2_2 = startIndex3;
          if (startIndex3 == -1)
            val2_2 = 9999999;
          int val2_3 = startIndex4;
          if (startIndex4 == -1)
            val2_3 = 9999999;
          int val1_3 = num2;
          if (val1_3 == -1)
            val1_3 = 9999999;
          int val2_4 = val1_1;
          if (val2_4 == -1)
            val2_4 = 9999999;
          int val2_5 = startIndex5;
          if (val2_5 == -1)
            val2_5 = 9999999;
          if (num2 != -1 && num2 == startIndex5)
            val2_5 = 9999999;
          int val2_6 = startIndex7;
          if (val2_6 == -1)
            val2_6 = 9999999;
          int num5 = Math.Min(Math.Min(Math.Min(Math.Min(val1_2, Math.Min(Math.Min(val1_3, val2_4), val2_5)), val2_6), val2_3), val2_2);
          switch (num5 != val1_2 ? (num5 != val1_3 ? (num5 != val2_4 ? (num5 != val2_5 ? (num5 != val2_2 ? (num5 != val2_3 ? 7 : 6) : 5) : 4) : 3) : 2) : 1)
          {
            case 1:
              int num6 = text.IndexOf('[', startIndex1);
              run1.Text = text.Substring(startIndex1, num6 - startIndex1);
              paragraph1.Inlines.Add((Inline) run1);
              Hyperlink hyperlink1 = new Hyperlink();
              int startIndex8 = num6 + 1;
              int startIndex9 = text.IndexOf(']', startIndex8);
              int startIndex10 = text.IndexOf('(', startIndex9) + 1;
              int num7 = text.IndexOf(')', startIndex10);
              string text1 = text.Substring(startIndex8, startIndex9 - startIndex8);
              string str1 = text.Substring(startIndex10, num7 - startIndex10);
              if (text1.Equals(""))
                text1 = str1;
              if (str1.StartsWith("/r/"))
                str1 = "https://www.reddit.com" + str1;
              hyperlink1.Inlines.Add(DataManager.SimpleFormatString(text1));
              hyperlink1.Foreground = (Brush) new SolidColorBrush(DataManager.ACCENT_COLOR_COLOR);
              bool flag1 = false;
              string uriString1 = "";
              try
              {
                uriString1 = DataManager.GetLocalNavStringFromRedditURL(str1);
                if (!string.IsNullOrEmpty(uriString1))
                  flag1 = true;
              }
              catch
              {
                flag1 = false;
              }
              if (flag1 && !uriString1.Equals(""))
              {
                hyperlink1.NavigateUri = new Uri(uriString1, UriKind.Relative);
                paragraph1.Inlines.Add((Inline) hyperlink1);
              }
              else if (str1.Contains("://"))
              {
                try
                {
                  if (this.SettingsMan.UseInAppBroser)
                  {
                    hyperlink1.NavigateUri = new Uri("/InAppWebBrowser.xaml?Url=" + str1, UriKind.Relative);
                  }
                  else
                  {
                    hyperlink1.TargetName = "_blank";
                    hyperlink1.NavigateUri = new Uri(str1);
                  }
                  paragraph1.Inlines.Add((Inline) hyperlink1);
                }
                catch
                {
                }
              }
              else
                paragraph1.Inlines.Add((Inline) new Run()
                {
                  Text = (text1 + " [comment image]")
                });
              startIndex1 = num7 + 1;
              continue;
            case 2:
              run1.Text = text.Substring(startIndex1, num2 - startIndex1);
              paragraph1.Inlines.Add((Inline) run1);
              int num8 = text.IndexOf("**", num2 + 2);
              Run run2 = new Run();
              run2.Text = text.Substring(num2 + 2, num8 - num2 - 2);
              run2.FontWeight = FontWeights.Bold;
              paragraph1.Inlines.Add((Inline) run2);
              startIndex1 = num8 + 2;
              continue;
            case 3:
              int num9 = text.IndexOf("http://", startIndex1);
              int val2_7 = text.IndexOf("https://", startIndex1);
              if (num9 == -1)
                num9 = val2_7;
              else if (val2_7 != -1)
                num9 = Math.Min(num9, val2_7);
              run1.Text = text.Substring(startIndex1, num9 - startIndex1);
              paragraph1.Inlines.Add((Inline) run1);
              Hyperlink hyperlink2 = new Hyperlink();
              int num10 = Math.Min(Math.Min(text.IndexOf(' ', num9) == -1 ? 9999999 : text.IndexOf(' ', num9), text.IndexOf('\n', num9) == -1 ? 9999999 : text.IndexOf('\n', num9)), text.Length);
              hyperlink2.Inlines.Add(DataManager.SimpleFormatString(text.Substring(num9, num10 - num9)) + " ");
              hyperlink2.Foreground = (Brush) new SolidColorBrush(DataManager.ACCENT_COLOR_COLOR);
              string str2 = text.Substring(num9, num10 - num9);
              bool flag2 = false;
              string uriString2 = "";
              try
              {
                uriString2 = DataManager.GetLocalNavStringFromRedditURL(str2);
                if (!string.IsNullOrEmpty(uriString2))
                  flag2 = true;
              }
              catch
              {
                flag2 = false;
              }
              if (flag2 && !uriString2.Equals(""))
              {
                hyperlink2.NavigateUri = new Uri(uriString2, UriKind.Relative);
              }
              else
              {
                try
                {
                  if (this.SettingsMan.UseInAppBroser)
                  {
                    hyperlink2.NavigateUri = new Uri("/InAppWebBrowser.xaml?Url=" + str2, UriKind.Relative);
                  }
                  else
                  {
                    hyperlink2.NavigateUri = new Uri(str2);
                    hyperlink2.TargetName = "_blank";
                  }
                }
                catch
                {
                }
              }
              paragraph1.Inlines.Add((Inline) hyperlink2);
              startIndex1 = num10 + 1;
              continue;
            case 4:
              run1.Text = text.Substring(startIndex1, startIndex5 - startIndex1);
              paragraph1.Inlines.Add((Inline) run1);
              int num11 = text.IndexOf("*", startIndex5 + 1);
              if (num11 == -1)
                num11 = text.Length;
              Run run3 = new Run();
              run3.Text = text.Substring(startIndex5 + 1, num11 - startIndex5 - 1);
              run3.FontStyle = FontStyles.Italic;
              paragraph1.Inlines.Add((Inline) run3);
              startIndex1 = num11 + 1;
              continue;
            case 5:
              run1.Text = text.Substring(startIndex1, startIndex3 - startIndex1);
              paragraph1.Inlines.Add((Inline) run1);
              int num12 = text.IndexOf(" ", startIndex3);
              if (num12 == -1)
                num12 = text.IndexOf(")", startIndex3);
              if (num12 == -1)
                num12 = text.IndexOf("\"", startIndex3);
              if (num12 == -1)
                num12 = text.Length;
              Hyperlink hyperlink3 = new Hyperlink();
              string text2 = text.Substring(startIndex3, num12 - startIndex3);
              hyperlink3.Inlines.Add(text2);
              string uriString3 = "/RedditsViewer.xaml?subredditURL=" + text2;
              hyperlink3.NavigateUri = new Uri(uriString3, UriKind.Relative);
              paragraph1.Inlines.Add((Inline) hyperlink3);
              startIndex1 = num12;
              continue;
            case 6:
              run1.Text = text.Substring(startIndex1, startIndex4 - startIndex1);
              paragraph1.Inlines.Add((Inline) run1);
              int num13 = text.IndexOf(" ", startIndex4);
              if (num13 == -1)
                num13 = text.IndexOf(")", startIndex4);
              if (num13 == -1)
                num13 = text.IndexOf("\"", startIndex4);
              if (num13 == -1)
                num13 = text.Length;
              Hyperlink hyperlink4 = new Hyperlink();
              string text3 = text.Substring(startIndex4, num13 - startIndex4);
              hyperlink4.Inlines.Add(text3);
              string uriString4 = "/ProfileView.xaml?userName=" + text3.Substring(3);
              hyperlink4.NavigateUri = new Uri(uriString4, UriKind.Relative);
              paragraph1.Inlines.Add((Inline) hyperlink4);
              startIndex1 = num13;
              continue;
            case 7:
              run1.Text = text.Substring(startIndex1, startIndex7 - startIndex1);
              paragraph1.Inlines.Add((Inline) run1);
              int num14 = Math.Min(text.IndexOf(' ', startIndex7), text.IndexOf('\n', startIndex7));
              if (num14 == -1)
                num14 = text.Length;
              string str3 = text.Substring(startIndex7, num14 - startIndex7);
              int num15 = str3.IndexOf('=');
              string str4 = str3.Substring(num15 + 1);
              Hyperlink hyperlink5 = new Hyperlink();
              hyperlink5.Inlines.Add(str3 + "\n");
              hyperlink5.Foreground = (Brush) new SolidColorBrush(DataManager.ACCENT_COLOR_COLOR);
              try
              {
                if (this.SettingsMan.UseInAppBroser)
                {
                  hyperlink5.NavigateUri = new Uri("/InAppWebBrowser.xaml?Url=http://windowsphone.com/s?appId=" + str4, UriKind.Relative);
                }
                else
                {
                  hyperlink5.NavigateUri = new Uri("http://windowsphone.com/s?appId=" + str4);
                  hyperlink5.TargetName = "_blank";
                }
              }
              catch
              {
              }
              paragraph1.Inlines.Add((Inline) hyperlink5);
              startIndex1 = num14 + 1;
              continue;
            default:
              continue;
          }
        }
        AddTo.Clear();
        AddTo.Add((Block) paragraph1);
      }
      catch (Exception ex)
      {
        this.DebugDia("Error parsing selfText", ex);
        Run run = new Run();
        run.Text = text;
        Paragraph paragraph2 = new Paragraph();
        paragraph2.Inlines.Add((Inline) run);
        AddTo.Clear();
        AddTo.Add((Block) paragraph2);
      }
    }

    public static string GetLocalNavStringFromRedditURL(string urlString)
    {
      try
      {
        urlString = urlString.ToLower();
        if (urlString.Equals("https://reddit.com") || urlString.Equals("https://www.reddit.com") || urlString.Equals("https://reddit.com/") || urlString.Equals("https://www.reddit.com/"))
          return "/RedditsViewer.xaml?subredditURL=/";
        if (urlString.Contains("reddit.com/r/"))
        {
          int num1 = urlString.IndexOf("comments/");
          if (num1 != -1)
          {
            int startIndex1 = num1 + 9;
            int num2 = urlString.IndexOf("/", startIndex1);
            if (num2 == -1)
              num2 = urlString.Length;
            int startIndex2 = urlString.LastIndexOf("/") + 1;
            string str = urlString.Substring(startIndex2, urlString.Length - startIndex2);
            return !string.IsNullOrEmpty(str) && str.Length > 5 && str.Length < 10 ? "/StoryDetails.xaml?StoryDataRedditID=t3_" + urlString.Substring(startIndex1, num2 - startIndex1) + "&ShowComment=t1_" + str : "/StoryDetails.xaml?StoryDataRedditID=t3_" + urlString.Substring(startIndex1, num2 - startIndex1);
          }
          int startIndex = urlString.IndexOf("/r/");
          int num3 = urlString.IndexOf("/", startIndex + 3) + 1;
          if (num3 == 0)
            num3 = urlString.Length;
          string str1 = urlString.Substring(startIndex, num3 - startIndex);
          if (!str1.EndsWith("/"))
            str1 += "/";
          return "/RedditsViewer.xaml?subredditURL=" + str1;
        }
        if (urlString.Contains("reddit.com/user/"))
        {
          int startIndex = urlString.IndexOf("user/") + 5;
          int length = urlString.Length;
          string str = urlString.Substring(startIndex, length - startIndex);
          if (str.EndsWith("/"))
            str = str.Substring(0, str.Length - 1);
          return "/ProfileView.xaml?userName=" + str;
        }
      }
      catch
      {
      }
      return "";
    }

    public static string SimpleFormatString(string text)
    {
      if (text == null)
        return "";
      text = text.Trim();
      text = HttpUtility.HtmlDecode(text);
      try
      {
        string str1 = string.Empty;
        int num1;
        for (int startIndex1 = 0; startIndex1 < text.Length; startIndex1 = num1 + 1)
        {
          if (text.IndexOf("](", startIndex1) == -1)
          {
            str1 += text.Substring(startIndex1, text.Length - startIndex1);
            int length = text.Length;
            break;
          }
          int startIndex2 = text.IndexOf('[', startIndex1);
          string str2 = str1 + text.Substring(startIndex1, startIndex2 - startIndex1);
          int startIndex3 = text.IndexOf(']', startIndex2);
          string str3 = text.Substring(startIndex2 + 1, startIndex3 - startIndex2 - 1);
          int startIndex4 = text.IndexOf('(', startIndex3) + 1;
          num1 = text.IndexOf(')', startIndex3);
          if (num1 == -1)
            num1 = text.Length;
          str1 = !text.Substring(startIndex4, num1 - startIndex4).Contains("://") ? str2 + str3 : str2 + "[link] " + str3;
        }
        int startIndex5 = 0;
        int startIndex6 = text.IndexOf("http://", startIndex5);
        while (startIndex6 != -1)
        {
          if (startIndex6 > 1 && text[startIndex6 - 1] == '[' || startIndex6 > 2 && text[startIndex6 - 2] == ']' && text[startIndex6 - 1] == '(')
          {
            startIndex6 = text.IndexOf("http://", startIndex6 + 1);
          }
          else
          {
            int val1 = text.IndexOf(" ", startIndex6);
            int val2 = text.IndexOf("\n", startIndex6);
            int num2 = val1 != -1 || val2 != -1 ? (val1 != -1 || val2 == -1 ? (val1 == -1 || val2 != -1 ? Math.Min(val1, val2) : val1) : val2) : text.Length;
            text.Substring(startIndex6, num2 - startIndex6);
            startIndex6 = text.IndexOf("http://", startIndex6 + 1);
          }
        }
        return str1.Replace("**", "");
      }
      catch (Exception ex)
      {
        return text;
      }
    }

    public static string FilterText(string text)
    {
      if (text == null)
        return (string) null;
      text = new Regex(" ass", RegexOptions.IgnoreCase).Replace(text, " a##");
      text = new Regex("bitch", RegexOptions.IgnoreCase).Replace(text, "b####");
      text = new Regex(" butt", RegexOptions.IgnoreCase).Replace(text, " b###");
      text = new Regex(" cock", RegexOptions.IgnoreCase).Replace(text, " c###");
      text = new Regex(" cum", RegexOptions.IgnoreCase).Replace(text, " c##");
      text = new Regex(" cunt", RegexOptions.IgnoreCase).Replace(text, " c###");
      text = new Regex(" damn", RegexOptions.IgnoreCase).Replace(text, " d###");
      text = new Regex(" dick", RegexOptions.IgnoreCase).Replace(text, " d###");
      text = new Regex(" fag", RegexOptions.IgnoreCase).Replace(text, "f##");
      text = new Regex("fuck", RegexOptions.IgnoreCase).Replace(text, " f###");
      text = new Regex(" damn", RegexOptions.IgnoreCase).Replace(text, " d###");
      text = new Regex(" dick", RegexOptions.IgnoreCase).Replace(text, " d###");
      text = new Regex(" fag", RegexOptions.IgnoreCase).Replace(text, " f##");
      text = new Regex(" jackass", RegexOptions.IgnoreCase).Replace(text, " j######");
      text = new Regex(" jizz ", RegexOptions.IgnoreCase).Replace(text, " j### ");
      text = new Regex(" nigger", RegexOptions.IgnoreCase).Replace(text, " n#####");
      text = new Regex(" nutsack ", RegexOptions.IgnoreCase).Replace(text, " n###### ");
      text = new Regex(" penis ", RegexOptions.IgnoreCase).Replace(text, " p#### ");
      text = new Regex("pussy", RegexOptions.IgnoreCase).Replace(text, "p####");
      text = new Regex("queer", RegexOptions.IgnoreCase).Replace(text, "q####");
      text = new Regex(" shit", RegexOptions.IgnoreCase).Replace(text, " s###");
      text = new Regex(" tit ", RegexOptions.IgnoreCase).Replace(text, " t### ");
      text = new Regex("vagina", RegexOptions.IgnoreCase).Replace(text, " v#####");
      text = new Regex(" whore", RegexOptions.IgnoreCase).Replace(text, " w####");
      return text;
    }

    public void UpdateApplicationTile()
    {
      ShellTile shellTile = ShellTile.ActiveTiles.First<ShellTile>();
      if (shellTile == null)
        return;
      string str1 = "";
      string str2;
      string str3;
      if (this.SettingsMan.UnreadInboxMessageCount > 0 && this.SettingsMan.MainLiveTileShowUnread)
      {
        str1 = this.SettingsMan.UnreadInboxMessageCount != 1 ? this.SettingsMan.UnreadInboxMessageCount.ToString() + " New Messages" : "1 New Message";
        str2 = this.SettingsMan.MostRecentInboxMessageTextBody;
        str3 = this.SettingsMan.MostRecentInboxMessageTextAuthor;
      }
      else if (this.SettingsMan.MainLiveTileShowKarma)
      {
        str3 = "Link Karma: " + (object) this.SettingsMan.LinkKarma;
        str2 = "Comment Karma: " + (object) this.SettingsMan.CommentKarma;
        if (this.SettingsMan.UserAccountCreated >= 0.0)
        {
          DateTime dateTime1 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
          DateTime now = DateTime.Now;
          DateTime dateTime2 = dateTime1.AddSeconds(this.SettingsMan.UserAccountCreated);
          int number = now.Year - dateTime2.Year;
          DateTime dateTime3 = dateTime2.AddYears(number);
          int num = -now.ToUniversalTime().Subtract(dateTime3).Days;
          if (dateTime2.Month == now.Month && now.Day == dateTime2.Day && number != 0)
          {
            str1 = "Happy " + DataManager.NumberEndingAdder(number) + " Cake Day!!!";
            str2 = "Happy cake day to you sir / ma'am,";
            str3 = "have a gold star \uD83C\uDF1F";
          }
          else
            str1 = num != 0 ? (num >= 0 ? "Cake Day in " + (object) num + " days" : "Cake Day in " + (object) (num + 365) + " days") : "Welcome to Reddit!";
        }
      }
      else
      {
        str1 = "";
        str2 = "";
        str3 = "";
      }
      int num1 = this.SettingsMan.UnreadInboxMessageCount;
      if (num1 < 0)
        num1 = 0;
      if (!this.SettingsMan.MainLiveTileShowUnread || !this.SettingsMan.IsSignedIn)
        num1 = 0;
      string str4 = this.SettingsMan.MainTileBackground < 0 || this.SettingsMan.MainTileBackground >= ((IEnumerable<string>) DataManager.LiveTileBackgrounds).Count<string>() ? DataManager.LiveTileBackgrounds[0] : DataManager.LiveTileBackgrounds[this.SettingsMan.MainTileBackground];
      IconicTileData iconicTileData = new IconicTileData();
      iconicTileData.Count = new int?(num1);
      iconicTileData.IconImage = new Uri(str4 + "m.png", UriKind.Relative);
      iconicTileData.SmallIconImage = new Uri(str4 + "s.png", UriKind.Relative);
      iconicTileData.Title = "Baconit";
      iconicTileData.WideContent1 = str1;
      iconicTileData.WideContent2 = str2;
      iconicTileData.WideContent3 = str3;
      ShellTileData data = (ShellTileData) iconicTileData;
      shellTile.Update(data);
    }

    public static string NumberEndingAdder(int number)
    {
      switch (number)
      {
        case 1:
          return "1st";
        case 2:
          return "2nd";
        case 3:
          return "3rd";
        default:
          if (number < 20)
            return number.ToString() + "th";
          switch (number % 10)
          {
            case 1:
              return number.ToString() + "st";
            case 2:
              return number.ToString() + "nd";
            case 3:
              return number.ToString() + "rd";
            default:
              return number.ToString() + "th";
          }
      }
    }

    public void DebugDia(string str, Exception e)
    {
      if (this.MessageManager == null)
        return;
      this.MessageManager.DebugDia(str, e);
    }

    public delegate void MessageInboxUpdatedEventHandler(
      bool success,
      List<Message> NewList,
      bool UpdateUI);

    public class CountHolder
    {
      public int StoryCount;
      public int ImageCount;
    }

    public delegate void AccountChangedEvent(bool AccountAdded, bool AccountLogedout);
  }
}
