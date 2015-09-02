using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Testility.Domain.Entities;
using Testility.WebUI.Services.Abstract;

namespace Testility.WebUI.Services.Concrete
{
    public class CSharpCodeProvider : ICodeProvider
    {
        public string Create(Class c)
        {
            string code = $@"
using System;
using NUnit.Framework;

[TestFixture] 
public class {c.Name}Test 
{{
    [TestFixtureSetUp]    
    public void Init()
    {{                  
    }}     

    [TestFixtureTearDown]                                                          
    public void Dispose()
    {{                  
    }}     
    
    {methods(c.Methods)}    
}}";
            return code.TrimStart();
        }

        private string methods(ICollection<Method> methods)
        {
            string ret = string.Empty;
            foreach (Method m in methods)
            {
                if (m.Tests.Count > 0)
                {
                    ret += $@"
    /*Method[{m.Name}]-{m.Description}*/
";
                }
                ret += method(m);
            }
            return ret.TrimStart();
        }

        private string method(Method method)
        {
            string ret = string.Empty;
            foreach (Domain.Entities.Test t in method.Tests)
            {
                ret += test(method, t);
            }
            return ret;
        }

        private string test(Method method, Domain.Entities.Test t)
        {
            string ret = $@"
    /*{t.Description}*/
    [Test]
    public  void {t.Name}_Test()
    {{
    }}
";
            return ret;
        }
    }
}