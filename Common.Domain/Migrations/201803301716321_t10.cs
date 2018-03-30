namespace Common.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ryt_VideoInfo", "Presenter", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ryt_VideoInfo", "Presenter");
        }
    }
}
