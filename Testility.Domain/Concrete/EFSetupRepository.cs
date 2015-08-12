using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public void AddResultToDb(SourceCode sourceCode, TestedClass testedClass)
        {
                context.SourCodes.Add(sourceCode);
                testedClass.SourceCode = sourceCode;
                context.TestedClasses.Add(testedClass);
                Commit();
        }

        public void AddMethodsToDb(TestedClass testedClass, TestedMethod testedMethod)
        {
            testedMethod.TestedClass = testedClass;
            context.TestedMethods.Add(testedMethod);
        }

        public void AddTestsToDb(TestedMethod testedMethod, Test test)
        {
            test.TestedMethod = testedMethod;
            context.Tests.Add(test);
        }


        public bool CheckSourceCodeNameIsUnique(string name)
        {
             SourceCode sourceCode =  context.SourCodes.Select(a => a).FirstOrDefault(b => b.Name == name);
            if (sourceCode != null)
                return false;
            return true;
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
