using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Testility.Domain.Entities;
using Testility.WebUI.Infrastructure.Atrribute;

namespace Testility.WebUI.Areas.Setup.Models
{
    [ValidateFile(ErrorMessage = "File is required for new records.")]
    public class EditViewModel
    {
        public SourceCode SourceCode { get; set; }
        public HttpPostedFileBase UploadedFile { get; set; }
    }
}
