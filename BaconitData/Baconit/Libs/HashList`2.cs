// Decompiled with JetBrains decompiler
// Type: Baconit.Libs.HashList`2
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using System.Collections.Generic;
using System.Runtime.Serialization;

#nullable disable
namespace Baconit.Libs
{
  [DataContract]
  public class HashList<T, S>
  {
    [DataMember]
    public Dictionary<T, S> Dictonary;
    [DataMember]
    public System.Collections.Generic.List<T> List;
    [DataMember]
    public int MaxSize;

    public HashList(int maxSize)
    {
      this.Dictonary = new Dictionary<T, S>();
      this.List = new System.Collections.Generic.List<T>();
      this.MaxSize = maxSize;
    }

    public S this[T i]
    {
      get => this.Dictonary[i];
      set => this.Add(i, value);
    }

    public void Add(T key, S value)
    {
      if (!this.Dictonary.ContainsKey(key))
      {
        this.List.Add(key);
        this.Dictonary.Add(key, value);
        while (this.List.Count > this.MaxSize)
        {
          T key1 = this.List[0];
          this.Dictonary.Remove(key1);
          this.List.Remove(key1);
        }
      }
      else
        this.Dictonary[key] = value;
    }

    public void AddIfNotAlready(T key, S value)
    {
      if (this.Dictonary.ContainsKey(key))
        return;
      this.List.Add(key);
      this.Dictonary.Add(key, value);
      while (this.List.Count > this.MaxSize)
      {
        T key1 = this.List[0];
        this.Dictonary.Remove(key1);
        this.List.Remove(key1);
      }
    }

    public int GetCount() => this.List.Count;

    public void Clear()
    {
      this.List.Clear();
      this.Dictonary.Clear();
    }

    public T GetRemoveNextKey() => this.List.Count > 0 ? this.List[0] : default (T);

    public void RemoveKey(T item)
    {
      this.Dictonary.Remove(item);
      this.List.Remove(item);
    }

    public bool ContainsKey(T item) => this.Dictonary.ContainsKey(item);

    public KeyValuePair<T, S> Get(int pos)
    {
      T key = this.List[pos];
      S s = this.Dictonary[key];
      return new KeyValuePair<T, S>(key, s);
    }
  }
}
