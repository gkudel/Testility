using System.Collections.Generic;
using Testility.Domain.Entities;

namespace Testility.Domain.Abstract
{
    public interface ITestedMethodsRepository
    {
        IList<TestedMethod> GetMethods();
        void SaveTestedMethod(TestedMethod testedMethod);
        void Commit();
    }
}
