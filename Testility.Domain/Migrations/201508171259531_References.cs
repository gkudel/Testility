namespace Testility.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class References : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reference",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Solution", "Reference_Id", c => c.Int());
            CreateIndex("dbo.Solution", "Reference_Id");
            AddForeignKey("dbo.Solution", "Reference_Id", "dbo.Reference", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Solution", "Reference_Id", "dbo.Reference");
            DropIndex("dbo.Solution", new[] { "Reference_Id" });
            DropColumn("dbo.Solution", "Reference_Id");
            DropTable("dbo.Reference");
        }
    }
}
