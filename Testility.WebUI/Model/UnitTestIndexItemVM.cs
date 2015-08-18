using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.WebUI.Model
{
    public class UnitTestIndexItemVM
    {
        public int Id { get; set; }
        [Display(Name ="Solution Name")]
        public String SolutionName { get; set; }
    }
}
