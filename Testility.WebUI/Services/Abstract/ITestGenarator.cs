using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Entities;

namespace Testility.WebUI.Services.Abstract
{
    public interface ITestGenarator
    {
        string Create(Class c, Language l);
    }
}
