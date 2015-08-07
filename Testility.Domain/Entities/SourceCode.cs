using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Domain.Entities
{
    public enum Language
    {
        CSharp, VisualBasic
    }

    public class SourceCode
    {
        public SourceCode()
        {
            Clasess = new HashSet<TestedClass>();
        }
        public int Id { get; set; }
        [Required]
        [StringLength(10)]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        public Language Language { get; set; }
        public String ReferencedAssemblies { get; set; }
        public virtual ICollection<TestedClass> Clasess { get; set; }
    }
}
