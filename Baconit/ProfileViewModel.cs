// Decompiled with JetBrains decompiler
// Type: Baconit.ProfileViewModel
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Baconit.Database;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

#nullable disable
namespace Baconit
{
  public class ProfileViewModel : INotifyPropertyChanged
  {
    public static ObservableCollection<CommentData> Comments { get; private set; }

    public ProfileViewModel()
    {
      ProfileViewModel.Comments = new ObservableCollection<CommentData>();
    }

    public void setComments(List<CommentData> data)
    {
      ProfileViewModel.Comments.Clear();
      foreach (CommentData commentData in data)
        ProfileViewModel.Comments.Add(commentData);
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
