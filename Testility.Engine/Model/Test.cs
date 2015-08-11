using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Engine.Model
{
    [Serializable]
    public class Test 
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Fail { get; set; }
    }
}
