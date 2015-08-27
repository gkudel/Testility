using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Engine.Model
{
    [Serializable]
    public class Error
    {
        public string ErrorText { get; set; }
        public int Column { get; set; }
        public string ErrorNumber { get; set; }
        public bool IsWarning { get; set; }
        public int Line { get; set; }
        public override string ToString()
        {
            return ErrorNumber + " " + ErrorText + "("+Column+","+Line+")";
        }
    }
}
