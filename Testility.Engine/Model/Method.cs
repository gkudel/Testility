using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Engine.Model
{
    [Serializable]
    public class Method
    {
        public Method()
        {
            Tests = new HashSet<Test>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Test> Tests { get; set; }
    }
}
