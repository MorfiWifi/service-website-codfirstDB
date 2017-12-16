namespace se_CodeFirst_3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial14 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Order_Detail", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Order_Detail", "Product_Id", "dbo.Products");
            DropIndex("dbo.Order_Detail", new[] { "Order_Id" });
            DropIndex("dbo.Order_Detail", new[] { "Product_Id" });
            RenameColumn(table: "dbo.Order_Detail", name: "Order_Id", newName: "OrderId");
            RenameColumn(table: "dbo.Order_Detail", name: "Product_Id", newName: "ProductId");
            CreateTable(
                "dbo.Contracts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Customers", "PhoneNumber", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "Contract_Id", c => c.Int());
            AddColumn("dbo.Suppliers", "PhoneNumber", c => c.Int(nullable: false));
            AlterColumn("dbo.Order_Detail", "OrderId", c => c.Int(nullable: false));
            AlterColumn("dbo.Order_Detail", "ProductId", c => c.Int(nullable: false));
            AlterColumn("dbo.Suppliers", "CompanyName", c => c.String(nullable: false));
            AlterColumn("dbo.Suppliers", "Name", c => c.String(nullable: false));
            CreateIndex("dbo.Orders", "Contract_Id");
            CreateIndex("dbo.Order_Detail", "ProductId");
            CreateIndex("dbo.Order_Detail", "OrderId");
            AddForeignKey("dbo.Orders", "Contract_Id", "dbo.Contracts", "Id");
            AddForeignKey("dbo.Order_Detail", "OrderId", "dbo.Orders", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Order_Detail", "ProductId", "dbo.Products", "Id", cascadeDelete: true);
            DropColumn("dbo.Customers", "Phone");
            DropColumn("dbo.Orders", "Content");
            DropColumn("dbo.Orders", "ShippedDate");
            DropColumn("dbo.Suppliers", "Phone");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Suppliers", "Phone", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "ShippedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "Content", c => c.String(nullable: false));
            AddColumn("dbo.Customers", "Phone", c => c.Int(nullable: false));
            DropForeignKey("dbo.Order_Detail", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Order_Detail", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "Contract_Id", "dbo.Contracts");
            DropIndex("dbo.Order_Detail", new[] { "OrderId" });
            DropIndex("dbo.Order_Detail", new[] { "ProductId" });
            DropIndex("dbo.Orders", new[] { "Contract_Id" });
            AlterColumn("dbo.Suppliers", "Name", c => c.String());
            AlterColumn("dbo.Suppliers", "CompanyName", c => c.String());
            AlterColumn("dbo.Order_Detail", "ProductId", c => c.Int());
            AlterColumn("dbo.Order_Detail", "OrderId", c => c.Int());
            DropColumn("dbo.Suppliers", "PhoneNumber");
            DropColumn("dbo.Orders", "Contract_Id");
            DropColumn("dbo.Customers", "PhoneNumber");
            DropTable("dbo.Contracts");
            RenameColumn(table: "dbo.Order_Detail", name: "ProductId", newName: "Product_Id");
            RenameColumn(table: "dbo.Order_Detail", name: "OrderId", newName: "Order_Id");
            CreateIndex("dbo.Order_Detail", "Product_Id");
            CreateIndex("dbo.Order_Detail", "Order_Id");
            AddForeignKey("dbo.Order_Detail", "Product_Id", "dbo.Products", "Id");
            AddForeignKey("dbo.Order_Detail", "Order_Id", "dbo.Orders", "Id");
        }
    }
}
