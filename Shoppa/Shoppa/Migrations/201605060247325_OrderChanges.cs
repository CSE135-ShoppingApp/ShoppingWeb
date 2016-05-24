namespace Shoppa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "Username", c => c.String());
            AlterColumn("dbo.Orders", "FirstName", c => c.String(nullable: false, maxLength: 160));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "FirstName", c => c.String());
            AlterColumn("dbo.Orders", "Username", c => c.String(nullable: false, maxLength: 160));
        }
    }
}
