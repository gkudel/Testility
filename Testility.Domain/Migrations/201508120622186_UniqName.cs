namespace Testility.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UniqName : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.TestedClass", new[] { "SourceCodeId" });
            DropIndex("dbo.TestedMethod", new[] { "TestedClassId" });
            DropIndex("dbo.Test", new[] { "TestedMethodId" });
            AlterColumn("dbo.SourceCode", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.TestedClass", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.TestedMethod", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Test", "Name", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.SourceCode", "Name", unique: true, name: "IX_SourceCode_Name");
            CreateIndex("dbo.TestedClass", new[] { "SourceCodeId", "Name" }, unique: true, name: "IX_TestedClass_Name_SourceCodeId");
            CreateIndex("dbo.TestedMethod", new[] { "TestedClassId", "Name" }, unique: true, name: "IX_TestedMethod_Name_TestedClassId");
            CreateIndex("dbo.Test", new[] { "TestedMethodId", "Name" }, unique: true, name: "IX_Test_Name_TestedMethodId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Test", "IX_Test_Name_TestedMethodId");
            DropIndex("dbo.TestedMethod", "IX_TestedMethod_Name_TestedClassId");
            DropIndex("dbo.TestedClass", "IX_TestedClass_Name_SourceCodeId");
            DropIndex("dbo.SourceCode", "IX_SourceCode_Name");
            AlterColumn("dbo.Test", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.TestedMethod", "Name", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.TestedClass", "Name", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.SourceCode", "Name", c => c.String(nullable: false, maxLength: 10));
            CreateIndex("dbo.Test", "TestedMethodId");
            CreateIndex("dbo.TestedMethod", "TestedClassId");
            CreateIndex("dbo.TestedClass", "SourceCodeId");
        }
    }
}
