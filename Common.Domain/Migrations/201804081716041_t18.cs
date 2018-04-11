namespace Common.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t18 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ryt_MedicalToolData", "FrontPic", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ryt_MedicalToolData", "FrontPic");
        }
    }
}
