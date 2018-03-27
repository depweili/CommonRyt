namespace Common.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t07 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ryt_Attention", "Title", c => c.String(maxLength: 100));
            AddColumn("dbo.Ryt_Attention", "PicUrl", c => c.String(maxLength: 200));
            AlterColumn("dbo.Ryt_Attention", "Type", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Ryt_Attention", "Type", c => c.Int(nullable: false));
            DropColumn("dbo.Ryt_Attention", "PicUrl");
            DropColumn("dbo.Ryt_Attention", "Title");
        }
    }
}
