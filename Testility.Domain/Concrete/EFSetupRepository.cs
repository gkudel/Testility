using System;
using System.Linq;
using System.Xml.Linq;
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
                     Mapper.Map(sourcode , sourceCode);
                     Commit();
                }
                else
                {
                    context.SourCodes.Add(sourcode);
                    Commit();
                }
            }

        }

        public void SaveResultToDb(SourceCode sourceCode, TestedClass testedClass)
        {
                context.SourCodes.Add(sourceCode);
                testedClass.SourceCode = sourceCode;
                context.TestedClasses.Add(testedClass);
                Commit();
        }

        public void SaveMethodsToDb(TestedClass testedClass, TestedMethod testedMethod)
        {
            testedMethod.TestedClass = testedClass;
            context.TestedMethods.Add(testedMethod);
        }

        public void SaveTestsToDb(TestedMethod testedMethod, Test test)
        {
            test.TestedMethod = testedMethod;
            context.Tests.Add(test);
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
