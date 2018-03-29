namespace Common.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t09 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ryt_MyConference",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConferenceID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_Conference", t => t.ConferenceID)
                .ForeignKey("dbo.Ryt_User", t => t.UserID)
                .Index(t => t.ConferenceID)
                .Index(t => t.UserID);
            
            AddColumn("dbo.Ryt_FundProject", "FrontPic", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ryt_MyConference", "UserID", "dbo.Ryt_User");
            DropForeignKey("dbo.Ryt_MyConference", "ConferenceID", "dbo.Ryt_Conference");
            DropIndex("dbo.Ryt_MyConference", new[] { "UserID" });
            DropIndex("dbo.Ryt_MyConference", new[] { "ConferenceID" });
            DropColumn("dbo.Ryt_FundProject", "FrontPic");
            DropTable("dbo.Ryt_MyConference");
        }
    }
}
