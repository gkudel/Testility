using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Entities;

namespace Testility.Domain.Abstract
{
    public interface IDbContext: IDisposable
    {
        #region Setup Entities
        DbSet<Item> Items { get; set; }
        DbSet<Class> Classes { get; set; }
        DbSet<Method> Methods { get; set; }
        DbSet<Test> Tests { get; set; }
        DbSet<Reference> References { get; set; }
        #endregion Setup Entities

        #region Solution
        DbSet<Solution> Solutions { get; set; }
        DbSet<SetupSolution> SetupSolutions { get; set; }
        DbSet<UnitTestSolution> UnitTestSolutions { get; set; }
        #endregion Solution

        int SaveChanges();
        IEnumerable<DbEntityValidationResult> GetValidationErrors();
    }
}
