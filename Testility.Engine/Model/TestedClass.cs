using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Engine.Model
{
    public class TestedClass
    {
        public TestedClass()
        {
            Methods = new List<TestedMethod>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<TestedMethod> Methods { get; set; }
    }
}
