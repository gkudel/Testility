namespace Testility.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SetUp : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TestedMethod", "TestedClass_Id", "dbo.TestedClass");
            DropForeignKey("dbo.Test", "TestedMethod_Id", "dbo.TestedMethod");
            DropIndex("dbo.TestedMethod", new[] { "TestedClass_Id" });
            DropIndex("dbo.Test", new[] { "TestedMethod_Id" });
            RenameColumn(table: "dbo.TestedMethod", name: "TestedClass_Id", newName: "TestedClassId");
            RenameColumn(table: "dbo.Test", name: "TestedMethod_Id", newName: "TestedMethodId");
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
            AlterColumn("dbo.TestedMethod", "TestedClassId", c => c.Int(nullable: false));
            AlterColumn("dbo.Test", "TestedMethodId", c => c.Int(nullable: false));
            CreateIndex("dbo.TestedClass", "FileId");
            CreateIndex("dbo.TestedMethod", "TestedClassId");
            CreateIndex("dbo.Test", "TestedMethodId");
            AddForeignKey("dbo.TestedClass", "FileId", "dbo.File", "Id", cascadeDelete: true);
            AddForeignKey("dbo.TestedMethod", "TestedClassId", "dbo.TestedClass", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Test", "TestedMethodId", "dbo.TestedMethod", "Id", cascadeDelete: true);
            DropColumn("dbo.TestedClass", "SourceCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TestedClass", "SourceCode", c => c.String(nullable: false));
            DropForeignKey("dbo.Test", "TestedMethodId", "dbo.TestedMethod");
            DropForeignKey("dbo.TestedMethod", "TestedClassId", "dbo.TestedClass");
            DropForeignKey("dbo.TestedClass", "FileId", "dbo.File");
            DropIndex("dbo.Test", new[] { "TestedMethodId" });
            DropIndex("dbo.TestedMethod", new[] { "TestedClassId" });
            DropIndex("dbo.TestedClass", new[] { "FileId" });
            AlterColumn("dbo.Test", "TestedMethodId", c => c.Int());
            AlterColumn("dbo.TestedMethod", "TestedClassId", c => c.Int());
            DropColumn("dbo.TestedClass", "FileId");
            DropTable("dbo.File");
            RenameColumn(table: "dbo.Test", name: "TestedMethodId", newName: "TestedMethod_Id");
            RenameColumn(table: "dbo.TestedMethod", name: "TestedClassId", newName: "TestedClass_Id");
            CreateIndex("dbo.Test", "TestedMethod_Id");
            CreateIndex("dbo.TestedMethod", "TestedClass_Id");
            AddForeignKey("dbo.Test", "TestedMethod_Id", "dbo.TestedMethod", "Id");
            AddForeignKey("dbo.TestedMethod", "TestedClass_Id", "dbo.TestedClass", "Id");
        }
    }
}
