namespace Common.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t01 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ryt_CharityDrugApplication",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CharityDrugUid = c.Guid(nullable: false),
                        PatientID = c.Int(),
                        PatientName = c.String(maxLength: 100),
                        Gender = c.Int(),
                        BirthDay = c.DateTime(),
                        MobilePhone = c.String(maxLength: 100),
                        IDCard = c.String(maxLength: 100),
                        Area = c.String(maxLength: 100),
                        Address = c.String(maxLength: 100),
                        EmergencyContact = c.String(maxLength: 100),
                        EmergencyPhone = c.String(maxLength: 100),
                        MedicalRecordNo = c.String(maxLength: 100),
                        Chemotherapy = c.String(maxLength: 100),
                        DrugName = c.String(maxLength: 100),
                        BeginDrugTime = c.DateTime(),
                        ProjectDoctorID = c.Int(),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_Patient", t => t.PatientID)
                .ForeignKey("dbo.Ryt_Doctor", t => t.ProjectDoctorID)
                .Index(t => t.PatientID)
                .Index(t => t.ProjectDoctorID);
            
            AddColumn("dbo.Ryt_UserProfile", "BirthDay", c => c.DateTime());
            AddColumn("dbo.Ryt_UserProfile", "Area", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ryt_CharityDrugApplication", "ProjectDoctorID", "dbo.Ryt_Doctor");
            DropForeignKey("dbo.Ryt_CharityDrugApplication", "PatientID", "dbo.Ryt_Patient");
            DropIndex("dbo.Ryt_CharityDrugApplication", new[] { "ProjectDoctorID" });
            DropIndex("dbo.Ryt_CharityDrugApplication", new[] { "PatientID" });
            DropColumn("dbo.Ryt_UserProfile", "Area");
            DropColumn("dbo.Ryt_UserProfile", "BirthDay");
            DropTable("dbo.Ryt_CharityDrugApplication");
        }
    }
}
