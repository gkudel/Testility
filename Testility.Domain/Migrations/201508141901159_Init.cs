namespace Testility.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Class",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        SolutionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Solution", t => t.SolutionId, cascadeDelete: true)
                .Index(t => t.SolutionId);
            
            CreateTable(
                "dbo.Method",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        ClassId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Class", t => t.ClassId, cascadeDelete: true)
                .Index(t => t.ClassId);
            
            CreateTable(
                "dbo.Test",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        Fail = c.Boolean(nullable: false),
                        MethodId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Method", t => t.MethodId, cascadeDelete: true)
                .Index(t => t.MethodId);
            
            CreateTable(
                "dbo.Solution",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Language = c.Int(nullable: false),
                        ReferencedAssemblies = c.String(),
                        CompiledDll = c.Binary(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "IX_Solution_Name");
            
            CreateTable(
                "dbo.Item",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Code = c.String(),
                        Type = c.Int(nullable: false),
                        SolutionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Solution", t => t.SolutionId, cascadeDelete: true)
                .Index(t => t.SolutionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Item", "SolutionId", "dbo.Solution");
            DropForeignKey("dbo.Class", "SolutionId", "dbo.Solution");
            DropForeignKey("dbo.Test", "MethodId", "dbo.Method");
            DropForeignKey("dbo.Method", "ClassId", "dbo.Class");
            DropIndex("dbo.Item", new[] { "SolutionId" });
            DropIndex("dbo.Solution", "IX_Solution_Name");
            DropIndex("dbo.Test", new[] { "MethodId" });
            DropIndex("dbo.Method", new[] { "ClassId" });
            DropIndex("dbo.Class", new[] { "SolutionId" });
            DropTable("dbo.Item");
            DropTable("dbo.Solution");
            DropTable("dbo.Test");
            DropTable("dbo.Method");
            DropTable("dbo.Class");
        }
    }
}
