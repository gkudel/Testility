namespace Testility.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class References1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Solution", "Reference_Id", "dbo.Reference");
            DropIndex("dbo.Solution", new[] { "Reference_Id" });
            CreateTable(
                "dbo.ReferencedAssemblies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SolutionId = c.Int(nullable: false),
                        ReferenceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Reference", t => t.ReferenceId, cascadeDelete: true)
                .ForeignKey("dbo.Solution", t => t.SolutionId, cascadeDelete: true)
                .Index(t => t.SolutionId)
                .Index(t => t.ReferenceId);
            
            DropColumn("dbo.Solution", "Reference_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Solution", "Reference_Id", c => c.Int());
            DropForeignKey("dbo.ReferencedAssemblies", "SolutionId", "dbo.Solution");
            DropForeignKey("dbo.ReferencedAssemblies", "ReferenceId", "dbo.Reference");
            DropIndex("dbo.ReferencedAssemblies", new[] { "ReferenceId" });
            DropIndex("dbo.ReferencedAssemblies", new[] { "SolutionId" });
            DropTable("dbo.ReferencedAssemblies");
            CreateIndex("dbo.Solution", "Reference_Id");
            AddForeignKey("dbo.Solution", "Reference_Id", "dbo.Reference", "Id");
        }
    }
}
