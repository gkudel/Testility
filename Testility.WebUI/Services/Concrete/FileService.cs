using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Testility.Domain.Entities;
using Testility.WebUI.Services.Abstract;

namespace Testility.WebUI.Services.Concrete
{
    public class FileService : IFileService
    {
        public const string ReferencesDirectory = "References";

        public void UploadReference(Reference r, HttpPostedFileBase file)
        {
            Contract.Requires<ArgumentNullException>(r != null);
            Contract.Requires(file != null);

            string path = HostingEnvironment.ApplicationPhysicalPath.ToString();
            var fullPath = Path.Combine(path, ReferencesDirectory, r.Id.ToString());

            file.SaveAs(fullPath);
        }
    }
}
