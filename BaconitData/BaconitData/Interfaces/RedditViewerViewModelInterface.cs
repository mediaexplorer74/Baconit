// Decompiled with JetBrains decompiler
// Type: BaconitData.Interfaces.RedditViewerViewModelInterface
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using Baconit;
using Baconit.Database;

#nullable disable
namespace BaconitData.Interfaces
{
  public interface RedditViewerViewModelInterface
  {
    bool GetisOpen();

    void SubRedditDataFeeder_Start(int type, int VirturalType, bool boo);

    void SubRedditDataFeeder_Feed(
      SubRedditData subRedditDataObj,
      DataManager.CountHolder countHolder,
      int type,
      int virtualType,
      bool boo);

    void SubRedditDataFeeder_Done(
      int whichType,
      int VirturalType,
      DataManager.CountHolder countHolder,
      bool FromWeb);

    void SetLoadingBar(int type, bool boo);

    void SetLoadingFromWeb(int type, bool SetTo);
  }
}
