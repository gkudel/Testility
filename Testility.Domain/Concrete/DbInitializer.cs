using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Domain.Concrete
{
    public class DbInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<DbContext>
    {
    }
}
