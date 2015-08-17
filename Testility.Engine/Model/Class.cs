using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Engine.Model
{
    [Serializable]
    public class Class
    {
        public Class()
        {
            Methods = new List<Method>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<Method> Methods { get; set; }
    }
}
