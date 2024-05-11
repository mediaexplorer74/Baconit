// Decompiled with JetBrains decompiler
// Type: Baconit.SettingPages.Donate
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using BaconitData.Libs;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using Phone.Controls;
using System;
using System.Collections;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Windows.ApplicationModel.Store;

#nullable disable
namespace Baconit.SettingPages
{
  public class Donate : PhoneApplicationPage
  {
    private bool IngoreCommand;
    internal Grid LayoutRoot;
    internal StackPanel TitlePanel;
    internal TextBlock ApplicationTitle;
    internal TextBlock PageTitle;
    internal Grid ContentPanel;
    private bool _contentLoaded;

    public Donate()
    {
      this.InitializeComponent();
      TransitionService.SetNavigationInTransition((UIElement) this, App.FeatherInTransition);
      TransitionService.SetNavigationOutTransition((UIElement) this, App.FeatherOutTransition);
      App.DataManager.BaconitAnalytics.LogPage("Settings - Donate");
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        new MarketplaceDetailTask()
        {
          ContentIdentifier = "4a3cc839-5f75-40e5-8b5f-8d09479e7370",
          ContentType = MarketplaceContentType.Applications
        }.Show();
        App.DataManager.BaconitAnalytics.LogEvent("Donate Market Clicked");
      }
      catch
      {
      }
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
      try
      {
        PickerBoxDialog pickerBoxDialog = new PickerBoxDialog();
        pickerBoxDialog.ItemSource = (IEnumerable) new string[6]
        {
          "$0.99 Donation",
          "$1.99 Donation",
          "$2.99 Donation",
          "$4.99 Donation",
          "$9.99 Donation",
          "$14.99 Donation"
        };
        pickerBoxDialog.Title = "SELECT A DONATION";
        pickerBoxDialog.Closed += new EventHandler(this.sharePicker_closed);
        pickerBoxDialog.Show();
      }
      catch
      {
      }
    }

    private void sharePicker_closed(object sender, EventArgs e)
    {
      if (this.IngoreCommand)
        return;
      this.IngoreCommand = true;
      switch ((sender as PickerBoxDialog).SelectedIndex)
      {
        case 0:
          this.PurchaseProduct("99centDonationc");
          break;
        case 1:
          this.PurchaseProduct("1.99Donationc");
          break;
        case 2:
          this.PurchaseProduct("2.99Donationc");
          break;
        case 3:
          this.PurchaseProduct("4.99Donationc");
          break;
        case 4:
          this.PurchaseProduct("9.99Donationc");
          break;
        case 5:
          this.PurchaseProduct("14.99Donationc");
          break;
      }
    }

    private async void PurchaseProduct(string product)
    {
      int num;
      if (num != 0 && DataManager.IS_DONATE)
        product += "d";
      try
      {
        string str = await CurrentApp.RequestProductPurchaseAsync(product, false);
      }
      catch
      {
      }
      if (CurrentApp.LicenseInformation.ProductLicenses[product].IsActive)
      {
        App.DataManager.MessageManager.QueueMessage(new BaconitUserMessage("Thank you for your donation!", true, false, "", ""));
        try
        {
          if (this.NavigationService.CanGoBack)
            this.NavigationService.GoBack();
          CurrentApp.ReportProductFulfillment(product);
        }
        catch
        {
        }
      }
      this.IngoreCommand = false;
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/SettingPages/Donate.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TitlePanel = (StackPanel) this.FindName("TitlePanel");
      this.ApplicationTitle = (TextBlock) this.FindName("ApplicationTitle");
      this.PageTitle = (TextBlock) this.FindName("PageTitle");
      this.ContentPanel = (Grid) this.FindName("ContentPanel");
    }
  }
}
