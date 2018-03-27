namespace Common.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t06 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Ryt_MedicalRecord", new[] { "DoctorID" });
            AlterColumn("dbo.Ryt_MedicalRecord", "DoctorID", c => c.Int());
            CreateIndex("dbo.Ryt_MedicalRecord", "DoctorID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Ryt_MedicalRecord", new[] { "DoctorID" });
            AlterColumn("dbo.Ryt_MedicalRecord", "DoctorID", c => c.Int(nullable: false));
            CreateIndex("dbo.Ryt_MedicalRecord", "DoctorID");
        }
    }
}
