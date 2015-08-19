using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Testility.Domain.Entities;

namespace Testility.WebUI.Areas.Setup.Models
{    
    public class SolutionVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Solution Name is requred")]
        [StringLength(100)]
        [Remote("IsNameUnique", "Validation", ErrorMessage = "Solution already exists.", AdditionalFields = "Id")]
        public string Name { get; set; }
        public Language Language { get; set; }
        [Display(Name = "References")]
        public int[] Refrences { get; set; }
        public virtual ICollection<ItemVM> Items { get; set; }
    }
}
