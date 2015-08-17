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
            Methods = new HashSet<Method>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Method> Methods { get; set; }
    }
}
