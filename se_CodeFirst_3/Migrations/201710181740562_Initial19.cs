namespace se_CodeFirst_3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial19 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "ContractId", "dbo.Contracts");
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropIndex("dbo.Orders", new[] { "ContractId" });
            RenameColumn(table: "dbo.Orders", name: "ContractId", newName: "Contract_Id");
            RenameColumn(table: "dbo.Orders", name: "CustomerId", newName: "Customer_Id");
            AlterColumn("dbo.Orders", "Customer_Id", c => c.Int());
            AlterColumn("dbo.Orders", "Contract_Id", c => c.Int());
            CreateIndex("dbo.Orders", "Contract_Id");
            CreateIndex("dbo.Orders", "Customer_Id");
            AddForeignKey("dbo.Orders", "Contract_Id", "dbo.Contracts", "Id");
            AddForeignKey("dbo.Orders", "Customer_Id", "dbo.Customers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Orders", "Contract_Id", "dbo.Contracts");
            DropIndex("dbo.Orders", new[] { "Customer_Id" });
            DropIndex("dbo.Orders", new[] { "Contract_Id" });
            AlterColumn("dbo.Orders", "Contract_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Orders", "Customer_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Orders", name: "Customer_Id", newName: "CustomerId");
            RenameColumn(table: "dbo.Orders", name: "Contract_Id", newName: "ContractId");
            CreateIndex("dbo.Orders", "ContractId");
            CreateIndex("dbo.Orders", "CustomerId");
            AddForeignKey("dbo.Orders", "CustomerId", "dbo.Customers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Orders", "ContractId", "dbo.Contracts", "Id", cascadeDelete: true);
        }
    }
}
