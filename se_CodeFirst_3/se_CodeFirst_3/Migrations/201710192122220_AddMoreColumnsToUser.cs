namespace se_CodeFirst_3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMoreColumnsToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "AbsentDays", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "OverTime", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Benifits", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Benifits");
            DropColumn("dbo.AspNetUsers", "OverTime");
            DropColumn("dbo.AspNetUsers", "AbsentDays");
        }
    }
}
