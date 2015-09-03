using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.WebUI.Model
{
    public class UnitTestIndexItemViewModel
    {
        public int Id { get; set; }
        [Display(Name ="Solution Name")]
        public string Name { get; set; }
        [Display(Name = "Setup Solution Name")]
        public string SetupName { get; set; }
    }
}
