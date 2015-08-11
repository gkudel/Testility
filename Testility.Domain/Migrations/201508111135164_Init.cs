namespace Testility.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
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
            
            CreateTable(
                "dbo.TestedClass",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 10),
                        Description = c.String(nullable: false),
                        SourceCodeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SourceCode", t => t.SourceCodeId, cascadeDelete: true)
                .Index(t => t.SourceCodeId);
            
            CreateTable(
                "dbo.TestedMethod",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 10),
                        Description = c.String(nullable: false),
                        TestedClassId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TestedClass", t => t.TestedClassId, cascadeDelete: true)
                .Index(t => t.TestedClassId);
            
            CreateTable(
                "dbo.Test",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Fail = c.Boolean(nullable: false),
                        TestedMethodId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TestedMethod", t => t.TestedMethodId, cascadeDelete: true)
                .Index(t => t.TestedMethodId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TestedClass", "SourceCodeId", "dbo.SourceCode");
            DropForeignKey("dbo.Test", "TestedMethodId", "dbo.TestedMethod");
            DropForeignKey("dbo.TestedMethod", "TestedClassId", "dbo.TestedClass");
            DropIndex("dbo.Test", new[] { "TestedMethodId" });
            DropIndex("dbo.TestedMethod", new[] { "TestedClassId" });
            DropIndex("dbo.TestedClass", new[] { "SourceCodeId" });
            DropTable("dbo.Test");
            DropTable("dbo.TestedMethod");
            DropTable("dbo.TestedClass");
            DropTable("dbo.SourceCode");
        }
    }
}
