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
            Classes = new List<Class>();
            Errors = new List<Error>();
        }
        public string TemporaryFile { get; set; }
        public byte[] RawData { get; set; }
        public IList<Class> Classes { get; set; }
        public IList<Error> Errors { get; set; }
    }
}
