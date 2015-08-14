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

        public IQueryable<SourceCode> GetAllSourceCodes()
        {
            return context.SourCodes;
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
            else
            {
                var classes = context.TestedClasses.Where(c => c.SourceCodeId == sourceCode.Id).ToList();                    
                foreach (TestedClass c in classes)
                {
                    if (sourceCode.Clasess.FirstOrDefault(i => i.Id == c.Id) != null)
                    {
                        var methods = context.TestedMethods.Where(m => m.TestedClassId == c.Id).ToList();
                        foreach (TestedMethod m in methods)
                        {
                            if (c.Methods.FirstOrDefault(i => i.Id == m.Id) != null)
                            {
                                var tests = context.Tests.Where(t => t.TestedMethodId == m.Id).ToList();
                                foreach (Test t in tests)
                                {
                                    if (m.Tests.FirstOrDefault(i => i.Id == t.Id) == null)
                                    {
                                        context.Tests.Remove(t);
                                    }
                                }
                            }
                            else
                            {
                                context.TestedMethods.Remove(m);
                            }
                        }
                    }
                    else
                    {
                        context.TestedClasses.Remove(c);
                    }
                }
            }
            Commit();
        }

        public bool IsUnique(string name, int id)
        {
            SourceCode sourceCode = context.SourCodes.FirstOrDefault(b => b.Name == name && b.Id == id);
            if (sourceCode != null)
                return false;
            return true;
        }

        public bool IsUniqueName(string name)
        {
            SourceCode sourceCode = context.SourCodes.FirstOrDefault(b => b.Name == name);
            if (sourceCode != null)
                return false;
            return true;
        }

        public TestedClass GetTestedClass(int? id, bool lazyLoading = true)
        {
            var query = context.TestedClasses.FirstOrDefault(a => a.Id == id);
            if (!lazyLoading)
            {

            }
            return query;
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
