using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using Testility.Domain.Entities;

namespace Testility.WebUI.Services.Abstract
{
    [ContractClass(typeof(FileServiceContract))]
    public interface IFileService
    {
        void UploadReference(Reference r, HttpPostedFileBase file);
    }
}