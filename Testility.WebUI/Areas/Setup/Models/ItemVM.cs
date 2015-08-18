using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Testility.Domain.Entities;

namespace Testility.WebUI.Areas.Setup.Models
{
    public class ItemVM
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
    }
}
