using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Entities;

namespace Testility.Domain.Concrete
{
    public class EFDbContext : DbContext
    {
        public virtual DbSet<SourceCode> Files { get; set; }
        public virtual DbSet<TestedClass> TestedClasses { get; set; }
        public virtual DbSet<TestedMethod> TestedMethods { get; set; }
        public virtual DbSet<Test> Tests { get; set; }

        public EFDbContext() : base("EFDbContext")
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
