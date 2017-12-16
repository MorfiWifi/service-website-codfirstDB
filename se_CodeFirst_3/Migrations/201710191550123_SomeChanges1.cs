namespace se_CodeFirst_3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SomeChanges1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Products", "QuantityPerUnit");
            DropColumn("dbo.Products", "UnitsOnOrder");
            DropColumn("dbo.Order_Detail", "UnitPrice");
            DropColumn("dbo.Order_Detail", "Discount");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Order_Detail", "Discount", c => c.Int(nullable: false));
            AddColumn("dbo.Order_Detail", "UnitPrice", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "UnitsOnOrder", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "QuantityPerUnit", c => c.Int(nullable: false));
        }
    }
}
