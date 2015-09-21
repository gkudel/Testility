namespace Testility.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Misspeling : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.References", "Version", c => c.String());
            DropColumn("dbo.References", "Vesrion");
        }
        
        public override void Down()
        {
            AddColumn("dbo.References", "Vesrion", c => c.String());
            DropColumn("dbo.References", "Version");
        }
    }
}
