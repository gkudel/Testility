using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public IQueryable<SourceCode> Files
        {
            get
            {
                return context.Files;
            }
        }


        public void SourceCode(SourceCode sourcode)
        {
            SourceCode orgSourceCode = context.Files.Select(a => a).First(b => b.Id == sourcode.Id);
            orgSourceCode.Id = sourcode.Id;
            orgSourceCode.Clasess = sourcode.Clasess;
            orgSourceCode.Code = sourcode.Code;
            orgSourceCode.Language = sourcode.Language;
            orgSourceCode.Name = sourcode.Name;
            orgSourceCode.ReferencedAssemblies = sourcode.ReferencedAssemblies;
            Commit();


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
            context.Files.Add(sourcode);
            Commit();
        }

        //public IQueryable<SourceCode> GetAllSourceCodes()
        //{
        //    return context.Files;
        //}

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
