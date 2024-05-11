// Decompiled with JetBrains decompiler
// Type: Baconit.Database.UserAccountInformation
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using System.ComponentModel;

#nullable disable
namespace Baconit.Database
{
  public class UserAccountInformation : INotifyPropertyChanged
  {
    public string Name { get; set; }

    public double Created { get; set; }

    public int LinkKarma { get; set; }

    public int CommentKarma { get; set; }

    public bool IsGold { get; set; }

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
