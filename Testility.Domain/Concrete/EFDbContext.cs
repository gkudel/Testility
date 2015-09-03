using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;

namespace Testility.Domain.Concrete
{
    public class EFDbContext : IdentityDbContext<IdentityUser>, IEFDbContext
    {   
        #region Setup Entities
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Method> Methods { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<Reference> References { get; set; }
        #endregion Setup Entities

        #region Solution
        public virtual DbSet<Solution> Solutions { get; set; }
        public virtual DbSet<UnitTestSolution> UnitTestSolutions { get; set; }
        public virtual DbSet<SetupSolution> SetupSolutions { get; set; }
        #endregion Solution 


        public EFDbContext() : base("EFDbContext")
        {
        }        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
        }

    }
}
