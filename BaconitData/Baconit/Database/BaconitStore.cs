// Decompiled with JetBrains decompiler
// Type: Baconit.Database.BaconitStore
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;

#nullable disable
namespace Baconit.Database
{
  public class BaconitStore
  {
    private string FILENAME = "TimeData.data";
    private Dictionary<string, double> TimeDict;
    private static int databaseVersion = 27;
    private DataManager DataMan;

    public BaconitStore(DataManager data) => this.DataMan = data;

    public void Initialize()
    {
      using (BaconitDataContext baconitDataContext = new BaconitDataContext("Data Source = 'isostore:/dataEight.sdf'; Max Database Size = 256"))
      {
        if (this.DataMan.SettingsMan.DatabaseVersion != BaconitStore.databaseVersion)
        {
          baconitDataContext.DeleteDatabase();
          this.DataMan.SettingsMan.DatabaseVersion = BaconitStore.databaseVersion;
        }
        if (baconitDataContext.DatabaseExists())
          return;
        baconitDataContext.CreateDatabase();
        this.InitSubReddits();
      }
    }

    public void Save()
    {
      IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication();
      lock (this.FILENAME)
      {
        try
        {
          using (IsolatedStorageFileStream storageFileStream = storeForApplication.OpenFile(this.FILENAME, FileMode.OpenOrCreate))
            new DataContractSerializer(typeof (Dictionary<string, double>)).WriteObject((Stream) storageFileStream, (object) this.TimeDict);
        }
        catch
        {
        }
      }
    }

    private void EnsureDictionary()
    {
      if (this.TimeDict != null)
        return;
      IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication();
      if (storeForApplication.FileExists(this.FILENAME))
      {
        lock (this.FILENAME)
        {
          try
          {
            using (IsolatedStorageFileStream storageFileStream = storeForApplication.OpenFile(this.FILENAME, FileMode.OpenOrCreate))
              this.TimeDict = (Dictionary<string, double>) new DataContractSerializer(typeof (Dictionary<string, double>)).ReadObject((Stream) storageFileStream);
          }
          catch
          {
            try
            {
              storeForApplication.DeleteFile(this.FILENAME);
            }
            catch
            {
            }
          }
          if (this.TimeDict != null)
            return;
          this.TimeDict = new Dictionary<string, double>();
        }
      }
      else
        this.TimeDict = new Dictionary<string, double>();
    }

    public void ClearLongText(string subReddit, int type)
    {
      using (BaconitDataContext baconitDataContext = new BaconitDataContext("Data Source = 'isostore:/dataEight.sdf'; Max Database Size = 256"))
      {
        try
        {
          IQueryable<LongTextData> source = baconitDataContext.LongTextData.Where<LongTextData>((Expression<Func<LongTextData, bool>>) (data => data.SubRedditUrl == subReddit && data.SubType == type && data.isPinned == false));
          baconitDataContext.LongTextData.DeleteAllOnSubmit<LongTextData>((IEnumerable<LongTextData>) source.ToArray<LongTextData>());
          baconitDataContext.SubmitChanges();
        }
        catch (Exception ex)
        {
          this.DataMan.DebugDia("Error clearing long text", ex);
        }
      }
    }

    public void SetLongText(
      List<LongTextData> longTextData,
      string subReddit,
      int type,
      bool LoadingMore)
    {
      if (longTextData.Count < 1)
        return;
      using (BaconitDataContext baconitDataContext = new BaconitDataContext("Data Source = 'isostore:/dataEight.sdf'; Max Database Size = 256"))
      {
        try
        {
          IQueryable<LongTextData> source = baconitDataContext.LongTextData.Where<LongTextData>((Expression<Func<LongTextData, bool>>) (data => data.RStoryName == longTextData[0].RStoryName));
          baconitDataContext.LongTextData.DeleteAllOnSubmit<LongTextData>((IEnumerable<LongTextData>) source.ToArray<LongTextData>());
          baconitDataContext.LongTextData.InsertAllOnSubmit<LongTextData>((IEnumerable<LongTextData>) longTextData);
          baconitDataContext.SubmitChanges();
        }
        catch (Exception ex)
        {
          this.DataMan.DebugDia("Error inserting long text", ex);
        }
      }
    }

