// Decompiled with JetBrains decompiler
// Type: Baconit.ItemViewModel
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Baconit
{
  public class ItemViewModel : INotifyPropertyChanged
  {
    private string _lineOne;
    private string _lineTwo;
    private string _favIcon;
    public string URL = "";
    public string RName = "";
    private SolidColorBrush _LineTwoColor;
    private Thickness _Padding;
    private Visibility _Header;
    private Visibility _Normal;

    public string LineOne
    {
      get => this._lineOne;
      set
      {
        if (!(value != this._lineOne))
          return;
        this._lineOne = value;
        this.NotifyPropertyChanged(nameof (LineOne));
      }
    }

    public string LineTwo
    {
      get => this._lineTwo;
      set
      {
        if (!(value != this._lineTwo))
          return;
        this._lineTwo = value;
        this.NotifyPropertyChanged(nameof (LineTwo));
      }
    }

    public string favIcon
    {
      get => this._favIcon;
      set
      {
        if (!(value != this._favIcon))
          return;
        this._favIcon = value;
        this.NotifyPropertyChanged(nameof (favIcon));
      }
    }

    public SolidColorBrush LineTwoColor
    {
      get
      {
        if (this._LineTwoColor == null)
          this._LineTwoColor = (SolidColorBrush) Application.Current.Resources[(object) "PhoneSubtleBrush"];
        return this._LineTwoColor;
      }
      set
      {
        if (value == this._LineTwoColor)
          return;
        this._LineTwoColor = value;
        this.NotifyPropertyChanged(nameof (LineTwoColor));
      }
    }

    public Thickness Padding
    {
      get => this._Padding;
      set
      {
        if (!(value != this._Padding))
          return;
        this._Padding = value;
        this.NotifyPropertyChanged(nameof (Padding));
      }
    }

    public Visibility Header
    {
      get => this._Header;
      set
      {
        if (value == this._Header)
          return;
        this._Header = value;
        this.NotifyPropertyChanged(nameof (Header));
      }
    }

    public Visibility Normal
    {
      get => this._Normal;
      set
      {
        if (value == this._Normal)
          return;
        this._Normal = value;
        this.NotifyPropertyChanged(nameof (Normal));
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
