// Decompiled with JetBrains decompiler
// Type: Baconit.SuperRichTextBox
// Assembly: Baconit, Version=2.6.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CE134144-F574-4C8D-A763-121793803534
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\Baconit.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Baconit
{
  public class SuperRichTextBox : UserControl
  {
    private string Text;
    private List<RichTextBox> RichBoxList;
    private List<bool> TriedToFixBox;
    internal Grid LayoutRoot;
    internal StackPanel TextHolder;
    private bool _contentLoaded;

    public SuperRichTextBox()
    {
      this.InitializeComponent();
      this.RichBoxList = new List<RichTextBox>();
      this.TriedToFixBox = new List<bool>();
    }

    public void SetText(string SetText)
    {
      this.Text = HttpUtility.HtmlDecode(SetText);
      int length = this.Text.Length;
      for (int index = 0; index < length; ++index)
      {
        if (this.Text[index] == '\n')
        {
          int num = index;
          int startIndex = index;
          while (startIndex + 1 < this.Text.Length && this.Text[startIndex + 1] == '\n')
            ++startIndex;
          if (startIndex > num + 1)
          {
            try
            {
              this.Text = this.Text.Substring(0, index + 1) + this.Text.Substring(startIndex, length - startIndex);
            }
            catch
            {
            }
            length = this.Text.Length;
          }
        }
      }
      this.TextHolder.Children.Clear();
      this.RichBoxList.Clear();
      this.TriedToFixBox.Clear();
      RichTextBox richTextBox = new RichTextBox();
      richTextBox.Margin = new Thickness(0.0);
      richTextBox.Name = this.RichBoxList.Count.ToString() + string.Empty;
      richTextBox.SizeChanged += new SizeChangedEventHandler(this.SelfText_SizeChanged);
      if (DataManager.LIGHT_THEME)
        richTextBox.Foreground = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, (byte) 0));
      App.DataManager.FormatText(richTextBox.Blocks, this.Text);
      this.TextHolder.Children.Add((UIElement) richTextBox);
      this.RichBoxList.Add(richTextBox);
      this.TriedToFixBox.Add(false);
    }

    public void ClearText()
    {
      this.TextHolder.Children.Clear();
      this.RichBoxList.Clear();
      this.TriedToFixBox.Clear();
      this.Text = "";
    }

    private void SelfText_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      int index1 = (int) short.Parse(((FrameworkElement) sender).Name);
      if (this.TriedToFixBox[index1] || e.NewSize.Height < 2048.0 || this.Text.Length <= 5)
        return;
      for (int index2 = index1; index2 < this.TriedToFixBox.Count; ++index2)
      {
        if (this.TriedToFixBox[index2])
          return;
      }
      this.TriedToFixBox[index1] = true;
      RichTextBox richTextBox = new RichTextBox();
      richTextBox.Margin = new Thickness(0.0);
      richTextBox.Name = this.RichBoxList.Count.ToString() + string.Empty;
      richTextBox.SizeChanged += new SizeChangedEventHandler(this.SelfText_SizeChanged);
      if (DataManager.LIGHT_THEME)
        richTextBox.Foreground = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 0, (byte) 0, (byte) 0));
      this.TextHolder.Children.Add((UIElement) richTextBox);
      this.RichBoxList.Add(richTextBox);
      this.TriedToFixBox.Add(false);
      int count = this.RichBoxList.Count;
      int[] numArray1 = new int[count];
      int num1 = (int) Math.Ceiling((double) this.Text.Length / (double) count);
      for (int index3 = 0; index3 < numArray1.Length; ++index3)
      {
        int startIndex = num1 * (index3 + 1);
        numArray1[index3] = startIndex < this.Text.Length ? this.Text.LastIndexOf('\n', startIndex) : this.Text.Length;
        if (numArray1[index3] == -1)
          numArray1[index3] = this.Text.LastIndexOf('.', startIndex) + 1;
        if (numArray1[index3] == -1)
          numArray1[index3] = this.Text.LastIndexOf(' ', startIndex) + 1;
        if (numArray1[index3] == -1 || index3 > 0 && numArray1[index3] <= numArray1[index3 - 1])
          numArray1[index3] = (int) ((double) this.Text.Length / (double) count * (double) index3);
      }
      int[] numArray2 = numArray1;
      numArray2[numArray2.Length - 1] = this.Text.Length;
      for (int index4 = 0; index4 < this.RichBoxList.Count; ++index4)
      {
        RichTextBox richBox = this.RichBoxList[index4];
        int startIndex = index4 != 0 ? numArray1[index4 - 1] : 0;
        int num2 = numArray1[index4];
        richBox.Blocks.Clear();
        string text = this.Text.Substring(startIndex, num2 - startIndex);
        if (text.StartsWith("\n"))
          text = text.Substring(1);
        App.DataManager.FormatText(richBox.Blocks, text);
      }
    }

    [DebuggerNonUserCode]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Baconit;component/CustomControls/SuperRichTextBox.xaml", UriKind.Relative));
      this.LayoutRoot = (Grid) this.FindName("LayoutRoot");
      this.TextHolder = (StackPanel) this.FindName("TextHolder");
    }
  }
}
