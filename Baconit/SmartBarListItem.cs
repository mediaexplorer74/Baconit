// Decompiled with JetBrains decompiler
// Type: Baconit.SmartBarListItem
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Baconit.Database;
using System.ComponentModel;
using System.Windows;

#nullable disable
namespace Baconit
{
  public class SmartBarListItem : INotifyPropertyChanged
  {
    public SubReddit subreddit;
    public string storyName;
    public string UserName;
    private string _SubSectionTitle;
    private Visibility _ShowSubSection;
    private Visibility _ShowSubReddit;
    private string _SubRedditLineOne;
    private string _SubRedditLineTwo;
    private Visibility _ShowSearchResult;
    private string _SearchResOne;
    private string _SearchResTwo;
    private string _SearchResThree;
    private int _MaxTitleHeight;

    public string SubSectionTitle
    {
      get => this._SubSectionTitle;
      set
      {
        if (!(value != this._SubSectionTitle))
          return;
        this._SubSectionTitle = value;
        this.NotifyPropertyChanged(nameof (SubSectionTitle));
      }
    }

    public Visibility ShowSubSection
    {
      get => this._ShowSubSection;
      set
      {
        if (value == this._ShowSubSection)
          return;
        this._ShowSubSection = value;
        this.NotifyPropertyChanged(nameof (ShowSubSection));
      }
    }

    public Visibility ShowSubReddit
    {
      get => this._ShowSubReddit;
      set
      {
        if (value == this._ShowSubReddit)
          return;
        this._ShowSubReddit = value;
        this.NotifyPropertyChanged(nameof (ShowSubReddit));
      }
    }

    public string SubRedditLineOne
    {
      get => this._SubRedditLineOne;
      set
      {
        if (!(value != this._SubRedditLineOne))
          return;
        this._SubRedditLineOne = value;
        this.NotifyPropertyChanged(nameof (SubRedditLineOne));
      }
    }

    public string SubRedditLineTwo
    {
      get => this._SubRedditLineTwo;
      set
      {
        if (!(value != this._SubRedditLineTwo))
          return;
        this._SubRedditLineTwo = value;
        this.NotifyPropertyChanged(nameof (SubRedditLineTwo));
      }
    }

    public Visibility ShowSearchResult
    {
      get => this._ShowSearchResult;
      set
      {
        if (value == this._ShowSearchResult)
          return;
        this._ShowSearchResult = value;
        this.NotifyPropertyChanged(nameof (ShowSearchResult));
      }
    }

    public string SearchResOne
    {
      get => this._SearchResOne;
      set
      {
        if (!(value != this._SearchResOne))
          return;
        this._SearchResOne = value;
        this.NotifyPropertyChanged(nameof (SearchResOne));
      }
    }

    public string SearchResTwo
    {
      get => this._SearchResTwo;
      set
      {
        if (!(value != this._SearchResTwo))
          return;
        this._SearchResTwo = value;
        this.NotifyPropertyChanged(nameof (SearchResTwo));
      }
    }

    public string SearchResThree
    {
      get => this._SearchResThree;
      set
      {
        if (!(value != this._SearchResThree))
          return;
        this._SearchResThree = value;
        this.NotifyPropertyChanged(nameof (SearchResThree));
      }
    }

    public int MaxTitleHeight
    {
      get => this._MaxTitleHeight;
      set
      {
        if (value == this._MaxTitleHeight)
          return;
        this._MaxTitleHeight = value;
        this.NotifyPropertyChanged(nameof (MaxTitleHeight));
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
