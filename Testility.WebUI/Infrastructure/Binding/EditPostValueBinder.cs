using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Testility.Domain.Entities;
using Testility.WebUI.Areas.Setup.Models;

namespace Testility.WebUI.Infrastructure.Binding
{
    public class EditPostValueBinder : IModelBinder
    {
        public string Include { get; set; }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            EditViewModel model = (EditViewModel)bindingContext.Model ?? new EditViewModel();
            model.SourceCode = new SourceCode();
            foreach (string property in bindingContext.PropertyMetadata.Keys)
            {
                if (bindingContext.PropertyFilter(property))
                {                    
                }
            }
            //bindingContext.ValueProvider.GetValue(bindingContext.ModelName).RawValue as SourceCode;
            model.UploadedFile = bindingContext.ValueProvider.GetValue("UploadedFile").RawValue as HttpPostedFileBase;
            return model;
        }
    }
}
