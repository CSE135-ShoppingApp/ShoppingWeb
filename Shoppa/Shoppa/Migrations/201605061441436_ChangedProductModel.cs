namespace Shoppa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedProductModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductOrderViewModels",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Products", t => t.ProductID)
                .Index(t => t.ProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductOrderViewModels", "ProductID", "dbo.Products");
            DropIndex("dbo.ProductOrderViewModels", new[] { "ProductID" });
            DropTable("dbo.ProductOrderViewModels");
        }
    }
}
