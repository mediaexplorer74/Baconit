// Decompiled with JetBrains decompiler
// Type: Baconit.SubredditDiscoeryCategories
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using BaconitData.Libs;
using Microsoft.Phone.Controls;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;

#nullable disable
namespace Baconit
{
  public class SubredditDiscoeryCategories : PhoneApplicationPage
  {
    private bool isLoaded;
    private string[,] CurrentList;
    private bool isCat;
    private string[,] CategoriesList = new string[17, 2]
    {
      {
        "search",
        "search for subreddits"
      },
      {
        "current events",
        "current news and stories"
      },
      {
        "diy and crafts",
        "do it yourself projects and crafts"
      },
      {
        nameof (funny),
        "for when you need a good laugh"
      },
      {
        "games",
        "specific subreddits for specific games"
      },
      {
        nameof (gaming),
        "general gaming subreddits"
      },
      {
        nameof (history),
        "history from now to the beginning of time"
      },
      {
        nameof (lifestyle),
        "for all shapes and types of people"
      },
      {
        nameof (microsoft),
        "everything microsoft and windows phone and"
      },
      {
        nameof (music),
        "all types of music for all people"
      },
      {
        "photography",
        "cameras, images, and more"
      },
      {
        "random and misc.",
        "interesting random odd and ends"
      },
      {
        "reading and stories",
        "indie stories and good reads"
      },
      {
        nameof (regional),
        "things from your aera"
      },
      {
        nameof (sports),
        "soccer, racing, football, and more"
      },
      {
        "technology",
        "technology for all geeks and nerds"
      },
      {
        "tv shows",
        "new and old show"
      }
    };
    private string[,] currentEvents = new string[9, 3]
    {
      {
        "askscience",
        "",
        "/r/askscience"
      },
      {
        "currentevents",
        "",
        "/r/currentevents"
      },
      {
        "economics",
        "",
        "/r/economics"
      },
      {
        "politics",
        "",
        "/r/politics"
      },
      {
        "science",
        "",
        "/r/science"
      },
      {
        "UKPolitics",
        "",
        "/r/UKPolitics"
      },
      {
        "UnitedKingdom",
        "",
        "/r/UnitedKingdom"
      },
      {
        "usnews",
        "",
        "/r/usnews"
      },
      {
        "worldnews",
        "",
        "/r/worldnews"
      }
    };
    private string[,] diycrafts = new string[21, 3]
    {
      {
        "bleachshirts",
        "",
        "/r/bleachshirts"
      },
      {
        "buildapc",
        "",
        "/r/buildapc"
      },
      {
        "buildapcsales",
        "",
        "/r/buildapcsales"
      },
      {
        "crafts",
        "",
        "/r/crafts"
      },
      {
        "DIY",
        "",
        "/r/DIY"
      },
      {
        "edc",
        "",
        "/r/edc"
      },
      {
        "Homebrewing",
        "",
        "/r/Homebrewing"
      },
      {
        "hometheater",
        "",
        "/r/hometheater"
      },
      {
        "HowTo",
        "",
        "/r/HowTo"
      },
      {
        "hypnosis",
        "",
        "/r/hypnosis"
      },
      {
        "ICanDrawThat",
        "",
        "/r/ICanDrawThat"
      },
      {
        "knitting",
        "",
        "/r/knitting"
      },
      {
        "knives",
        "",
        "/r/knives"
      },
      {
        "learnart",
        "",
        "/r/learnart"
      },
      {
        "LearnUselessTalents",
        "",
        "/r/LearnUselessTalents"
      },
      {
        "sewing",
        "",
        "/r/sewing"
      },
      {
        "SketchDaily",
        "",
        "/r/SketchDaily"
      },
      {
        "SomethingIMade",
        "",
        "/r/SomethingIMade"
      },
      {
        "stencils",
        "",
        "/r/stencils"
      },
      {
        "Woodshed",
        "",
        "/r/Woodshed"
      },
      {
        "youshouldknow",
        "",
        "/r/youshouldknow"
      }
    };
    private string[,] funny = new string[16, 3]
    {
      {
        "adviceanimals",
        "",
        "/r/adviceanimals"
      },
      {
        "aww",
        "",
        "/r/aww"
      },
      {
        "bollywoodgifs",
        "",
        "/r/bollywoodgifs"
      },
      {
        "classicrage",
        "",
        "/r/classicrage"
      },
      {
        "facepalm",
        "",
        "/r/facepalm"
      },
      {
        "fffffffuuuuuuuuuuuu",
        "",
        "/r/fffffffuuuuuuuuuuuu"
      },
      {
        nameof (funny),
        "",
        "/r/funny"
      },
      {
        "gif",
        "",
        "/r/gif"
      },
      {
        "gifs",
        "",
        "/r/gifs"
      },
      {
        "humor",
        "",
        "/r/humor"
      },
      {
        "pandr",
        "",
        "/r/pandr"
      },
      {
        "perfecttiming",
        "",
        "/r/perfecttiming"
      },
      {
        "redditcribs",
        "",
        "/r/redditcribs"
      },
      {
        "ShutUpAndTakeMyMoney",
        "",
        "/r/ShutUpAndTakeMyMoney"
      },
      {
        "WOAHDude",
        "",
        "/r/WOAHDude"
      },
      {
        "wtf",
        "",
        "/r/wtf"
      }
    };
    private string[,] sgames = new string[23, 3]
    {
      {
        "alan wake",
        "",
        "/r/alanwake"
      },
      {
        "battlefield3",
        "",
        "/r/battlefield3"
      },
      {
        "civ",
        "",
        "/r/civ"
      },
      {
        "diablo",
        "",
        "/r/diablo"
      },
      {
        "fez",
        "",
        "/r/fez"
      },
      {
        "fnv",
        "",
        "/r/fnv"
      },
      {
        "fo3",
        "",
        "/r/fo3"
      },
      {
        "guildwars2",
        "",
        "/r/guildwars2"
      },
      {
        "halo",
        "",
        "/r/halo"
      },
      {
        "leagueoflegends",
        "",
        "/r/leagueoflegends"
      },
      {
        "lotro",
        "",
        "/r/lotro"
      },
      {
        "magictcg",
        "",
        "/r/magictcg"
      },
      {
        "masseffect",
        "",
        "/r/masseffect"
      },
      {
        "minecraft",
        "",
        "/r/minecraft"
      },
      {
        "pokemon",
        "",
        "/r/pokemon"
      },
      {
        "rct",
        "",
        "/r/rct"
      },
      {
        "skyrim",
        "",
        "/r/skyrim"
      },
      {
        "starcraft",
        "",
        "/r/starcraft"
      },
      {
        "swtor",
        "",
        "/r/swtor"
      },
      {
        "tf2",
        "",
        "/r/tf2"
      },
      {
        "torchlight",
        "",
        "/r/torchlight"
      },
      {
        "tribes",
        "",
        "/r/tribes"
      },
      {
        "zelda",
        "",
        "/r/zelda"
      }
    };
    private string[,] gaming = new string[8, 3]
    {
      {
        "boardgames",
        "",
        "/r/boardgames"
      },
      {
        "gamedeals",
        "",
        "/r/gamedeals"
      },
      {
        "games",
        "",
        "/r/games"
      },
      {
        nameof (gaming),
        "",
        "/r/gaming"
      },
      {
        "lanparty",
        "",
        "/r/lanparty"
      },
      {
        "steam",
        "",
        "/r/steam"
      },
      {
        "truegaming",
        "",
        "/r/truegaming"
      },
      {
        "xbox360",
        "",
        "/r/xbox360"
      }
    };
    private string[,] history = new string[32, 3]
    {
      {
        "1920s",
        "",
        "/r/1920s"
      },
      {
        "1950s",
        "",
        "/r/1950s"
      },
      {
        "1960s",
        "",
        "/r/1960s"
      },
      {
        "1970s",
        "",
        "/r/1970s"
      },
      {
        "1980s",
        "",
        "/r/1980s"
      },
      {
        "1990s",
        "",
        "/r/1990s"
      },
      {
        "2000s",
        "",
        "/r/2000s"
      },
      {
        "AfricanHistory",
        "",
        "/r/AfricanHistory"
      },
      {
        "AmericanHistory",
        "",
        "/r/AmericanHistory"
      },
      {
        "ancientegypt",
        "",
        "/r/ancientegypt"
      },
      {
        "ancientgreece",
        "",
        "/r/ancientgreece"
      },
      {
        "ancientrome",
        "",
        "/r/ancientrome"
      },
      {
        "ancientworldproblems",
        "",
        "/r/ancientworldproblems"
      },
      {
        "Anthropology",
        "",
        "/r/Anthropology"
      },
      {
        "Archaeology",
        "",
        "/r/Archaeology"
      },
      {
        "AskHistorians",
        "",
        "/r/AskHistorians"
      },
      {
        "AskHistory",
        "",
        "/r/AskHistory"
      },
      {
        "culturalstudies",
        "",
        "/r/culturalstudies"
      },
      {
        "Documentaries",
        "",
        "/r/Documentaries"
      },
      {
        "Foodforthought",
        "",
        "/r/Foodforthought"
      },
      {
        "historicalrage",
        "",
        "/r/historicalrage"
      },
      {
        nameof (history),
        "",
        "/r/history"
      },
      {
        "interview",
        "",
        "/r/interview"
      },
      {
        "Map",
        "",
        "/r/MapPorn"
      },
      {
        "Maps",
        "",
        "/r/Maps"
      },
      {
        "MedievalHistory",
        "",
        "/r/MedievalHistory"
      },
      {
        "Photoessay",
        "",
        "/r/Photoessay"
      },
      {
        "Shipwrecks",
        "",
        "/r/Shipwrecks"
      },
      {
        "ThisDayInHistory",
        "",
        "/r/ThisDayInHistory"
      },
      {
        "USCivilWar",
        "",
        "/r/USCivilWar"
      },
      {
        "USHistory",
        "",
        "/r/USHistory"
      },
      {
        "WorldHistory",
        "",
        "/r/WorldHistory"
      }
    };
    private string[,] lifestyle = new string[15, 3]
    {
      {
        "2xlookbook",
        "",
        "/r/2XLookbook"
      },
      {
        "beer",
        "",
        "/r/beer"
      },
      {
        "bodybuilding",
        "",
        "/r/bodybuilding"
      },
      {
        "femalefashionadvice",
        "",
        "/r/femalefashionadvice"
      },
      {
        "fitness",
        "",
        "/r/fitness"
      },
      {
        "food",
        "",
        "/r/food"
      },
      {
        "frugal",
        "",
        "/r/Frugal"
      },
      {
        "interiordesign",
        "",
        "/r/interiordesign"
      },
      {
        "loseit",
        "",
        "/r/loseit"
      },
      {
        "malefashionadvice",
        "",
        "/r/malefashionadvice"
      },
      {
        "malegrooming",
        "",
        "/r/malegrooming"
      },
      {
        "minimalism",
        "",
        "/r/minimalism"
      },
      {
        "seduction",
        "",
        "/r/seduction"
      },
      {
        "socialskills",
        "",
        "/r/socialskills"
      },
      {
        "vegetarian ",
        "",
        "/r/vegetarian "
      }
    };
    private string[,] microsoft = new string[14, 3]
    {
      {
        "baconit",
        "",
        "/r/Baconit"
      },
      {
        "bing",
        "",
        "/r/bing"
      },
      {
        "htc",
        "",
        "/r/htc"
      },
      {
        nameof (microsoft),
        "",
        "/r/microsoft"
      },
      {
        "narwhalw8",
        "",
        "/r/narwhalw8"
      },
      {
        "nokia",
        "",
        "/r/nokia"
      },
      {
        "windows",
        "",
        "/r/windows"
      },
      {
        "windows 8",
        "",
        "/r/windows8"
      },
      {
        "window sphone",
        "",
        "/r/WindowsPhone"
      },
      {
        "wp7 dev",
        "",
        "/r/wp7 dev"
      },
      {
        "xbox 360",
        "",
        "/r/xbox 360"
      },
      {
        "zune",
        "",
        "/r/zune"
      },
      {
        "sysadmin",
        "",
        "/r/sysadmin"
      },
      {
        "power shell",
        "",
        "/r/powershell"
      }
    };
    private string[,] music = new string[17, 3]
    {
      {
        "album art",
        "",
        "/r/AlbumArtPorn"
      },
      {
        "break beat",
        "",
        "/r/breakbeat"
      },
      {
        "breakcore",
        "",
        "/r/breakcore"
      },
      {
        "daft punk",
        "",
        "/r/daftpunk"
      },
      {
        "dnb",
        "",
        "/r/dnb"
      },
      {
        "electronicmusic",
        "",
        "/r/electronicmusic"
      },
      {
        "folkpunk",
        "",
        "/r/folkpunk"
      },
      {
        "idm",
        "",
        "/r/idm"
      },
      {
        "jazz",
        "",
        "/r/jazz"
      },
      {
        "jungle",
        "",
        "/r/jungle"
      },
      {
        "listentothis",
        "",
        "/r/listentothis"
      },
      {
        nameof (music),
        "",
        "/r/music"
      },
      {
        "realdubstep",
        "",
        "/r/realdubstep"
      },
      {
        "swingdancing",
        "",
        "/r/SwingDancing"
      },
      {
        "vintagednb",
        "",
        "/r/vintagednb"
      },
      {
        "wearethemusicmakers",
        "",
        "/r/WeAreTheMusicMakers"
      },
      {
        "woodshed",
        "",
        "/r/Woodshed"
      }
    };
    private string[,] photo = new string[20, 3]
    {
      {
        "abandoned",
        "",
        "/r/abandonedporn"
      },
      {
        "architecture",
        "",
        "/r/architectureporn"
      },
      {
        "aviationpics",
        "",
        "/r/aviationpics"
      },
      {
        "cinemagraphs",
        "",
        "/r/Cinemagraphs"
      },
      {
        "colorization",
        "",
        "/r/colorization"
      },
      {
        "design",
        "",
        "/r/designporn"
      },
      {
        "earth",
        "",
        "/r/earthporn"
      },
      {
        "exposure",
        "",
        "/r/exposureporn"
      },
      {
        "hdr",
        "",
        "/r/HDR"
      },
      {
        "historypron",
        "",
        "/r/HistoryPron"
      },
      {
        "itookapicture",
        "",
        "/r/itookapicture"
      },
      {
        "macro",
        "",
        "/r/macroporn"
      },
      {
        "old school cool",
        "",
        "/r/oldschoolcool"
      },
      {
        "photocritique",
        "",
        "/r/photocritique"
      },
      {
        "photography",
        "",
        "/r/photography"
      },
      {
        "photoshop",
        "",
        "/r/photoshop"
      },
      {
        "pics",
        "",
        "/r/pics"
      },
      {
        "tiltshift",
        "",
        "/r/tiltshift"
      },
      {
        "wallpapers",
        "",
        "/r/wallpapers"
      },
      {
        "windowshots",
        "",
        "/r/windowshots"
      }
    };
    private string[,] random = new string[18, 3]
    {
      {
        "asmr",
        "",
        "/r/asmr"
      },
      {
        "bestof",
        "",
        "/r/bestof"
      },
      {
        "bleach",
        "",
        "/r/bleach"
      },
      {
        "datasets",
        "",
        "/r/datasets"
      },
      {
        "freebies",
        "",
        "/r/freebies"
      },
      {
        "frisson",
        "",
        "/r/frisson"
      },
      {
        "gaben",
        "",
        "/r/gaben"
      },
      {
        "GoneNatural",
        "",
        "/r/GoneNatural"
      },
      {
        "Offbeat",
        "",
        "/r/Offbeat"
      },
      {
        "onepiece",
        "",
        "/r/onepiece"
      },
      {
        "onetruegod",
        "",
        "/r/onetruegod"
      },
      {
        "reactiongifs",
        "",
        "/r/reactiongifs"
      },
      {
        "SecretSanta",
        "",
        "/r/SecretSanta"
      },
      {
        "self",
        "",
        "/r/self"
      },
      {
        "shareastory",
        "",
        "/r/shareastory"
      },
      {
        "snackexchange",
        "",
        "/r/snackexchange"
      },
      {
        "subredditoftheday",
        "",
        "/r/subredditoftheday"
      },
      {
        "WTF",
        "",
        "/r/WTF"
      }
    };
    private string[,] storeisReading = new string[14, 3]
    {
      {
        "ask reddit",
        "",
        "/r/AskReddit"
      },
      {
        "confessions",
        "",
        "/r/confessions"
      },
      {
        "creepypasta",
        "",
        "/r/creepypasta"
      },
      {
        "does anybody else",
        "",
        "/r/DoesAnybodyElse"
      },
      {
        "explainlikeimfive",
        "",
        "/r/explainlikeimfive"
      },
      {
        "hypotheticalsituation",
        "",
        "/r/hypotheticalsituation"
      },
      {
        "iama",
        "",
        "/r/iama"
      },
      {
        "infographics",
        "",
        "/r/infographics"
      },
      {
        "nightmares",
        "",
        "/r/nightmares"
      },
      {
        "nosleep",
        "",
        "/r/nosleep"
      },
      {
        "talesfromtechsupport",
        "",
        "/r/talesfromtechsupport"
      },
      {
        "today i learned",
        "",
        "/r/TodayILearned"
      },
      {
        "truereddit",
        "",
        "/r/truereddit"
      },
      {
        "universityofreddit",
        "",
        "/r/universityofreddit"
      }
    };
    private string[,] regional = new string[16, 3]
    {
      {
        "Australia",
        "",
        "/r/Australia"
      },
      {
        "chennai",
        "",
        "/r/chennai"
      },
      {
        "denmark",
        "",
        "/r/denmark"
      },
      {
        "finland",
        "",
        "/r/finland"
      },
      {
        "iceland",
        "",
        "/r/iceland"
      },
      {
        "india",
        "",
        "/r/india"
      },
      {
        "Ireland",
        "",
        "/r/Ireland"
      },
      {
        "London",
        "",
        "/r/London"
      },
      {
        "Mexico",
        "",
        "/r/Mexico"
      },
      {
        "nordiccontr",
        "",
        "/r/nordiccontr"
      },
      {
        "norway",
        "",
        "/r/norway"
      },
      {
        "Philippines",
        "",
        "/r/Philippines"
      },
      {
        "canada",
        "",
        "/r/canada"
      },
      {
        "Singapore",
        "",
        "/r/Singapore"
      },
      {
        "sweden",
        "",
        "/r/sweden"
      },
      {
        "unitedkingdom",
        "",
        "/r/unitedkingdom"
      }
    };
    private string[,] sports = new string[13, 3]
    {
      {
        "bicycling",
        "",
        "/r/bicycling"
      },
      {
        "cricket",
        "",
        "/r/cricket"
      },
      {
        "formula1",
        "",
        "/r/formula1"
      },
      {
        "longboarding",
        "",
        "/r/longboarding"
      },
      {
        "nascar",
        "",
        "/r/nascar"
      },
      {
        "nba",
        "",
        "/r/nba"
      },
      {
        "nfl",
        "",
        "/r/nfl"
      },
      {
        "rugbyunion",
        "",
        "/r/rugbyunion"
      },
      {
        "sailing",
        "",
        "/r/sailing"
      },
      {
        "skateboarding",
        "",
        "/r/snowboarding"
      },
      {
        "soccer",
        "",
        "/r/soccer"
      },
      {
        "tna",
        "",
        "/r/tna"
      },
      {
        "wwe",
        "",
        "/r/wwe"
      }
    };
    private string[,] tech = new string[13, 3]
    {
      {
        "3d printing",
        "",
        "/r/3dprinting"
      },
      {
        "android",
        "",
        "/r/android"
      },
      {
        "apple",
        "",
        "/r/apple"
      },
      {
        "battle stations",
        "",
        "/r/battlestations"
      },
      {
        "build a pc",
        "",
        "/r/buildapc"
      },
      {
        "coding",
        "",
        "/r/coding"
      },
      {
        "geek",
        "",
        "/r/geek"
      },
      {
        "hardware",
        "",
        "/r/hardware"
      },
      {
        "linux",
        "",
        "/r/linux"
      },
      {
        "programming",
        "",
        "/r/programming"
      },
      {
        "science",
        "",
        "/r/science"
      },
      {
        "technology",
        "",
        "/r/technology"
      },
      {
        "twitter",
        "",
        "/r/twitter"
      }
    };
    private string[,] tvShows = new string[16, 3]
    {
      {
        "anime",
        "",
        "/r/anime"
      },
      {
        "arresteddevelopment",
        "",
        "/r/arresteddevelopment"
      },
      {
        "breakingbad",
        "",
        "/r/breakingbad"
      },
      {
        "community",
        "",
        "/r/community"
      },
      {
        "dexter",
        "",
        "/r/Dexter"
      },
      {
        "gameofthrones",
        "",
        "/r/gameofthrones"
      },
      {
        "himym",
        "",
        "/r/HIMYM"
      },
      {
        "IASIP",
        "",
        "/r/IASIP"
      },
      {
        "ModernFamily",
        "",
        "/r/ModernFamily"
      },
      {
        "MyLittlePony",
        "",
        "/r/MyLittlePony"
      },
      {
        "PandR",
        "",
        "/r/PandR"
      },
      {
        "Scrubs",
        "",
        "/r/Scrubs"
      },
      {
        "Sherlock",
        "",
        "/r/Sherlock"
      },
      {
        "thelastairbender",
        "",
        "/r/thelastairbender"
      },
      {
        "thewire",
        "",
        "/r/thewire"
      },
      {
        "whoselineisitanyway",
        "",
        "/r/whoselineisitanyway"
      }
    };
    internal Storyboard ShowList;
    internal Grid LayoutRoot;
    internal StackPanel TitlePanel;
    internal TextBlock ApplicationTitle;
    internal Grid ContentPanel;
    internal ListBox List;
    private bool _contentLoaded;

