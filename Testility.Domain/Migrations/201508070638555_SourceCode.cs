namespace Testility.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SourceCode : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TestedClass", "FileId", "dbo.File");
            DropIndex("dbo.TestedClass", new[] { "FileId" });
            CreateTable(
                "dbo.SourceCode",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 10),
                        Code = c.String(nullable: false),
                        Language = c.Int(nullable: false),
                        ReferencedAssemblies = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TestedClass", "SourceCodeId", c => c.Int(nullable: false));
            CreateIndex("dbo.TestedClass", "SourceCodeId");
            AddForeignKey("dbo.TestedClass", "SourceCodeId", "dbo.SourceCode", "Id", cascadeDelete: true);
            DropColumn("dbo.TestedClass", "FileId");
            DropTable("dbo.File");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.File",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 10),
                        SourceCode = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TestedClass", "FileId", c => c.Int(nullable: false));
            DropForeignKey("dbo.TestedClass", "SourceCodeId", "dbo.SourceCode");
            DropIndex("dbo.TestedClass", new[] { "SourceCodeId" });
            DropColumn("dbo.TestedClass", "SourceCodeId");
            DropTable("dbo.SourceCode");
            CreateIndex("dbo.TestedClass", "FileId");
            AddForeignKey("dbo.TestedClass", "FileId", "dbo.File", "Id", cascadeDelete: true);
        }
    }
}
