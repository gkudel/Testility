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
    public enum ItemType
    {
        Class, Interface
    }

    public class Item
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [Index("IX_Item_Name_SolutionId", Order = 2,  IsUnique = true)]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Source Code")]
        public string Code { get; set; }
        [Index("IX_Item_Name_SolutionId", Order = 1, IsUnique = true)]
        public int SolutionId { get; set; }
        public virtual Solution Solution { get; set; }
        public virtual ICollection<TestedClass> Clasess { get; set; }
    }
}
