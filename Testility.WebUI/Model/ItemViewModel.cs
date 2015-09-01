using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Testility.WebUI.Model
{
    public class ItemViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Item Name is requred")]
        [StringLength(100)]
        public string Name { get; set; }
        public string Code { get; set; }
        public int SolutionId { get; set; }
    }
}