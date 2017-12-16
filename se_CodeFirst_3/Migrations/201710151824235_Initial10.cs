namespace se_CodeFirst_3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial10 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.IncomingCalls", name: "User_Id", newName: "ApplicationUser_Id");
            RenameColumn(table: "dbo.Orders", name: "User_Id", newName: "ApplicationUser_Id");
            RenameColumn(table: "dbo.Products", name: "User_Id", newName: "ApplicationUser_Id");
            RenameIndex(table: "dbo.Orders", name: "IX_User_Id", newName: "IX_ApplicationUser_Id");
            RenameIndex(table: "dbo.IncomingCalls", name: "IX_User_Id", newName: "IX_ApplicationUser_Id");
            RenameIndex(table: "dbo.Products", name: "IX_User_Id", newName: "IX_ApplicationUser_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Products", name: "IX_ApplicationUser_Id", newName: "IX_User_Id");
            RenameIndex(table: "dbo.IncomingCalls", name: "IX_ApplicationUser_Id", newName: "IX_User_Id");
            RenameIndex(table: "dbo.Orders", name: "IX_ApplicationUser_Id", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Products", name: "ApplicationUser_Id", newName: "User_Id");
            RenameColumn(table: "dbo.Orders", name: "ApplicationUser_Id", newName: "User_Id");
            RenameColumn(table: "dbo.IncomingCalls", name: "ApplicationUser_Id", newName: "User_Id");
        }
    }
}
