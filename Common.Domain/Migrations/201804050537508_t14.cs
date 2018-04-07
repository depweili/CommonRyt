namespace Common.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ryt_Conference", "ArticleID", c => c.Int());
            AddColumn("dbo.Ryt_Fund", "ArticleID", c => c.Int());
            AddColumn("dbo.Ryt_FundProject", "ArticleID", c => c.Int());
            CreateIndex("dbo.Ryt_Conference", "ArticleID");
            CreateIndex("dbo.Ryt_Fund", "ArticleID");
            CreateIndex("dbo.Ryt_FundProject", "ArticleID");
            AddForeignKey("dbo.Ryt_Conference", "ArticleID", "dbo.Ryt_Article", "Id");
            AddForeignKey("dbo.Ryt_Fund", "ArticleID", "dbo.Ryt_Article", "Id");
            AddForeignKey("dbo.Ryt_FundProject", "ArticleID", "dbo.Ryt_Article", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ryt_FundProject", "ArticleID", "dbo.Ryt_Article");
            DropForeignKey("dbo.Ryt_Fund", "ArticleID", "dbo.Ryt_Article");
            DropForeignKey("dbo.Ryt_Conference", "ArticleID", "dbo.Ryt_Article");
            DropIndex("dbo.Ryt_FundProject", new[] { "ArticleID" });
            DropIndex("dbo.Ryt_Fund", new[] { "ArticleID" });
            DropIndex("dbo.Ryt_Conference", new[] { "ArticleID" });
            DropColumn("dbo.Ryt_FundProject", "ArticleID");
            DropColumn("dbo.Ryt_Fund", "ArticleID");
            DropColumn("dbo.Ryt_Conference", "ArticleID");
        }
    }
}
