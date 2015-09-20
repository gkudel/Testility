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
        private readonly IFilesPath filesPath;
        public FileService(IFilesPath filesPath)
        {
            this.filesPath = filesPath;
        }

        public void UploadReference(string s)
        {
            /*string path
            File.Copy()*/
        }
    }
}
