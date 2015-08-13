using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Testility.WebUI.Areas.Setup.Models;

namespace Testility.WebUI.Infrastructure.Atrribute
{
    public class ValidateFileAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var model = value as EditViewModel;
            if (model == null)
            {
                return false;
            }
            if (model.SourceCode == null)
            {
                return false;
            }
            if (model.SourceCode.Id == 0 && model.UploadedFile == null)
            {
                return false;
            }
            return true;
        }
    }
}
