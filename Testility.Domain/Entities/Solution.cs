using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Testility.Domain.Entities
{
    public enum Language
    {
        CSharp, VisualBasic
    }

    public class Solution
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [Index("IX_Solution_Name", IsUnique = true)]
        public string Name { get; set; }
        public Language Language { get; set; }
        public String ReferencedAssemblies { get; set; }
        public byte[] CompiledDll { get; set; }

        public virtual Reference Reference { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
        
    }
}
