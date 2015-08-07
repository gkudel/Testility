using System;
using System.Linq;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;
using AutoMapper;

namespace Testility.Domain.Concrete
{
    public class EFSetupRepository : ISetupRepository, IDisposable
    {
       private EFDbContext context;

        public EFSetupRepository(EFDbContext context)
        {
            this.context = context;
        }

        public IQueryable<SourceCode> SourceCodes
        {
            get
            {
                return context.SourCodes;
            }
        }

        public bool DeleteSourceCode(int id)
        {
<<<<<<< HEAD

            SourceCode sourceCode =  context.SourCodes.Select(a => a).FirstOrDefault(b => b.Id == id);
            if (sourceCode!=null)
            {
                context.SourCodes.Remove((sourceCode));
                Commit();
                return true;
            }

            return false;
=======
            SourceCode sourceCode =  context.SourCodes.Find(id);
            context.SourCodes.Remove((sourceCode));
            Commit();
>>>>>>> 00cdf21638cfd1ff18c084182c9c535188464a53
        }

        public SourceCode GetSourceCode(int? id)
        {
            return  context.SourCodes.Find(id);
        }

        public void SaveSourceCode(SourceCode sourcode)
        {
            if (sourcode != null)
            {
                SourceCode sourceCode = context.SourCodes.Find(sourcode.Id);
                if (sourceCode != null)
                {
                    context.Entry(sourceCode).CurrentValues.SetValues(sourcode);
                    Commit();
                }
                else
                {
                    context.SourCodes.Add(sourcode);
                    Commit();
                }
            }

        }

        private void Commit()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
