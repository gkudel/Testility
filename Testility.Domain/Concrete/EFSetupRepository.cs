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
                return context.Files;
            }
        }

        public void DeleteSourceCode(int id)
        {
            SourceCode sourceCode =  context.Files.Select(a => a).First(b => b.Id == id);
            context.Files.Remove((sourceCode));
            Commit();
        }

        public SourceCode GetSourceCode(int? id)
        {
            return  context.Files.Select(a => a).First(b => b.Id == id);
        }

        public void SaveSourceCode(SourceCode sourcode)
        {
             SourceCode sourceCode = context.Files.Select(a => a).First(b => b.Id == sourcode.Id);
                if (sourceCode!=null)
                {
                    context.Entry(sourceCode).CurrentValues.SetValues(sourcode);
                    Commit();
                }
                else
                {
                    context.Files.Add(sourcode);
                  
                }
                Commit();
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
