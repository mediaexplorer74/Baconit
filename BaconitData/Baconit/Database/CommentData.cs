// Decompiled with JetBrains decompiler
// Type: Baconit.Database.CommentData
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using Microsoft.Phone.Data.Linq.Mapping;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Baconit.Database
{
  [Table(Name = "Comments")]
  [Index(Columns = "PrimeKey", IsUnique = true)]
  public class CommentData : INotifyPropertyChanged
  {
    private string _BackGroundColor = "";
    private Visibility _SendingProgressBar = Visibility.Collapsed;
    private Visibility _UpVoteIcon = Visibility.Collapsed;
    private Visibility _DownVoteIcon = Visibility.Collapsed;
    private bool _SendingProgressBarIsInd;
    private string _AuthorBackColor = "";
    public bool Openable = true;
    private bool _isCollapsed;
    public bool _isCommentVisible = true;
    public Visibility _isVisible;
    public int _commentCloseTime;
    public int _titleHeight = 31;

    [Column(IsPrimaryKey = true, IsDbGenerated = true, UpdateCheck = UpdateCheck.Never)]
    public int PrimeKey { get; set; }

    [Column]
    public int Order { get; set; }

    [Column]
    public string RID { get; set; }

    [Column]
    public string RName { get; set; }

    [Column]
    public string Body { get; set; }

    [Column]
    public string Body2 { get; set; }

    public string Body3 { get; set; }

    [Column]
    public string SubRedditRName { get; set; }

    [Column]
    public string SubRedditURL { get; set; }

    [Column]
    public string StoryRID { get; set; }

    [Column]
    public double Created { get; set; }

    [Column]
    public int Downs { get; set; }

    [Column]
    public int Up { get; set; }

    [Column]
    public string Author { get; set; }

    [Column]
    public string LinkRID { get; set; }

    [Column]
    public string Permalink { get; set; }

    [Column]
    public int IndentLevel { get; set; }

    [Column]
    public int Likes { get; set; }

    [Column]
    public int Sort { get; set; }

    public string LineOne { get; set; }

    private string _LineOnePartTwo { get; set; }

    public string LineOnePartTwo
    {
      get => this._LineOnePartTwo;
      set
      {
        this._LineOnePartTwo = value;
        this.NotifyPropertyChanged(nameof (LineOnePartTwo));
      }
    }

    public Thickness Padding { get; set; }

    public int PanelWidth { get; set; }

    public SolidColorBrush Color { get; set; }

    private string _Content { get; set; }

    public string Content
    {
      get => this._Content;
      set
      {
        this._Content = value;
        this.NotifyPropertyChanged(nameof (Content));
      }
    }

    private string _MoreCommentsText { get; set; }

    public string MoreCommentsText
    {
      get => this._MoreCommentsText;
      set
      {
        this._MoreCommentsText = value;
        this.NotifyPropertyChanged(nameof (MoreCommentsText));
      }
    }

    private bool _UpVoteStatus { get; set; }

    public bool UpVoteStatus
    {
      get => this._UpVoteStatus;
      set
      {
        this._UpVoteStatus = value;
        this.NotifyPropertyChanged(nameof (UpVoteStatus));
      }
    }

    private bool _DownVoteStatus { get; set; }

    public bool DownVoteStatus
    {
      get => this._DownVoteStatus;
      set
      {
        this._DownVoteStatus = value;
        this.NotifyPropertyChanged(nameof (DownVoteStatus));
      }
    }

    public string BackGroundColor
    {
      get
      {
        if (!this._BackGroundColor.Equals(""))
          return this._BackGroundColor;
        return DataManager.LIGHT_THEME ? "#FFFFFFFF" : "#FF000000";
      }
      set
      {
        this._BackGroundColor = value;
        this.NotifyPropertyChanged(nameof (BackGroundColor));
      }
    }

    public Visibility SendingProgressBar
    {
      get => this._SendingProgressBar;
      set
      {
        this._SendingProgressBar = value;
        this.NotifyPropertyChanged(nameof (SendingProgressBar));
      }
    }

    public Visibility UpVoteIcon
    {
      get => this._UpVoteIcon;
      set
      {
        this._UpVoteIcon = value;
        this.NotifyPropertyChanged(nameof (UpVoteIcon));
      }
    }

    public Visibility DownVoteIcon
    {
      get => this._DownVoteIcon;
      set
      {
        this._DownVoteIcon = value;
        this.NotifyPropertyChanged(nameof (DownVoteIcon));
      }
    }

    public bool SendingProgressBarIsInd
    {
      get => this._SendingProgressBarIsInd;
      set
      {
        this._SendingProgressBarIsInd = value;
        this.NotifyPropertyChanged(nameof (SendingProgressBarIsInd));
      }
    }

    public string AuthorBackColor
    {
      get
      {
        if (!this._AuthorBackColor.Equals(""))
          return this._AuthorBackColor;
        return DataManager.LIGHT_THEME ? "#FFFFFFFF" : "#FF000000";
      }
      set
      {
        this._AuthorBackColor = value;
        this.NotifyPropertyChanged(nameof (AuthorBackColor));
      }
    }

    public bool isCollapsed
    {
      get => this._isCollapsed;
      set
      {
        this._isCollapsed = value;
        this.NotifyPropertyChanged(nameof (isCollapsed));
      }
    }

    public bool IsCommentVisible
    {
      get => this._isCommentVisible;
      set
      {
        this._isCommentVisible = value;
        this.NotifyPropertyChanged(nameof (IsCommentVisible));
      }
    }

    public Visibility isVisible
    {
      get => this._isVisible;
      set
      {
        this._isVisible = value;
        this.NotifyPropertyChanged(nameof (isVisible));
      }
    }

    public int CommentCloseTime
    {
      get => this._commentCloseTime;
      set
      {
        this._commentCloseTime = value;
        this.NotifyPropertyChanged(nameof (CommentCloseTime));
      }
    }

    public int TitleHeight
    {
      get => this._titleHeight;
      set
      {
        this._titleHeight = value;
        this.NotifyPropertyChanged(nameof (TitleHeight));
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
