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
            var query = context.Solutions.Where(s => s.Id == id)
                .Include(s => s.Items)
                .Include("Classes.Methods.Tests");
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
                var classes = context.Classes.Where(c => c.SolutionId == solution.Id).ToList();                    
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
                }
                var newclasses = solution.Classes.ToList();
                foreach (Class c in newclasses)
                {
                    if (c.Id == 0)
                    {
                        c.SolutionId = solution.Id;
                        context.Classes.Add(c);
                    }
                    var newmethods = c.Methods.ToList();
                    foreach(Method m in newmethods)
                    {
                        if (m.Id == 0)
                        {
                            m.ClassId = c.Id;
                            context.Methods.Add(m);
                        }
                        var newtests = m.Tests.ToList();
                        foreach (Test t in newtests)
                        {
                            if (t.Id == 0)
                            {
                                t.MethodId = m.Id;
                                context.Tests.Add(t);
                            }
                        }
                    }
                }
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


        public IQueryable<Reference> GetReferences()
        {
            return context.References;
        }


        public Reference GetReference(int id)
        {
            var query = context.References.Where(s => s.Id == id)
                .Include("Solutions.Classes.Methods.Tests");
            return query.FirstOrDefault();
        }

        public void SaveReferences(Reference reference)
        {
            if (reference.Id == 0)
            {
                context.References.Add(reference);
            }
            else
            {
                context.References.Attach(reference);
                context.Entry(reference).State = EntityState.Modified;
            }
            Commit();
        }



        public bool DeleteReferences(int id)
        {
            Reference references = context.References.FirstOrDefault(s => s.Id == id);
            if (references != null)
            {
                context.References.Remove(references);
                Commit();
                return true;
            }
            return false;
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
