using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Testility.WebUI.Areas.Setup.Models;

namespace Testility.WebUI.Services
{
    public class FileService : IFileRepository
    {
        public void Save(HttpPostedFileBase file, Func<string, string> urlFunc )
        {
            if (file.ContentLength > 0) {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(urlFunc(Constants.UploadFolder), fileName);
                file.SaveAs(path);
            }
        }
    }
}