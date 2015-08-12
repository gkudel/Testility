using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Domain.Entities
{
    public class TestedClass
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [Index("IX_TestedClass_Name_SourceCodeId", Order = 2, IsUnique = true)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Index("IX_TestedClass_Name_SourceCodeId", Order = 1, IsUnique = true)]
        public int SourceCodeId { get; set; }
        public virtual SourceCode SourceCode { get; set; }
        public virtual ICollection<TestedMethod> Methods { get; set; }
    }
}
