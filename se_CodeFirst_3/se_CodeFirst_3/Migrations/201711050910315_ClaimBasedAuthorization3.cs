namespace se_CodeFirst_3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ClaimBasedAuthorization3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClaimViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ClaimViewModels");
        }
    }
}
