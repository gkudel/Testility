using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Domain.Infrastructure
{
    public class DbContextConfiguration : DbConfiguration
    {
        public DbContextConfiguration()
        {
            DbInterception.Add(new DbContextInterceptorError());
            DbInterception.Add(new DbContextInterceptorLogging());
        }
    }
}
