using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Engine.Model
{
    public class Result
    {
        public Result()
        {
            TestedClasses = new List<TestedClass>();
        }

        public IList<TestedClass> TestedClasses { get; set; }
    }
}
