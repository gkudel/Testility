namespace Testility.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RunResult : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tests", "RunResult", c => c.Byte(nullable: false));
            DropColumn("dbo.Tests", "Fail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tests", "Fail", c => c.Boolean(nullable: false));
            DropColumn("dbo.Tests", "RunResult");
        }
    }
}
