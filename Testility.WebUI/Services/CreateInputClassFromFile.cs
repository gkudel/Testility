using System.IO;
using System.Web;
using Testility.Domain.Entities;
using Testility.Engine.Model;
using AutoMapper;

namespace Testility.WebUI.Services
{
    public class CreateInputClassFromFile : ICreateInputClassFromFile
    {
        public Input CreateInputClass(SourceCode sourceCode , HttpPostedFileBase file)
        {
            Input input = Mapper.Map<Input>(sourceCode);
            input.Code = new StreamReader(file.InputStream).ReadToEnd();
            return input;
        }
    }
}