namespace Testility.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReferenceFilePathUniq : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.References", "FilePath", c => c.String(maxLength: 100));
            CreateIndex("dbo.References", "FilePath", unique: true, name: "IX_Reference_FilePath");
        }
        
        public override void Down()
        {
            DropIndex("dbo.References", "IX_Reference_FilePath");
            AlterColumn("dbo.References", "FilePath", c => c.String());
        }
    }
}
