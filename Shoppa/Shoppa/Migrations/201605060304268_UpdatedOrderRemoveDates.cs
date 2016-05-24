namespace Shoppa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedOrderRemoveDates : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Orders", "UpdatedDate");
            DropColumn("dbo.Orders", "ExpirationDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "ExpirationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "UpdatedDate", c => c.DateTime(nullable: false));
        }
    }
}
