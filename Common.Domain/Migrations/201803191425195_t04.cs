namespace Common.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t04 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ryt_Conference",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConferenceUid = c.Guid(nullable: false),
                        Title = c.String(maxLength: 100),
                        Content = c.String(maxLength: 500),
                        Country = c.String(maxLength: 200),
                        Address = c.String(maxLength: 200),
                        BeginDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Memo = c.String(maxLength: 200),
                        State = c.Int(nullable: false),
                        FrontPic = c.String(maxLength: 200),
                        ClicksCount = c.Int(nullable: false),
                        CommentsCount = c.Int(nullable: false),
                        Score = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ScoreCount = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ryt_Attention",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserID = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Uid = c.Guid(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_User", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Ryt_MedicalRecord",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MedicalRecordUid = c.Guid(nullable: false),
                        DoctorID = c.Int(nullable: false),
                        MedicineCategoryID = c.Int(nullable: false),
                        Title = c.String(maxLength: 200),
                        Content = c.String(maxLength: 500),
                        ArticleID = c.Int(),
                        FrontPic = c.String(maxLength: 200),
                        ClicksCount = c.Int(nullable: false),
                        CommentsCount = c.Int(nullable: false),
                        Score = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ScoreCount = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_Article", t => t.ArticleID)
                .ForeignKey("dbo.Ryt_Doctor", t => t.DoctorID)
                .ForeignKey("dbo.Ryt_MedicineCategory", t => t.MedicineCategoryID)
                .Index(t => t.DoctorID)
                .Index(t => t.MedicineCategoryID)
                .Index(t => t.ArticleID);
            
            CreateTable(
                "dbo.Ryt_Fund",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FundUid = c.Guid(nullable: false),
                        Name = c.String(maxLength: 200),
                        Type = c.Int(),
                        Introduction = c.String(maxLength: 500),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ryt_FundProject",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FundProjectUid = c.Guid(nullable: false),
                        FundID = c.Int(nullable: false),
                        Name = c.String(maxLength: 200),
                        Introduction = c.String(maxLength: 500),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_Fund", t => t.FundID)
                .Index(t => t.FundID);
            
            CreateTable(
                "dbo.Ryt_ImageServerInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServerId = c.Int(nullable: false),
                        ServerName = c.String(maxLength: 50),
                        ServerUrl = c.String(maxLength: 50),
                        MaxPicAmount = c.Int(nullable: false),
                        CurPicAmount = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ryt_ImageInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ImageServerInfoID = c.Int(nullable: false),
                        ImagePath = c.String(maxLength: 50),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_ImageServerInfo", t => t.ImageServerInfoID)
                .Index(t => t.ImageServerInfoID);
            
            CreateTable(
                "dbo.Ryt_Comment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SubjectKey = c.Guid(nullable: false),
                        UserID = c.Int(nullable: false),
                        Content = c.String(nullable: false, maxLength: 50),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_User", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Ryt_VideoSeries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VideoSeriesUID = c.Guid(nullable: false),
                        Title = c.String(maxLength: 50),
                        Cover = c.String(maxLength: 100),
                        Folder = c.String(maxLength: 100),
                        Introduction = c.String(maxLength: 200),
                        Total = c.Int(),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ryt_VideoInfo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VideoUID = c.Guid(nullable: false),
                        Title = c.String(maxLength: 50),
                        Introduction = c.String(maxLength: 100),
                        File = c.String(maxLength: 100),
                        Source = c.String(maxLength: 100),
                        Snapshot = c.String(maxLength: 100),
                        Length = c.Double(),
                        Order = c.Int(nullable: false),
                        VideoSeriesID = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_VideoSeries", t => t.VideoSeriesID)
                .Index(t => t.VideoSeriesID);
            
            AddColumn("dbo.Ryt_Article", "Order", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Ryt_Article", "FrontPic", c => c.String());
            AddColumn("dbo.Ryt_Article", "ClicksCount", c => c.Int(nullable: false));
            AddColumn("dbo.Ryt_Article", "CommentsCount", c => c.Int(nullable: false));
            AddColumn("dbo.Ryt_Article", "Score", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Ryt_Article", "ScoreCount", c => c.Int(nullable: false));
            AddColumn("dbo.Ryt_UserAuth", "ErrorNum", c => c.Int(nullable: false));
            AlterColumn("dbo.Ryt_Navigation", "Desc", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ryt_VideoInfo", "VideoSeriesID", "dbo.Ryt_VideoSeries");
            DropForeignKey("dbo.Ryt_Comment", "UserID", "dbo.Ryt_User");
            DropForeignKey("dbo.Ryt_ImageInfo", "ImageServerInfoID", "dbo.Ryt_ImageServerInfo");
            DropForeignKey("dbo.Ryt_FundProject", "FundID", "dbo.Ryt_Fund");
            DropForeignKey("dbo.Ryt_MedicalRecord", "MedicineCategoryID", "dbo.Ryt_MedicineCategory");
            DropForeignKey("dbo.Ryt_MedicalRecord", "DoctorID", "dbo.Ryt_Doctor");
            DropForeignKey("dbo.Ryt_MedicalRecord", "ArticleID", "dbo.Ryt_Article");
            DropForeignKey("dbo.Ryt_Attention", "UserID", "dbo.Ryt_User");
            DropIndex("dbo.Ryt_VideoInfo", new[] { "VideoSeriesID" });
            DropIndex("dbo.Ryt_Comment", new[] { "UserID" });
            DropIndex("dbo.Ryt_ImageInfo", new[] { "ImageServerInfoID" });
            DropIndex("dbo.Ryt_FundProject", new[] { "FundID" });
            DropIndex("dbo.Ryt_MedicalRecord", new[] { "ArticleID" });
            DropIndex("dbo.Ryt_MedicalRecord", new[] { "MedicineCategoryID" });
            DropIndex("dbo.Ryt_MedicalRecord", new[] { "DoctorID" });
            DropIndex("dbo.Ryt_Attention", new[] { "UserID" });
            AlterColumn("dbo.Ryt_Navigation", "Desc", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Ryt_UserAuth", "ErrorNum");
            DropColumn("dbo.Ryt_Article", "ScoreCount");
            DropColumn("dbo.Ryt_Article", "Score");
            DropColumn("dbo.Ryt_Article", "CommentsCount");
            DropColumn("dbo.Ryt_Article", "ClicksCount");
            DropColumn("dbo.Ryt_Article", "FrontPic");
            DropColumn("dbo.Ryt_Article", "Order");
            DropTable("dbo.Ryt_VideoInfo");
            DropTable("dbo.Ryt_VideoSeries");
            DropTable("dbo.Ryt_Comment");
            DropTable("dbo.Ryt_ImageInfo");
            DropTable("dbo.Ryt_ImageServerInfo");
            DropTable("dbo.Ryt_FundProject");
            DropTable("dbo.Ryt_Fund");
            DropTable("dbo.Ryt_MedicalRecord");
            DropTable("dbo.Ryt_Attention");
            DropTable("dbo.Ryt_Conference");
        }
    }
}
