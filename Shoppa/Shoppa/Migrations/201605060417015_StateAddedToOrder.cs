namespace Shoppa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StateAddedToOrder : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "State", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "State", c => c.String(nullable: false, maxLength: 40));
        }
    }
}
