namespace se_CodeFirst_3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Logs2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Logs", "State", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Logs", "State");
        }
    }
}
