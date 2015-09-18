using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using Testility.Domain.Entities;

namespace Testility.WebUI.Services.Abstract
{
    [ContractClassFor(typeof(IFileService))]
    abstract public class FileServiceContract : IFileService
    {
        public string UploadReference(Reference r, HttpPostedFileBase file)
        {
            Contract.Requires<ArgumentNullException>(file != null);
            Contract.Requires<ArgumentNullException>(r != null);
            return string.Empty;
        }
    }
}