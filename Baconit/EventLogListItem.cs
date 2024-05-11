// Decompiled with JetBrains decompiler
// Type: Baconit.EventLogListItem
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using System.ComponentModel;

#nullable disable
namespace Baconit
{
  public class EventLogListItem : INotifyPropertyChanged
  {
    public string _EventText = "";

    public string EventText
    {
      get => this._EventText;
      set
      {
        this._EventText = value;
        this.NotifyPropertyChanged(nameof (EventText));
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
