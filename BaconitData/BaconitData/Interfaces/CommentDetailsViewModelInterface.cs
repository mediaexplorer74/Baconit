// Decompiled with JetBrains decompiler
// Type: BaconitData.Interfaces.CommentDetailsViewModelInterface
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using Baconit.Database;

#nullable disable
namespace BaconitData.Interfaces
{
  public interface CommentDetailsViewModelInterface
  {
    bool GetisPinned();

    bool GetisOpen();

    void SetStoryData(SubRedditData story, bool update);

    void CommentFeeder_Start(bool boo, int sort);

    void CommentFeeder_Feed(CommentData Comment, int commentFeederCount, int sort);

    void CommentFeeder_Done(int num, int sort);

    void LoadingFromWebDone(bool hasComments);

    void LoadingStoryFailed();
  }
}
