namespace Shoppa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "SKU", c => c.String(nullable: false));
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false, maxLength: 160));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Name", c => c.String(maxLength: 60));
            AlterColumn("dbo.Products", "SKU", c => c.String());
        }
    }
}
