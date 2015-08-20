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
        private ICollection<Reference> references;

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [Index("IX_Solution_Name", IsUnique = true)]
        public string Name { get; set; }
        public Language Language { get; set; }
        public byte[] CompiledDll { get; set; }

        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Reference> References
        {
            get { return references ?? (references = new HashSet<Reference>()); }
            set { references = value; }
        }        
    }
}
