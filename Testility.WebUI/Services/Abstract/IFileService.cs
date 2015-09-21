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
        Task<string> UploadReferenceAsync(Reference r, string path);
    }
}