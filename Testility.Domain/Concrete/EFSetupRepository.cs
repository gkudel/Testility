using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;

namespace Testility.Domain.Concrete
{
    public class EFSetupRepository : ISetupRepository, IDisposable
    {
        private EFDbContext context = new EFDbContext();

        public EFSetupRepository()
        { }

        public IQueryable<SourceCode> Files
        {
            get
            {
                return context.Files;
            }
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
