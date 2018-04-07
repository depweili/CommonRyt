namespace Common.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ryt_ImageInfo", "UserID", c => c.Int());
            DropColumn("dbo.Ryt_ImageInfo", "UploadUserUid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ryt_ImageInfo", "UploadUserUid", c => c.Guid(nullable: false));
            DropColumn("dbo.Ryt_ImageInfo", "UserID");
        }
    }
}
