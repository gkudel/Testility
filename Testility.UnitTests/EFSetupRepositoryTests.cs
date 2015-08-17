using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Testility.Domain.Concrete;
using Testility.Domain.Entities;


namespace Testility.UnitTests
{
    [TestClass]
    public class EFSetupRepositoryTests
    {
        #region Members
        public Mock<EFDbContext> MockContext { get; set; }

        public Mock<DbSet<Solution>> MockSolution { get; set; }

        public EFSetupRepository Service { get; set; }
        #endregion


        #region Init
        [TestInitialize]
        public void Init()
        {
            var SolutionsData = new List<Solution>
            {
                new Solution() {Id = 2, Name = "12ok"},
                new Solution() {Id = 1, Name = "1"}
            };

            var ItemsData = new List<Item>
            {
                new Item() {Id = 2, Name = "12ok"}
            };

            var ClassesData = new List<Class>
            {
                new Class() {Id = 2, Name = "12ok"}
            };

            var MethodsData = new List<Method>
            {
                new Method() {Id = 2, Name = "12ok"}
            };

            var TestsData = new List<Test>
            {
                new Test() {Id = 2, Name = "12ok"}
            };

            MockContext = new Mock<EFDbContext>();


            MockSolution = GetQueryableMockDbSet(SolutionsData);
            MockContext.Setup(x => x.Solutions).Returns(MockSolution.Object);

            MockContext.Setup(x => x.Items).Returns(GetQueryableMockDbSet(ItemsData).Object);
            MockContext.Setup(x => x.Classes).Returns(GetQueryableMockDbSet(ClassesData).Object);
            MockContext.Setup(x => x.Methods).Returns(GetQueryableMockDbSet(MethodsData).Object);
            MockContext.Setup(x => x.Tests).Returns(GetQueryableMockDbSet(TestsData).Object);

            Service = new EFSetupRepository(MockContext.Object);
        }

        #endregion



        #region Supp
        private Mock<DbSet<T>> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            return dbSet;
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
            Service.Save(solution);
            MockSolution.Verify(x => x.Add(It.IsAny<Solution>()), Times.Once);
            MockContext.Verify(x => x.SaveChanges(), Times.Once);
        } 

        [TestMethod]
        public void Cannot_DeleteSolutionWithWrongId()
        {
            var result = Service.Delete(10);

            MockSolution.Verify(m => m.Remove(It.IsAny<Solution>()), Times.Never);
            MockContext.Verify(m => m.SaveChanges(), Times.Never);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Can_DeleteSolution()
        {
            var result = Service.Delete(1);

            MockSolution.Verify(m => m.Remove(It.IsAny<Solution>()), Times.Once);
            MockContext.Verify(m => m.SaveChanges(), Times.Once);
            Assert.AreEqual(true, result);
        }
    }
}
