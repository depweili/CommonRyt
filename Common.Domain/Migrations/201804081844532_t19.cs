namespace Common.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t19 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ryt_Doctor", "CertifiedState", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ryt_Doctor", "CertifiedState");
        }
    }
}
