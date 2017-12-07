namespace se_CodeFirst_3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Order_Detail", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Order_Detail", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Products", "Supplier_Id", "dbo.Suppliers");
            DropForeignKey("dbo.IncomingCalls", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Products", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "Customer_Id" });
            DropIndex("dbo.Orders", new[] { "User_Id" });
            DropIndex("dbo.Order_Detail", new[] { "Order_Id" });
            DropIndex("dbo.Order_Detail", new[] { "Product_Id" });
            DropIndex("dbo.Products", new[] { "Supplier_Id" });
            DropIndex("dbo.Products", new[] { "User_Id" });
            DropIndex("dbo.IncomingCalls", new[] { "User_Id" });
            DropTable("dbo.Customers");
            DropTable("dbo.Orders");
            DropTable("dbo.Order_Detail");
            DropTable("dbo.Products");
            DropTable("dbo.Suppliers");
            DropTable("dbo.IncomingCalls");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.IncomingCalls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CompanyName = c.String(),
                        Name = c.String(),
                        Address = c.String(),
                        Phone = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        QuantityPerUnit = c.Int(nullable: false),
                        UnitPrice = c.Int(nullable: false),
                        UnitsInStock = c.Int(nullable: false),
                        UnitsOnOrder = c.Int(nullable: false),
                        Supplier_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Order_Detail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UnitPrice = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Discount = c.Int(nullable: false),
                        Order_Id = c.Int(),
                        Product_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(nullable: false),
                        OrderDate = c.DateTime(nullable: false),
                        RequiredDate = c.DateTime(nullable: false),
                        ShippedDate = c.DateTime(nullable: false),
                        Customer_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CompanyName = c.String(),
                        Phone = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.IncomingCalls", "User_Id");
            CreateIndex("dbo.Products", "User_Id");
            CreateIndex("dbo.Products", "Supplier_Id");
            CreateIndex("dbo.Order_Detail", "Product_Id");
            CreateIndex("dbo.Order_Detail", "Order_Id");
            CreateIndex("dbo.Orders", "User_Id");
            CreateIndex("dbo.Orders", "Customer_Id");
            AddForeignKey("dbo.Products", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Orders", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.IncomingCalls", "User_Id", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Products", "Supplier_Id", "dbo.Suppliers", "Id");
            AddForeignKey("dbo.Order_Detail", "Product_Id", "dbo.Products", "Id");
            AddForeignKey("dbo.Order_Detail", "Order_Id", "dbo.Orders", "Id");
            AddForeignKey("dbo.Orders", "Customer_Id", "dbo.Customers", "Id");
        }
    }
}
