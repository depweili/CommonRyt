namespace Common.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t05 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ryt_MedicalRecord", "Order", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Ryt_Fund", "Order", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Ryt_FundProject", "Order", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ryt_FundProject", "Order");
            DropColumn("dbo.Ryt_Fund", "Order");
            DropColumn("dbo.Ryt_MedicalRecord", "Order");
        }
    }
}
