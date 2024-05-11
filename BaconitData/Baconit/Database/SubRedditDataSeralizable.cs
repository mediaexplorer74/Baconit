// Decompiled with JetBrains decompiler
// Type: Baconit.Database.SubRedditDataSeralizable
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using System.Windows;

#nullable disable
namespace Baconit.Database
{
  public class SubRedditDataSeralizable
  {
    public int score;
    public string Title;
    public string UpVoteColor;
    public string DownVoteColor;
    public int TitleWidth;
    public string LineOne;
    public string LineTwo;
    public bool SelfTextLoaded;
    public bool SelfTextLoading;
    public int MaxTitleHeight = 62;
    public string SaveStoryText;

    public SubRedditDataSeralizable()
    {
    }

    public SubRedditDataSeralizable(SubRedditData data)
    {
      this.PrimeKey = data.PrimeKey;
      this.ID = data.ID;
      this.SubRedditURL = data.SubRedditURL;
      this.SubTypeOfSubReddit = data.SubTypeOfSubReddit;
      this.SubRedditRank = data.SubRedditRank;
      this.Domain = data.Domain;
      this.SubReddit = data.SubReddit;
      this.RedditID = data.RedditID;
      this.Author = data.Author;
      this.score = data.score;
      this.thumbnail = data.thumbnail;
      this.downs = data.downs;
      this.isSelf = data.isSelf;
      this.created = data.created;
      this.URL = data.URL;
      this.Title = data.Title;
      this.comments = data.comments;
      this.ups = data.ups;
      this.Permalink = data.Permalink;
      this.SelfText = data.SelfText;
      this.RedditRealID = data.RedditRealID;
      this.Like = data.Like;
      this.isPinned = data.isPinned;
      this.isSaved = data.isSaved;
      this.isHidden = data.isHidden;
      this.isNotSafeForWork = data.isNotSafeForWork;
      this.hasSelfText = data.hasSelfText;
      this.UpVoteColor = data.UpVoteColor;
      this.DownVoteColor = data.DownVoteColor;
      this.LineOne = data.LineOne;
      this.LineTwo = data.LineTwo;
      this.SelfTextLoaded = data.SelfTextLoaded;
      this.SelfTextLoading = data.SelfTextLoading;
      this.MaxTitleHeight = data.MaxTitleHeight;
      this.SaveStoryText = data.SaveStoryText;
    }

    public SubRedditData Restore()
    {
      return new SubRedditData()
      {
        PrimeKey = this.PrimeKey,
        ID = this.ID,
        SubRedditURL = this.SubRedditURL,
        SubTypeOfSubReddit = this.SubTypeOfSubReddit,
        SubRedditRank = this.SubRedditRank,
        Domain = this.Domain,
        SubReddit = this.SubReddit,
        RedditID = this.RedditID,
        Author = this.Author,
        score = this.score,
        thumbnail = this.thumbnail,
        downs = this.downs,
        isSelf = this.isSelf,
        created = this.created,
        URL = this.URL,
        Title = this.Title,
        comments = this.comments,
        ups = this.ups,
        Permalink = this.Permalink,
        SelfText = this.SelfText,
        RedditRealID = this.RedditRealID,
        Like = this.Like,
        isPinned = this.isPinned,
        isSaved = this.isSaved,
        isHidden = this.isHidden,
        isNotSafeForWork = this.isNotSafeForWork,
        hasSelfText = this.hasSelfText,
        UpVoteColor = this.UpVoteColor,
        DownVoteColor = this.DownVoteColor,
        LineOne = this.LineOne,
        LineTwo = this.LineTwo,
        SelfTextLoaded = this.SelfTextLoaded,
        SelfTextLoading = this.SelfTextLoading,
        MaxTitleHeight = this.MaxTitleHeight,
        SaveStoryText = this.SaveStoryText,
        StoryVisible = Visibility.Visible,
        ThumbnailVis = Visibility.Collapsed
      };
    }

    public int PrimeKey { get; set; }

    public int ID { get; set; }

    public string SubRedditURL { get; set; }

    public int SubTypeOfSubReddit { get; set; }

    public int SubRedditRank { get; set; }

    public string Domain { get; set; }

    public string SubReddit { get; set; }

    public string RedditID { get; set; }

    public string Author { get; set; }

    public string thumbnail { get; set; }

    public int downs { get; set; }

    public bool isSelf { get; set; }

    public float created { get; set; }

    public string URL { get; set; }

    public int comments { get; set; }

    public int ups { get; set; }

    public string Permalink { get; set; }

    public string SelfText { get; set; }

    public string RedditRealID { get; set; }

    public int Like { get; set; }

    public bool isPinned { get; set; }

    public bool isSaved { get; set; }

    public bool isHidden { get; set; }

    public bool isNotSafeForWork { get; set; }

    public bool hasSelfText { get; set; }
  }
}
