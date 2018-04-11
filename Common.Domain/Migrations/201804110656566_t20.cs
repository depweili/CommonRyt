namespace Common.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t20 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Ryt_ReadPatientRecord", new[] { "PatientMedicalRecordID" });
            DropIndex("dbo.Ryt_ReadPatientRecord", new[] { "DoctorID" });
            CreateTable(
                "dbo.Ryt_Assistant",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AssistantUid = c.Guid(nullable: false),
                        Name = c.String(maxLength: 100),
                        AvatarUrl = c.String(maxLength: 200),
                        MobilePhone = c.String(maxLength: 50),
                        QQ = c.String(maxLength: 50),
                        WeChat = c.String(maxLength: 50),
                        Level = c.Int(nullable: false),
                        InvitationCode = c.String(maxLength: 50),
                        ICodeCreateTime = c.DateTime(),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ryt_AssistantManager",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AssistantID = c.Int(nullable: false),
                        HospitalID = c.Int(),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_Assistant", t => t.AssistantID)
                .ForeignKey("dbo.Ryt_Hospital", t => t.HospitalID)
                .Index(t => t.AssistantID)
                .Index(t => t.HospitalID);
            
            CreateTable(
                "dbo.Ryt_AssistantDoctor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AssistantID = c.Int(nullable: false),
                        DoctorID = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_Assistant", t => t.AssistantID)
                .ForeignKey("dbo.Ryt_Doctor", t => t.DoctorID)
                .Index(t => t.AssistantID)
                .Index(t => t.DoctorID);
            
            AddColumn("dbo.Ryt_ReadPatientRecord", "Memo", c => c.String(maxLength: 200));
            AlterColumn("dbo.Ryt_ReadPatientRecord", "PatientMedicalRecordID", c => c.Int(nullable: false));
            AlterColumn("dbo.Ryt_ReadPatientRecord", "DoctorID", c => c.Int(nullable: false));
            AlterColumn("dbo.Ryt_ReadPatientRecord", "Diagnostic", c => c.String(maxLength: 500));
            CreateIndex("dbo.Ryt_ReadPatientRecord", "PatientMedicalRecordID");
            CreateIndex("dbo.Ryt_ReadPatientRecord", "DoctorID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ryt_AssistantDoctor", "DoctorID", "dbo.Ryt_Doctor");
            DropForeignKey("dbo.Ryt_AssistantDoctor", "AssistantID", "dbo.Ryt_Assistant");
            DropForeignKey("dbo.Ryt_AssistantManager", "HospitalID", "dbo.Ryt_Hospital");
            DropForeignKey("dbo.Ryt_AssistantManager", "AssistantID", "dbo.Ryt_Assistant");
            DropIndex("dbo.Ryt_ReadPatientRecord", new[] { "DoctorID" });
            DropIndex("dbo.Ryt_ReadPatientRecord", new[] { "PatientMedicalRecordID" });
            DropIndex("dbo.Ryt_AssistantDoctor", new[] { "DoctorID" });
            DropIndex("dbo.Ryt_AssistantDoctor", new[] { "AssistantID" });
            DropIndex("dbo.Ryt_AssistantManager", new[] { "HospitalID" });
            DropIndex("dbo.Ryt_AssistantManager", new[] { "AssistantID" });
            AlterColumn("dbo.Ryt_ReadPatientRecord", "Diagnostic", c => c.String(maxLength: 1000));
            AlterColumn("dbo.Ryt_ReadPatientRecord", "DoctorID", c => c.Int());
            AlterColumn("dbo.Ryt_ReadPatientRecord", "PatientMedicalRecordID", c => c.Int());
            DropColumn("dbo.Ryt_ReadPatientRecord", "Memo");
            DropTable("dbo.Ryt_AssistantDoctor");
            DropTable("dbo.Ryt_AssistantManager");
            DropTable("dbo.Ryt_Assistant");
            CreateIndex("dbo.Ryt_ReadPatientRecord", "DoctorID");
            CreateIndex("dbo.Ryt_ReadPatientRecord", "PatientMedicalRecordID");
        }
    }
}
