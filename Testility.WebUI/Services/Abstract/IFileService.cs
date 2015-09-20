using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;
using Testility.Domain.Entities;

namespace Testility.WebUI.Services.Abstract
{
    public interface IFileService
    {
        void UploadReference(string s);
    }
}