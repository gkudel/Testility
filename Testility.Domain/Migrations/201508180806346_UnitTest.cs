namespace Testility.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UnitTest : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UnitTestSolution",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SolutionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Solution", t => t.SolutionId, cascadeDelete: true)
                .Index(t => t.SolutionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UnitTestSolution", "SolutionId", "dbo.Solution");
            DropIndex("dbo.UnitTestSolution", new[] { "SolutionId" });
            DropTable("dbo.UnitTestSolution");
        }
    }
}
