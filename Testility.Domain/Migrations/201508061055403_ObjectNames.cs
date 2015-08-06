namespace Testility.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ObjectNames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TestedClass", "Name", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.TestedMethod", "Name", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.Test", "Name", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Test", "Name");
            DropColumn("dbo.TestedMethod", "Name");
            DropColumn("dbo.TestedClass", "Name");
        }
    }
}
