using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Testility.Domain.Entities;

namespace Testility.WebUI.Areas.Setup.Models
{
    public class EditViewModel
    {
        public SourceCode SourceCode { get; set; }
        public HttpPostedFileBase UploadedFile { get; set; }
    }
}
