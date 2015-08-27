using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Testility.Domain.Entities;

namespace Testility.WebUI.Areas.Setup.Model
{    
    public class SolutionViewModel
    {
        private string _json = string.Empty;
        public int Id { get; set; }
        [Required(ErrorMessage = "Solution Name is requred")]
        [StringLength(100)]
        [Remote("IsNameUnique", "Validation", ErrorMessage = "Solution already exists.", AdditionalFields = "Id")]
        public string Name { get; set; }
        public Language Language { get; set; }
        [Display(Name = "References")]
        public int[] References { get; set; }
        public virtual ICollection<ItemViewModel> Items { get; set; }
        [ScriptIgnore]
        public bool IncludeJson { get; set; }
        [ScriptIgnore]
        public string Json
        {
            get
            {
                if (IncludeJson && string.IsNullOrEmpty(_json))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    _json = js.Serialize(this);
                }
                return _json;
            }
        }
    }
}
