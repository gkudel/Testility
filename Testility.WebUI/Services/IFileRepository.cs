using System;
using System.Web;

namespace Testility.WebUI.Services
{
    public interface IFileRepository
    {
        void Save(HttpPostedFileBase file, Func<string, string> urlFunc);

    }
}
