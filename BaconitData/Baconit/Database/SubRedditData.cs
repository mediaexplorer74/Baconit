// Decompiled with JetBrains decompiler
// Type: Baconit.Database.SubRedditData
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using Microsoft.Phone.Data.Linq.Mapping;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Windows;
using System.Windows.Media.Imaging;

#nullable disable
namespace Baconit.Database
{
  [Table(Name = "RedditsData")]
  [Index(Columns = "PrimeKey", IsUnique = true)]
  public class SubRedditData : INotifyPropertyChanged
  {
    public const int NONE = 0;
    public const int UP = 1;
    public const int DOWN = -1;
    public static string NewCommentsColor;
    public static string UnReadTitleColor;
    public static string ReadTitleColor;
    private string _TitleColor;
    public bool SelfTextLoaded;
    public bool SelfTextLoading;
    private int _MaxTitleHeight = 62;
    public string _SaveStoryText = "save story";
    public Visibility _StoryVisible;
    public Visibility _GoToSubRedditVis = Visibility.Collapsed;
    public Visibility _StoryReadVis;
    public int ImagePostNumber = -1;

    [Column(IsPrimaryKey = true, IsDbGenerated = true, UpdateCheck = UpdateCheck.Never)]
    public int PrimeKey { get; set; }

    [Column]
    public int ID { get; set; }

    [Column]
    public string SubRedditURL { get; set; }

    [Column]
    public int SubTypeOfSubReddit { get; set; }

    [Column]
    public int SubRedditRank { get; set; }

    [Column]
    public string Domain { get; set; }

    [Column]
    public string SubReddit { get; set; }

    [Column]
    public string RedditID { get; set; }

    [Column]
    public string Author { get; set; }

    [Column]
    private int _score { get; set; }

    public int score
    {
      get => this._score;
      set
      {
        this._score = value;
        this.NotifyPropertyChanged(nameof (score));
      }
    }

    [Column]
    public string thumbnail { get; set; }

    [Column]
    public int downs { get; set; }

    [Column]
    public bool isSelf { get; set; }

    [Column]
    public float created { get; set; }

    [Column]
    public string URL { get; set; }

    [Column]
    private string _Title { get; set; }

    public string Title
    {
      get => this._Title;
      set
      {
        this._Title = value;
        this.NotifyPropertyChanged(nameof (Title));
      }
    }

    [Column]
    public int comments { get; set; }

    [Column]
    public int ups { get; set; }

    [Column]
    public string Permalink { get; set; }

    [Column]
    public string SelfText { get; set; }

    [Column]
    public string RedditRealID { get; set; }

    [Column]
    public int Like { get; set; }

    [Column]
    public bool isPinned { get; set; }

    [Column]
    public bool isSaved { get; set; }

    [Column]
    public bool isHidden { get; set; }

    [Column]
    public bool isNotSafeForWork { get; set; }

    [Column]
    public bool hasSelfText { get; set; }

    private string _UpVoteColor { get; set; }

    public string UpVoteColor
    {
      get => this._UpVoteColor;
      set
      {
        this._UpVoteColor = value;
        this.NotifyPropertyChanged(nameof (UpVoteColor));
      }
    }

    private string _DownVoteColor { get; set; }

    public string DownVoteColor
    {
      get => this._DownVoteColor;
      set
      {
        this._DownVoteColor = value;
        this.NotifyPropertyChanged(nameof (DownVoteColor));
      }
    }

    private BitmapSource _Thumbnail { get; set; }

    public BitmapSource Thumbnail
    {
      get => this._Thumbnail;
      set
      {
        this._Thumbnail = value;
        this.NotifyPropertyChanged(nameof (Thumbnail));
      }
    }

    private Visibility _ThumbnailVis { get; set; }

    public Visibility ThumbnailVis
    {
      get => this._ThumbnailVis;
      set
      {
        this._ThumbnailVis = value;
        this.NotifyPropertyChanged(nameof (ThumbnailVis));
      }
    }

    public string TitleColor
    {
      get
      {
        if (this._TitleColor != null)
          return this._TitleColor;
        return SubRedditData.UnReadTitleColor != null ? SubRedditData.UnReadTitleColor : "#FFFFFFFF";
      }
      set
      {
        this._TitleColor = value;
        this.NotifyPropertyChanged(nameof (TitleColor));
      }
    }

    private string _NewCommentsText { get; set; }

    public string NewCommentsText
    {
      get => this._NewCommentsText;
      set
      {
        this._NewCommentsText = value;
        this.NotifyPropertyChanged(nameof (NewCommentsText));
      }
    }

    private string _NewCommentsTextColor { get; set; }

    public string NewCommentsTextColor
    {
      get => this._NewCommentsTextColor;
      set
      {
        this._NewCommentsTextColor = value;
        this.NotifyPropertyChanged(nameof (NewCommentsTextColor));
      }
    }

    private string _LineOne { get; set; }

    public string LineOne
    {
      get => this._LineOne;
      set
      {
        this._LineOne = value;
        this.NotifyPropertyChanged(nameof (LineOne));
      }
    }

    private string _LineTwo { get; set; }

    public string LineTwo
    {
      get => this._LineTwo;
      set
      {
        this._LineTwo = value;
        this.NotifyPropertyChanged(nameof (LineTwo));
      }
    }

    public int MaxTitleHeight
    {
      get => this._MaxTitleHeight;
      set
      {
        this._MaxTitleHeight = value;
        this.NotifyPropertyChanged(nameof (MaxTitleHeight));
      }
    }

    public string SaveStoryText
    {
      get => this._SaveStoryText;
      set
      {
        this._SaveStoryText = value;
        this.NotifyPropertyChanged(nameof (SaveStoryText));
      }
    }

    public Visibility StoryVisible
    {
      get => this._StoryVisible;
      set
      {
        this._StoryVisible = value;
        this.NotifyPropertyChanged(nameof (StoryVisible));
      }
    }

    public Visibility GoToSubRedditVis
    {
      get => this._GoToSubRedditVis;
      set
      {
        this._GoToSubRedditVis = value;
        this.NotifyPropertyChanged(nameof (GoToSubRedditVis));
      }
    }

    public Visibility StoryReadVis
    {
      get => this._StoryReadVis;
      set
      {
        this._StoryReadVis = value;
        this.NotifyPropertyChanged(nameof (StoryReadVis));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
