using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Testility.Domain.Entities;
using Testility.WebUI.Infrastructure.Attributes;

namespace Testility.WebUI.Model
{
    public class SolutionViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Solution Name is required")]
        [StringLength(100)]
        [RemoteApi("IsNameUnique", "api/Solution", "", HttpMethod = "GET", ErrorMessage = "Solution already exists.", AdditionalFields = "Id")]
        public string Name { get; set; }
        public Language Language { get; set; }
        [Display(Name = "References")]
        public int[] References { get; set; }
        public ItemViewModel[] Items { get; set; }
        public string Title { get; set; }

    }
}
