// Decompiled with JetBrains decompiler
// Type: BaconitData.Libs.MainLandingImageManager
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using Baconit;
using Baconit.Database;
using BaconitData.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

#nullable disable
namespace BaconitData.Libs
{
  public class MainLandingImageManager : RedditViewerViewModelInterface
  {
    private DataManager DataMan;
    private const string ImagePath = "MainLandingImage_";
    private const int IMAGE_LIMIT = 5;
    private RunWorkerCompletedEventHandler TextCallback;

    public MainLandingImageManager(DataManager data) => this.DataMan = data;

    public string GetTopSubreddit()
    {
      if (!string.IsNullOrWhiteSpace(this.DataMan.SettingsMan.LandingStoryProvider) && !this.DataMan.SettingsMan.LandingStoryProvider.Equals("mostViewed"))
        return this.DataMan.SettingsMan.LandingStoryProvider;
      string str = (string) null;
      int num = -1;
      for (int pos = 0; pos < this.DataMan.SettingsMan.MostViewSubreddits.GetCount(); ++pos)
      {
        KeyValuePair<string, int> keyValuePair = this.DataMan.SettingsMan.MostViewSubreddits.Get(pos);
        if (keyValuePair.Value > num)
        {
          str = keyValuePair.Key;
          num = keyValuePair.Value;
        }
      }
      return string.IsNullOrWhiteSpace(str) ? "/" : str;
    }

    public BitmapImage GetSolid()
    {
      return DataManager.LIGHT_THEME ? new BitmapImage(new Uri("Images/MainPageBackgrounds/PanoramaBackground.White.jpg", UriKind.Relative)) : new BitmapImage(new Uri("Images/MainPageBackgrounds/PanoramaBackground.jpg", UriKind.Relative));
    }

    public BitmapImage GetImage()
    {
      lock (this.DataMan.FileStorage)
      {
        if (this.DataMan.SettingsMan.MainLaindingImageUsed >= this.DataMan.SettingsMan.MainLandingMaxImage)
          this.DataMan.SettingsMan.MainLaindingImageUsed = 0;
        string path = "MainLandingImage_" + (object) this.DataMan.SettingsMan.MainLaindingImageUsed + ".jpg";
        if (this.DataMan.FileStorage.FileExists(path))
        {
          using (IsolatedStorageFileStream streamSource = new IsolatedStorageFileStream(path, FileMode.Open, this.DataMan.FileStorage))
          {
            BitmapImage image = new BitmapImage();
            image.CreateOptions = BitmapCreateOptions.None;
            image.SetSource((Stream) streamSource);
            ++this.DataMan.SettingsMan.MainLaindingImageUsed;
            return image;
          }
        }
      }
      return new BitmapImage(new Uri("Images/MainPageBackgrounds/DefaultImage.jpg", UriKind.Relative));
    }

    public void CheckForUpdate(bool force)
    {
      if (Math.Abs((int) new TimeSpan(0, 0, 0, 0, (int) (BaconitStore.currentTime() - this.DataMan.BaconitStore.LastUpdatedTime("MainLandingImageUpdateTime"))).TotalMinutes) < 720 && !force)
        return;
      try
      {
        using (AutoResetEvent are = new AutoResetEvent(false))
        {
          this.DataMan.GetSubredditTopImages(this.DataMan.SettingsMan.LandingWallpaper, 5, (RunWorkerCompletedEventHandler) ((obj, e) =>
          {
            try
            {
              if (obj != null)
              {
                List<string> stringList1 = (List<string>) obj;
                List<string> stringList2 = new List<string>();
                int num = 0;
                foreach (string url in stringList1)
                {
                  string fileName = "MainLandingImage_" + (object) num + ".jpg";
                  if (this.GetImage(url, fileName))
                  {
                    stringList2.Add(fileName);
                    ++num;
                  }
                }
                if (stringList2.Count != 0)
                {
                  this.DataMan.SettingsMan.MainLandingMaxImage = stringList2.Count;
                  this.DataMan.SettingsMan.MainLaindingImageUsed = 0;
                  this.DataMan.BaconitStore.UpdateLastUpdatedTime("MainLandingImageUpdateTime");
                }
              }
            }
            catch
            {
            }
            are.Set();
          }));
          are.WaitOne();
        }
      }
      catch (Exception ex)
      {
        this.DataMan.MessageManager.DebugDia("Main Landing image", ex);
      }
    }

