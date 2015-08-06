namespace Testility.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TestedClass",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        SourceCode = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TestedMethod",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        TestedClass_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TestedClass", t => t.TestedClass_Id)
                .Index(t => t.TestedClass_Id);
            
            CreateTable(
                "dbo.Test",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false),
                        Fail = c.Boolean(nullable: false),
                        TestedMethod_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TestedMethod", t => t.TestedMethod_Id)
                .Index(t => t.TestedMethod_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TestedMethod", "TestedClass_Id", "dbo.TestedClass");
            DropForeignKey("dbo.Test", "TestedMethod_Id", "dbo.TestedMethod");
            DropIndex("dbo.Test", new[] { "TestedMethod_Id" });
            DropIndex("dbo.TestedMethod", new[] { "TestedClass_Id" });
            DropTable("dbo.Test");
            DropTable("dbo.TestedMethod");
            DropTable("dbo.TestedClass");
        }
    }
}