    public ObservableCollection<SubredditUI> CatList { get; private set; }

    public SubredditDiscoeryCategories()
    {
      this.InitializeComponent();
      this.CatList = new ObservableCollection<SubredditUI>();
      TransitionService.SetNavigationInTransition((UIElement) this, App.FeatherInTransition);
      TransitionService.SetNavigationOutTransition((UIElement) this, App.FeatherOutTransition);
      this.DataContext = (object) this;
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
      base.OnNavigatedTo(e);
      App.DataManager.BaconitAnalytics.LogPage("Subreddit Discovery");
      if (this.isLoaded)
        return;
      if (this.NavigationContext.QueryString.ContainsKey("category"))
      {
        int num;
        try
        {
          num = int.Parse(this.NavigationContext.QueryString["category"]);
        }
        catch
        {
          num = -1;
        }
        switch (num)
        {
          case 1:
            this.CurrentList = this.currentEvents;
            break;
          case 2:
            this.CurrentList = this.diycrafts;
            break;
          case 3:
            this.CurrentList = this.funny;
            break;
          case 4:
            this.CurrentList = this.sgames;
            break;
          case 5:
            this.CurrentList = this.gaming;
            break;
          case 6:
            this.CurrentList = this.history;
            break;
          case 7:
            this.CurrentList = this.lifestyle;
            break;
          case 8:
            this.CurrentList = this.microsoft;
            break;
          case 9:
            this.CurrentList = this.music;
            break;
          case 10:
            this.CurrentList = this.photo;
            break;
          case 11:
            this.CurrentList = this.random;
            break;
          case 12:
            this.CurrentList = this.storeisReading;
            break;
          case 13:
            this.CurrentList = this.regional;
            break;
          case 14:
            this.CurrentList = this.sports;
            break;
          case 15:
            this.CurrentList = this.tech;
            break;
          case 16:
            this.CurrentList = this.tvShows;
            break;
          default:
            this.CurrentList = this.CategoriesList;
            this.isCat = true;
            break;
        }
        if (App.DataManager.SettingsMan.ShowDiscoveryMessage)
        {
          App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("", false, false, "Quick Tip", "If you find a subreddit you like, you can subscribe to it by opening the subreddit and then selecting “subscribe to account” in the app bar menu."));
          App.DataManager.SettingsMan.ShowDiscoveryMessage = false;
        }
      }
      else
      {
        this.CurrentList = this.CategoriesList;
        this.isCat = true;
      }
      this.LoadList();
      this.isLoaded = true;
    }

