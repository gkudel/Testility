using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Entities;
using Testility.WebUI.Model;

namespace Testility.WebUI.Areas.Setup.Models
{
    public class ProcjetsIndexData
    {
        public IEnumerable<Solution> Solutions { get; set; }
        public Solution Selected { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
