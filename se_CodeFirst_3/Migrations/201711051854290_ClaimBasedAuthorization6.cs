namespace se_CodeFirst_3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClaimBasedAuthorization6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClaimViewModels", "TypeToShow", c => c.String());
            AddColumn("dbo.ClaimViewModels", "ValueToShow", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ClaimViewModels", "ValueToShow");
            DropColumn("dbo.ClaimViewModels", "TypeToShow");
        }
    }
}
