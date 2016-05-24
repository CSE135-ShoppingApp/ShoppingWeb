namespace Shoppa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Categories", "CreatedByID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Products", "Category_ID", "dbo.Categories");
            DropIndex("dbo.Categories", new[] { "CreatedByID" });
            DropIndex("dbo.Products", new[] { "Category_ID" });
            RenameColumn(table: "dbo.Products", name: "Category_ID", newName: "CategoryID");
            AddColumn("dbo.Categories", "Description", c => c.String());
            AlterColumn("dbo.Products", "CategoryID", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "CategoryID");
            AddForeignKey("dbo.Products", "CategoryID", "dbo.Categories", "ID", cascadeDelete: true);
            DropColumn("dbo.Categories", "CreatedByID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "CreatedByID", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.Products", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "CategoryID" });
            AlterColumn("dbo.Products", "CategoryID", c => c.Int());
            DropColumn("dbo.Categories", "Description");
            RenameColumn(table: "dbo.Products", name: "CategoryID", newName: "Category_ID");
            CreateIndex("dbo.Products", "Category_ID");
            CreateIndex("dbo.Categories", "CreatedByID");
            AddForeignKey("dbo.Products", "Category_ID", "dbo.Categories", "ID");
            AddForeignKey("dbo.Categories", "CreatedByID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
