using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;
using Testility.WebUI.Services.Abstract;

namespace Testility.WebUI.Services.Concrete
{
    public class FileService : IFileService
    {
        private readonly IFilesPath filesPath;
        public FileService(IFilesPath filesPath, IDbRepository dbRepository)
        {
            this.filesPath = filesPath;
        }

        public string UploadReference(string path)
        {
            if (File.Exists(path))
            {
                string destpath = Path.Combine(filesPath.GetReferencesDirectory(), Path.GetFileName(path));
                File.Move(path, destpath);                
                return Path.GetFileName(destpath);
            }
            return string.Empty;
        }

        public void DeleteReference(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                string destpath = Path.Combine(filesPath.GetReferencesDirectory(), name);
                if (File.Exists(destpath))
                {
                    try { File.Delete(destpath); } catch (Exception) { /*TODO add To Logs */ }
                }
            }
        }
    }
}