    public string GetSingleLongText(string storyRName, string subRedditURL)
    {
      using (BaconitDataContext baconitDataContext = new BaconitDataContext("Data Source = 'isostore:/dataEight.sdf'; Max Database Size = 256"))
      {
        try
        {
          List<LongTextData> list = baconitDataContext.LongTextData.Where<LongTextData>((Expression<Func<LongTextData, bool>>) (data => data.RStoryName == storyRName)).OrderBy<LongTextData, int>((Expression<Func<LongTextData, int>>) (f => f.PartNum)).ToList<LongTextData>();
          string str = "";
          bool flag = false;
          if (list.Count > 0)
            flag = list[0].isPinned;
          for (int index = 0; index < list.Count; ++index)
          {
            if (list[index].isPinned == flag)
              str += list[index].text;
          }
          return str.Trim();
        }
        catch (Exception ex)
        {
          this.DataMan.DebugDia("Error getting long text", ex);
        }
        return "";
      }
    }

    public void SetSingleLongTextPinned(string storyRName)
    {
      using (BaconitDataContext baconitDataContext = new BaconitDataContext("Data Source = 'isostore:/dataEight.sdf'; Max Database Size = 256"))
      {
        try
        {
          Table<LongTextData> longTextData1 = baconitDataContext.LongTextData;
          Expression<Func<LongTextData, bool>> predicate = (Expression<Func<LongTextData, bool>>) (data => data.RStoryName == storyRName);
          foreach (LongTextData longTextData2 in longTextData1.Where<LongTextData>(predicate).ToList<LongTextData>())
            longTextData2.isPinned = true;
          baconitDataContext.SubmitChanges();
        }
        catch (Exception ex)
        {
          this.DataMan.DebugDia("Error pinning long text", ex);
        }
      }
    }

    public void UpdateMessageNewStatus(string RName, int isNew)
    {
      using (BaconitDataContext baconitDataContext = new BaconitDataContext("Data Source = 'isostore:/dataEight.sdf'; Max Database Size = 256"))
      {
        try
        {
          List<Message> list = baconitDataContext.MessageInbox.Where<Message>((Expression<Func<Message, bool>>) (data => data.RName == RName)).ToList<Message>();
          if (list.Count == 0)
            return;
          list[0].isNew = isNew;
          baconitDataContext.SubmitChanges();
        }
        catch (Exception ex)
        {
          this.DataMan.DebugDia("Update Message New Status", ex);
        }
      }
    }

    public void SetInboxMessages(List<Message> messages)
    {
      using (BaconitDataContext baconitDataContext = new BaconitDataContext("Data Source = 'isostore:/dataEight.sdf'; Max Database Size = 256"))
      {
        try
        {
          baconitDataContext.MessageInbox.DeleteAllOnSubmit<Message>((IEnumerable<Message>) baconitDataContext.MessageInbox.ToList<Message>());
          baconitDataContext.MessageInbox.InsertAllOnSubmit<Message>((IEnumerable<Message>) messages);
          baconitDataContext.SubmitChanges();
        }
        catch (Exception ex)
        {
          this.DataMan.DebugDia("setting inbox messages", ex);
        }
      }
    }

    public List<Message> GetInboxMessages()
    {
      using (BaconitDataContext baconitDataContext = new BaconitDataContext("Data Source = 'isostore:/dataEight.sdf'; Max Database Size = 256"))
      {
        try
        {
          baconitDataContext.ObjectTrackingEnabled = false;
          return baconitDataContext.MessageInbox.OrderBy<Message, int>((Expression<Func<Message, int>>) (f => f.ID)).ToList<Message>();
        }
        catch (Exception ex)
        {
          this.DataMan.DebugDia("", ex);
          return new List<Message>();
        }
      }
    }

    public void DeleteInboxMessages()
    {
      using (BaconitDataContext baconitDataContext = new BaconitDataContext("Data Source = 'isostore:/dataEight.sdf'; Max Database Size = 256"))
      {
        try
        {
          baconitDataContext.MessageInbox.DeleteAllOnSubmit<Message>((IEnumerable<Message>) baconitDataContext.MessageInbox.ToList<Message>());
          baconitDataContext.SubmitChanges();
        }
        catch (Exception ex)
        {
          this.DataMan.DebugDia("", ex);
        }
      }
    }

    public void SetPinnedStory(SubRedditData story)
    {
      story.SelfText = "";
      using (BaconitDataContext baconitDataContext = new BaconitDataContext("Data Source = 'isostore:/dataEight.sdf'; Max Database Size = 256"))
      {
        try
        {
          IQueryable<SubRedditData> source = baconitDataContext.PinnedStoryCom.Where<SubRedditData>((Expression<Func<SubRedditData, bool>>) (data => data.RedditID == story.RedditID));
          baconitDataContext.PinnedStoryCom.DeleteAllOnSubmit<SubRedditData>((IEnumerable<SubRedditData>) source.ToList<SubRedditData>());
          baconitDataContext.PinnedStoryCom.InsertOnSubmit(story);
          baconitDataContext.SubmitChanges();
        }
        catch (Exception ex)
        {
          this.DataMan.DebugDia("", ex);
        }
      }
    }

