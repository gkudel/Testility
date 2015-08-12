using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testility.Domain.Entities
{
    public class Test
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [Index("IX_Test_Name_TestedMethodId", Order = 2, IsUnique = true)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public bool Fail { get; set; }
        [Index("IX_Test_Name_TestedMethodId", Order = 1, IsUnique = true)]
        public int TestedMethodId { get; set; }        
        public virtual TestedMethod TestedMethod { get; set; }
    }
}
