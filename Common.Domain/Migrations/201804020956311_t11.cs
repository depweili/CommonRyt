namespace Common.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ryt_Survey",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyUid = c.Guid(nullable: false),
                        Type = c.Int(nullable: false),
                        Title = c.String(maxLength: 100),
                        Description = c.String(maxLength: 200),
                        FrontPic = c.String(maxLength: 100),
                        BeginDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        Count = c.Int(nullable: false),
                        Order = c.Int(),
                        Memo = c.String(maxLength: 100),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ryt_SurveyQuestion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyID = c.Int(nullable: false),
                        Number = c.Int(nullable: false),
                        Title = c.String(maxLength: 200),
                        Type = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_Survey", t => t.SurveyID)
                .Index(t => t.SurveyID);
            
            CreateTable(
                "dbo.Ryt_SurveyQuestionOption",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyQuestionID = c.Int(nullable: false),
                        Value = c.String(maxLength: 5),
                        Content = c.String(maxLength: 50),
                        SelectCount = c.Int(nullable: false),
                        Order = c.Int(),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_SurveyQuestion", t => t.SurveyQuestionID)
                .Index(t => t.SurveyQuestionID);
            
            CreateTable(
                "dbo.Ryt_SurveyAnswer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        SurveyQuestionID = c.Int(nullable: false),
                        Answer = c.String(maxLength: 200),
                        Memo = c.String(maxLength: 100),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_SurveyQuestion", t => t.SurveyQuestionID)
                .ForeignKey("dbo.Ryt_User", t => t.UserID)
                .Index(t => t.UserID)
                .Index(t => t.SurveyQuestionID);
            
            AlterColumn("dbo.Ryt_FundProject", "CreateTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ryt_SurveyAnswer", "UserID", "dbo.Ryt_User");
            DropForeignKey("dbo.Ryt_SurveyAnswer", "SurveyQuestionID", "dbo.Ryt_SurveyQuestion");
            DropForeignKey("dbo.Ryt_SurveyQuestion", "SurveyID", "dbo.Ryt_Survey");
            DropForeignKey("dbo.Ryt_SurveyQuestionOption", "SurveyQuestionID", "dbo.Ryt_SurveyQuestion");
            DropIndex("dbo.Ryt_SurveyAnswer", new[] { "SurveyQuestionID" });
            DropIndex("dbo.Ryt_SurveyAnswer", new[] { "UserID" });
            DropIndex("dbo.Ryt_SurveyQuestionOption", new[] { "SurveyQuestionID" });
            DropIndex("dbo.Ryt_SurveyQuestion", new[] { "SurveyID" });
            AlterColumn("dbo.Ryt_FundProject", "CreateTime", c => c.DateTime(nullable: false));
            DropTable("dbo.Ryt_SurveyAnswer");
            DropTable("dbo.Ryt_SurveyQuestionOption");
            DropTable("dbo.Ryt_SurveyQuestion");
            DropTable("dbo.Ryt_Survey");
        }
    }
}
