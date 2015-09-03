namespace Testility.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Classes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        SolutionId = c.Int(nullable: false),
                        SetupSolution_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Solutions", t => t.SetupSolution_Id)
                .ForeignKey("dbo.Solutions", t => t.SolutionId, cascadeDelete: true)
                .Index(t => t.SolutionId)
                .Index(t => t.SetupSolution_Id);
            
            CreateTable(
                "dbo.Methods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        ClassId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Classes", t => t.ClassId, cascadeDelete: true)
                .Index(t => t.ClassId);
            
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Description = c.String(),
                        Fail = c.Boolean(nullable: false),
                        MethodId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Methods", t => t.MethodId, cascadeDelete: true)
                .Index(t => t.MethodId);
            
            CreateTable(
                "dbo.Solutions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Language = c.Int(),
                        SetupSolutionId = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Solutions", t => t.SetupSolutionId)
                .Index(t => t.Name, unique: true, name: "IX_Solution_Name")
                .Index(t => t.SetupSolutionId);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Code = c.String(),
                        Type = c.Int(nullable: false),
                        SolutionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Solutions", t => t.SolutionId, cascadeDelete: true)
                .Index(t => t.SolutionId);
            
            CreateTable(
                "dbo.References",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.UserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.ReferenceSolutions",
                c => new
                    {
                        Reference_Id = c.Int(nullable: false),
                        Solution_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Reference_Id, t.Solution_Id })
                .ForeignKey("dbo.References", t => t.Reference_Id, cascadeDelete: true)
                .ForeignKey("dbo.Solutions", t => t.Solution_Id, cascadeDelete: true)
                .Index(t => t.Reference_Id)
                .Index(t => t.Solution_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserLogins", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserClaims", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Classes", "SolutionId", "dbo.Solutions");
            DropForeignKey("dbo.Solutions", "SetupSolutionId", "dbo.Solutions");
            DropForeignKey("dbo.Classes", "SetupSolution_Id", "dbo.Solutions");
            DropForeignKey("dbo.ReferenceSolutions", "Solution_Id", "dbo.Solutions");
            DropForeignKey("dbo.ReferenceSolutions", "Reference_Id", "dbo.References");
            DropForeignKey("dbo.Items", "SolutionId", "dbo.Solutions");
            DropForeignKey("dbo.Tests", "MethodId", "dbo.Methods");
            DropForeignKey("dbo.Methods", "ClassId", "dbo.Classes");
            DropIndex("dbo.ReferenceSolutions", new[] { "Solution_Id" });
            DropIndex("dbo.ReferenceSolutions", new[] { "Reference_Id" });
            DropIndex("dbo.UserLogins", new[] { "UserId" });
            DropIndex("dbo.UserClaims", new[] { "UserId" });
            DropIndex("dbo.Users", "UserNameIndex");
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.Roles", "RoleNameIndex");
            DropIndex("dbo.Items", new[] { "SolutionId" });
            DropIndex("dbo.Solutions", new[] { "SetupSolutionId" });
            DropIndex("dbo.Solutions", "IX_Solution_Name");
            DropIndex("dbo.Tests", new[] { "MethodId" });
            DropIndex("dbo.Methods", new[] { "ClassId" });
            DropIndex("dbo.Classes", new[] { "SetupSolution_Id" });
            DropIndex("dbo.Classes", new[] { "SolutionId" });
            DropTable("dbo.ReferenceSolutions");
            DropTable("dbo.UserLogins");
            DropTable("dbo.UserClaims");
            DropTable("dbo.Users");
            DropTable("dbo.UserRoles");
            DropTable("dbo.Roles");
            DropTable("dbo.References");
            DropTable("dbo.Items");
            DropTable("dbo.Solutions");
            DropTable("dbo.Tests");
            DropTable("dbo.Methods");
            DropTable("dbo.Classes");
        }
    }
}
