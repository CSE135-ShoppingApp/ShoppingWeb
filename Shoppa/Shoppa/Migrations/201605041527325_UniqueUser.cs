namespace Shoppa.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UniqueUser : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AspNetUsers", "IX_UserUserName");
            DropIndex("dbo.AspNetUsers", "IX_UserState");
            DropIndex("dbo.AspNetUsers", "IX_UserAge");
            DropIndex("dbo.AspNetUsers", "IX_UserRole");
            CreateIndex("dbo.AspNetUsers", new[] { "UserName", "State", "Age", "Role" }, unique: true, name: "IX_UniqueUser");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AspNetUsers", "IX_UniqueUser");
            CreateIndex("dbo.AspNetUsers", "Role", unique: true, name: "IX_UserRole");
            CreateIndex("dbo.AspNetUsers", "Age", unique: true, name: "IX_UserAge");
            CreateIndex("dbo.AspNetUsers", "State", unique: true, name: "IX_UserState");
            CreateIndex("dbo.AspNetUsers", "UserName", unique: true, name: "IX_UserUserName");
        }
    }
}
