using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Domain.Entities
{
    public class Reference
    {
        
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public string Version{ get; set; }
        public string Description { get; set; }
        [Index("IX_Reference_FileName", IsUnique = true)]
        [StringLength(100)]
        public string FileName{ get; set; }        
        public virtual ICollection<Solution> Solutions { get; set; }
    }
}
