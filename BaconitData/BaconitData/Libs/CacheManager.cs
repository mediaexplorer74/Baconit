// Decompiled with JetBrains decompiler
// Type: BaconitData.Libs.CacheManager
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;

#nullable disable
namespace BaconitData.Libs
{
  public class CacheManager
  {
    public string GetCacheReport()
    {
      try
      {
        List<string> list = new List<string>();
        this.GetFileList("", IsolatedStorageFile.GetUserStoreForApplication(), list);
        string cacheReport = "";
        foreach (string str in list)
          cacheReport = cacheReport + "\n" + str;
        return cacheReport;
      }
      catch
      {
        return "";
      }
    }

    public void DeleteCache()
    {
      try
      {
        List<string> list = new List<string>();
        string currentDir = "";
        IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication();
        this.GetFileList(currentDir, storeForApplication, list);
        foreach (string file in list)
        {
          if (file.Contains("\\"))
          {
            if (!file.EndsWith("\\"))
            {
              try
              {
                storeForApplication.DeleteFile(file);
              }
              catch
              {
              }
            }
          }
        }
      }
      catch
      {
      }
    }

    private void GetFileList(string currentDir, IsolatedStorageFile storage, List<string> list)
    {
      list.Add(currentDir);
      string searchPattern = currentDir + "*";
      foreach (string fileName in storage.GetFileNames(searchPattern))
      {
        FileInfo fileInfo = new FileInfo(fileName);
        if (fileInfo.Exists)
          list.Add(currentDir + fileName + " " + (object) fileInfo.Length + " Bytes");
        else
          list.Add(currentDir + fileName);
      }
      foreach (string directoryName in storage.GetDirectoryNames(searchPattern))
        this.GetFileList(currentDir + directoryName + "\\", storage, list);
    }
  }
}
