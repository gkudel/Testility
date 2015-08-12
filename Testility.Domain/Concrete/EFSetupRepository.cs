using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Xml.Linq;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;
using AutoMapper;
using System.Linq.Expressions;

namespace Testility.Domain.Concrete
{
    public class EFSetupRepository : ISetupRepository, IDisposable
    {
       private EFDbContext context;

        public EFSetupRepository(EFDbContext context)
        {
            this.context = context;
        }


        public IList<SourceCode> GetAllSourceCodes()
        {
            return context.SourCodes.ToList();
        }

        public bool Delete(int id)
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

        public SourceCode GetSourceCode(int? id, bool lazyLoading = true)
        {
            var query = context.SourCodes.Where(s => s.Id == id);
            if (!lazyLoading)
            {
                query = query.Include("Clasess.Methods.Tests");
            }
            return query.FirstOrDefault();
        }

        public void Save(SourceCode sourceCode)
        {
            if (sourceCode.Id == 0)
            {
                context.SourCodes.Add(sourceCode);
            }
           /* else
            {
                foreach(TestedClass c in context.TestedClasses.Where(c => c.SourceCodeId == sourceCode.Id))
                {
                    if (sourceCode.Clasess.FirstOrDefault(i => i.Id == c.Id) != null)
                    {
                        foreach (TestedMethod m in context.TestedMethods.Where(m => m.TestedClassId == c.Id))
                        {
                            if (c.Methods.FirstOrDefault(i => i.Id == m.Id) != null)
                            {
                                foreach (Test t in context.Tests.Where(t => t.TestedMethodId == m.Id))
                                {
                                    if (m.Tests.FirstOrDefault(i => i.Id == t.Id) == null)
                                    {
                                        context.Entry(t).State = EntityState.Deleted;
                                    }
                                }
                            }
                            else
                            {
                                context.Entry(m).State = EntityState.Deleted;
                            }
                        }
                    }
                    else
                    {
                        context.Entry(c).State = EntityState.Deleted;
                    }
                }
            }*/
            Commit();
        }

        public bool CheckSourceCodeNameIsUnique(string name)
        {
            SourceCode sourceCode = context.SourCodes.FirstOrDefault(b => b.Name == name);
            if (sourceCode != null)
                return false;
            return true;
        }

        private void Update<T>(T o, DbSet<T> dbSet) where T : class
        {            
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
