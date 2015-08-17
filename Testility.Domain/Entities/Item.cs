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
        [Required(ErrorMessage = "Item Name is requred")]
        [StringLength(100)]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Source Code")]
        public string Code { get; set; }
        public ItemType Type { get; set; }
        public int SolutionId { get; set; }
        public virtual Solution Solution { get; set; }        
    }
}
