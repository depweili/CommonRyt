namespace Common.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ryt_Hospital",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        FullName = c.String(maxLength: 50),
                        Level = c.Int(nullable: false),
                        Introduction = c.String(maxLength: 200),
                        Address = c.String(maxLength: 100),
                        AreaID = c.Int(),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_BaseArea", t => t.AreaID)
                .Index(t => t.AreaID);
            
            CreateTable(
                "dbo.Ryt_BaseArea",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Pid = c.Int(),
                        Level = c.Int(),
                        Type = c.Int(),
                        Name = c.String(maxLength: 50),
                        Area = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ryt_MedicineDepartment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MedicineCategoryID = c.Int(nullable: false),
                        HospitalID = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_Hospital", t => t.HospitalID)
                .ForeignKey("dbo.Ryt_MedicineCategory", t => t.MedicineCategoryID)
                .Index(t => t.MedicineCategoryID)
                .Index(t => t.HospitalID);
            
            CreateTable(
                "dbo.Ryt_Doctor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uid = c.Guid(nullable: false),
                        MedicineDepartmentID = c.Int(),
                        DepartmentAlias = c.String(maxLength: 100),
                        Name = c.String(maxLength: 20),
                        Code = c.String(maxLength: 20),
                        Avatar = c.String(maxLength: 200),
                        Gender = c.Int(),
                        Certificate = c.String(maxLength: 50),
                        CertificatePhoto = c.Binary(storeType: "image"),
                        Title = c.String(maxLength: 20),
                        EduTitle = c.String(maxLength: 20),
                        Level = c.String(maxLength: 20),
                        Tag = c.String(maxLength: 20),
                        Expert = c.String(maxLength: 200),
                        Academic = c.String(maxLength: 200),
                        Introduction = c.String(maxLength: 500),
                        OpenMessage = c.String(maxLength: 20),
                        OutpatientSchedule = c.String(maxLength: 20),
                        IsVerified = c.Boolean(nullable: false),
                        Score = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_MedicineDepartment", t => t.MedicineDepartmentID)
                .ForeignKey("dbo.Ryt_User", t => t.User_Id)
                .Index(t => t.MedicineDepartmentID)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Ryt_User",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 50),
                        Password = c.String(maxLength: 50),
                        AuthID = c.Guid(nullable: false),
                        LastActiveTime = c.DateTime(),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ryt_UserIntegral",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        IntegralID = c.Guid(nullable: false),
                        TotalPoints = c.Int(nullable: false),
                        CurrentPoints = c.Int(nullable: false),
                        TotalExpense = c.Int(nullable: false),
                        IntegralGradeID = c.Int(),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_IntegralGrade", t => t.IntegralGradeID)
                .ForeignKey("dbo.Ryt_User", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.IntegralGradeID);
            
            CreateTable(
                "dbo.Ryt_IntegralGrade",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Grade = c.Int(nullable: false),
                        Title = c.String(maxLength: 50),
                        Icon = c.String(maxLength: 50),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ryt_IntegralRecord",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ShortMark = c.String(maxLength: 20),
                        Points = c.Int(nullable: false),
                        TotalPoints = c.Int(nullable: false),
                        CurrentPoints = c.Int(nullable: false),
                        ValidPoints = c.Int(nullable: false),
                        LockPoints = c.Int(nullable: false),
                        ExpensePoints = c.Int(nullable: false),
                        ExpiredPoints = c.Int(nullable: false),
                        Memo = c.String(maxLength: 200),
                        ExpiredTime = c.DateTime(),
                        RecordTime = c.DateTime(nullable: false),
                        IntegralActivityID = c.Int(),
                        UserIntegralID = c.Int(),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_IntegralActivity", t => t.IntegralActivityID)
                .ForeignKey("dbo.Ryt_UserIntegral", t => t.UserIntegralID)
                .Index(t => t.IntegralActivityID)
                .Index(t => t.UserIntegralID);
            
            CreateTable(
                "dbo.Ryt_IntegralActivity",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Code = c.String(maxLength: 30),
                        Name = c.String(maxLength: 50),
                        Description = c.String(maxLength: 200),
                        MinGrade = c.Int(),
                        MaxGrade = c.Int(),
                        ArticleID = c.Int(),
                        BeginTime = c.DateTime(),
                        EndTime = c.DateTime(),
                        IsOpen = c.Boolean(),
                        IntegralRuleID = c.Int(),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_Article", t => t.ArticleID)
                .ForeignKey("dbo.Ryt_IntegralRule", t => t.IntegralRuleID)
                .Index(t => t.ArticleID)
                .Index(t => t.IntegralRuleID);
            
            CreateTable(
                "dbo.Ryt_Article",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArticleUID = c.Guid(nullable: false),
                        Code = c.String(maxLength: 20),
                        Type = c.Int(nullable: false),
                        Author = c.String(maxLength: 20),
                        Title = c.String(maxLength: 200),
                        Content = c.String(storeType: "ntext"),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ryt_IntegralRule",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Description = c.String(maxLength: 500),
                        ArticleID = c.Int(),
                        Points = c.Int(),
                        Formula = c.String(maxLength: 50),
                        StepPoints = c.Int(),
                        StepInterval = c.String(maxLength: 20),
                        CycleType = c.String(maxLength: 20),
                        MaxPoints = c.Int(),
                        MaxTotalPoints = c.Int(),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_Article", t => t.ArticleID)
                .Index(t => t.ArticleID);
            
            CreateTable(
                "dbo.Ryt_IntegralUserActivity",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Memo = c.String(maxLength: 50),
                        TotalPoints = c.Int(nullable: false),
                        UserIntegralID = c.Int(nullable: false),
                        IntegralActivityID = c.Int(nullable: false),
                        State = c.Int(nullable: false),
                        CompleteTime = c.DateTime(),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_IntegralActivity", t => t.IntegralActivityID)
                .ForeignKey("dbo.Ryt_UserIntegral", t => t.UserIntegralID)
                .Index(t => t.UserIntegralID)
                .Index(t => t.IntegralActivityID);
            
            CreateTable(
                "dbo.Ryt_UserProfile",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        NickName = c.String(maxLength: 50),
                        AvatarUrl = c.String(maxLength: 200),
                        RealName = c.String(maxLength: 50),
                        Gender = c.Int(),
                        Age = c.Int(),
                        Email = c.String(maxLength: 50),
                        Address = c.String(maxLength: 50),
                        MobilePhone = c.String(maxLength: 50),
                        IDCard = c.String(maxLength: 50),
                        IsVerified = c.Boolean(),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_User", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Ryt_MedicineCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                        Order = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ryt_Navigation",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Desc = c.String(nullable: false, maxLength: 50),
                        Pic = c.String(maxLength: 50),
                        Target = c.String(maxLength: 100),
                        Order = c.Int(nullable: false),
                        ArticleID = c.Int(),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_Article", t => t.ArticleID)
                .Index(t => t.ArticleID);
            
            CreateTable(
                "dbo.Ryt_IntegralSignIn",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastTime = c.DateTime(),
                        DurationDays = c.Int(nullable: false),
                        UserIntegralID = c.Int(),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_UserIntegral", t => t.UserIntegralID)
                .Index(t => t.UserIntegralID);
            
            CreateTable(
                "dbo.Ryt_Patient",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Uid = c.Guid(nullable: false),
                        Name = c.String(maxLength: 50),
                        Gender = c.String(),
                        Birthday = c.DateTime(),
                        Telephone = c.String(maxLength: 30),
                        AreaInfo = c.String(maxLength: 50),
                        Address = c.String(maxLength: 50),
                        Height = c.Int(nullable: false),
                        Weight = c.Int(nullable: false),
                        Health = c.String(),
                        UserID = c.Int(),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_User", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Ryt_PatientDoctor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PatientID = c.Int(),
                        DoctorID = c.Int(),
                        NewRecord = c.Int(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        PatientOrder = c.Int(),
                        DoctorOrder = c.Int(),
                        State = c.Int(),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_Doctor", t => t.DoctorID)
                .ForeignKey("dbo.Ryt_Patient", t => t.PatientID)
                .Index(t => t.PatientID)
                .Index(t => t.DoctorID);
            
            CreateTable(
                "dbo.Ryt_PatientMedicalRecord",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PatientID = c.Int(),
                        Content = c.String(maxLength: 50),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_Patient", t => t.PatientID)
                .Index(t => t.PatientID);
            
            CreateTable(
                "dbo.Ryt_ReadPatientRecord",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PatientMedicalRecordID = c.Int(),
                        DoctorID = c.Int(),
                        Diagnostic = c.String(maxLength: 50),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_Doctor", t => t.DoctorID)
                .ForeignKey("dbo.Ryt_PatientMedicalRecord", t => t.PatientMedicalRecordID)
                .Index(t => t.PatientMedicalRecordID)
                .Index(t => t.DoctorID);
            
            CreateTable(
                "dbo.Ryt_UserAuth",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdentityType = c.String(maxLength: 50),
                        Identifier = c.String(maxLength: 50),
                        Credential = c.String(maxLength: 100),
                        LastActiveTime = c.DateTime(),
                        CreateTime = c.DateTime(nullable: false),
                        IsValid = c.Boolean(),
                        IsDeleted = c.Boolean(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ryt_User", t => t.User_Id)
                .Index(t => new { t.IdentityType, t.Identifier }, unique: true, name: "UserAuthIdentity")
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ryt_UserAuth", "User_Id", "dbo.Ryt_User");
            DropForeignKey("dbo.Ryt_ReadPatientRecord", "PatientMedicalRecordID", "dbo.Ryt_PatientMedicalRecord");
            DropForeignKey("dbo.Ryt_ReadPatientRecord", "DoctorID", "dbo.Ryt_Doctor");
            DropForeignKey("dbo.Ryt_PatientMedicalRecord", "PatientID", "dbo.Ryt_Patient");
            DropForeignKey("dbo.Ryt_PatientDoctor", "PatientID", "dbo.Ryt_Patient");
            DropForeignKey("dbo.Ryt_PatientDoctor", "DoctorID", "dbo.Ryt_Doctor");
            DropForeignKey("dbo.Ryt_Patient", "UserID", "dbo.Ryt_User");
            DropForeignKey("dbo.Ryt_IntegralSignIn", "UserIntegralID", "dbo.Ryt_UserIntegral");
            DropForeignKey("dbo.Ryt_Navigation", "ArticleID", "dbo.Ryt_Article");
            DropForeignKey("dbo.Ryt_MedicineDepartment", "MedicineCategoryID", "dbo.Ryt_MedicineCategory");
            DropForeignKey("dbo.Ryt_MedicineDepartment", "HospitalID", "dbo.Ryt_Hospital");
            DropForeignKey("dbo.Ryt_Doctor", "User_Id", "dbo.Ryt_User");
            DropForeignKey("dbo.Ryt_UserProfile", "Id", "dbo.Ryt_User");
            DropForeignKey("dbo.Ryt_UserIntegral", "Id", "dbo.Ryt_User");
            DropForeignKey("dbo.Ryt_IntegralUserActivity", "UserIntegralID", "dbo.Ryt_UserIntegral");
            DropForeignKey("dbo.Ryt_IntegralUserActivity", "IntegralActivityID", "dbo.Ryt_IntegralActivity");
            DropForeignKey("dbo.Ryt_IntegralRecord", "UserIntegralID", "dbo.Ryt_UserIntegral");
            DropForeignKey("dbo.Ryt_IntegralRecord", "IntegralActivityID", "dbo.Ryt_IntegralActivity");
            DropForeignKey("dbo.Ryt_IntegralActivity", "IntegralRuleID", "dbo.Ryt_IntegralRule");
            DropForeignKey("dbo.Ryt_IntegralRule", "ArticleID", "dbo.Ryt_Article");
            DropForeignKey("dbo.Ryt_IntegralActivity", "ArticleID", "dbo.Ryt_Article");
            DropForeignKey("dbo.Ryt_UserIntegral", "IntegralGradeID", "dbo.Ryt_IntegralGrade");
            DropForeignKey("dbo.Ryt_Doctor", "MedicineDepartmentID", "dbo.Ryt_MedicineDepartment");
            DropForeignKey("dbo.Ryt_Hospital", "AreaID", "dbo.Ryt_BaseArea");
            DropIndex("dbo.Ryt_UserAuth", new[] { "User_Id" });
            DropIndex("dbo.Ryt_UserAuth", "UserAuthIdentity");
            DropIndex("dbo.Ryt_ReadPatientRecord", new[] { "DoctorID" });
            DropIndex("dbo.Ryt_ReadPatientRecord", new[] { "PatientMedicalRecordID" });
            DropIndex("dbo.Ryt_PatientMedicalRecord", new[] { "PatientID" });
            DropIndex("dbo.Ryt_PatientDoctor", new[] { "DoctorID" });
            DropIndex("dbo.Ryt_PatientDoctor", new[] { "PatientID" });
            DropIndex("dbo.Ryt_Patient", new[] { "UserID" });
            DropIndex("dbo.Ryt_IntegralSignIn", new[] { "UserIntegralID" });
            DropIndex("dbo.Ryt_Navigation", new[] { "ArticleID" });
            DropIndex("dbo.Ryt_UserProfile", new[] { "Id" });
            DropIndex("dbo.Ryt_IntegralUserActivity", new[] { "IntegralActivityID" });
            DropIndex("dbo.Ryt_IntegralUserActivity", new[] { "UserIntegralID" });
            DropIndex("dbo.Ryt_IntegralRule", new[] { "ArticleID" });
            DropIndex("dbo.Ryt_IntegralActivity", new[] { "IntegralRuleID" });
            DropIndex("dbo.Ryt_IntegralActivity", new[] { "ArticleID" });
            DropIndex("dbo.Ryt_IntegralRecord", new[] { "UserIntegralID" });
            DropIndex("dbo.Ryt_IntegralRecord", new[] { "IntegralActivityID" });
            DropIndex("dbo.Ryt_UserIntegral", new[] { "IntegralGradeID" });
            DropIndex("dbo.Ryt_UserIntegral", new[] { "Id" });
            DropIndex("dbo.Ryt_Doctor", new[] { "User_Id" });
            DropIndex("dbo.Ryt_Doctor", new[] { "MedicineDepartmentID" });
            DropIndex("dbo.Ryt_MedicineDepartment", new[] { "HospitalID" });
            DropIndex("dbo.Ryt_MedicineDepartment", new[] { "MedicineCategoryID" });
            DropIndex("dbo.Ryt_Hospital", new[] { "AreaID" });
            DropTable("dbo.Ryt_UserAuth");
            DropTable("dbo.Ryt_ReadPatientRecord");
            DropTable("dbo.Ryt_PatientMedicalRecord");
            DropTable("dbo.Ryt_PatientDoctor");
            DropTable("dbo.Ryt_Patient");
            DropTable("dbo.Ryt_IntegralSignIn");
            DropTable("dbo.Ryt_Navigation");
            DropTable("dbo.Ryt_MedicineCategory");
            DropTable("dbo.Ryt_UserProfile");
            DropTable("dbo.Ryt_IntegralUserActivity");
            DropTable("dbo.Ryt_IntegralRule");
            DropTable("dbo.Ryt_Article");
            DropTable("dbo.Ryt_IntegralActivity");
            DropTable("dbo.Ryt_IntegralRecord");
            DropTable("dbo.Ryt_IntegralGrade");
            DropTable("dbo.Ryt_UserIntegral");
            DropTable("dbo.Ryt_User");
            DropTable("dbo.Ryt_Doctor");
            DropTable("dbo.Ryt_MedicineDepartment");
            DropTable("dbo.Ryt_BaseArea");
            DropTable("dbo.Ryt_Hospital");
        }
    }
}
