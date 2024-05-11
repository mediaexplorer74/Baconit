// Decompiled with JetBrains decompiler
// Type: BaconitData.Libs.VIPDataManager
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using Baconit;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;

#nullable disable
namespace BaconitData.Libs
{
  public class VIPDataManager
  {
    private string FILENAME = "VIPData.data";
    private Dictionary<string, object> InternalDict;
    private DataManager DataMan;

    public VIPDataManager(DataManager da) => this.DataMan = da;

    public object GetValue(string key, object defaultValue)
    {
      this.EnsureDictionary();
      lock (this.FILENAME)
        return this.InternalDict.ContainsKey(key) ? this.InternalDict[key] : defaultValue;
    }

    public void SetValue(string key, object Value)
    {
      this.EnsureDictionary();
      lock (this.FILENAME)
        this.InternalDict[key] = Value;
    }

    public bool Contains(string key)
    {
      this.EnsureDictionary();
      lock (this.FILENAME)
        return this.InternalDict.ContainsKey(key);
    }

    public void Save()
    {
      IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication();
      lock (this.FILENAME)
      {
        try
        {
          using (IsolatedStorageFileStream storageFileStream = storeForApplication.OpenFile(this.FILENAME, FileMode.Create))
            new DataContractSerializer(typeof (Dictionary<string, object>)).WriteObject((Stream) storageFileStream, (object) this.InternalDict);
        }
        catch (Exception ex)
        {
          this.DataMan.MessageManager.DebugDia("VIP SAVE FAIL", ex);
          this.DataMan.BaconitAnalytics.LogException(ex.Message + ex.StackTrace, false);
        }
      }
    }

    private void EnsureDictionary()
    {
      if (this.InternalDict != null)
        return;
      IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication();
      if (storeForApplication.FileExists(this.FILENAME))
      {
        lock (this.FILENAME)
        {
          try
          {
            using (IsolatedStorageFileStream storageFileStream = storeForApplication.OpenFile(this.FILENAME, FileMode.OpenOrCreate))
              this.InternalDict = (Dictionary<string, object>) new DataContractSerializer(typeof (Dictionary<string, object>)).ReadObject((Stream) storageFileStream);
          }
          catch (Exception ex)
          {
            this.DataMan.MessageManager.DebugDia("VIP READ FAIL", ex);
            this.DataMan.BaconitAnalytics.LogException(ex.Message + ex.StackTrace, false);
          }
        }
      }
      if (this.InternalDict != null)
        return;
      this.InternalDict = new Dictionary<string, object>();
    }
  }
}
