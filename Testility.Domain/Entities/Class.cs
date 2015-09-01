using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Domain.Entities
{
    public class Class
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }
        public int SolutionId { get; set; }
        public virtual SolutionApi Solution { get; set; }
        public virtual ICollection<Method> Methods { get; set; }
    }
}
