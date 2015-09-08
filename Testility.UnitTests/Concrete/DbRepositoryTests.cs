using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Testility.Domain.Concrete;
using Testility.Domain.Entities;
using Testility.UnitTests.DbContextMock;
using Testility.Domain.Abstract;

namespace Testility.Domain.Concrete.Tests
{
    [TestClass]
    public class DbRepositoryTests
    {
        #region Members
        public Mock<IDbContext> MockContext { get; set; }

        public DbRepository Service { get; set; }        
        #endregion


        #region Init
        [TestInitialize]
        public void Init()
        {
            ICollection<SetupSolution> SolutionsData = new HashSet<SetupSolution>
            {
                new SetupSolution() {Id = 2, Name = "12ok"},
                new SetupSolution() {Id = 1, Name = "1"}
            };

            ICollection<UnitTestSolution> UnitSolutionsData = new HashSet<UnitTestSolution>
            {
                new UnitTestSolution() {Id = 1, Name = "1", SetupSolutionId = 1},
                new UnitTestSolution() {Id = 2, Name = "2", SetupSolutionId = 2}
            };

            ICollection<Reference> References = new HashSet<Reference>
            {
                new Reference() {Id = 1, Name = "System.dll"},
                new Reference() {Id = 2, Name = "System.Web.dll"}
            };

            ICollection<Item> ItemsData = new HashSet<Item>
            {
                new Item() {Id = 2, Name = "12ok"}
            };

            ICollection<Class> ClassesData = new HashSet<Class>
            {
                new Class() {Id = 2, Name = "12ok"}
            };

            ICollection<Method> MethodsData = new HashSet<Method>
            {
                new Method() {Id = 2, Name = "12ok"}
            };

            ICollection<Test> TestsData = new HashSet<Test>
            {
                new Test() {Id = 2, Name = "12ok"}
            };

            MockContext = EntityFrameworkMockHelper.GetMockContext<IDbContext>();
            MockContext.Object.SetupSolutions.AddRange(SolutionsData);
            MockContext.Object.UnitTestSolutions.AddRange(UnitSolutionsData);
            MockContext.Object.References.AddRange(References);
            MockContext.Object.Items.AddRange(ItemsData);
            MockContext.Object.Classes.AddRange(ClassesData);
            MockContext.Object.Methods.AddRange(MethodsData);
            MockContext.Object.Tests.AddRange(TestsData);

            Service = new DbRepository(MockContext.Object);
        }
        #endregion

        [TestMethod]
        public void Can_Get_All_Solutions()
        {
            Assert.AreEqual(2, Service.GetSetupSolutions().Count());
        }

        [TestMethod]
        public void Can_Get_Single_Solution()
        {
            Assert.AreEqual("1", Service.GetSetupSolution(1).Name);
        }

        [TestMethod]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void Cannot_Get_Single_Solution()
        {
            Assert.AreEqual(null, Service.GetSetupSolution(10).Name);
        }

        [TestMethod]
        public void Can_Add_Solution()
        {
            SetupSolution solution = new SetupSolution() { Name = "ok" };
            Service.SaveSetupSolution(solution, null);            
            MockContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual(MockContext.Object.SetupSolutions.Count(), 3);
        }

        [TestMethod]
        public void Can_Add_Solution_WhitReferences()
        {
            SetupSolution solution = new SetupSolution() { Name = "ok" };
            Service.SaveSetupSolution(solution, new int[] { 1 } );
            Assert.AreEqual(MockContext.Object.SetupSolutions.Count(), 3);
            MockContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual(solution.References.Count, 1);
            Assert.AreEqual(solution.References.First().Id, 1);
        }

