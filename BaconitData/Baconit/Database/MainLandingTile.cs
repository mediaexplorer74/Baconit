// Decompiled with JetBrains decompiler
// Type: Baconit.Database.MainLandingTile
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using System.ComponentModel;
using System.Runtime.Serialization;

#nullable disable
namespace Baconit.Database
{
  [DataContract(Name = "MainLandingTile")]
  public class MainLandingTile : INotifyPropertyChanged
  {
    public const int SUBREDDIT = 0;
    public const int PROFILE = 1;
    public const int STORY_COMMENTS = 2;
    public const int STORY_WEBVIEW = 3;

    public MainLandingTile(int tt, string key, string display)
    {
      this.TileKind = tt;
      this.KeyElement = key;
      this.DisplayText = display;
    }

    [DataMember]
    public string _KeyElement { get; set; }

    public string KeyElement
    {
      get => this._KeyElement;
      set
      {
        this._KeyElement = value;
        this.NotifyPropertyChanged(nameof (KeyElement));
      }
    }

    [DataMember]
    public int _TileKind { get; set; }

    public int TileKind
    {
      get => this._TileKind;
      set
      {
        this._TileKind = value;
        this.NotifyPropertyChanged(nameof (TileKind));
      }
    }

    [DataMember]
    public string _DisplayText { get; set; }

    public string DisplayText
    {
      get => this._DisplayText;
      set
      {
        this._DisplayText = value;
        this.NotifyPropertyChanged(nameof (DisplayText));
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
