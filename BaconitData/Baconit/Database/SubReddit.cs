// Decompiled with JetBrains decompiler
// Type: Baconit.Database.SubReddit
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

#nullable disable
namespace Baconit.Database
{
  public class SubReddit
  {
    public string RName { get; set; }

    public string DisplayName { get; set; }

    public float created { get; set; }

    public string URL { get; set; }

    public string Title { get; set; }

    public string PublicDescription { get; set; }

    public int Subscribers { get; set; }

    public string Description { get; set; }

    public bool isLocal { get; set; }

    public bool isAccount { get; set; }

    public bool isBuiltIn { get; set; }

    public bool isGroup { get; set; }
  }
}
