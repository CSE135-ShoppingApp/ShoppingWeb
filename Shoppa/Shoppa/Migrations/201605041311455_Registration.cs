namespace Shoppa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Registration : DbMigration
    {
        public override void Up()
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
            AddColumn("dbo.AspNetUsers", "Age", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Role", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "UserName", unique: true, name: "IX_UserUserName");
            CreateIndex("dbo.AspNetUsers", "StateID", unique: true, name: "IX_UserState");
            CreateIndex("dbo.AspNetUsers", "Age", unique: true, name: "IX_UserAge");
            CreateIndex("dbo.AspNetUsers", "Role", unique: true, name: "IX_UserRole");
            AddForeignKey("dbo.AspNetUsers", "StateID", "dbo.States", "ID", cascadeDelete: true);
            DropColumn("dbo.AspNetUsers", "HomeTown");
            DropColumn("dbo.AspNetUsers", "BirthDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "BirthDate", c => c.DateTime());
            AddColumn("dbo.AspNetUsers", "HomeTown", c => c.String());
            DropForeignKey("dbo.AspNetUsers", "StateID", "dbo.States");
            DropIndex("dbo.AspNetUsers", "IX_UserRole");
            DropIndex("dbo.AspNetUsers", "IX_UserAge");
            DropIndex("dbo.AspNetUsers", "IX_UserState");
            DropIndex("dbo.AspNetUsers", "IX_UserUserName");
            DropColumn("dbo.AspNetUsers", "Role");
            DropColumn("dbo.AspNetUsers", "Age");
            DropColumn("dbo.AspNetUsers", "StateID");
            DropTable("dbo.States");
        }
    }
}
