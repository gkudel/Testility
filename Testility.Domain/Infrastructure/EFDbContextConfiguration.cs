using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Domain.Infrastructure
{
    public class EFDbContextConfiguration : DbConfiguration
    {
        public EFDbContextConfiguration()
        {
            DbInterception.Add(new EFDbContextInterceptorError());
            DbInterception.Add(new EFDbContextInterceptorLogging());
        }
    }
}
