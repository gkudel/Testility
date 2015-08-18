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
            IQueryable<Solution> SolutionsData = new HashSet<Solution>
            {
                new Solution() {Id = 2, Name = "12ok"},
                new Solution() {Id = 1, Name = "1"}
            }.AsQueryable();

            IQueryable<Item> ItemsData = new HashSet<Item>
            {
                new Item() {Id = 2, Name = "12ok"}
            }.AsQueryable();

            IQueryable<Class> ClassesData = new HashSet<Class>
            {
                new Class() {Id = 2, Name = "12ok"}
            }.AsQueryable();

            IQueryable<Method> MethodsData = new HashSet<Method>
            {
                new Method() {Id = 2, Name = "12ok"}
            }.AsQueryable();

            IQueryable<Test> TestsData = new HashSet<Test>
            {
                new Test() {Id = 2, Name = "12ok"}
            }.AsQueryable();

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
        private Mock<DbSet<T>> GetQueryableMockDbSet<T>(IQueryable<T> sourceList) where T : class
        {
            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(sourceList.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(sourceList.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(sourceList.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(sourceList.GetEnumerator());

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
