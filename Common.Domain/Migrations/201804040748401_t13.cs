namespace Common.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t13 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ryt_SurveyUser",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        Memo = c.String(maxLength: 100),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_Survey", t => t.SurveyID)
                .ForeignKey("dbo.Ryt_User", t => t.UserID)
                .Index(t => t.SurveyID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Ryt_SurveyAuth",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyID = c.Int(nullable: false),
                        UserID = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_Survey", t => t.SurveyID)
                .ForeignKey("dbo.Ryt_User", t => t.UserID)
                .Index(t => t.SurveyID)
                .Index(t => t.UserID);
            
            AlterColumn("dbo.Ryt_FundProject", "CreateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ryt_SurveyAuth", "UserID", "dbo.Ryt_User");
            DropForeignKey("dbo.Ryt_SurveyAuth", "SurveyID", "dbo.Ryt_Survey");
            DropForeignKey("dbo.Ryt_SurveyUser", "UserID", "dbo.Ryt_User");
            DropForeignKey("dbo.Ryt_SurveyUser", "SurveyID", "dbo.Ryt_Survey");
            DropIndex("dbo.Ryt_SurveyAuth", new[] { "UserID" });
            DropIndex("dbo.Ryt_SurveyAuth", new[] { "SurveyID" });
            DropIndex("dbo.Ryt_SurveyUser", new[] { "UserID" });
            DropIndex("dbo.Ryt_SurveyUser", new[] { "SurveyID" });
            AlterColumn("dbo.Ryt_FundProject", "CreateTime", c => c.DateTime());
            DropTable("dbo.Ryt_SurveyAuth");
            DropTable("dbo.Ryt_SurveyUser");
        }
    }
}
