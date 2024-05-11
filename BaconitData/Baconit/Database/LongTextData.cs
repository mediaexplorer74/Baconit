// Decompiled with JetBrains decompiler
// Type: Baconit.Database.LongTextData
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using Microsoft.Phone.Data.Linq.Mapping;
using System.ComponentModel;
using System.Data.Linq.Mapping;

#nullable disable
namespace Baconit.Database
{
  [Table(Name = "LongTextData")]
  [Index(Columns = "PrimeKey", IsUnique = true)]
  public class LongTextData : INotifyPropertyChanged
  {
    [Column(IsPrimaryKey = true, IsDbGenerated = true, UpdateCheck = UpdateCheck.Never)]
    public int PrimeKey { get; set; }

    [Column]
    public string RStoryName { get; set; }

    [Column]
    public string SubRedditUrl { get; set; }

    [Column]
    public int SubType { get; set; }

    [Column]
    public int PartNum { get; set; }

    [Column]
    public string text { get; set; }

    [Column]
    public bool isPinned { get; set; }

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
