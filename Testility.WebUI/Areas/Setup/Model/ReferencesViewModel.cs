using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Testility.WebUI.Infrastructure.CustomValidation;

namespace Testility.WebUI.Areas.Setup.Model
{
    public class ReferencesViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must enter name for Assembly")]
        [CheckReferenceExtensions(".dll", ErrorMessage = "Please enter a valid assembly name.")]
        public string Name { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
    }
}