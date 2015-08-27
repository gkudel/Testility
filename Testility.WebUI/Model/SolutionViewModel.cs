using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Testility.Domain.Entities;

namespace Testility.WebUI.Model
{
    public class SolutionViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Solution Name is requred")]
        [StringLength(100)]
        [Remote("IsNameUnique", "Validation", ErrorMessage = "Solution already exists.", AdditionalFields = "Id")]
        public string Name { get; set; }
        public Language Language { get; set; }
        [Display(Name = "References")]
        public int[] References { get; set; }
        public ItemViewModel[] Items { get; set; }
    }
}
