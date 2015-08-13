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
        
        bool Delete(int id);
        SourceCode GetSourceCode(int? id, bool lazyLoading = true);
        void Save(SourceCode sourceCode);
        bool CheckSourceCodeNameIsUnique(string name);
        IList<SourceCode> GetAllSourceCodes();

    }
}
