using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Entities;

namespace Testility.Domain.Abstract
{
    public interface ISetupRepository : IDisposable
    {
        IQueryable<Solution> GetSolutions(bool lazyloading = true);
        Solution GetSolution(int id);
        void Save(Solution solution, int[] references);
        bool Delete(int id);
        bool IsAlreadyDefined(string name, int? id = null);
        IQueryable<Reference> GetReferences();
        Reference GetReference(int id);
        void SaveReferences(Reference reference);
        bool DeleteReferences(int id);
        string[] GetSelectedReferencesNames(int[] ids);        
    }
}
