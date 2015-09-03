namespace Testility.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Language : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Solutions", "Language", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Solutions", "Language", c => c.Int());
        }
    }
}
