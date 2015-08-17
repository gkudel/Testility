using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Engine.Model
{
    [Serializable]
    public class Result
    {
        public Result()
        {
            Classes = new HashSet<Class>();
            Errors = new List<Error>();
        }
        public string TemporaryFile { get; set; }
        public byte[] RawData { get; set; }
        public ICollection<Class> Classes { get; set; }
        public IList<Error> Errors { get; set; }
    }
}
