using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Domain.Attribute
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class TestedMethodAttribute : System.Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
