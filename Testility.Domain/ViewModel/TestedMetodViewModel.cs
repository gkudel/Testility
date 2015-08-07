using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Entities;

namespace Testility.Domain.ViewModels
{
    public class TestedMetodViewModel
    {

        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name="Name of tested class")]
        public IList<TestedClass> TestedClasses { get; set; }

        public int TestedClassId { get; set; }
        public string TestedClassName { get; set; }

    }
}
