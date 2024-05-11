// Decompiled with JetBrains decompiler
// Type: Baconit.StoryViewModel
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using System.ComponentModel;

#nullable disable
namespace Baconit
{
  public class StoryViewModel : INotifyPropertyChanged
  {
    private string _Title;
    private string _user;
    private string _url;
    private string _upImage;
    private string _downImage;

    public string Title
    {
      get => this._Title;
      set
      {
        if (!(value != this._Title))
          return;
        this._Title = value;
        this.NotifyPropertyChanged(nameof (Title));
      }
    }

    public string User
    {
      get => this._user;
      set
      {
        if (!(value != this._user))
          return;
        this._user = value;
        this.NotifyPropertyChanged(nameof (User));
      }
    }

    public string URL
    {
      get => this._url;
      set
      {
        if (!(value != this._url))
          return;
        this._url = value;
        this.NotifyPropertyChanged(nameof (URL));
      }
    }

    public string UpImage
    {
      get => this._upImage;
      set
      {
        if (!(value != this._upImage))
          return;
        this._upImage = value;
        this.NotifyPropertyChanged(nameof (UpImage));
      }
    }

    public string DownImage
    {
      get => this._downImage;
      set
      {
        if (!(value != this._downImage))
          return;
        this._downImage = value;
        this.NotifyPropertyChanged(nameof (DownImage));
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
