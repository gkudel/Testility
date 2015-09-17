using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Engine.Model;

namespace Testility.Engine.Attribute
{
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = true)]
    public class TestAttribute : System.Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Fail { get; set; }
    }
}
