namespace Common.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t02 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ryt_CharityDrugApplication", "State", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ryt_CharityDrugApplication", "State");
        }
    }
}
