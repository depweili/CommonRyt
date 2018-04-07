namespace Common.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t15 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Ryt_SurveyUser", new[] { "SurveyID" });
            DropIndex("dbo.Ryt_SurveyUser", new[] { "UserID" });
            DropIndex("dbo.Ryt_SurveyAuth", new[] { "SurveyID" });
            DropIndex("dbo.Ryt_SurveyAuth", new[] { "UserID" });
            AddColumn("dbo.Ryt_Doctor", "InvitationCode", c => c.String(maxLength: 20));
            AddColumn("dbo.Ryt_Doctor", "CertificatePicZy", c => c.String(maxLength: 200));
            AddColumn("dbo.Ryt_Doctor", "CertificatePicZc", c => c.String(maxLength: 200));
            AddColumn("dbo.Ryt_ImageInfo", "UploadUserUid", c => c.Guid(nullable: false));
            CreateIndex("dbo.Ryt_SurveyUser", new[] { "SurveyID", "UserID" }, unique: true, name: "UKSurveyUser");
            CreateIndex("dbo.Ryt_SurveyAuth", new[] { "SurveyID", "UserID" }, unique: true, name: "UKSurveyAuth");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Ryt_SurveyAuth", "UKSurveyAuth");
            DropIndex("dbo.Ryt_SurveyUser", "UKSurveyUser");
            DropColumn("dbo.Ryt_ImageInfo", "UploadUserUid");
            DropColumn("dbo.Ryt_Doctor", "CertificatePicZc");
            DropColumn("dbo.Ryt_Doctor", "CertificatePicZy");
            DropColumn("dbo.Ryt_Doctor", "InvitationCode");
            CreateIndex("dbo.Ryt_SurveyAuth", "UserID");
            CreateIndex("dbo.Ryt_SurveyAuth", "SurveyID");
            CreateIndex("dbo.Ryt_SurveyUser", "UserID");
            CreateIndex("dbo.Ryt_SurveyUser", "SurveyID");
        }
    }
}
