using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Domain.Infrastructure
{
    public class EFDbContextInterceptorError : DbCommandInterceptor
    {
        public override void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            if (command.Parameters.Count > 0 && command.Parameters[0].Value.ToString() == "Throw")
            {
                interceptionContext.Exception = new DataException("Exception Test");
            }
        }
    }
}
