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
        public TestedClass()
        {
            Methods = new HashSet<TestedMethod>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public int SourceCodeId { get; set; }

        public virtual SourceCode SourceCode { get; set; }

        public virtual ICollection<TestedMethod> Methods { get; set; }
    }
}
