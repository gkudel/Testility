using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;
using Testility.WebUI.Model;

namespace Testility.WebUI.Infrastructure.Binders
{
    public class FlowChunkBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof(FlowChunk))
            {
                return false;
            }
            var req = HttpContext.Current.Request;
            var model = (FlowChunk)bindingContext.Model ?? new FlowChunk();
            try
            {
                if (string.IsNullOrEmpty(req.Form["flowIdentifier"]) || string.IsNullOrEmpty(req.Form["flowFilename"]))
                    return false;

                model.Number = int.Parse(req.Form["flowChunkNumber"]);
                model.Size = long.Parse(req.Form["flowChunkSize"]);
                model.TotalSize = long.Parse(req.Form["flowTotalSize"]);
                model.Identifier = CleanIdentifier(req.Form["flowIdentifier"]);
                model.FileName = req.Form["flowFilename"];
                model.TotalChunks = int.Parse(req.Form["flowTotalChunks"]);
                model.Files = req.Files;
                bindingContext.Model = model;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private string CleanIdentifier(string identifier)
        {
            identifier = Regex.Replace(identifier, "/[^0-9A-Za-z_-]/g", "");
            return identifier;
        }

    }
}
