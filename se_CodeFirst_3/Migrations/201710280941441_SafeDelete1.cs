namespace se_CodeFirst_3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SafeDelete1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Suppliers", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Customers", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "IsDeleted");
            DropColumn("dbo.Suppliers", "IsDeleted");
        }
    }
}
