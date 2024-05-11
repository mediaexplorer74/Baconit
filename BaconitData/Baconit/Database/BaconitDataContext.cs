// Decompiled with JetBrains decompiler
// Type: Baconit.Database.BaconitDataContext
// Assembly: BaconitData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CACEC6D0-806E-4408-84A0-1923DE8375EA
// Assembly location: C:\Users\Admin\Desktop\RE\Baconit-3.0.1\BaconitData.dll

using System.Data.Linq;

#nullable disable
namespace Baconit.Database
{
  public class BaconitDataContext : DataContext
  {
    public const string ConnectionString = "Data Source = 'isostore:/dataEight.sdf'; Max Database Size = 256";

    public BaconitDataContext(string connectionString)
      : base(connectionString)
    {
      this.SubRedditsData = this.GetTable<SubRedditData>();
      this.Comments = this.GetTable<CommentData>();
      this.PinnedStoryCom = this.GetTable<SubRedditData>();
      this.MessageInbox = this.GetTable<Message>();
      this.LongTextData = this.GetTable<Baconit.Database.LongTextData>();
    }

    public Table<SubRedditData> SubRedditsData { get; set; }

    public Table<CommentData> Comments { get; set; }

    public Table<SubRedditData> PinnedStoryCom { get; set; }

    public Table<Message> MessageInbox { get; set; }

    public Table<Baconit.Database.LongTextData> LongTextData { get; set; }
  }
}
