using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;

namespace Testility.Domain.Concrete
{
    public class UnitTestRepository : IUnitTestRepository, IDisposable
    {
        private IEFDbContext context;
        public UnitTestRepository(IEFDbContext context)
        {
            this.context = context;
        }

        public IQueryable<UnitTestSolution> GetSolutions()
        {
            return context.UnitTestSolutions;
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
