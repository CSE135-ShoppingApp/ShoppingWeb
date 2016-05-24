namespace Shoppa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Categories : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "CreatedByID", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Categories", "CreatedByID");
            AddForeignKey("dbo.Categories", "CreatedByID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Categories", "CreatedByID", "dbo.AspNetUsers");
            DropIndex("dbo.Categories", new[] { "CreatedByID" });
            DropColumn("dbo.Categories", "CreatedByID");
        }
    }
}
