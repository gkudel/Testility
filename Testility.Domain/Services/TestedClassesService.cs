using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;

namespace Testility.Domain.Concrete
{
    public class TestedClassesService : ITestedClassesRepository, IDisposable
    {
        private EFDbContext context;

        public TestedClassesService(EFDbContext context)
        {
            this.context = context;
        }

        
        public IList<TestedClass> GetTestetClasses()
        {
            return context.TestedClasses.ToList();
        }


        

        public void AddTestClasses(TestedClass testedClass)
        {
            if (testedClass != null)
            {
                context.TestedClasses.Add(testedClass);
                Commit();
            }

        }

        public string GetClassName(int id) 
        {
            return context.TestedClasses.Where(b => b.Id == id).Select(a => a.Name).SingleOrDefault();
        }

        public void DeleteTestClasses(int id)
        {
            TestedClass testedClass = context.TestedClasses.Select(a => a).FirstOrDefault(b => b.Id == id);
            if (testedClass != null)
            {
                context.TestedClasses.Remove(testedClass);
                Commit();
            }
        }

        public void UpdateTestClasses(TestedClass testedClass)
        {
            TestedClass orgClass = context.TestedClasses.Select(a => a).FirstOrDefault(b => b.Id == testedClass.Id);

            if (testedClass != null)
            {
                if (orgClass != null)
                {
                    orgClass.Id = testedClass.Id;
                    orgClass.Description = testedClass.Description;
                    orgClass.Methods = testedClass.Methods;
                    orgClass.Name = testedClass.Name;
                    orgClass.SourceCode = testedClass.SourceCode;
                    Commit();
                }
                
            }
        }

        public TestedClass DetailsTestedClass(int? id)
        {
            TestedClass orgClass = context.TestedClasses.Select(a => a).FirstOrDefault(b => b.Id == id);
            if (orgClass != null)
            {
                return orgClass;
            }

            return null;

        }

        public void Commit()
        {
            context.SaveChanges();
        }


        public void Dispose()
        {
            context.Dispose();
        }
    }
}
