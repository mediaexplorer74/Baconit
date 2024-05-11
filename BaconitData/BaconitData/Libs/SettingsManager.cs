// Decompiled with JetBrains decompiler
// Type: BaconitData.Libs.SettingsManager
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using Baconit;
using Baconit.Database;
using Baconit.Libs;
using BaconitData.Database;
using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;

#nullable disable
namespace BaconitData.Libs
{
  public class SettingsManager
  {
    private DataManager DataMan;
    public IsolatedStorageSettings settings;
    private int _AdultFilter = -1;
    private double _UserAccountCreated = -1.0;
    private List<RedditAccount> _Accounts;
    private string _UserName;
    private string _ModHash;
    private string _UserCookie;
    private int _LinkKarma = -1337;
    private int _CommentKarma = -1337;
    private double _accountUpdateTime = -1.0;
    private List<MainLandingTile> _MainLandingTiles;
    private int _NumberOfRecentTiles = -1;
    private int _LogLength = -1;
    private int _EnableLogging = -1;
    private int _ShowAprilFools = -1;
    private int _ShowDiscoveryMessage = -1;
    private int _ShowSmartBarHelp = -1;
    private int _OpenedAppCounter = -1;
    private int _OpenedAppRatingCounter = -1;
    private int _Debugging = -1;
    private int _ScaleFactor = -1;
    private int _UseInAppBroser = -1;
    private int _ResetOrenationOnStart = -1;
    private int _ScreenOrenLocked = -1;
    private int _ScreenOrenLockedPos = -1;
    private int _OnlyUpdateOnWifi = -1;
    private int _freshStartFromLiveTile = -1;
    private int _showSystemBar = -1;
    private int _UnreadInboxMessageCount = -1;
    private string _MostRecentInboxMessageTextAuthor;
    private string _MostRecentInboxMessageTextBody;
    private int _HasMail = -1;
    private int _ShowReadStories = -1;
    private int _OpenSelfTextIntoFlipView = -1;
    private int _ShrinkStoryHeaderOnScroll = -1;
    private HashList<string, int> _ReadStories;
    private int _ShowWholeTitle = -1;
    private double _storyUpdateTime = -1.0;
    private int _OpenedCommentsCounter = -1;
    private int _DefaultCommentSort = -1;
    private double _commentUpdateTime = -1.0;
    private int _showExpandedSelfText = -1;
    private Dictionary<string, SettingsManager.FlipPlaceHolder> _FlipPlaceHolders;
    private int _ResumeFlipMode = -1;
    private int _FlipViewShowBrowserHelp = -1;
    private int _FlipViewShowBrowserHelpTwo = -1;
    private int _NSFWClickThrough = -1;
    private int _FlipViewCacheNextWebPage = -1;
    private int _ShowImagesOnlyInFlipView = -1;
    private HashList<string, bool> _OptimizeWebsites;
    private int _OptimizeWebByDefault = -1;
    private int _SwipeToVote = -1;
    private int _showStoryOverLay = -1;
    private int _showStorySelfOverLay = -1;
    private int _ShowStoryListOverLay = -1;
    private int _ShowImageViewerHelpOverlay = -1;
    private HashList<string, int> _MostViewSubreddits;
    private int _SortSubRedditsBy = -1;
    private int _MainLandingLastImageUsed = -1;
    private int _MainLandingMaxImage = -1;
    private string _LandingWallpaper;
    private string _LandingStoryProvider;
    private int _MainTileBack = -1;
    private int _SubRedditBackground = -1;
    private int _StoryTileBackground = -1;
    private int _MainLiveTileShowUnread = -1;
    private int _MainLiveTileShowKarma = -1;
    private int _StoryLiveTileShowNewMessage = -1;
    private int _ForceUpdateTiles = -1;
    private int _OnlyUpdateWifiLockScreenImages = -1;
    private int _OnlyUpdateWifiTilesImages = -1;
    private int _ShowTostNotifications = -1;
    private int _ShowDisabledBackgroundWarning = -1;
    private int _BackgroundAgentEnabled = -1;
    private int _DoNotDistEnabled = -1;
    private double _TileImageFullUpdate = -1.0;
    private double _TileImageHalfUpdate = -1.0;
    private double _LastStartUpRun = -1.0;
    private int _UseLockScreenImage = -1;
    private string _LockScreenSubredditURL;
    private int _LockScreenUpdateTime = -1;
    private List<string> _LockScreenImageFiles;
    private int _LockScreenLastUsedImage = -1;
    private int _BaconSyncShowError = -1;
    private int _UseBaconSync = -1;
    private string _BaconSyncDeviceName;
    private string _BaconSyncPassCode;
    private string _BaconSyncPushURL;
    private string _BaconSyncAccountName;
    private string _BaconSyncStatus;
    private List<string> _BaconSyncReadList;
    private int _ShowBaconSyncSiginMessage = -1;
    private string _SubmitLinkText;
    private string _SubmitLinkURL;
    private string _SubmitLinkSubreddit;
    private int _SubmitLinkIsSelfText = -1;
    private Dictionary<string, object> _DeactivatedPageObjects;
    private SubReddit _DeactivatedActiveSubReddit;
    private SubRedditDataSeralizable _DeactivatedActiveStoryData;

    public SettingsManager(DataManager data)
    {
      this.DataMan = data;
      this.settings = IsolatedStorageSettings.ApplicationSettings;
    }

    public void Save() => this.settings.Save();

    public bool AdultFilter
    {
      get
      {
        if (this._AdultFilter == -1)
          this._AdultFilter = (int) this.DataMan.VIPDataMan.GetValue("_AdultFilter", (object) 1);
        return this._AdultFilter != 0;
      }
      set
      {
        if (value)
        {
          this.DataMan.VIPDataMan.SetValue("_AdultFilter", (object) 1);
          this._AdultFilter = 1;
        }
        else
        {
          this.DataMan.VIPDataMan.SetValue("_AdultFilter", (object) 0);
          this._AdultFilter = 0;
        }
      }
    }

    public bool AdultFilterSet() => this.DataMan.VIPDataMan.Contains("_AdultFilter");

    public double UserAccountCreated
    {
      get
      {
        if (this._UserAccountCreated == -1.0)
          this._UserAccountCreated = (double) this.DataMan.VIPDataMan.GetValue("_UserAccountCreated", (object) -1.0);
        return this._UserAccountCreated;
      }
      set => this.DataMan.VIPDataMan.SetValue("_UserAccountCreated", (object) value);
    }

    public List<RedditAccount> UserAccounts
    {
      get
      {
        if (this._Accounts == null)
        {
          lock (this.settings)
            this._Accounts = !this.settings.Contains("_Accounts") ? new List<RedditAccount>() : (List<RedditAccount>) this.settings["_Accounts"];
        }
        return this._Accounts;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_Accounts"] = (object) value;
          this._Accounts = value;
        }
      }
    }

    public void SaveAccounts()
    {
      lock (this.settings)
        this.settings["_Accounts"] = (object) this._Accounts;
    }

    public string UserName
    {
      get
      {
        if (this._UserName == null)
          this._UserName = (string) this.DataMan.VIPDataMan.GetValue("userName", (object) "");
        return this._UserName;
      }
      set
      {
        this.DataMan.VIPDataMan.SetValue("userName", (object) value);
        this._UserName = value;
      }
    }

    public string ModHash
    {
      get
      {
        if (this._ModHash == null)
          this._ModHash = (string) this.DataMan.VIPDataMan.GetValue("_ModHash", (object) "");
        return this._ModHash;
      }
      set
      {
        lock (this.settings)
        {
          this.DataMan.VIPDataMan.SetValue("_ModHash", (object) value);
          this._ModHash = value;
        }
      }
    }

