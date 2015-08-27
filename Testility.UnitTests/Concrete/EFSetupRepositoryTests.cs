using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Testility.Domain.Concrete;
using Testility.Domain.Entities;
using Testility.UnitTests.DbContextMock;

namespace Testility.Domain.Concrete.Tests
{
    [TestClass]
    public class EFSetupRepositoryTests
    {
        #region Members
        public Mock<EFDbContext> MockContext { get; set; }

        public EFSetupRepository Service { get; set; }        
        #endregion


        #region Init
        [TestInitialize]
        public void Init()
        {
            ICollection<Solution> SolutionsData = new HashSet<Solution>
            {
                new Solution() {Id = 2, Name = "12ok"},
                new Solution() {Id = 1, Name = "1"}
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

            MockContext = EntityFrameworkMockHelper.GetMockContext<EFDbContext>();
            MockContext.Object.Solutions.AddRange(SolutionsData);
            MockContext.Object.References.AddRange(References);
            MockContext.Object.Items.AddRange(ItemsData);
            MockContext.Object.Classes.AddRange(ClassesData);
            MockContext.Object.Methods.AddRange(MethodsData);
            MockContext.Object.Tests.AddRange(TestsData);

            Service = new EFSetupRepository(MockContext.Object);
        }
        #endregion

        [TestMethod]
        public void Can_Get_All_Solutions()
        {
            Assert.AreEqual(2, Service.GetSolutions().Count());
        }

        [TestMethod]
        public void Can_Get_Single_Solution()
        {
            Assert.AreEqual("1", Service.GetSolution(1).Name);
        }

        [TestMethod]
        [ExpectedException(typeof(System.NullReferenceException))]
        public void Cannot_Get_Single_Solution()
        {
            Assert.AreEqual(null, Service.GetSolution(10).Name);
        }

        [TestMethod]
        public void Can_Add_Solution()
        {
            Solution solution = new Solution() { Name = "ok" };
            Service.Save(solution, null);            
            MockContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual(MockContext.Object.Solutions.Count(), 3);
        }

        [TestMethod]
        public void Can_Add_Solution_WhitReferences()
        {
            Solution solution = new Solution() { Name = "ok" };
            Service.Save(solution, new int[] { 1 } );
            Assert.AreEqual(MockContext.Object.Solutions.Count(), 3);
            MockContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual(solution.References.Count, 1);
            Assert.AreEqual(solution.References.First().Id, 1);
        }

        [TestMethod]
        public void Can_Edit_Solution()
        {
            Solution solution = Service.GetSolution(1);
            Service.Save(solution, null);
            MockContext.Verify(x => x.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void Can_Edit_Solution_RemoveClass()
        {
            Solution solution = Service.GetSolution(1);
            solution.Classes = new HashSet<Class>();
            MockContext.Object.Classes.First().SolutionId = 1;
            Service.Save(solution, null);
            MockContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual(MockContext.Object.Classes.Count(), 0);
        }

        [TestMethod]
        public void Can_Edit_Solution_RemoveMethod()
        {
            Solution solution = Service.GetSolution(1);
            solution.Classes = new HashSet<Class>() { MockContext.Object.Classes.First() };
            MockContext.Object.Classes.First().SolutionId = 1;
            MockContext.Object.Classes.First().Methods = new HashSet<Method>();
            MockContext.Object.Methods.First().ClassId = 2;
            Service.Save(solution, null);
            MockContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual(MockContext.Object.Classes.Count(), 1);
            Assert.AreEqual(MockContext.Object.Methods.Count(), 0);
        }

        [TestMethod]
        public void Can_Edit_Solution_RemoveTest()
        {
            Solution solution = Service.GetSolution(1);
            solution.Classes = new HashSet<Class>() { MockContext.Object.Classes.First() };
            MockContext.Object.Classes.First().SolutionId = 1;
            MockContext.Object.Classes.First().Methods = new HashSet<Method>() { MockContext.Object.Methods.First() } ;
            MockContext.Object.Methods.First().ClassId = 2;
            MockContext.Object.Classes.First().Methods.First().Tests = new HashSet<Test>();
            MockContext.Object.Tests.First().MethodId = 2;

            Service.Save(solution, null);
            MockContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual(MockContext.Object.Classes.Count(), 1);
            Assert.AreEqual(MockContext.Object.Methods.Count(), 1);
            Assert.AreEqual(MockContext.Object.Tests.Count(), 0);
        }

        [TestMethod]
        public void Can_Edit_Solution_RemoveItem()
        {
            Solution solution = Service.GetSolution(1);
            solution.Items = new HashSet<Item>();
            MockContext.Object.Items.First().SolutionId = 1;
            Service.Save(solution, null);
            MockContext.Verify(x => x.SaveChanges(), Times.Once);
            Assert.AreEqual(MockContext.Object.Items.Count(), 0);
        }

        [TestMethod]
        public void Cannot_DeleteSolutionWithWrongId()
        {
            var result = Service.DeleteSolution(10);

            Assert.AreEqual(MockContext.Object.Solutions.Count(), 2);
            MockContext.Verify(m => m.SaveChanges(), Times.Never);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Can_DeleteSolution()
        {
            var result = Service.DeleteSolution(1);

            Assert.AreEqual(MockContext.Object.Solutions.Count(), 1);
            MockContext.Verify(m => m.SaveChanges(), Times.Once);
            Assert.AreEqual(true, result);
        }
    }
}
