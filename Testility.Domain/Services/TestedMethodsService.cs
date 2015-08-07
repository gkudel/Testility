using System.Collections.Generic;
using System.Linq;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;


namespace Testility.Domain.Concrete
{
    public class TestedMethodsService : ITestedMethodsRepository
    {
        private EFDbContext context;

        public TestedMethodsService(EFDbContext context)
        {
            this.context = context;
        }

        public IList<TestedMethod> GetMethods()
        {
            return context.TestedMethods.Select(a => a).ToList();

        }


        public void SaveTestedMethod(TestedMethod testedMethod) 
        {
            if (testedMethod != null)
            {
                context.TestedMethods.Add(testedMethod);
                Commit();
            }
        }


        public void Commit() 
        {
            context.SaveChanges();
        }
    }
}
