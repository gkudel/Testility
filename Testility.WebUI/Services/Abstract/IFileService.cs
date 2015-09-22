using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Testility.Domain.Entities;

namespace Testility.WebUI.Services.Abstract
{
    public interface IFileService
    {
        string UploadReference(string path);
        void DeleteReference(string name);
    }
}