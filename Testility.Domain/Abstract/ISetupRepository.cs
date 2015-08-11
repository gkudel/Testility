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
        void SaveResultToDb(SourceCode sourceCode, TestedClass testedClass);
        void SaveMethodsToDb(TestedClass testedClass, TestedMethod testedMethod);
        void SaveTestsToDb(TestedMethod testedMethod, Test test);

      
        TestedClass GetTestedClass(int sourceCodeId);
        TestedMethod GetTestedMethod(string name);
        Test GetTest(string name);

    }


}
