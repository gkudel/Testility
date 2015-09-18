using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Entities;
using Testility.WebUI.Services.Abstract;

namespace Testility.WebUI.Services.Concrete
{
    public static class CodeProvider
    {
        public static ICodeProvider CreateProvider(Language l)
        {
            Contract.Ensures(Contract.Result<ICodeProvider>() != null);
            ICodeProvider ret = null;
            switch (l)
            {
                case Language.CSharp: ret = new CSharpCodeProvider();
                    break;
            }

            return ret;
        }
    }
}
