namespace se_CodeFirst_3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoreColumnsToUser1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Benefits", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "Benifits");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Benifits", c => c.Int(nullable: false));
            DropColumn("dbo.AspNetUsers", "Benefits");
        }
    }
}
