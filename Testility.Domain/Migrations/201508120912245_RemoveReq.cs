namespace Testility.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveReq : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SourceCode", "Code", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SourceCode", "Code", c => c.String(nullable: false));
        }
    }
}