    public void LoadList()
    {
      if (this.CurrentList == null)
        return;
      int num = this.CurrentList.Length / 3;
      if (this.isCat)
        num = this.CurrentList.Length / 2;
      for (int index = 0; index < num; ++index)
        this.CatList.Add(new SubredditUI()
        {
          SubredditTitle = this.CurrentList[index, 0].ToLower(),
          SecondLineText = this.CurrentList[index, 1]
        });
      this.ShowList.Begin();
    }

    private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (this.List.SelectedIndex == -1 || this.CurrentList == null)
        return;
      if (this.isCat)
      {
        if (this.List.SelectedIndex == 0)
          this.NavigationService.Navigate(new Uri("/AddSubreddit.xaml", UriKind.Relative));
        this.NavigationService.Navigate(new Uri("/SubredditDiscovery.xaml?category=" + (object) this.List.SelectedIndex, UriKind.Relative));
      }
      else
        this.NavigationService.Navigate(new Uri("/RedditsViewer.xaml?subredditURL=" + this.CurrentList[this.List.SelectedIndex, 2], UriKind.Relative));
      this.List.SelectedIndex = -1;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/SubredditDiscovery.xaml", UriKind.Relative));
      this.ShowList = (Storyboard) this.FindName("ShowList");
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TitlePanel = (StackPanel) this.FindName("TitlePanel");
      this.ApplicationTitle = (TextBlock) this.FindName("ApplicationTitle");
      this.ContentPanel = (Grid) this.FindName("ContentPanel");
      this.List = (ListBox) this.FindName("List");
    }
  }
}