    public SubRedditData GetPinnedStory(string story)
    {
      using (BaconitDataContext baconitDataContext = new BaconitDataContext("Data Source = 'isostore:/dataEight.sdf'; Max Database Size = 256"))
      {
        try
        {
          baconitDataContext.ObjectTrackingEnabled = false;
          IQueryable<SubRedditData> source = baconitDataContext.PinnedStoryCom.Where<SubRedditData>((Expression<Func<SubRedditData, bool>>) (data => data.RedditID == story));
          return source.ToList<SubRedditData>().Count > 0 ? source.ToList<SubRedditData>()[0] : (SubRedditData) null;
        }
        catch (Exception ex)
        {
          this.DataMan.DebugDia("get pinned story", ex);
          return (SubRedditData) null;
        }
      }
    }

    public void InitSubReddits()
    {
      List<SubReddit> subReddits = new List<SubReddit>();
      subReddits.Add(new SubReddit()
      {
        DisplayName = "all",
        URL = "/r/all/",
        RName = "all",
        Title = "Videos"
      });
      subReddits.Add(new SubReddit()
      {
        DisplayName = "askreddit",
        URL = "/r/AskReddit/",
        RName = "t5_2qh1i",
        Title = "Ask Reddit...",
        Subscribers = 869916
      });
      subReddits.Add(new SubReddit()
      {
        DisplayName = "front page",
        URL = "/",
        RName = "frontpage",
        Title = "Stories from the front page of reddit",
        Subscribers = 580468
      });
      subReddits.Add(new SubReddit()
      {
        DisplayName = "funny",
        URL = "/r/funny/",
        RName = "t5_2qh33",
        Title = "funny",
        Subscribers = 1000954
      });
      subReddits.Add(new SubReddit()
      {
        DisplayName = "gaming",
        URL = "/r/gaming/",
        RName = "t5_2qh03",
        Title = "gaming.reddit: what's new in gaming",
        Subscribers = 793688
      });
      subReddits.Add(new SubReddit()
      {
        DisplayName = "iama",
        URL = "/r/iama",
        RName = "t5_2qzb6",
        Title = "I Am A, where the mundane becomes fascinating and the outrageous suddenly seems normal.",
        Subscribers = 607898
      });
      subReddits.Add(new SubReddit()
      {
        DisplayName = "pics",
        URL = "/r/pics/",
        RName = "t5_2qh0u",
        Title = "Pictures and Images",
        Subscribers = 990080
      });
      subReddits.Add(new SubReddit()
      {
        DisplayName = "politics",
        URL = "/r/politics/",
        RName = "t5_2cneq",
        Title = "Politics",
        Subscribers = 796959
      });
      subReddits.Add(new SubReddit()
      {
        DisplayName = "videos",
        URL = "/r/videos/",
        RName = "t5_2qh1e",
        Title = "Videos",
        Subscribers = 580468
      });
      subReddits.Add(new SubReddit()
      {
        DisplayName = "worldnews",
        URL = "/r/worldnews/",
        RName = "t5_2qh13",
        Title = "World News  [ no US / US Politics news please ]",
        Subscribers = 861384
      });
      if (this.DataMan.SubredditDataManager.GetSubSortedReddits().Count != 0)
        return;
      this.DataMan.SubredditDataManager.SetSubSortedReddits(subReddits);
    }

    public void UpdateCommentLikeStatus(string RName, int status)
    {
      using (BaconitDataContext baconitDataContext = new BaconitDataContext("Data Source = 'isostore:/dataEight.sdf'; Max Database Size = 256"))
      {
        try
        {
          List<CommentData> list = baconitDataContext.Comments.Where<CommentData>((Expression<Func<CommentData, bool>>) (data => data.RName == RName)).ToList<CommentData>();
          for (int index = 0; index < list.Count<CommentData>(); ++index)
            list[index].Likes = status;
          baconitDataContext.SubmitChanges();
        }
        catch (Exception ex)
        {
          this.DataMan.DebugDia("update comment like status", ex);
        }
      }
    }

