namespace Shoppa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedCheckoutFields : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductOrderViewModels", "ProductID", "dbo.Products");
            DropIndex("dbo.ProductOrderViewModels", new[] { "ProductID" });
            AddColumn("dbo.Carts", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Orders", "CreditCard", c => c.String(nullable: false, maxLength: 16));
            DropTable("dbo.ProductOrderViewModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProductOrderViewModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropColumn("dbo.Orders", "CreditCard");
            DropColumn("dbo.Carts", "Price");
            CreateIndex("dbo.ProductOrderViewModels", "ProductID");
            AddForeignKey("dbo.ProductOrderViewModels", "ProductID", "dbo.Products", "ID");
        }
    }
}
