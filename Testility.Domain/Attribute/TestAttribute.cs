using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Domain.Attribute
{
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
    public class TestAttribute : System.Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Fail { get; set; }
    }
}
