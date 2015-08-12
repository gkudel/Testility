using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Domain.Entities
{
    public class TestedMethod
    {
        public TestedMethod()
        {
            Tests = new HashSet<Test>();
        }
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [Index("IX_TestedMethod_Name_TestedClassId", Order = 2, IsUnique = true)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Index("IX_TestedMethod_Name_TestedClassId", Order = 1, IsUnique = true)]
        public int TestedClassId { get; set; }
        public virtual TestedClass TestedClass { get; set; }
        public virtual ICollection<Test> Tests { get; set; }
    }
}
