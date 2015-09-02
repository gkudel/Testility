using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Testility.Domain.Entities;
using Testility.WebUI.Services.Abstract;

namespace Testility.WebUI.Services.Concrete
{
    public class TestGenarator : ITestGenarator
    {
        public string Create(Class c, Language l)
        {
            return CodeProvider.CreateProvider(l).Create(c);
        }                    
    }
}