using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Engine.Attribute
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class TestedClassesAttribute : System.Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
