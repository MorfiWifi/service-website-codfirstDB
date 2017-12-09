namespace se_CodeFirst_3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Finalizing_3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "BuyUnitPrice", c => c.Int(nullable: false));
            AddColumn("dbo.Products", "SellUnitPrice", c => c.Int(nullable: false));
            DropColumn("dbo.Products", "UnitPrice");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "UnitPrice", c => c.Int(nullable: false));
            DropColumn("dbo.Products", "SellUnitPrice");
            DropColumn("dbo.Products", "BuyUnitPrice");
        }
    }
}
