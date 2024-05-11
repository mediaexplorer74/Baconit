// Decompiled with JetBrains decompiler
// Type: Baconit.Database.Message
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using Microsoft.Phone.Data.Linq.Mapping;
using System.ComponentModel;
using System.Data.Linq.Mapping;

#nullable disable
namespace Baconit.Database
{
  [Table(Name = "MessageInbox")]
  [Index(Columns = "PrimeKey", IsUnique = true)]
  public class Message : INotifyPropertyChanged
  {
    public const int WAS_COMMENT = 1;
    public const int WASNT_COMMENT = 0;
    public const int NEW = 1;
    public const int NOT_NEW = 0;

    [Column]
    public int ID { get; set; }

    [Column(IsPrimaryKey = true)]
    public string PrimeKey { get; set; }

    [Column]
    public string body { get; set; }

    [Column]
    public string bodyOver { get; set; }

    [Column]
    public int wasComment { get; set; }

    [Column]
    public int isNew { get; set; }

    [Column]
    public string RName { get; set; }

    [Column]
    public float Created { get; set; }

    [Column]
    public string Author { get; set; }

    [Column]
    public string Subreddit { get; set; }

    [Column]
    public string Context { get; set; }

    [Column]
    public string Subject { get; set; }

    public string FirstLineAccent { get; set; }

    public string FirstLineNormal { get; set; }

    public string Content { get; set; }

    private string _BarColor { get; set; }

    public string BarColor
    {
      get => this._BarColor;
      set
      {
        this._BarColor = value;
        this.NotifyPropertyChanged(nameof (BarColor));
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
