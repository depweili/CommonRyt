namespace Common.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t12 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ryt_FundMedicalRecord",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FundProjectID = c.Int(nullable: false),
                        MedicalRecordID = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        Order = c.Decimal(precision: 18, scale: 2),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_FundProject", t => t.FundProjectID)
                .ForeignKey("dbo.Ryt_MedicalRecord", t => t.MedicalRecordID)
                .Index(t => t.FundProjectID)
                .Index(t => t.MedicalRecordID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ryt_FundMedicalRecord", "MedicalRecordID", "dbo.Ryt_MedicalRecord");
            DropForeignKey("dbo.Ryt_FundMedicalRecord", "FundProjectID", "dbo.Ryt_FundProject");
            DropIndex("dbo.Ryt_FundMedicalRecord", new[] { "MedicalRecordID" });
            DropIndex("dbo.Ryt_FundMedicalRecord", new[] { "FundProjectID" });
            DropTable("dbo.Ryt_FundMedicalRecord");
        }
    }
}
