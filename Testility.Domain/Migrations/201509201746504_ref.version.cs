namespace Testility.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refversion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.References", "Vesrion", c => c.String());
            AlterColumn("dbo.References", "Name", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.References", "Name", c => c.String());
            DropColumn("dbo.References", "Vesrion");
        }
    }
}
