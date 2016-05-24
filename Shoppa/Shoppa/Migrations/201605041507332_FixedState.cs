namespace Shoppa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixedState : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "StateID", "dbo.States");
            DropIndex("dbo.AspNetUsers", "IX_UserState");
            AddColumn("dbo.AspNetUsers", "State", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "State", unique: true, name: "IX_UserState");
            DropColumn("dbo.AspNetUsers", "StateID");
            DropTable("dbo.States");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.States",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FullName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.AspNetUsers", "StateID", c => c.Int(nullable: false));
            DropIndex("dbo.AspNetUsers", "IX_UserState");
            DropColumn("dbo.AspNetUsers", "State");
            CreateIndex("dbo.AspNetUsers", "StateID", unique: true, name: "IX_UserState");
            AddForeignKey("dbo.AspNetUsers", "StateID", "dbo.States", "ID", cascadeDelete: true);
        }
    }
}
