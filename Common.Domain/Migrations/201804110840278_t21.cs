namespace Common.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ryt_PatientMedicalRecord", "PatientRecordUid", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ryt_PatientMedicalRecord", "PatientRecordUid");
        }
    }
}
