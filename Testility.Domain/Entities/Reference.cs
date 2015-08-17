using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Domain.Entities
{
    public class Reference
    {

        public int Id { get; set; }
        public string Name { get; set; }


    }

    public class ReferencedAssemblies
    {

        public int Id { get; set; }
        public int SolutionId { get; set; }
        public int ReferenceId { get; set; }
        public virtual Reference Reference { get; set; }
        public virtual Solution Solution { get; set; }


    }
}
