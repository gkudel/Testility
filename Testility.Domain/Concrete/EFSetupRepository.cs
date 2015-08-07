using System;
using System.Linq;
using System.Xml.Linq;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;

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
            SourceCode sourceCode =  context.SourCodes.Select(a => a).FirstOrDefault(b => b.Id == id);
            if (sourceCode!=null)
            {
                context.SourCodes.Remove((sourceCode));
                Commit();
                return true;
            }

            return false;
        }

        public SourceCode GetSourceCode(int? id)
        {
            return  context.SourCodes.Find(id);
        }

        public void SaveSourceCode(SourceCode sourcode)
        {
            if (sourcode != null)
            {
                SourceCode sourceCode = context.SourCodes.FirstOrDefault(a =>a.Id == sourcode.Id);
                if (sourceCode != null)
                {
                    //Brzydkie ale działa 
                    sourceCode.Id = sourcode.Id;
                    sourceCode.Name = sourcode.Name;
                    sourceCode.Clasess = sourcode.Clasess;
                    sourceCode.Code = sourcode.Code;
                    sourceCode.Language = sourcode.Language;
                    sourceCode.ReferencedAssemblies = sourcode.ReferencedAssemblies;
                    //context.Entry(sourceCode).CurrentValues.SetValues(sourcode);
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
