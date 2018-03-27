namespace Common.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t08 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Ryt_ImageInfo", new[] { "ImageServerInfoID" });
            AddColumn("dbo.Ryt_Conference", "City", c => c.String(maxLength: 200));
            AddColumn("dbo.Ryt_Attention", "ArticleUid", c => c.String(maxLength: 50));
            AddColumn("dbo.Ryt_MedicalRecord", "MedicalHistory", c => c.String(maxLength: 500));
            AddColumn("dbo.Ryt_MedicalRecord", "PhysicalExamination", c => c.String(maxLength: 500));
            AddColumn("dbo.Ryt_MedicalRecord", "Inspection", c => c.String(maxLength: 500));
            AddColumn("dbo.Ryt_MedicalRecord", "Diagnosis", c => c.String(maxLength: 500));
            AddColumn("dbo.Ryt_ImageInfo", "SubjectKey", c => c.Guid(nullable: false));
            AddColumn("dbo.Ryt_ImageInfo", "ImageName", c => c.String(maxLength: 100));
            AlterColumn("dbo.Ryt_Conference", "EndDate", c => c.DateTime());
            AlterColumn("dbo.Ryt_ImageInfo", "ImageServerInfoID", c => c.Int());
            AlterColumn("dbo.Ryt_ImageInfo", "ImagePath", c => c.String(maxLength: 100));
            CreateIndex("dbo.Ryt_ImageInfo", "ImageServerInfoID");
            DropColumn("dbo.Ryt_ImageServerInfo", "ServerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ryt_ImageServerInfo", "ServerId", c => c.Int(nullable: false));
            DropIndex("dbo.Ryt_ImageInfo", new[] { "ImageServerInfoID" });
            AlterColumn("dbo.Ryt_ImageInfo", "ImagePath", c => c.String(maxLength: 50));
            AlterColumn("dbo.Ryt_ImageInfo", "ImageServerInfoID", c => c.Int(nullable: false));
            AlterColumn("dbo.Ryt_Conference", "EndDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Ryt_ImageInfo", "ImageName");
            DropColumn("dbo.Ryt_ImageInfo", "SubjectKey");
            DropColumn("dbo.Ryt_MedicalRecord", "Diagnosis");
            DropColumn("dbo.Ryt_MedicalRecord", "Inspection");
            DropColumn("dbo.Ryt_MedicalRecord", "PhysicalExamination");
            DropColumn("dbo.Ryt_MedicalRecord", "MedicalHistory");
            DropColumn("dbo.Ryt_Attention", "ArticleUid");
            DropColumn("dbo.Ryt_Conference", "City");
            CreateIndex("dbo.Ryt_ImageInfo", "ImageServerInfoID");
        }
    }
}
