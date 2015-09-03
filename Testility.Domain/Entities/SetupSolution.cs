using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Domain.Entities
{
    public class SetupSolution : Solution
    {        
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<UnitTestSolution> UnitTests { get; set; }
    }
}