        [TestMethod]
        public void Can_Edit_Solution()
        {
            SetupSolution solution = Service.GetSetupSolution(1);
            Service.SaveSetupSolution(solution, null);
            MockContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void Can_Edit_Solution_RemoveClass()
        {
            SetupSolution solution = Service.GetSetupSolution(1);
            solution.Classes = new HashSet<Class>();
            MockContext.Object.Classes.First().SolutionId = 1;
            Service.SaveSetupSolution(solution, null);
            MockContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual(MockContext.Object.Classes.Count(), 0);
        }

        [TestMethod]
        public void Can_Edit_Solution_RemoveMethod()
        {
            SetupSolution solution = Service.GetSetupSolution(1);
            solution.Classes = new HashSet<Class>() { MockContext.Object.Classes.First() };
            MockContext.Object.Classes.First().SolutionId = 1;
            MockContext.Object.Classes.First().Methods = new HashSet<Method>();
            MockContext.Object.Methods.First().ClassId = 2;
            Service.SaveSetupSolution(solution, null);
            MockContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual(MockContext.Object.Classes.Count(), 1);
            Assert.AreEqual(MockContext.Object.Methods.Count(), 0);
        }

        [TestMethod]
        public void Can_Edit_Solution_RemoveTest()
        {
            SetupSolution solution = Service.GetSetupSolution(1);
            solution.Classes = new HashSet<Class>() { MockContext.Object.Classes.First() };
            MockContext.Object.Classes.First().SolutionId = 1;
            MockContext.Object.Classes.First().Methods = new HashSet<Method>() { MockContext.Object.Methods.First() } ;
            MockContext.Object.Methods.First().ClassId = 2;
            MockContext.Object.Classes.First().Methods.First().Tests = new HashSet<Test>();
            MockContext.Object.Tests.First().MethodId = 2;

            Service.SaveSetupSolution(solution, null);
            MockContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual(MockContext.Object.Classes.Count(), 1);
            Assert.AreEqual(MockContext.Object.Methods.Count(), 1);
            Assert.AreEqual(MockContext.Object.Tests.Count(), 0);
        }

        [TestMethod]
        public void Can_Edit_Solution_RemoveItem()
        {
            SetupSolution solution = Service.GetSetupSolution(1);
            solution.Items = new HashSet<Item>();
            MockContext.Object.Items.First().SolutionId = 1;
            Service.SaveSetupSolution(solution, null);
            MockContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual(MockContext.Object.Items.Count(), 0);
        }

        [TestMethod]
        public void Cannot_Delete_SetupSolutionWithWrongId()
        {
            var result = Service.DeleteSetupSolution(10);

            Assert.AreEqual(MockContext.Object.SetupSolutions.Count(), 2);
            MockContext.Verify(m => m.SaveChanges(), Times.Never);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Can_Delete_SetupSolution()
        {
            var result = Service.DeleteSetupSolution(1);

            Assert.AreEqual(MockContext.Object.SetupSolutions.Count(), 1);
            MockContext.Verify(m => m.SaveChanges(), Times.Once);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Can_Delete_SetupSolution_WithUnitTest()
        {
            SetupSolution s = MockContext.Object.SetupSolutions.First();
            UnitTestSolution u = MockContext.Object.UnitTestSolutions.First();
            int sc = MockContext.Object.SetupSolutions.Count();
            int uc = MockContext.Object.UnitTestSolutions.Count();
            u.SetupSolutionId = s.Id;
            s.UnitTests = new List<UnitTestSolution>();
            s.UnitTests.Add(u);

            var result = Service.DeleteSetupSolution(s.Id);

            Assert.AreEqual(MockContext.Object.SetupSolutions.Count(), sc - 1);
            Assert.AreEqual(MockContext.Object.UnitTestSolutions.Count(), uc - 1);
            MockContext.Verify(m => m.SaveChanges(), Times.Once);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Cannot_Delete_UnitSolutionWithWrongId()
        {
            int uc = MockContext.Object.UnitTestSolutions.Count();

            var result = Service.DeleteUnitSolution(10);

            Assert.AreEqual(MockContext.Object.SetupSolutions.Count(), uc);
            MockContext.Verify(m => m.SaveChanges(), Times.Never);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Can_Delete_UnitSolution()
        {
            int uc = MockContext.Object.UnitTestSolutions.Count();
                    
            var result = Service.DeleteUnitSolution(1);

            Assert.AreEqual(MockContext.Object.UnitTestSolutions.Count(), uc - 1);
            MockContext.Verify(m => m.SaveChanges(), Times.Once);
            Assert.AreEqual(true, result);
        }

    }
}
