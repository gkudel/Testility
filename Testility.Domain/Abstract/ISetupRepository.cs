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
        IQueryable<SourceCode> SourceCodes { get; }
        bool Delete(int id);
        SourceCode GetSourceCode(int? id, bool lazyLoading = true);
        void Save(SourceCode sourceCode);
    }


}
