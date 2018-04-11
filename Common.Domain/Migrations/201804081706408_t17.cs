namespace Common.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t17 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ryt_MedicalToolData",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MedicalToolUid = c.Guid(nullable: false),
                        Type = c.Int(nullable: false),
                        Title = c.String(maxLength: 200),
                        Content = c.String(maxLength: 500),
                        ArticleID = c.Int(),
                        Memo = c.String(maxLength: 200),
                        Order = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_Article", t => t.ArticleID)
                .Index(t => t.ArticleID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ryt_MedicalToolData", "ArticleID", "dbo.Ryt_Article");
            DropIndex("dbo.Ryt_MedicalToolData", new[] { "ArticleID" });
            DropTable("dbo.Ryt_MedicalToolData");
        }
    }
}