    public List<CommentData> GetComments(string storyID, string subReddit, int sort)
    {
      using (BaconitDataContext baconitDataContext = new BaconitDataContext("Data Source = 'isostore:/dataEight.sdf'; Max Database Size = 256"))
      {
        try
        {
          baconitDataContext.ObjectTrackingEnabled = false;
          return baconitDataContext.Comments.Where<CommentData>((Expression<Func<CommentData, bool>>) (data => data.StoryRID == storyID && data.Sort == sort)).OrderBy<CommentData, int>((Expression<Func<CommentData, int>>) (f => f.Order)).ToList<CommentData>();
        }
        catch (Exception ex)
        {
          this.DataMan.DebugDia("get comments", ex);
          return new List<CommentData>();
        }
      }
    }

    public SubRedditData GetStory(string redditID)
    {
      using (BaconitDataContext baconitDataContext = new BaconitDataContext("Data Source = 'isostore:/dataEight.sdf'; Max Database Size = 256"))
      {
        try
        {
          baconitDataContext.ObjectTrackingEnabled = false;
          List<SubRedditData> list = baconitDataContext.SubRedditsData.Where<SubRedditData>((Expression<Func<SubRedditData, bool>>) (data => data.RedditID == redditID)).ToList<SubRedditData>();
          return list.Count > 0 ? list[0] : (SubRedditData) null;
        }
        catch (Exception ex)
        {
          this.DataMan.DebugDia("get story", ex);
          return (SubRedditData) null;
        }
      }
    }

    public bool SetComments(List<CommentData> commentData, string storyID, int sort)
    {
      using (BaconitDataContext baconitDataContext = new BaconitDataContext("Data Source = 'isostore:/dataEight.sdf'; Max Database Size = 256"))
      {
        try
        {
          IQueryable<CommentData> source = baconitDataContext.Comments.Where<CommentData>((Expression<Func<CommentData, bool>>) (data => data.StoryRID == storyID && data.Sort == sort));
          baconitDataContext.Comments.DeleteAllOnSubmit<CommentData>((IEnumerable<CommentData>) source.ToArray<CommentData>());
          baconitDataContext.Comments.InsertAllOnSubmit<CommentData>((IEnumerable<CommentData>) commentData);
          baconitDataContext.SubmitChanges();
          return true;
        }
        catch (Exception ex)
        {
          this.DataMan.DebugDia("set comments", ex);
          return false;
        }
      }
    }

    public void UpdateSubRedditData(string name, int vote)
    {
      try
      {
        using (BaconitDataContext baconitDataContext = new BaconitDataContext("Data Source = 'isostore:/dataEight.sdf'; Max Database Size = 256"))
        {
          List<SubRedditData> list = baconitDataContext.SubRedditsData.Where<SubRedditData>((Expression<Func<SubRedditData, bool>>) (data => data.RedditID == name)).ToList<SubRedditData>();
          for (int index = 0; index < list.Count<SubRedditData>(); ++index)
            list[index].Like = vote;
          baconitDataContext.SubmitChanges();
        }
      }
      catch (Exception ex)
      {
        this.DataMan.DebugDia("update sub reddit data", ex);
      }
    }

    public void ClearData()
    {
      using (BaconitDataContext baconitDataContext = new BaconitDataContext("Data Source = 'isostore:/dataEight.sdf'; Max Database Size = 256"))
      {
        try
        {
          this.TimeDict = new Dictionary<string, double>();
          baconitDataContext.DeleteDatabase();
          baconitDataContext.CreateDatabase();
          this.InitSubReddits();
          this.Save();
        }
        catch
        {
        }
      }
    }

    public void UpdateStory(SubRedditData subRedditData)
    {
      using (BaconitDataContext baconitDataContext = new BaconitDataContext("Data Source = 'isostore:/dataEight.sdf'; Max Database Size = 256"))
      {
        try
        {
          IQueryable<SubRedditData> source = baconitDataContext.SubRedditsData.Where<SubRedditData>((Expression<Func<SubRedditData, bool>>) (data => data.RedditID == subRedditData.RedditID));
          baconitDataContext.SubRedditsData.DeleteAllOnSubmit<SubRedditData>((IEnumerable<SubRedditData>) source.ToList<SubRedditData>());
          baconitDataContext.SubRedditsData.InsertOnSubmit(subRedditData);
          baconitDataContext.SubmitChanges();
        }
        catch (Exception ex)
        {
          this.DataMan.DebugDia("Error updating story", ex);
        }
      }
    }

