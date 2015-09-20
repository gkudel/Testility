using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Testility.WebUI.Services.Abstract;

namespace Testility.WebUI.Services.Concrete
{
    public class FilesPath : IFilesPath
    {
        private string rootPath;
        public FilesPath()
        {
            rootPath = HttpContext.Current.Server.MapPath("~/Upload/");
        }

        public string GetFlowJsTempDirectory()
        {
            return Path.Combine(rootPath, "FlowJs");
        }
    }
}