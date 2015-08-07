using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Entities;

namespace Testility.Domain.Abstract
{
    public interface ITestedClassesRepository
    {
        //IQueryable<TestedClass> TestedClasses { get; set; }

        IList<TestedClass> GetTestetClasses();
        void AddTestClasses(TestedClass tested);
        TestedClass DetailsTestedClass(int? id);
        string GetClassName(int id);
        void DeleteTestClasses(int id);
        void UpdateTestClasses(TestedClass testedClass);
        void Commit();



        
    }
}
