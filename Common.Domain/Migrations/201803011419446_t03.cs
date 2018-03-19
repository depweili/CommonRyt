namespace Common.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t03 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Ryt_PatientMedicalRecord", "Content", c => c.String(storeType: "ntext"));
            AlterColumn("dbo.Ryt_ReadPatientRecord", "Diagnostic", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ryt_ReadPatientRecord", "Diagnostic", c => c.String(maxLength: 50));
            AlterColumn("dbo.Ryt_PatientMedicalRecord", "Content", c => c.String(maxLength: 50));
        }
    }
}