    private bool GetImage(string url, string fileName)
    {
      bool image = false;
      WriteableBitmap wb = (WriteableBitmap) null;
      using (AutoResetEvent are = new AutoResetEvent(false))
      {
        WebClient userToken = new WebClient();
        OpenReadCompletedEventHandler completedEventHandler = (OpenReadCompletedEventHandler) ((sender, e) =>
        {
          if (e.Error == null)
          {
            if (!e.Cancelled)
            {
              try
              {
                using (AutoResetEvent areAlso = new AutoResetEvent(false))
                {
                  Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
                  {
                    try
                    {
                      BitmapImage source = new BitmapImage();
                      source.SetSource(e.Result);
                      wb = new WriteableBitmap((BitmapSource) source);
                      source.UriSource = new Uri("/Images/cancel.png", UriKind.Relative);
                    }
                    catch
                    {
                      wb = (WriteableBitmap) null;
                    }
                    areAlso.Set();
                  }));
                  areAlso.WaitOne();
                }
              }
              catch
              {
                wb = (WriteableBitmap) null;
              }
            }
          }
          are.Set();
        });
        userToken.OpenReadCompleted += completedEventHandler;
        userToken.OpenReadAsync(new Uri(url), (object) userToken);
        are.WaitOne();
        userToken.OpenReadCompleted -= completedEventHandler;
      }
      if (wb != null)
      {
        int num1 = Math.Abs(1024 - wb.PixelWidth);
        int num2 = Math.Abs(680 - wb.PixelHeight);
        int widthScaleTo = 1024;
        int heightScaleTo = 680;
        int num3 = num2;
        if (num1 < num3)
        {
          float num4 = 1024f / (float) wb.PixelWidth;
          heightScaleTo = (int) ((double) wb.PixelHeight * (double) num4);
        }
        else
        {
          float num5 = 680f / (float) wb.PixelHeight;
          widthScaleTo = (int) ((double) wb.PixelWidth * (double) num5);
        }
        using (AutoResetEvent are = new AutoResetEvent(false))
        {
          Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
          {
            try
            {
              wb = wb.Resize(widthScaleTo, heightScaleTo, WriteableBitmapExtensions.Interpolation.Bilinear);
              wb = wb.Crop(0, 0, 1024, 680);
            }
            catch
            {
              wb = (WriteableBitmap) null;
            }
            are.Set();
          }));
          are.WaitOne();
        }
        lock (this.DataMan.FileStorage)
        {
          using (IsolatedStorageFileStream targetStream = new IsolatedStorageFileStream(fileName, FileMode.Create, this.DataMan.FileStorage))
          {
            wb.SaveJpeg((Stream) targetStream, wb.PixelWidth, wb.PixelHeight, 0, 100);
            targetStream.Close();
            image = true;
          }
        }
      }
      if (wb != null)
        wb = (WriteableBitmap) null;
      return image;
    }

    public void GetText(RunWorkerCompletedEventHandler callback)
    {
      this.TextCallback = callback;
      this.DataMan.UpdateSubRedditStories((RedditViewerViewModelInterface) this, this.GetTopSubreddit(), 0, 0, -1);
    }

    public bool GetisOpen() => true;

    public void SubRedditDataFeeder_Start(int type, int VirturalType, bool boo)
    {
    }

    public void SubRedditDataFeeder_Feed(
      SubRedditData subRedditDataObj,
      DataManager.CountHolder countHolder,
      int type,
      int virtualType,
      bool boo)
    {
      if (subRedditDataObj.SubRedditRank >= 3 || this.TextCallback == null)
        return;
      this.TextCallback((object) subRedditDataObj, new RunWorkerCompletedEventArgs((object) subRedditDataObj, (Exception) null, false));
    }

    public void SubRedditDataFeeder_Done(
      int whichType,
      int VirturalType,
      DataManager.CountHolder countHolder,
      bool FromWeb)
    {
    }

    public void SetLoadingBar(int type, bool boo)
    {
    }

    public void SetLoadingFromWeb(int type, bool SetTo)
    {
    }
  }
}
