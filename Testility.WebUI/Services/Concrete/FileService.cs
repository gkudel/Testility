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
        public async Task<string> UploadReferenceAsync(Reference r, string path)
        {
            if (File.Exists(path))
            {
                string destpath = Path.Combine(filesPath.GetReferencesDirectory(), r.Id.ToString() + ".dll");
                using (FileStream SourceStream = File.Open(path, FileMode.Open))
                {                    
                    using (FileStream DestinationStream = File.Create(destpath))
                    {
                        await SourceStream.CopyToAsync(DestinationStream);
                    }                    
                }
                try { File.Delete(path); } catch (Exception) { }
                return destpath;
            }
            return string.Empty;
        }
    }
}
