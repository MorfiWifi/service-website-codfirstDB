namespace se_CodeFirst_3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial4 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Books", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Books", new[] { "ApplicationUser_Id" });
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.User_Id);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Order_Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .Index(t => t.Order_Id)
                .Index(t => t.Product_Id);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Suppliers", t => t.Supplier_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Supplier_Id)
                .Index(t => t.User_Id);
            
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
                "dbo.IncomingCalls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Message = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            DropTable("dbo.Books");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Writer = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Products", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.IncomingCalls", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Products", "Supplier_Id", "dbo.Suppliers");
            DropForeignKey("dbo.Order_Detail", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Order_Detail", "Order_Id", "dbo.Orders");
            DropForeignKey("dbo.Orders", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.IncomingCalls", new[] { "User_Id" });
            DropIndex("dbo.Products", new[] { "User_Id" });
            DropIndex("dbo.Products", new[] { "Supplier_Id" });
            DropIndex("dbo.Order_Detail", new[] { "Product_Id" });
            DropIndex("dbo.Order_Detail", new[] { "Order_Id" });
            DropIndex("dbo.Orders", new[] { "User_Id" });
            DropIndex("dbo.Orders", new[] { "Customer_Id" });
            DropTable("dbo.IncomingCalls");
            DropTable("dbo.Suppliers");
            DropTable("dbo.Products");
            DropTable("dbo.Order_Detail");
            DropTable("dbo.Orders");
            DropTable("dbo.Customers");
            CreateIndex("dbo.Books", "ApplicationUser_Id");
            AddForeignKey("dbo.Books", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