    public bool IsSignedIn
    {
      get
      {
        return !string.IsNullOrWhiteSpace(this.ModHash) && !string.IsNullOrWhiteSpace(this.UserCookie) && !string.IsNullOrWhiteSpace(this.UserName);
      }
    }

    public string UserCookie
    {
      get
      {
        if (this._UserCookie == null)
          this._UserCookie = (string) this.DataMan.VIPDataMan.GetValue("_SettingsMan.UserCookie", (object) "");
        return this._UserCookie;
      }
      set
      {
        lock (this.settings)
        {
          this.DataMan.VIPDataMan.SetValue("_SettingsMan.UserCookie", (object) value);
          this._UserCookie = value;
        }
      }
    }

    public int LinkKarma
    {
      get
      {
        if (this._LinkKarma == -1337)
        {
          lock (this.settings)
            this._LinkKarma = !this.settings.Contains(nameof (LinkKarma)) ? 0 : (int) this.settings[nameof (LinkKarma)];
        }
        return this._LinkKarma;
      }
      set
      {
        lock (this.settings)
        {
          this.settings[nameof (LinkKarma)] = (object) value;
          this._LinkKarma = value;
        }
      }
    }

    public int CommentKarma
    {
      get
      {
        if (this._CommentKarma == -1337)
        {
          lock (this.settings)
            this._CommentKarma = !this.settings.Contains(nameof (CommentKarma)) ? 0 : (int) this.settings[nameof (CommentKarma)];
        }
        return this._CommentKarma;
      }
      set
      {
        lock (this.settings)
        {
          this.settings[nameof (CommentKarma)] = (object) value;
          this._CommentKarma = value;
        }
      }
    }

    public double AccountUpdateTime
    {
      get
      {
        if (this._accountUpdateTime == -1.0)
        {
          lock (this.settings)
            this._accountUpdateTime = !this.settings.Contains(nameof (AccountUpdateTime)) ? 1800000.0 : (double) this.settings[nameof (AccountUpdateTime)];
        }
        return this._accountUpdateTime;
      }
      set
      {
        lock (this.settings)
        {
          this.settings[nameof (AccountUpdateTime)] = (object) value;
          this._accountUpdateTime = value;
        }
      }
    }

    public List<MainLandingTile> MainLandingTiles
    {
      get
      {
        if (this._MainLandingTiles == null)
        {
          lock (this.settings)
            this._MainLandingTiles = !this.settings.Contains(nameof (MainLandingTiles)) ? new List<MainLandingTile>() : (List<MainLandingTile>) this.settings[nameof (MainLandingTiles)];
        }
        return this._MainLandingTiles;
      }
      set
      {
        lock (this.settings)
        {
          this.settings[nameof (MainLandingTiles)] = (object) value;
          this._MainLandingTiles = value;
        }
      }
    }

    public void AddMainLandingTile(MainLandingTile tile)
    {
      bool flag = false;
      foreach (MainLandingTile mainLandingTile in this.MainLandingTiles)
      {
        if (mainLandingTile.KeyElement.Equals(tile.KeyElement))
        {
          this.MainLandingTiles.Remove(mainLandingTile);
          this.MainLandingTiles.Insert(0, tile);
          flag = true;
          break;
        }
      }
      if (flag)
        return;
      this.MainLandingTiles.Insert(0, tile);
      this.CheckMainLandingTileCount();
    }

    public void CheckMainLandingTileCount()
    {
      while (this.MainLandingTiles.Count > this.NumberOfRecentTiles)
        this.MainLandingTiles.RemoveAt(this.MainLandingTiles.Count - 1);
    }

    public void SaveMainLandingTiles()
    {
      lock (this.settings)
        this.settings["MainLandingTiles"] = (object) this.MainLandingTiles;
    }

    public int NumberOfRecentTiles
    {
      get
      {
        if (this._NumberOfRecentTiles == -1)
        {
          lock (this.settings)
            this._NumberOfRecentTiles = !this.settings.Contains(nameof (NumberOfRecentTiles)) ? 8 : (int) this.settings[nameof (NumberOfRecentTiles)];
        }
        return this._NumberOfRecentTiles;
      }
      set
      {
        lock (this.settings)
        {
          this.settings[nameof (NumberOfRecentTiles)] = (object) value;
          this._NumberOfRecentTiles = value;
        }
      }
    }

    public bool WasAppRest
    {
      get
      {
        lock (this.settings)
          return !this.settings.Contains(nameof (WasAppRest));
      }
      set
      {
        lock (this.settings)
          this.settings[nameof (WasAppRest)] = (object) false;
      }
    }

