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

        public IQueryable<Solution> GetSolutions()
        {
            return context.Solutions;
        }

        public Solution GetSolution(int id)
        {
            var query = context.Solutions.Where(s => s.Id == id).Include(s => s.Items);
            return query.FirstOrDefault();
        }

        public void Save(Solution solution)
        {
            if (solution.Id == 0)
            {
                context.Solutions.Add(solution);
            }
            else
            {
                /*var classes = context.Classes.Where(c => c.SolutionId == solution.Id).ToList();                    
                foreach (Class c in classes)
                {
                    if (solution.Classes.FirstOrDefault(i => i.Id == c.Id) != null)
                    {
                        var methods = context.Methods.Where(m => m.ClassId == c.Id).ToList();
                        foreach (Method m in methods)
                        {
                            if (c.Methods.FirstOrDefault(i => i.Id == m.Id) != null)
                            {
                                var tests = context.Tests.Where(t => t.MethodId == m.Id).ToList();
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
                                context.Methods.Remove(m);
                            }
                        }
                    }
                    else
                    {
                        context.Classes.Remove(c);
                    }
                }*/
            }
            Commit();
        }

        public bool Delete(int id)
        {
            Solution solution = context.Solutions.FirstOrDefault(s => s.Id == id);
            if (solution != null)
            {
                context.Solutions.Remove(solution);
                Commit();
                return true;
            }
            return false;
        }

        public bool IsAlreadyDefined(string name, int? id = null)
        {
            return context.Solutions.FirstOrDefault(s => s.Name == name && (id == null || s.Id != id)) != null;
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
