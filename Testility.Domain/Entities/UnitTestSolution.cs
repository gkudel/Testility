using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Domain.Entities
{
    public class UnitTestSolution
    {
        public int Id { get; set; }
        public int SolutionId { get; set; }
        public virtual Solution Solution { get; set; }
    }
}
