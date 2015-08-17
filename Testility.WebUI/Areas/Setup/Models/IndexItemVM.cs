using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Entities;

namespace Testility.WebUI.Areas.Setup.Models
{
    public class IndexItemVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public Language Language { get; set; }
    }
}
