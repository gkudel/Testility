namespace Testility.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReferenceFileName : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.References", "IX_Reference_FilePath");
            AddColumn("dbo.References", "FileName", c => c.String(maxLength: 100));
            CreateIndex("dbo.References", "FileName", unique: true, name: "IX_Reference_FileName");
            DropColumn("dbo.References", "FilePath");
        }
        
        public override void Down()
        {
            AddColumn("dbo.References", "FilePath", c => c.String(maxLength: 100));
            DropIndex("dbo.References", "IX_Reference_FileName");
            DropColumn("dbo.References", "FileName");
            CreateIndex("dbo.References", "FilePath", unique: true, name: "IX_Reference_FilePath");
        }
    }
}
