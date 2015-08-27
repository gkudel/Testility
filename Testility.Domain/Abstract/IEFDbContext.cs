using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Entities;

namespace Testility.Domain.Abstract
{
    public interface IEFDbContext: IDisposable
    {
        #region Setup Entities
        DbSet<Solution> Solutions { get; set; }
        DbSet<Item> Items { get; set; }
        DbSet<Class> Classes { get; set; }
        DbSet<Method> Methods { get; set; }
        DbSet<Test> Tests { get; set; }
        DbSet<Reference> References { get; set; }
        #endregion Setup Entities

        #region UnitTest Entities
        DbSet<UnitTestSolution> UnitTestSolutions { get; set; }
        #endregion UnitTest Entities

        int SaveChanges();
    }
}
