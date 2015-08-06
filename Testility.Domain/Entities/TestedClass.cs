using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Domain.Entities
{
    public class TestedClass
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string SourceCode { get; set; }

        public virtual ICollection<TestedMethod> Methods { get; set; }
    }
}
