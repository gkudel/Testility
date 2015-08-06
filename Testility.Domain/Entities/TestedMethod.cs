using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Domain.Entities
{
    public class TestedMethod
    {
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        public virtual ICollection<Test> Tests { get; set; }
    }
}
