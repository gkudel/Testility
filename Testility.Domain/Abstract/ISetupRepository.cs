using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Entities;

namespace Testility.Domain.Abstract
{
    public interface ISetupRepository : IDisposable
    {
        IQueryable<SetupSolution> GetSolutions(bool lazyloading = true);
        SetupSolution GetSolution(int id);
        void Save(SetupSolution solution, int[] references);
        bool DeleteSolution(int id);
        bool IsAlreadyDefined(string name, int? id = null);
        IQueryable<Reference> GetReferences();
        Reference GetReference(int id);
        void Save(Reference reference);
        bool DeleteReference(int id);
        string[] GetSelectedReferencesNames(int[] ids);
        IEnumerable<DbEntityValidationResult> GetValidationErrors();
    }
}
