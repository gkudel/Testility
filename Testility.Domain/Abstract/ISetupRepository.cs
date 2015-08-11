using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Entities;

namespace Testility.Domain.Abstract
{
    public interface ISetupRepository
    {
        IQueryable<SourceCode> SourceCodes { get;  }
        bool DeleteSourceCode(int id);
        SourceCode GetSourceCode(int? id);
        void AddResultToDb(SourceCode sourceCode, TestedClass testedClass);
        void AddMethodsToDb(TestedClass testedClass, TestedMethod testedMethod);
        void AddTestsToDb(TestedMethod testedMethod, Test test);

    }


}
