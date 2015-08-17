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
        public virtual DbSet<Solution> Solutions { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Class> Classes { get; set; }
        public virtual DbSet<Method> Methods { get; set; }
        public virtual DbSet<Test> Tests { get; set; }

        public virtual DbSet<Reference> References { get; set; }

        public EFDbContext() : base("EFDbContext")
        { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}
