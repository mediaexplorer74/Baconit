// Decompiled with JetBrains decompiler
// Type: BaconitData.Libs.LogMan
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using Baconit;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace BaconitData.Libs
{
  public class LogMan
  {
    public LogMan(DataManager data)
    {
    }

    public void Info(string text)
    {
    }

    public void Warning(string text)
    {
    }

    public void Error(string text)
    {
    }

    public void Error(string text, Exception e)
    {
    }

    private void LogIt(string text, string level)
    {
    }

    public void FlushLog()
    {
    }

    public void SetMax(int max)
    {
    }

    public void RestoreLog()
    {
    }

    [DataContract]
    public class LitmitedLengthList<T>
    {
      [DataMember]
      public List<T> _list;
      [DataMember]
      public int _maxLen = 50;

      public LitmitedLengthList() => this._list = new List<T>();

      public void SetMax(int max) => this._maxLen = max;

      public void Add(T obj)
      {
        this._list.Add(obj);
        if (this._list.Count <= this._maxLen)
          return;
        this._list.RemoveAt(this._list.Count - 1);
      }

      public List<T> GetList() => this._list;
    }
  }
}
