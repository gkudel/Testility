using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;
using System.Data.Entity.Validation;

namespace Testility.Domain.Concrete
{
    public class DbRepository : IDbRepository, IDisposable
    {
        private readonly IDbContext context;
        public DbRepository(IDbContext context)
        {
            this.context = context;                        
        }

        public bool AutoCommit { get; set; } = true;

        public IQueryable<SetupSolution> GetSetupSolutions(bool lazyloading = true)
        {
            var ret = context.SetupSolutions.AsQueryable();
            if (!lazyloading)
            {
                ret = ret.Include(s => s.Items)
                    .Include("Classes.Methods.Tests");
            }
            return ret; 
        }
        
        public SetupSolution GetSetupSolution(int id)
        {
            var query = context.SetupSolutions.Where(s => s.Id == id)
                .Include(s => s.Items)
                .Include("Classes.Methods.Tests");
            return query.FirstOrDefault();
        }

        public void SaveSetupSolution(SetupSolution solution, int[] references)
        {
            var referencedAssemblies = solution.References.Where(r => !references?.Contains(r.Id) ?? true).ToList();
            foreach (Reference r in referencedAssemblies)
            {
                solution.References.Remove(r);
            }
            foreach (int id in references?.Where(id => solution.References.FirstOrDefault(r => r.Id == id) == null) ?? new int[0])
            {
                solution.References.Add(GetReference(id));
            }
            if (solution.Id == 0)
            {
                context.SetupSolutions.Add(solution);
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
                var items = context.Items.Where(i => i.SolutionId == solution.Id).ToList();
                foreach (Item item in items)
                {
                    if (solution.Items.FirstOrDefault(i => i.Id == item.Id) == null)
                    {
                        context.Items.Remove(item);
                    }
                }
            }
            commit();
        }

        public void SaveUnitTestSolution(UnitTestSolution solution, int[] references)
        {
            var referencedAssemblies = solution.References.Where(r => !references?.Contains(r.Id) ?? true).ToList();
            foreach (Reference r in referencedAssemblies)
            {
                solution.References.Remove(r);
            }
            foreach (int id in references?.Where(id => solution.References.FirstOrDefault(r => r.Id == id) == null) ?? new int[0])
            {
                solution.References.Add(GetReference(id));
            }
            if (solution.Id == 0)
            {
                context.UnitTestSolutions.Add(solution);
            }

            var items = context.Items.Where(i => i.SolutionId == solution.Id).ToList();
            foreach (Item item in items)
            {
                if (solution.Items.FirstOrDefault(i => i.Id == item.Id) == null)
                {
                    context.Items.Remove(item);
                }
            }
            commit();
        }        
        public bool DeleteSetupSolution(int id)
        {
            SetupSolution solution = context.SetupSolutions.FirstOrDefault(s => s.Id == id);
            if (solution != null)
            {
                var list = solution.UnitTests?.ToList() ?? new List<UnitTestSolution>();
                foreach (UnitTestSolution u in list)
                {
                    context.UnitTestSolutions.Remove(u);
                }
                context.SetupSolutions.Remove(solution);
                commit();
                return true;
            }
            return false;
        }
        public bool DeleteUnitSolution(int id)
        {
            UnitTestSolution solution = context.UnitTestSolutions.FirstOrDefault(s => s.Id == id);
            if (solution != null)
            {
                context.UnitTestSolutions.Remove(solution);
                commit();
                return true;
            }
            return false;
        }

        public IQueryable<UnitTestSolution> GetUnitTestSolutions(bool lazyloading = true)
        {
            var ret = context.UnitTestSolutions.AsQueryable();
            if (!lazyloading)
            {
                ret = ret.Include(s => s.SetupSolution);                    
            }
            return ret;
        }

        public UnitTestSolution GetUnitTestSolution(int id)
        {
            var query = context.UnitTestSolutions.Where(s => s.Id == id)
                .Include(s => s.SetupSolution);
            return query.FirstOrDefault();
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
            var query = context.References.Where(s => s.Id == id);
            return query.FirstOrDefault();
        }

        public void Save(Reference reference)
        {
            if (reference.Id == 0)
            {
                context.References.Add(reference);
            }
            else
            {
                context.SetState(reference, EntityState.Modified);
            }
            commit();
        }

        public bool DeleteReference(int id)
        {
            Reference references = context.References.FirstOrDefault(s => s.Id == id);
            if (references != null)
            {
                context.References.Remove(references);
                commit();
                return true;
            }
            return false;
        }

        public string[] GetSelectedReferencesNames(int[] ids)
        {
            var query = context.References
                 .Where(r => ids.Contains(r.Id))
                 .Select(r => r.Name);
            return query.ToArray();
        }

        public void Commit()
        {
            context.SaveChanges();
        }


        private void commit()
        {
            if(AutoCommit) context.SaveChanges();            
        }

        public IEnumerable<DbEntityValidationResult> GetValidationErrors()
        {
            return context.GetValidationErrors();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
