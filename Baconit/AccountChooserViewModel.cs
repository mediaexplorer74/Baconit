// Decompiled with JetBrains decompiler
// Type: Baconit.AccountChooserViewModel
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using BaconitData.Database;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

#nullable disable
namespace Baconit
{
  public class AccountChooserViewModel : INotifyPropertyChanged
  {
    public static ObservableCollection<AccountChooserViewModel.RedditAccountUI> Accounts { get; private set; }

    public AccountChooserViewModel()
    {
      AccountChooserViewModel.Accounts = new ObservableCollection<AccountChooserViewModel.RedditAccountUI>();
    }

    public void setAccounts(List<RedditAccount> accounts)
    {
      AccountChooserViewModel.Accounts.Clear();
      foreach (RedditAccount account in accounts)
      {
        AccountChooserViewModel.RedditAccountUI redditAccountUi = new AccountChooserViewModel.RedditAccountUI(account.UserName, account.DateString, account.ModHas, account.Cookie);
        if (account.UserName.ToLower().Equals(App.DataManager.SettingsMan.UserName.ToLower()))
          redditAccountUi.TitleColor = DataManager.ACCENT_COLOR;
        AccountChooserViewModel.Accounts.Add(redditAccountUi);
      }
    }

    public RedditAccount getAccount(AccountChooserViewModel.RedditAccountUI acc)
    {
      foreach (RedditAccount userAccount in App.DataManager.SettingsMan.UserAccounts)
      {
        if (acc.UserName.ToLower().Equals(userAccount.UserName.ToLower()))
          return userAccount;
      }
      return (RedditAccount) null;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void NotifyPropertyChanged(string propertyName)
    {
      PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
      if (propertyChanged == null)
        return;
      propertyChanged((object) this, new PropertyChangedEventArgs(propertyName));
    }

    public class RedditAccountUI : INotifyPropertyChanged
    {
      public string ModHas;
      public string Cookie;
      private string _TitleColor;

      private string _UserName { get; set; }

      public string UserName
      {
        get => this._UserName;
        set
        {
          this._UserName = value;
          this.NotifyPropertyChanged(nameof (UserName));
        }
      }

      private string _DateString { get; set; }

      public string DateString
      {
        get => this._DateString;
        set
        {
          this._DateString = value;
          this.NotifyPropertyChanged(nameof (DateString));
        }
      }

      public string TitleColor
      {
        get
        {
          if (this._TitleColor != null)
            return this._TitleColor;
          return DataManager.LIGHT_THEME ? "#FF000000" : "#FFFFFFFF";
        }
        set
        {
          this._TitleColor = value;
          this.NotifyPropertyChanged(nameof (TitleColor));
        }
      }

      public RedditAccountUI(string user, string dateString, string mod, string cookie)
      {
        this.UserName = user;
        this.DateString = dateString;
        this.ModHas = mod;
        this.Cookie = cookie;
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
}
