using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Entities;

namespace Testility.WebUI.Areas.Setup.Model
{
    public class SolutionIndexItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Classes { get; set; }
        public int Methods { get; set; }
        public int Tests { get; set; }
        public Language Language { get; set; }
    }
}
