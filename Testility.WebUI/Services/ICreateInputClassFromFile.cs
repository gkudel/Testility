using System.Web;
using Testility.Domain.Entities;
using Testility.Engine.Model;

namespace Testility.WebUI.Services
{
    public interface ICreateInputClassFromFile
    {
        Input CreateInputClass(SourceCode sourceCode, HttpPostedFileBase file);
    }
}
