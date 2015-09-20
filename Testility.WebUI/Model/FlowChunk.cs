using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Testility.WebUI.Model
{
    public class FlowChunk
    {
        public int Number { get; set; }
        public long Size { get; set; }
        public long TotalSize { get; set; }
        public string Identifier { get; set; }
        public string FileName { get; set; }
        public int TotalChunks { get; set; }
        public HttpFileCollection Files { get; set; }
    }
}
