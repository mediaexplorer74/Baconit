// Decompiled with JetBrains decompiler
// Type: Baconit.WordCompletions
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using Baconit.Database;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Baconit
{
  public class WordCompletions : IEnumerable
  {
    private int count;
    public IEnumerable AutoCompletions = (IEnumerable) new List<string>();

    public WordCompletions()
    {
      List<string> stringList = new List<string>();
      List<SubReddit> subSortedReddits = App.DataManager.SubredditDataManager.GetSubSortedReddits();
      this.count = subSortedReddits.Count;
      foreach (SubReddit subReddit in subSortedReddits)
      {
        if (subReddit.isAccount || subReddit.isLocal)
          stringList.Add(subReddit.DisplayName);
      }
      this.AutoCompletions = (IEnumerable) stringList;
    }

    public IEnumerator GetEnumerator() => this.AutoCompletions.GetEnumerator();
  }
}
