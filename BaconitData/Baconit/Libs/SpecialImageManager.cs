// Decompiled with JetBrains decompiler
// Type: Baconit.Libs.SpecialImageManager
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using Baconit.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;
using Windows.Networking.Connectivity;
using Windows.Phone.System.UserProfile;

#nullable disable
namespace Baconit.Libs
{
  public class SpecialImageManager
  {
    private DataManager DataMan;
    private const int IANA_INTERFACE_TYPE_WIFI = 71;

    public SpecialImageManager(DataManager data) => this.DataMan = data;

    public void CheckForUpdate()
    {
      if (!this.DataMan.SettingsMan.UseLockScreenImage || !LockScreenManager.IsProvidedByCurrentApplication || this.UpdateLockSreenWallpapersList(false))
        return;
      this.UpdateLockScreenWallpaper(false);
    }

    public bool UpdateLockSreenWallpapersList(bool overRide)
    {
      if (!this.DataMan.SettingsMan.UseLockScreenImage || !LockScreenManager.IsProvidedByCurrentApplication)
        return false;
      int num1 = Math.Abs((int) new TimeSpan(0, 0, 0, 0, (int) (BaconitStore.currentTime() - this.DataMan.BaconitStore.LastUpdatedTime("LockScreenLastUpdateInternetMS"))).TotalMinutes);
      if (num1 < this.DataMan.SettingsMan.LockScreenUpdateTime * 5 && num1 < 1320 && !overRide)
        return false;
      try
      {
        if (NetworkInformation.GetInternetConnectionProfile().NetworkAdapter.IanaInterfaceType != 71U)
        {
          if (!overRide)
          {
            if (this.DataMan.SettingsMan.OnlyUpdateWifiLockScreenImages)
              return false;
          }
        }
      }
      catch
      {
      }
      string redditUrl = this.DataMan.SettingsMan.LockScreenSubredditURL;
      if (redditUrl == null || redditUrl.Equals(""))
        redditUrl = "/";
      using (AutoResetEvent are = new AutoResetEvent(false))
      {
        this.DataMan.GetSubredditTopImages(redditUrl, 7, (RunWorkerCompletedEventHandler) ((obj, e) =>
        {
          try
          {
            if (obj != null)
            {
              List<string> stringList1 = (List<string>) obj;
              List<string> stringList2 = new List<string>();
              int num2 = 0;
              foreach (string ImageURL in stringList1)
              {
                string singleImage = this.GetSingleImage(ImageURL, "LockScreen_Wallpaper_" + (object) num2, true);
                if (singleImage != null && !singleImage.Equals(""))
                {
                  stringList2.Add(singleImage);
                  ++num2;
                }
              }
              if (stringList2.Count != 0)
              {
                this.DataMan.SettingsMan.LockScreenImageFiles = stringList2;
                this.DataMan.SettingsMan.SaveLockScreenImageFiles();
                this.DataMan.SettingsMan.LockScreenLastUsedImage = 0;
                this.DataMan.BaconitStore.UpdateLastUpdatedTime("LockScreenLastUpdateInternetMS");
                this.UpdateLockScreenWallpaper(true);
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
      return true;
    }

    public bool UpdateLockScreenWallpaper(bool overWrite)
    {
      if (Math.Abs((int) new TimeSpan(0, 0, 0, 0, (int) (BaconitStore.currentTime() - this.DataMan.BaconitStore.LastUpdatedTime("LockScreenLastUpdateImageMS"))).TotalMinutes) < this.DataMan.SettingsMan.LockScreenUpdateTime && !overWrite || !this.DataMan.SettingsMan.UseLockScreenImage || !LockScreenManager.IsProvidedByCurrentApplication)
        return false;
      List<string> screenImageFiles = this.DataMan.SettingsMan.LockScreenImageFiles;
      if (screenImageFiles == null || screenImageFiles.Count == 0)
        return false;
      if (this.DataMan.SettingsMan.LockScreenLastUsedImage >= screenImageFiles.Count)
        this.DataMan.SettingsMan.LockScreenLastUsedImage = 0;
      string str = screenImageFiles[this.DataMan.SettingsMan.LockScreenLastUsedImage];
      ++this.DataMan.SettingsMan.LockScreenLastUsedImage;
      try
      {
        LockScreen.SetImageUri(new Uri("ms-appdata:///Local" + str, UriKind.Absolute));
        this.DataMan.BaconitStore.UpdateLastUpdatedTime("LockScreenLastUpdateImageMS");
      }
      catch (Exception ex)
      {
        this.DataMan.DebugDia("Couldn't update wallpaper", ex);
        ++this.DataMan.SettingsMan.LockScreenLastUsedImage;
        return false;
      }
      return true;
    }

    public string GetSingleImage(string ImageURL, string filePath, bool resizeToScreen)
    {
      string FileName = "\\Shared\\ShellContent\\" + filePath + "_1.png";
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
                WriteableBitmap wb = (WriteableBitmap) null;
                using (AutoResetEvent areAlso = new AutoResetEvent(false))
                {
                  Deployment.Current.Dispatcher.BeginInvoke((Action) (() =>
                  {
                    try
                    {
                      double num4 = 1280.0;
                      double num5 = 768.0;
                      double num6 = 1.0;
                      bool flag = false;
                      if (this.DataMan.SettingsMan.ScaleFactor == 100)
                      {
                        num4 = 800.0;
                        num5 = 480.0;
                      }
                      else if (this.DataMan.SettingsMan.ScaleFactor == 160)
                      {
                        num4 = 1280.0;
                        num5 = 768.0;
                      }
                      else if (this.DataMan.SettingsMan.ScaleFactor == 150)
                      {
                        num4 = 1280.0;
                        num5 = 720.0;
                      }
                      BitmapImage source = new BitmapImage();
                      source.SetSource(e.Result);
                      wb = new WriteableBitmap((BitmapSource) source);
                      source.UriSource = new Uri("/Images/cancel.png", UriKind.Relative);
                      int pixelWidth = wb.PixelWidth;
                      int pixelHeight = wb.PixelHeight;
                      if (pixelWidth > pixelHeight)
                      {
                        if ((double) pixelHeight > num4)
                        {
                          flag = true;
                          num6 = num4 / (double) pixelHeight;
                        }
                      }
                      else if ((double) pixelWidth > num5)
                      {
                        flag = true;
                        num6 = num5 / (double) pixelWidth;
                      }
                      if (flag)
                      {
                        int height = (int) ((double) pixelHeight * num6);
                        wb = wb.Resize((int) ((double) pixelWidth * num6), height, WriteableBitmapExtensions.Interpolation.Bilinear);
                      }
                    }
                    catch
                    {
                      wb = (WriteableBitmap) null;
                    }
                    areAlso.Set();
                  }));
                  areAlso.WaitOne();
                }
                if (wb != null)
                {
                  try
                  {
                    lock (this.DataMan.FileStorage)
                    {
                      using (IsolatedStorageFileStream targetStream = new IsolatedStorageFileStream(FileName, FileMode.Create, this.DataMan.FileStorage))
                      {
                        wb.SaveJpeg((Stream) targetStream, wb.PixelWidth, wb.PixelHeight, 0, 100);
                        targetStream.Close();
                      }
                    }
                  }
                  catch (Exception ex)
                  {
                    if (this.DataMan.MessageManager != null)
                      this.DataMan.DebugDia("Failed to write image to file", ex);
                    FileName = "";
                  }
                }
                else
                  FileName = "";
                if (wb != null)
                {
                  wb.GetBitmapContext().Dispose();
                  wb = (WriteableBitmap) null;
                  goto label_29;
                }
                else
                  goto label_29;
              }
              catch (Exception ex)
              {
                if (this.DataMan.MessageManager != null)
                  this.DataMan.DebugDia("Error in request web image", ex);
                FileName = "";
                goto label_29;
              }
            }
          }
          if (this.DataMan.MessageManager != null)
            this.DataMan.DebugDia("Error in request web image", (Exception) null);
          FileName = "";
label_29:
          are.Set();
        });
        userToken.OpenReadCompleted += completedEventHandler;
        userToken.OpenReadAsync(new Uri(ImageURL), (object) userToken);
        are.WaitOne();
        userToken.OpenReadCompleted -= completedEventHandler;
      }
      return FileName;
    }
  }
}
