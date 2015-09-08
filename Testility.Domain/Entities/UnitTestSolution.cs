using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Domain.Entities
{
    public class UnitTestSolution : Solution
    {
        public int SetupSolutionId { get; set; }        
        public virtual SetupSolution SetupSolution { get; set;  }
    }
}