    public int LogLength
    {
      get
      {
        if (this._LogLength == -1)
        {
          lock (this.settings)
            this._LogLength = !this.settings.Contains("_LogLength") ? 500 : (int) this.settings["_LogLength"];
        }
        return this._LogLength;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_LogLength"] = (object) value;
          this._LogLength = value;
        }
      }
    }

    public bool EnableLogging
    {
      get
      {
        if (this._EnableLogging == -1)
        {
          lock (this.settings)
            this._EnableLogging = !this.settings.Contains("_EnableLogging") ? 0 : (int) this.settings["_EnableLogging"];
        }
        return this._EnableLogging != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_EnableLogging"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._EnableLogging = 1;
          else
            this._EnableLogging = 0;
        }
      }
    }

    public bool ShowAprilFools
    {
      get
      {
        if (this._ShowAprilFools == -1)
        {
          lock (this.settings)
            this._ShowAprilFools = !this.settings.Contains("_ShowAprilFools") ? 1 : (!(bool) this.settings["_ShowAprilFools"] ? 0 : 1);
        }
        return this._ShowAprilFools == 1;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_ShowAprilFools"] = (object) value;
          if (value)
            this._ShowAprilFools = 1;
          else
            this._ShowAprilFools = 0;
        }
      }
    }

    public bool ShowDiscoveryMessage
    {
      get
      {
        if (this._ShowDiscoveryMessage == -1)
        {
          lock (this.settings)
            this._ShowDiscoveryMessage = !this.settings.Contains("_ShowDiscoveryMessage") ? 1 : (int) this.settings["_ShowDiscoveryMessage"];
        }
        return this._ShowDiscoveryMessage != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_ShowDiscoveryMessage"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._ShowDiscoveryMessage = 1;
          else
            this._ShowDiscoveryMessage = 0;
        }
      }
    }

    public bool ShowSmartBarHelp
    {
      get
      {
        if (this._ShowSmartBarHelp == -1)
        {
          lock (this.settings)
            this._ShowSmartBarHelp = !this.settings.Contains("_ShowSmartBarHelp") ? 1 : (int) this.settings["_ShowSmartBarHelp"];
        }
        return this._ShowSmartBarHelp != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_ShowSmartBarHelp"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._ShowSmartBarHelp = 1;
          else
            this._ShowSmartBarHelp = 0;
        }
      }
    }

    public int OpenedAppCounter
    {
      get
      {
        lock (this.settings)
          this._OpenedAppCounter = !this.settings.Contains(nameof (OpenedAppCounter)) ? -1 : (int) this.settings[nameof (OpenedAppCounter)];
        return this._OpenedAppCounter;
      }
      set
      {
        lock (this.settings)
        {
          try
          {
            this.settings[nameof (OpenedAppCounter)] = (object) value;
            this._OpenedAppCounter = value;
          }
          catch
          {
          }
        }
      }
    }

    public int OpenedAppRatingCounter
    {
      get
      {
        lock (this.settings)
          this._OpenedAppRatingCounter = !this.settings.Contains("_OpenedAppRatingCounter") ? 0 : (int) this.settings["_OpenedAppRatingCounter"];
        return this._OpenedAppRatingCounter;
      }
      set
      {
        lock (this.settings)
        {
          try
          {
            this.settings["_OpenedAppRatingCounter"] = (object) value;
            this._OpenedAppRatingCounter = value;
          }
          catch
          {
          }
        }
      }
    }

    public string MessageOfTheDay
    {
      get
      {
        lock (this.settings)
          return this.settings.Contains(nameof (MessageOfTheDay)) ? (string) this.settings[nameof (MessageOfTheDay)] : "";
      }
      set
      {
        lock (this.settings)
          this.settings[nameof (MessageOfTheDay)] = (object) value;
      }
    }

    public bool ShowSaveNote
    {
      get
      {
        lock (this.settings)
          return !this.settings.Contains(nameof (ShowSaveNote)) || (bool) this.settings[nameof (ShowSaveNote)];
      }
      set
      {
        lock (this.settings)
          this.settings[nameof (ShowSaveNote)] = (object) value;
      }
    }

    public bool DEBUGGING
    {
      get
      {
        if (this._Debugging == -1)
        {
          lock (this.settings)
            this._Debugging = !this.settings.Contains("_Debugging") ? 0 : (int) this.settings["_Debugging"];
        }
        return this._Debugging != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_Debugging"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._Debugging = 1;
          else
            this._Debugging = 0;
        }
      }
    }

    public int DatabaseVersion
    {
      get
      {
        lock (this.settings)
          return this.settings.Contains(nameof (DatabaseVersion)) ? (int) this.settings[nameof (DatabaseVersion)] : 0;
      }
      set
      {
        lock (this.settings)
          this.settings[nameof (DatabaseVersion)] = (object) value;
      }
    }

    public double AppVersion
    {
      get => (double) this.DataMan.VIPDataMan.GetValue(nameof (AppVersion), (object) 0.0);
      set => this.DataMan.VIPDataMan.SetValue(nameof (AppVersion), (object) value);
    }

    public int ScaleFactor
    {
      get
      {
        if (this._ScaleFactor == -1)
        {
          lock (this.settings)
            this._ScaleFactor = !this.settings.Contains("_ScaleFactor") ? -1 : (int) this.settings["_ScaleFactor"];
        }
        return this._ScaleFactor;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_ScaleFactor"] = (object) value;
          this._ScaleFactor = value;
        }
      }
    }

    public bool UseInAppBroser
    {
      get
      {
        if (this._UseInAppBroser == -1)
        {
          lock (this.settings)
            this._UseInAppBroser = !this.settings.Contains("_UseInAppBroser") ? 1 : (int) this.settings["_UseInAppBroser"];
        }
        return this._UseInAppBroser != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_UseInAppBroser"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._UseInAppBroser = 1;
          else
            this._UseInAppBroser = 0;
        }
      }
    }

    public bool ResetOrenationOnStart
    {
      get
      {
        if (this._ResetOrenationOnStart == -1)
        {
          lock (this.settings)
            this._ResetOrenationOnStart = !this.settings.Contains("_ResetOrenationOnStart") ? 1 : (int) this.settings["_ResetOrenationOnStart"];
        }
        return this._ResetOrenationOnStart != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_ResetOrenationOnStart"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._ResetOrenationOnStart = 1;
          else
            this._ResetOrenationOnStart = 0;
        }
      }
    }

    public bool ScreenOrenLocked
    {
      get
      {
        if (this._ScreenOrenLocked == -1)
        {
          lock (this.settings)
            this._ScreenOrenLocked = !this.settings.Contains("_ScreenOrenLocked") ? 0 : (int) this.settings["_ScreenOrenLocked"];
        }
        return this._ScreenOrenLocked != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_ScreenOrenLocked"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._ScreenOrenLocked = 1;
          else
            this._ScreenOrenLocked = 0;
        }
      }
    }

    public bool ScreenOrenLockedtoPortrait
    {
      get
      {
        if (this._ScreenOrenLockedPos == -1)
        {
          lock (this.settings)
            this._ScreenOrenLockedPos = !this.settings.Contains("_ScreenOrenLockedPos") ? 1 : (int) this.settings["_ScreenOrenLockedPos"];
        }
        return this._ScreenOrenLockedPos != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_ScreenOrenLockedPos"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._ScreenOrenLockedPos = 1;
          else
            this._ScreenOrenLockedPos = 0;
        }
      }
    }

    public bool OnlyUpdateOnWifi
    {
      get
      {
        if (this._OnlyUpdateOnWifi == -1)
        {
          lock (this.settings)
            this._OnlyUpdateOnWifi = !this.settings.Contains("_OnlyUpdateOnWifi") ? 0 : (int) this.settings["_OnlyUpdateOnWifi"];
        }
        return this._OnlyUpdateOnWifi != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_OnlyUpdateOnWifi"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._OnlyUpdateOnWifi = 1;
          else
            this._OnlyUpdateOnWifi = 0;
        }
      }
    }

    public bool FreshStartFromLiveTile
    {
      get
      {
        if (this._freshStartFromLiveTile == -1)
        {
          lock (this.settings)
            this._freshStartFromLiveTile = !this.settings.Contains("_freshStartFromLiveTile") ? 0 : (int) this.settings["_freshStartFromLiveTile"];
        }
        return this._freshStartFromLiveTile != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_freshStartFromLiveTile"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._freshStartFromLiveTile = 1;
          else
            this._freshStartFromLiveTile = 0;
        }
      }
    }

    public bool ShowSystemBar
    {
      get
      {
        if (this._showSystemBar == -1)
        {
          lock (this.settings)
            this._showSystemBar = !this.settings.Contains("_showSystemBar") ? 0 : (int) this.settings["_showSystemBar"];
        }
        return this._showSystemBar != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_showSystemBar"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._showSystemBar = 1;
          else
            this._showSystemBar = 0;
        }
      }
    }

    public int UnreadInboxMessageCount
    {
      get
      {
        lock (this.settings)
          this._UnreadInboxMessageCount = !this.settings.Contains("_UnreadInboxMessageCount") ? -1 : (int) this.settings["_UnreadInboxMessageCount"];
        return this._UnreadInboxMessageCount;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_UnreadInboxMessageCount"] = (object) value;
          this._UnreadInboxMessageCount = value;
        }
      }
    }

    public string MostRecentInboxMessageTextAuthor
    {
      get
      {
        if (this._MostRecentInboxMessageTextAuthor == null)
        {
          lock (this.settings)
            this._MostRecentInboxMessageTextAuthor = !this.settings.Contains("_MostRecentInboxMessageTextAuthor") ? "" : (string) this.settings["_MostRecentInboxMessageTextAuthor"];
        }
        return this._MostRecentInboxMessageTextAuthor;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_MostRecentInboxMessageTextAuthor"] = (object) value;
          this._MostRecentInboxMessageTextAuthor = value;
        }
      }
    }

    public string MostRecentInboxMessageTextBody
    {
      get
      {
        if (this._MostRecentInboxMessageTextBody == null)
        {
          lock (this.settings)
            this._MostRecentInboxMessageTextBody = !this.settings.Contains("_MostRecentInboxMessageTextBody") ? "" : (string) this.settings["_MostRecentInboxMessageTextBody"];
        }
        return this._MostRecentInboxMessageTextBody;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_MostRecentInboxMessageTextBody"] = (object) value;
          this._MostRecentInboxMessageTextBody = value;
        }
      }
    }

    public bool HasMail
    {
      get
      {
        if (this._HasMail == -1)
        {
          lock (this.settings)
            this._HasMail = !this.settings.Contains(nameof (HasMail)) ? 0 : (!(bool) this.settings[nameof (HasMail)] ? 0 : 1);
        }
        return this._HasMail == 1;
      }
      set
      {
        lock (this.settings)
        {
          this.settings[nameof (HasMail)] = (object) value;
          if (value)
            this._HasMail = 1;
          else
            this._HasMail = 0;
        }
      }
    }

    public bool ShowReadStories
    {
      get
      {
        if (this._ShowReadStories == -1)
        {
          lock (this.settings)
            this._ShowReadStories = !this.settings.Contains("_ShowReadStories") ? 1 : (int) this.settings["_ShowReadStories"];
        }
        return this._ShowReadStories != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_ShowReadStories"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._ShowReadStories = 1;
          else
            this._ShowReadStories = 0;
        }
      }
    }

    public bool OpenSelfTextIntoFlipView
    {
      get
      {
        if (this._OpenSelfTextIntoFlipView == -1)
        {
          lock (this.settings)
            this._OpenSelfTextIntoFlipView = !this.settings.Contains("_OpenSelfTextIntoFlipView") ? 0 : (int) this.settings["_OpenSelfTextIntoFlipView"];
        }
        return this._OpenSelfTextIntoFlipView != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_OpenSelfTextIntoFlipView"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._OpenSelfTextIntoFlipView = 1;
          else
            this._OpenSelfTextIntoFlipView = 0;
        }
      }
    }

    public bool ShrinkStoryHeaderOnScroll
    {
      get
      {
        if (this._ShrinkStoryHeaderOnScroll == -1)
        {
          lock (this.settings)
            this._ShrinkStoryHeaderOnScroll = !this.settings.Contains("_ShrinkStoryHeaderOnScroll") ? 1 : (int) this.settings["_ShrinkStoryHeaderOnScroll"];
        }
        return this._ShrinkStoryHeaderOnScroll != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_ShrinkStoryHeaderOnScroll"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._ShrinkStoryHeaderOnScroll = 1;
          else
            this._ShrinkStoryHeaderOnScroll = 0;
        }
      }
    }

    public HashList<string, int> ReadStories
    {
      get
      {
        if (this._ReadStories == null)
        {
          lock (this.settings)
            this._ReadStories = !this.settings.Contains("ReadStories2.0") ? new HashList<string, int>(500) : (HashList<string, int>) this.settings["ReadStories2.0"];
        }
        return this._ReadStories;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["ReadStories2.0"] = (object) value;
          this._ReadStories = value;
        }
      }
    }

    public void AddReadStory(string redditid)
    {
      this.ReadStories.AddIfNotAlready(redditid, 0);
      this.DataMan.BaconSyncObj.AddToReadList(redditid);
    }

    public void SetReadStories()
    {
      lock (this.settings)
        this.settings["ReadStories2.0"] = (object) this.ReadStories;
    }

    public bool ShowWholeTitle
    {
      get
      {
        if (this._ShowWholeTitle == -1)
        {
          lock (this.settings)
            this._ShowWholeTitle = !this.settings.Contains("_ShowWholeTitle") ? 0 : (int) this.settings["_ShowWholeTitle"];
        }
        return this._ShowWholeTitle != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_ShowWholeTitle"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._ShowWholeTitle = 1;
          else
            this._ShowWholeTitle = 0;
        }
      }
    }

    public double StoryUpdateTime
    {
      get
      {
        if (this._storyUpdateTime == -1.0)
        {
          lock (this.settings)
            this._storyUpdateTime = !this.settings.Contains(nameof (StoryUpdateTime)) ? 600000.0 : (double) this.settings[nameof (StoryUpdateTime)];
        }
        return this._storyUpdateTime;
      }
      set
      {
        lock (this.settings)
        {
          this.settings[nameof (StoryUpdateTime)] = (object) value;
          this._storyUpdateTime = value;
        }
      }
    }

    public int OpenedCommentsCounter
    {
      get
      {
        lock (this.settings)
          this._OpenedCommentsCounter = !this.settings.Contains("_OpenedCommentsCounter") ? -1 : (int) this.settings["_OpenedCommentsCounter"];
        return this._OpenedCommentsCounter;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_OpenedCommentsCounter"] = (object) value;
          this._OpenedCommentsCounter = value;
        }
      }
    }

    public int DefaultCommentSort
    {
      get
      {
        lock (this.settings)
          this._DefaultCommentSort = !this.settings.Contains("_DefaultCommentSort") ? 0 : (int) this.settings["_DefaultCommentSort"];
        return this._DefaultCommentSort;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_DefaultCommentSort"] = (object) value;
          this._DefaultCommentSort = value;
        }
      }
    }

    public double CommentUpdateTime
    {
      get
      {
        if (this._commentUpdateTime == -1.0)
        {
          lock (this.settings)
            this._commentUpdateTime = !this.settings.Contains(nameof (CommentUpdateTime)) ? 600000.0 : (double) this.settings[nameof (CommentUpdateTime)];
        }
        return this._commentUpdateTime;
      }
      set
      {
        lock (this.settings)
        {
          this.settings[nameof (CommentUpdateTime)] = (object) value;
          this._commentUpdateTime = value;
        }
      }
    }

    public bool ShowExpandedSelfText
    {
      get
      {
        if (this._showExpandedSelfText == -1)
        {
          lock (this.settings)
            this._showExpandedSelfText = !this.settings.Contains("_showExpandedSelfText") ? 0 : (int) this.settings["_showExpandedSelfText"];
        }
        return this._showExpandedSelfText != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_showExpandedSelfText"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._showExpandedSelfText = 1;
          else
            this._showExpandedSelfText = 0;
        }
      }
    }

    public Dictionary<string, SettingsManager.FlipPlaceHolder> FlipPlaceHolders
    {
      get
      {
        if (this._FlipPlaceHolders == null)
        {
          lock (this.settings)
            this._FlipPlaceHolders = !this.settings.Contains("_FlipPlaceHolders") ? new Dictionary<string, SettingsManager.FlipPlaceHolder>(0) : (Dictionary<string, SettingsManager.FlipPlaceHolder>) this.settings["_FlipPlaceHolders"];
        }
        return this._FlipPlaceHolders;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_FlipPlaceHolders"] = (object) value;
          this._FlipPlaceHolders = value;
        }
      }
    }

    public void SetFlipPlaceHolders()
    {
      lock (this.settings)
        this.settings["_FlipPlaceHolders"] = (object) this._FlipPlaceHolders;
    }

    public bool ResumeFlipMode
    {
      get
      {
        if (this._ResumeFlipMode == -1)
        {
          lock (this.settings)
            this._ResumeFlipMode = !this.settings.Contains("_ResumeFlipMode") ? 1 : (int) this.settings["_ResumeFlipMode"];
        }
        return this._ResumeFlipMode != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_ResumeFlipMode"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._ResumeFlipMode = 1;
          else
            this._ResumeFlipMode = 0;
        }
      }
    }

    public bool FlipViewShowBrowserHelp
    {
      get
      {
        if (this._FlipViewShowBrowserHelp == -1)
        {
          lock (this.settings)
            this._FlipViewShowBrowserHelp = !this.settings.Contains("_FlipViewShowBrowserHelp") ? 1 : (int) this.settings["_FlipViewShowBrowserHelp"];
        }
        return this._FlipViewShowBrowserHelp != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_FlipViewShowBrowserHelp"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._FlipViewShowBrowserHelp = 1;
          else
            this._FlipViewShowBrowserHelp = 0;
        }
      }
    }

    public bool FlipViewShowBrowserHelpTwo
    {
      get
      {
        if (this._FlipViewShowBrowserHelpTwo == -1)
        {
          lock (this.settings)
            this._FlipViewShowBrowserHelpTwo = !this.settings.Contains("_FlipViewShowBrowserHelpTwo") ? 1 : (int) this.settings["_FlipViewShowBrowserHelpTwo"];
        }
        return this._FlipViewShowBrowserHelpTwo != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_FlipViewShowBrowserHelpTwo"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._FlipViewShowBrowserHelpTwo = 1;
          else
            this._FlipViewShowBrowserHelpTwo = 0;
        }
      }
    }

    public bool NSFWClickThrough
    {
      get
      {
        if (this._NSFWClickThrough == -1)
        {
          lock (this.settings)
            this._NSFWClickThrough = !this.settings.Contains("_NSFWClickThrough") ? 1 : (int) this.settings["_NSFWClickThrough"];
        }
        return this._NSFWClickThrough != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_NSFWClickThrough"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._NSFWClickThrough = 1;
          else
            this._NSFWClickThrough = 0;
        }
      }
    }

    public bool FlipViewCacheNextWebPage
    {
      get
      {
        if (this._FlipViewCacheNextWebPage == -1)
        {
          lock (this.settings)
            this._FlipViewCacheNextWebPage = !this.settings.Contains("_FlipViewCacheNextWebPage") ? 1 : (int) this.settings["_FlipViewCacheNextWebPage"];
        }
        return this._FlipViewCacheNextWebPage != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_FlipViewCacheNextWebPage"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._FlipViewCacheNextWebPage = 1;
          else
            this._FlipViewCacheNextWebPage = 0;
        }
      }
    }

    public bool ShowImagesOnlyInFlipView
    {
      get
      {
        if (this._ShowImagesOnlyInFlipView == -1)
        {
          lock (this.settings)
            this._ShowImagesOnlyInFlipView = !this.settings.Contains("_ShowImagesOnlyInFlipView") ? 0 : (int) this.settings["_ShowImagesOnlyInFlipView"];
        }
        return this._ShowImagesOnlyInFlipView != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_ShowImagesOnlyInFlipView"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._ShowImagesOnlyInFlipView = 1;
          else
            this._ShowImagesOnlyInFlipView = 0;
        }
      }
    }

    public HashList<string, bool> OptimizeWebsites
    {
      get
      {
        if (this._OptimizeWebsites == null)
        {
          lock (this.settings)
          {
            if (this.settings.Contains("_OptimizeWebsites"))
            {
              this._OptimizeWebsites = (HashList<string, bool>) this.settings["_OptimizeWebsites"];
            }
            else
            {
              this._OptimizeWebsites = new HashList<string, bool>(50);
              this._OptimizeWebsites.Add("imgur.com", false);
              this._OptimizeWebsites.Add("theverge.com", false);
              this._OptimizeWebsites.Add("allthingsd.com", false);
              this._OptimizeWebsites.Add("allaboutwindowsphone.com", false);
              this._OptimizeWebsites.Add("twitter.com", false);
              this._OptimizeWebsites.Add("youtube.com", false);
              this._OptimizeWebsites.Add("theguardian.com", false);
              this._OptimizeWebsites.Add("youtu.be", false);
              this._OptimizeWebsites.Add("windowsphone.com", false);
              this._OptimizeWebsites.Add("nokia.com", false);
              this._OptimizeWebsites.Add("google.com", false);
            }
          }
        }
        return this._OptimizeWebsites;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_OptimizeWebsites"] = (object) value;
          this._OptimizeWebsites = value;
        }
      }
    }

    public void SetOptimizeWebsites()
    {
      lock (this.settings)
        this.settings["_OptimizeWebsites"] = (object) this.OptimizeWebsites;
    }

    public bool OptimizeWebByDefault
    {
      get
      {
        if (this._OptimizeWebByDefault == -1)
        {
          lock (this.settings)
            this._OptimizeWebByDefault = !this.settings.Contains("_OptimizeWebByDefault") ? 0 : (int) this.settings["_OptimizeWebByDefault"];
        }
        return this._OptimizeWebByDefault != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_OptimizeWebByDefault"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._OptimizeWebByDefault = 1;
          else
            this._OptimizeWebByDefault = 0;
        }
      }
    }

    public bool SwipeToVote
    {
      get
      {
        if (this._SwipeToVote == -1)
        {
          lock (this.settings)
            this._SwipeToVote = !this.settings.Contains("_SwipeToVote") ? 1 : (int) this.settings["_SwipeToVote"];
        }
        return this._SwipeToVote != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_SwipeToVote"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._SwipeToVote = 1;
          else
            this._SwipeToVote = 0;
        }
      }
    }

    public bool ShowStoryOverLay
    {
      get
      {
        if (this._showStoryOverLay == -1)
          this._showStoryOverLay = (int) this.DataMan.VIPDataMan.GetValue("_showStoryOverLay", (object) 1);
        return this._showStoryOverLay != 0;
      }
      set
      {
        lock (this.settings)
        {
          if (value)
            this.DataMan.VIPDataMan.SetValue("_showStoryOverLay", (object) 1);
          else
            this.DataMan.VIPDataMan.SetValue("_showStoryOverLay", (object) 0);
          if (value)
            this._showStoryOverLay = 1;
          else
            this._showStoryOverLay = 0;
        }
      }
    }

    public bool ShowStorySelfOverLay
    {
      get
      {
        if (this._showStorySelfOverLay == -1)
          this._showStorySelfOverLay = (int) this.DataMan.VIPDataMan.GetValue("_showStorySelfOverLay", (object) 1);
        return this._showStorySelfOverLay != 0;
      }
      set
      {
        lock (this.settings)
        {
          if (value)
            this.DataMan.VIPDataMan.SetValue("_showStorySelfOverLay", (object) 1);
          else
            this.DataMan.VIPDataMan.SetValue("_showStorySelfOverLay", (object) 0);
          if (value)
            this._showStorySelfOverLay = 1;
          else
            this._showStorySelfOverLay = 0;
        }
      }
    }

    public bool ShowStoryListOverLay
    {
      get
      {
        if (this._ShowStoryListOverLay == -1)
          this._ShowStoryListOverLay = (int) this.DataMan.VIPDataMan.GetValue("_ShowStoryListOverLay", (object) 1);
        return this._ShowStoryListOverLay != 0;
      }
      set
      {
        lock (this.settings)
        {
          if (value)
          {
            this.DataMan.VIPDataMan.SetValue("_ShowStoryListOverLay", (object) 1);
            this._ShowStoryListOverLay = 1;
          }
          else
          {
            this.DataMan.VIPDataMan.SetValue("_ShowStoryListOverLay", (object) 0);
            this._ShowStoryListOverLay = 0;
          }
        }
      }
    }

    public bool ShowImageViewerHelpOverlay
    {
      get
      {
        if (this._ShowImageViewerHelpOverlay == -1)
          this._ShowImageViewerHelpOverlay = (int) this.DataMan.VIPDataMan.GetValue("_ShowImageViewerHelpOverlay", (object) 1);
        return this._ShowImageViewerHelpOverlay != 0;
      }
      set
      {
        lock (this.settings)
        {
          if (value)
            this.DataMan.VIPDataMan.SetValue("_ShowImageViewerHelpOverlay", (object) 1);
          else
            this.DataMan.VIPDataMan.SetValue("_ShowImageViewerHelpOverlay", (object) 0);
          if (value)
            this._ShowImageViewerHelpOverlay = 1;
          else
            this._ShowImageViewerHelpOverlay = 0;
        }
      }
    }

    public HashList<string, int> MostViewSubreddits
    {
      get
      {
        if (this._MostViewSubreddits == null)
        {
          lock (this.settings)
            this._MostViewSubreddits = !this.settings.Contains("_MostViewSubreddits") ? new HashList<string, int>(50) : (HashList<string, int>) this.settings["_MostViewSubreddits"];
        }
        return this._MostViewSubreddits;
      }
    }

    public void AddSubredditViewCount(SubReddit subreddit)
    {
      if (this.MostViewSubreddits.ContainsKey(subreddit.URL))
        this.MostViewSubreddits[subreddit.URL]++;
      else
        this.MostViewSubreddits.Add(subreddit.URL, 1);
      this.settings["_MostViewSubreddits"] = (object) this.MostViewSubreddits;
    }

    public int SortSubRedditsBy
    {
      get
      {
        lock (this.settings)
          this._SortSubRedditsBy = !this.settings.Contains("_SortSubRedditsBy") ? 0 : (int) this.settings["_SortSubRedditsBy"];
        return this._SortSubRedditsBy;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_SortSubRedditsBy"] = (object) value;
          this._SortSubRedditsBy = value;
        }
      }
    }

    public int MainLaindingImageUsed
    {
      get
      {
        if (this._MainLandingLastImageUsed == -1)
        {
          lock (this.settings)
            this._MainLandingLastImageUsed = !this.settings.Contains("_MainLandingLastImageUsed") ? 0 : (int) this.settings["_MainLandingLastImageUsed"];
        }
        return this._MainLandingLastImageUsed;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_MainLandingLastImageUsed"] = (object) value;
          this._MainLandingLastImageUsed = value;
        }
      }
    }

    public int MainLandingMaxImage
    {
      get
      {
        if (this._MainLandingMaxImage == -1)
        {
          lock (this.settings)
            this._MainLandingMaxImage = !this.settings.Contains("_MainLandingMaxImage") ? 0 : (int) this.settings["_MainLandingMaxImage"];
        }
        return this._MainLandingMaxImage;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_MainLandingMaxImage"] = (object) value;
          this._MainLandingMaxImage = value;
        }
      }
    }

    public string LandingWallpaper
    {
      get
      {
        if (this._LandingWallpaper == null)
        {
          lock (this.settings)
            this._LandingWallpaper = !this.settings.Contains(nameof (LandingWallpaper)) ? "/r/earthporn/" : (string) this.settings[nameof (LandingWallpaper)];
        }
        return this._LandingWallpaper;
      }
      set
      {
        lock (this.settings)
        {
          this.settings[nameof (LandingWallpaper)] = (object) value;
          this._LandingWallpaper = value;
        }
      }
    }

    public string LandingStoryProvider
    {
      get
      {
        if (this._LandingStoryProvider == null)
        {
          lock (this.settings)
            this._LandingStoryProvider = !this.settings.Contains(nameof (LandingStoryProvider)) ? "mostViewed" : (string) this.settings[nameof (LandingStoryProvider)];
        }
        return this._LandingStoryProvider;
      }
      set
      {
        lock (this.settings)
        {
          this.settings[nameof (LandingStoryProvider)] = (object) value;
          this._LandingStoryProvider = value;
        }
      }
    }

    public int MainTileBackground
    {
      get
      {
        if (this._MainTileBack == -1)
        {
          lock (this.settings)
            this._MainTileBack = !this.settings.Contains("_MainTileBack") ? 0 : (int) this.settings["_MainTileBack"];
        }
        return this._MainTileBack;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_MainTileBack"] = (object) value;
          this._MainTileBack = value;
        }
      }
    }

    public int SubRedditBackground
    {
      get
      {
        if (this._SubRedditBackground == -1)
        {
          lock (this.settings)
            this._SubRedditBackground = !this.settings.Contains("_SubRedditBackground") ? 1 : (int) this.settings["_SubRedditBackground"];
        }
        return this._SubRedditBackground;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_SubRedditBackground"] = (object) value;
          this._SubRedditBackground = value;
        }
      }
    }

    public int StoryTileBackground
    {
      get
      {
        if (this._StoryTileBackground == -1)
        {
          lock (this.settings)
            this._StoryTileBackground = !this.settings.Contains("_StoryTileBackground") ? 1 : (int) this.settings["_StoryTileBackground"];
        }
        return this._StoryTileBackground;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_StoryTileBackground"] = (object) value;
          this._StoryTileBackground = value;
        }
      }
    }

    public bool MainLiveTileShowUnread
    {
      get
      {
        if (this._MainLiveTileShowUnread == -1)
        {
          lock (this.settings)
            this._MainLiveTileShowUnread = !this.settings.Contains("_MainLiveTileShowUnread") ? 1 : (int) this.settings["_MainLiveTileShowUnread"];
        }
        return this._MainLiveTileShowUnread != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_MainLiveTileShowUnread"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._MainLiveTileShowUnread = 1;
          else
            this._MainLiveTileShowUnread = 0;
        }
      }
    }

    public bool MainLiveTileShowKarma
    {
      get
      {
        if (this._MainLiveTileShowKarma == -1)
        {
          lock (this.settings)
            this._MainLiveTileShowKarma = !this.settings.Contains("_MainLiveTileShowKarma") ? 1 : (int) this.settings["_MainLiveTileShowKarma"];
        }
        return this._MainLiveTileShowKarma != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_MainLiveTileShowKarma"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._MainLiveTileShowKarma = 1;
          else
            this._MainLiveTileShowKarma = 0;
        }
      }
    }

    public bool StoryLiveTileShowNewMessage
    {
      get
      {
        if (this._StoryLiveTileShowNewMessage == -1)
        {
          lock (this.settings)
            this._StoryLiveTileShowNewMessage = !this.settings.Contains(nameof (StoryLiveTileShowNewMessage)) ? 1 : (int) this.settings[nameof (StoryLiveTileShowNewMessage)];
        }
        return this._StoryLiveTileShowNewMessage != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings[nameof (StoryLiveTileShowNewMessage)] = !value ? (object) 0 : (object) 1;
          if (value)
            this._StoryLiveTileShowNewMessage = 1;
          else
            this._StoryLiveTileShowNewMessage = 0;
        }
      }
    }

    public bool ForceUpdateTiles
    {
      get
      {
        if (this._ForceUpdateTiles == -1)
        {
          lock (this.settings)
            this._ForceUpdateTiles = !this.settings.Contains("_ForceUpdateTiles") ? 0 : (int) this.settings["_ForceUpdateTiles"];
        }
        return this._ForceUpdateTiles != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_ForceUpdateTiles"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._ForceUpdateTiles = 1;
          else
            this._ForceUpdateTiles = 0;
        }
      }
    }

    public bool OnlyUpdateWifiLockScreenImages
    {
      get
      {
        if (this._OnlyUpdateWifiLockScreenImages == -1)
        {
          lock (this.settings)
            this._OnlyUpdateWifiLockScreenImages = !this.settings.Contains("_OnlyUpdateWifiLockScreenImages") ? 1 : (int) this.settings["_OnlyUpdateWifiLockScreenImages"];
        }
        return this._OnlyUpdateWifiLockScreenImages != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_OnlyUpdateWifiLockScreenImages"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._OnlyUpdateWifiLockScreenImages = 1;
          else
            this._OnlyUpdateWifiLockScreenImages = 0;
        }
      }
    }

    public bool OnlyUpdateWifiTilesImages
    {
      get
      {
        if (this._OnlyUpdateWifiTilesImages == -1)
        {
          lock (this.settings)
            this._OnlyUpdateWifiTilesImages = !this.settings.Contains("_OnlyUpdateWifiTilesImages") ? 1 : (int) this.settings["_OnlyUpdateWifiTilesImages"];
        }
        return this._OnlyUpdateWifiTilesImages != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_OnlyUpdateWifiTilesImages"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._OnlyUpdateWifiTilesImages = 1;
          else
            this._OnlyUpdateWifiTilesImages = 0;
        }
      }
    }

    public bool ShowTostNotifications
    {
      get
      {
        if (this._ShowTostNotifications == -1)
        {
          lock (this.settings)
            this._ShowTostNotifications = !this.settings.Contains("_ShowTostNotifications") ? 1 : (int) this.settings["_ShowTostNotifications"];
        }
        return this._ShowTostNotifications != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_ShowTostNotifications"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._ShowTostNotifications = 1;
          else
            this._ShowTostNotifications = 0;
        }
      }
    }

    public bool ShowDisabledBackgroundWarning
    {
      get
      {
        if (this._ShowDisabledBackgroundWarning == -1)
        {
          lock (this.settings)
            this._ShowDisabledBackgroundWarning = !this.settings.Contains("_ShowDisabledBackgroundWarning") ? 1 : (int) this.settings["_ShowDisabledBackgroundWarning"];
        }
        return this._ShowDisabledBackgroundWarning != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_ShowDisabledBackgroundWarning"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._ShowDisabledBackgroundWarning = 1;
          else
            this._ShowDisabledBackgroundWarning = 0;
        }
      }
    }

    public int BackgroundAgentEnabled
    {
      get
      {
        this._BackgroundAgentEnabled = (int) this.DataMan.VIPDataMan.GetValue("_BackgroundAgentEnabled", (object) -1);
        return this._BackgroundAgentEnabled;
      }
      set
      {
        this.DataMan.VIPDataMan.SetValue("_BackgroundAgentEnabled", (object) value);
        this._BackgroundAgentEnabled = value;
      }
    }

    public bool DoNotDistEnabled
    {
      get
      {
        if (this._DoNotDistEnabled == -1)
        {
          lock (this.settings)
            this._DoNotDistEnabled = !this.settings.Contains("_DoNotDistEnabled") ? 0 : (int) this.settings["_DoNotDistEnabled"];
        }
        return this._DoNotDistEnabled != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_DoNotDistEnabled"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._DoNotDistEnabled = 1;
          else
            this._DoNotDistEnabled = 0;
        }
      }
    }

    public DateTime DoNotDistFrom
    {
      get
      {
        lock (this.settings)
          return this.settings.Contains("_DoNotDistFrom") ? (DateTime) this.settings["_DoNotDistFrom"] : new DateTime(1989, 4, 19, 0, 0, 0);
      }
      set
      {
        lock (this.settings)
          this.settings["_DoNotDistFrom"] = (object) value;
      }
    }

    public DateTime DoNotDistTo
    {
      get
      {
        lock (this.settings)
          return this.settings.Contains("_DoNotDistTo") ? (DateTime) this.settings["_DoNotDistTo"] : new DateTime(1989, 4, 19, 0, 0, 0);
      }
      set
      {
        lock (this.settings)
          this.settings["_DoNotDistTo"] = (object) value;
      }
    }

    public double TileImageFullUpdate
    {
      get
      {
        if (this._TileImageFullUpdate == -1.0)
        {
          lock (this.settings)
            this._TileImageFullUpdate = !this.settings.Contains("_TileImageFullUpdate") ? 0.0 : (double) this.settings["_TileImageFullUpdate"];
        }
        return this._TileImageFullUpdate;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_TileImageFullUpdate"] = (object) value;
          this._TileImageFullUpdate = value;
        }
      }
    }

    public double TileImageHalfUpdate
    {
      get
      {
        if (this._TileImageHalfUpdate == -1.0)
        {
          lock (this.settings)
            this._TileImageHalfUpdate = !this.settings.Contains("_TileImageHalfUpdate") ? 0.0 : (double) this.settings["_TileImageHalfUpdate"];
        }
        return this._TileImageHalfUpdate;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_TileImageHalfUpdate"] = (object) value;
          this._TileImageHalfUpdate = value;
        }
      }
    }

    public double LastStartUpRun
    {
      get
      {
        if (this._LastStartUpRun == -1.0)
        {
          lock (this.settings)
            this._LastStartUpRun = !this.settings.Contains("_LastStartUpRun") ? 0.0 : (double) this.settings["_LastStartUpRun"];
        }
        return this._LastStartUpRun;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_LastStartUpRun"] = (object) value;
          this._LastStartUpRun = value;
        }
      }
    }

    public bool UseLockScreenImage
    {
      get
      {
        if (this._UseLockScreenImage == -1)
        {
          lock (this.settings)
            this._UseLockScreenImage = !this.settings.Contains("_UseLockScreenImage") ? 0 : (int) this.settings["_UseLockScreenImage"];
        }
        return this._UseLockScreenImage != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_UseLockScreenImage"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._UseLockScreenImage = 1;
          else
            this._UseLockScreenImage = 0;
        }
      }
    }

    public string LockScreenSubredditURL
    {
      get
      {
        if (this._LockScreenSubredditURL == null)
        {
          lock (this.settings)
            this._LockScreenSubredditURL = !this.settings.Contains("_LockScreenSubredditURL") ? "/r/WPLockscreens/" : (string) this.settings["_LockScreenSubredditURL"];
        }
        return this._LockScreenSubredditURL;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_LockScreenSubredditURL"] = (object) value;
          this._LockScreenSubredditURL = value;
        }
      }
    }

    public int LockScreenUpdateTime
    {
      get
      {
        if (this._LockScreenUpdateTime == -1)
        {
          lock (this.settings)
            this._LockScreenUpdateTime = !this.settings.Contains("_LockScreenUpdateTime") ? 295 : (int) this.settings["_LockScreenUpdateTime"];
        }
        return this._LockScreenUpdateTime;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_LockScreenUpdateTime"] = (object) value;
          this._LockScreenUpdateTime = value;
        }
      }
    }

    public List<string> LockScreenImageFiles
    {
      get
      {
        if (this._LockScreenImageFiles == null)
        {
          lock (this.settings)
            this._LockScreenImageFiles = !this.settings.Contains("_LockScreenImageFiles") ? new List<string>() : (List<string>) this.settings["_LockScreenImageFiles"];
        }
        return this._LockScreenImageFiles;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_LockScreenImageFiles"] = (object) value;
          this._LockScreenImageFiles = value;
        }
      }
    }

    public void SaveLockScreenImageFiles()
    {
      this.settings["_LockScreenImageFiles"] = (object) this.LockScreenImageFiles;
    }

    public int LockScreenLastUsedImage
    {
      get
      {
        if (this._LockScreenLastUsedImage == -1)
        {
          lock (this.settings)
            this._LockScreenLastUsedImage = !this.settings.Contains("_LockScreenLastUsedImage") ? 0 : (int) this.settings["_LockScreenLastUsedImage"];
        }
        return this._LockScreenLastUsedImage;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_LockScreenLastUsedImage"] = (object) value;
          this._LockScreenLastUsedImage = value;
        }
      }
    }

    public bool BaconSyncShowFurtureError
    {
      get
      {
        if (this._BaconSyncShowError == -1)
        {
          lock (this.settings)
            this._BaconSyncShowError = !this.settings.Contains("_BaconSyncShowError") ? 1 : (int) this.settings["_BaconSyncShowError"];
        }
        return this._BaconSyncShowError != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_BaconSyncShowError"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._BaconSyncShowError = 1;
          else
            this._BaconSyncShowError = 0;
        }
      }
    }

    public bool BaconSyncEnabled
    {
      get
      {
        if (this._UseBaconSync == -1)
        {
          lock (this.settings)
            this._UseBaconSync = !this.settings.Contains("_UseBaconSync") ? 0 : (int) this.settings["_UseBaconSync"];
        }
        return this._UseBaconSync != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_UseBaconSync"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._UseBaconSync = 1;
          else
            this._UseBaconSync = 0;
        }
      }
    }

    public string BaconSyncDeviceName
    {
      get
      {
        if (this._BaconSyncDeviceName == null)
        {
          lock (this.settings)
            this._BaconSyncDeviceName = !this.settings.Contains("_BaconSyncDeviceName") ? "" : (string) this.settings["_BaconSyncDeviceName"];
        }
        return this._BaconSyncDeviceName;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_BaconSyncDeviceName"] = (object) value;
          this._BaconSyncDeviceName = value;
        }
      }
    }

    public string BaconSyncPassCode
    {
      get
      {
        if (this._BaconSyncPassCode == null)
        {
          lock (this.settings)
            this._BaconSyncPassCode = !this.settings.Contains("_BaconSyncPassCode") ? "" : (string) this.settings["_BaconSyncPassCode"];
        }
        return this._BaconSyncPassCode;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_BaconSyncPassCode"] = (object) value;
          this._BaconSyncPassCode = value;
        }
      }
    }

    public string BaconSyncPushURL
    {
      get
      {
        if (this._BaconSyncPushURL == null)
        {
          lock (this.settings)
            this._BaconSyncPushURL = !this.settings.Contains("_BaconSyncPushURL") ? "" : (string) this.settings["_BaconSyncPushURL"];
        }
        return this._BaconSyncPushURL;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_BaconSyncPushURL"] = (object) value;
          this._BaconSyncPushURL = value;
        }
      }
    }

    public string BaconSyncAccountName
    {
      get
      {
        if (this._BaconSyncAccountName == null)
        {
          lock (this.settings)
            this._BaconSyncAccountName = !this.settings.Contains("_BaconSyncAccountName") ? "" : (string) this.settings["_BaconSyncAccountName"];
        }
        return this._BaconSyncAccountName;
      }
      set
      {
        lock (this.settings)
        {
          if (value != null)
            value = value.ToLower();
          this.settings["_BaconSyncAccountName"] = (object) value;
          this._BaconSyncAccountName = value;
        }
      }
    }

    public string BaconSyncStatus
    {
      get
      {
        if (this._BaconSyncStatus == null)
        {
          lock (this.settings)
            this._BaconSyncStatus = !this.settings.Contains("_BaconSyncStatus") ? "Waiting To Sync" : (string) this.settings["_BaconSyncStatus"];
        }
        return this._BaconSyncStatus;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_BaconSyncStatus"] = (object) value;
          this._BaconSyncStatus = value;
        }
      }
    }

    public List<string> BaconSyncReadList
    {
      get
      {
        if (this._BaconSyncReadList == null)
        {
          lock (this.settings)
            this._BaconSyncReadList = !this.settings.Contains("_BaconSyncReadList") ? new List<string>() : (List<string>) this.settings["_BaconSyncReadList"];
        }
        return this._BaconSyncReadList;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_BaconSyncReadList"] = (object) value;
          this._BaconSyncReadList = value;
        }
      }
    }

    public void SetBaconSyncReadList()
    {
      lock (this.settings)
        this.settings["_BaconSyncReadList"] = (object) this._BaconSyncReadList;
    }

    public bool ShowBaconSyncSiginMessage
    {
      get
      {
        if (this._ShowBaconSyncSiginMessage == -1)
        {
          lock (this.settings)
            this._ShowBaconSyncSiginMessage = !this.settings.Contains("_ShowBaconSyncSiginMessage") ? 1 : (!(bool) this.settings["_ShowBaconSyncSiginMessage"] ? 0 : 1);
        }
        return this._ShowBaconSyncSiginMessage == 1;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_ShowBaconSyncSiginMessage"] = (object) value;
          if (value)
            this._ShowBaconSyncSiginMessage = 1;
          else
            this._ShowBaconSyncSiginMessage = 0;
        }
      }
    }

    public string SubmitLinkText
    {
      get
      {
        if (this._SubmitLinkText == null)
        {
          lock (this.settings)
            this._SubmitLinkText = !this.settings.Contains("_SubmitLinkText") ? "" : (string) this.settings["_SubmitLinkText"];
        }
        return this._SubmitLinkText;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_SubmitLinkText"] = (object) value;
          this._SubmitLinkText = value;
        }
      }
    }

    public string SubmitLinkURL
    {
      get
      {
        if (this._SubmitLinkURL == null)
        {
          lock (this.settings)
            this._SubmitLinkURL = !this.settings.Contains("_SubmitLinkURL") ? "" : (string) this.settings["_SubmitLinkURL"];
        }
        return this._SubmitLinkURL;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_SubmitLinkURL"] = (object) value;
          this._SubmitLinkURL = value;
        }
      }
    }

    public string SubmitLinkSubreddit
    {
      get
      {
        if (this._SubmitLinkSubreddit == null)
        {
          lock (this.settings)
            this._SubmitLinkSubreddit = !this.settings.Contains("_SubmitLinkSubreddit") ? "" : (string) this.settings["_SubmitLinkSubreddit"];
        }
        return this._SubmitLinkSubreddit;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_SubmitLinkSubreddit"] = (object) value;
          this._SubmitLinkSubreddit = value;
        }
      }
    }

    public bool SubmitLinkIsSelfText
    {
      get
      {
        if (this._SubmitLinkIsSelfText == -1)
        {
          lock (this.settings)
            this._SubmitLinkIsSelfText = !this.settings.Contains("_SubmitLinkIsSelfText") ? 0 : (int) this.settings["_SubmitLinkIsSelfText"];
        }
        return this._SubmitLinkIsSelfText != 0;
      }
      set
      {
        lock (this.settings)
        {
          this.settings["_SubmitLinkIsSelfText"] = !value ? (object) 0 : (object) 1;
          if (value)
            this._SubmitLinkIsSelfText = 1;
          else
            this._SubmitLinkIsSelfText = 0;
        }
      }
    }

    public Dictionary<string, object> DeactivatedPageObjects
    {
      get
      {
        if (this._DeactivatedPageObjects == null)
        {
          lock (this.settings)
            this._DeactivatedPageObjects = !this.settings.Contains(nameof (DeactivatedPageObjects)) ? new Dictionary<string, object>() : (Dictionary<string, object>) this.settings[nameof (DeactivatedPageObjects)];
        }
        return this._DeactivatedPageObjects;
      }
      set
      {
        lock (this.settings)
        {
          this.settings[nameof (DeactivatedPageObjects)] = (object) value;
          this._DeactivatedPageObjects = value;
        }
      }
    }

    public void SetDeactivatedPageObjects()
    {
      lock (this.settings)
      {
        this.settings["DeactivatedPageObjects"] = (object) this.DeactivatedPageObjects;
        this._DeactivatedPageObjects = this.DeactivatedPageObjects;
      }
    }

    public SubReddit DeactivatedActiveSubReddit
    {
      get
      {
        if (this._DeactivatedActiveSubReddit == null)
        {
          lock (this.settings)
            this._DeactivatedActiveSubReddit = !this.settings.Contains(nameof (DeactivatedActiveSubReddit)) ? (SubReddit) null : (SubReddit) this.settings[nameof (DeactivatedActiveSubReddit)];
        }
        return this._DeactivatedActiveSubReddit;
      }
      set
      {
        lock (this.settings)
        {
          this.settings[nameof (DeactivatedActiveSubReddit)] = (object) value;
          this._DeactivatedActiveSubReddit = value;
        }
      }
    }

    public SubRedditDataSeralizable DeactivatedActiveStoryData
    {
      get
      {
        if (this._DeactivatedActiveStoryData == null)
        {
          lock (this.settings)
            this._DeactivatedActiveStoryData = !this.settings.Contains(nameof (DeactivatedActiveStoryData)) ? (SubRedditDataSeralizable) null : (SubRedditDataSeralizable) this.settings[nameof (DeactivatedActiveStoryData)];
        }
        return this._DeactivatedActiveStoryData;
      }
      set
      {
        lock (this.settings)
        {
          this.settings[nameof (DeactivatedActiveStoryData)] = (object) value;
          this._DeactivatedActiveStoryData = value;
        }
      }
    }

    public class FlipPlaceHolder
    {
      public DateTime added;
      public string Entry;
    }
  }
}
