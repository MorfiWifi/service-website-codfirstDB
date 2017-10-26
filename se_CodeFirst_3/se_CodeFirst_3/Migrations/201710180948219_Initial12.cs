namespace se_CodeFirst_3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial12 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "Supplier_Id", "dbo.Suppliers");
            DropIndex("dbo.Products", new[] { "Supplier_Id" });
            RenameColumn(table: "dbo.Products", name: "Supplier_Id", newName: "SupplierId");
            AlterColumn("dbo.Products", "SupplierId", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "SupplierId");
            AddForeignKey("dbo.Products", "SupplierId", "dbo.Suppliers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "SupplierId", "dbo.Suppliers");
            DropIndex("dbo.Products", new[] { "SupplierId" });
            AlterColumn("dbo.Products", "SupplierId", c => c.Int());
            RenameColumn(table: "dbo.Products", name: "SupplierId", newName: "Supplier_Id");
            CreateIndex("dbo.Products", "Supplier_Id");
            AddForeignKey("dbo.Products", "Supplier_Id", "dbo.Suppliers", "Id");
        }
    }
}
