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
    public interface IDbRepository : IDisposable
    {
        IQueryable<SetupSolution> GetSetupSolutions(bool lazyloading = true);
        SetupSolution GetSetupSolution(int id);
        void SaveSetupSolution(SetupSolution solution, int[] references);
        IQueryable<UnitTestSolution> GetUnitTestSolutions(bool lazyloading = true);
        UnitTestSolution GetUnitTestSolution(int id);
        void SaveUnitTestSolution(UnitTestSolution solution, int[] references);
        bool DeleteSetupSolution(int id);
        bool DeleteUnitSolution(int id);
        bool IsAlreadyDefined(string name, int? id = null);
        IQueryable<Reference> GetReferences();
        Reference GetReference(int id);
        void Save(Reference reference);
        bool DeleteReference(int id);
        string[] GetSelectedReferencesNames(int[] ids);
        IEnumerable<DbEntityValidationResult> GetValidationErrors();
    }
}
