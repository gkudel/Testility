using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Entities;

namespace Testility.Domain.Abstract
{
    public interface ISetupRepository
    {
        
        bool Delete(int id);
        SourceCode GetSourceCode(int? id, bool lazyLoading = true);
        void Save(SourceCode sourceCode);
        bool IsUniqueName(string name);
        bool IsUnique(string name, int id);
        IQueryable<SourceCode> GetAllSourceCodes();


        TestedClass GetTestedClass(int? id, bool lazyLoading = true);

    }
}