    public void UpdateComment(CommentData commentData)
    {
      using (BaconitDataContext baconitDataContext = new BaconitDataContext("Data Source = 'isostore:/dataEight.sdf'; Max Database Size = 256"))
      {
        try
        {
          IQueryable<CommentData> source = baconitDataContext.Comments.Where<CommentData>((Expression<Func<CommentData, bool>>) (data => data.RName == commentData.RName));
          baconitDataContext.Comments.DeleteAllOnSubmit<CommentData>((IEnumerable<CommentData>) source.ToList<CommentData>());
          baconitDataContext.Comments.InsertOnSubmit(commentData);
          baconitDataContext.SubmitChanges();
        }
        catch (Exception ex)
        {
          this.DataMan.DebugDia("Error updating comment", ex);
        }
      }
    }

    public void SetStories(
      List<SubRedditData> subRedditData,
      string subReddit,
      int type,
      bool LoadingMore)
    {
      using (BaconitDataContext baconitDataContext = new BaconitDataContext("Data Source = 'isostore:/dataEight.sdf'; Max Database Size = 256"))
      {
        try
        {
          if (!LoadingMore)
          {
            IQueryable<SubRedditData> source = baconitDataContext.SubRedditsData.Where<SubRedditData>((Expression<Func<SubRedditData, bool>>) (data => data.SubRedditURL == subReddit && data.SubTypeOfSubReddit == type && data.isPinned == false));
            baconitDataContext.SubRedditsData.DeleteAllOnSubmit<SubRedditData>((IEnumerable<SubRedditData>) source.ToArray<SubRedditData>());
          }
          baconitDataContext.SubRedditsData.InsertAllOnSubmit<SubRedditData>((IEnumerable<SubRedditData>) subRedditData);
          baconitDataContext.SubmitChanges();
        }
        catch (Exception ex)
        {
          this.DataMan.DebugDia("Error inserting stories", ex);
        }
      }
    }

    public List<SubRedditData> GetStories(string subReddit, int type)
    {
      try
      {
        using (BaconitDataContext baconitDataContext = new BaconitDataContext("Data Source = 'isostore:/dataEight.sdf'; Max Database Size = 256"))
        {
          baconitDataContext.ObjectTrackingEnabled = false;
          return baconitDataContext.SubRedditsData.Where<SubRedditData>((Expression<Func<SubRedditData, bool>>) (data => data.SubRedditURL == subReddit && data.SubTypeOfSubReddit == type)).OrderBy<SubRedditData, int>((Expression<Func<SubRedditData, int>>) (f => f.SubRedditRank)).ToList<SubRedditData>();
        }
      }
      catch (Exception ex)
      {
        this.DataMan.DebugDia("get stories", ex);
        return new List<SubRedditData>();
      }
    }

    public SubRedditData GetLastStoryInSubreddit(string subReddit, int type)
    {
      try
      {
        using (BaconitDataContext baconitDataContext = new BaconitDataContext("Data Source = 'isostore:/dataEight.sdf'; Max Database Size = 256"))
        {
          List<SubRedditData> list = baconitDataContext.SubRedditsData.Where<SubRedditData>((Expression<Func<SubRedditData, bool>>) (data => data.SubRedditURL == subReddit && data.SubTypeOfSubReddit == type)).OrderBy<SubRedditData, int>((Expression<Func<SubRedditData, int>>) (f => f.SubRedditRank)).ToList<SubRedditData>();
          if (list.Count == 0)
            return (SubRedditData) null;
          List<SubRedditData> subRedditDataList = list;
          return subRedditDataList[subRedditDataList.Count - 1];
        }
      }
      catch (Exception ex)
      {
        this.DataMan.DebugDia("get last story in subreddit", ex);
        return (SubRedditData) null;
      }
    }

    public void ResetAllTimes()
    {
      this.EnsureDictionary();
      this.TimeDict.Clear();
    }

    public double LastUpdatedTime(string name)
    {
      this.EnsureDictionary();
      lock (this.FILENAME)
        return this.TimeDict.ContainsKey(name) ? this.TimeDict[name] : 0.0;
    }

    public void UpdateLastUpdatedTime(string name)
    {
      this.EnsureDictionary();
      lock (this.FILENAME)
        this.TimeDict[name] = BaconitStore.currentTime();
    }

    public void UpdateLastUpdatedTime(string name, bool failed)
    {
      this.EnsureDictionary();
      lock (this.FILENAME)
        this.TimeDict[name] = failed ? 0.0 : BaconitStore.currentTime();
    }

    public static double currentTime()
    {
      return Math.Floor((DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds);
    }

    public static DateTime DoubleToDateTime(double time)
    {
      return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddMilliseconds(time);
    }
  }
}
