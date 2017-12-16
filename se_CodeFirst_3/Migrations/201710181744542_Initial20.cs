namespace se_CodeFirst_3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial20 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Orders", name: "Contract_Id", newName: "ContractId");
            RenameColumn(table: "dbo.Orders", name: "Customer_Id", newName: "CustomerId");
            RenameIndex(table: "dbo.Orders", name: "IX_Customer_Id", newName: "IX_CustomerId");
            RenameIndex(table: "dbo.Orders", name: "IX_Contract_Id", newName: "IX_ContractId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Orders", name: "IX_ContractId", newName: "IX_Contract_Id");
            RenameIndex(table: "dbo.Orders", name: "IX_CustomerId", newName: "IX_Customer_Id");
            RenameColumn(table: "dbo.Orders", name: "CustomerId", newName: "Customer_Id");
            RenameColumn(table: "dbo.Orders", name: "ContractId", newName: "Contract_Id");
        }
    }
}
