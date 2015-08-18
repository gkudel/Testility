using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testility.Domain.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using System.Data.Entity;
using Testility.Domain.Entities;

namespace Testility.Domain.Concrete.Tests
{
    [TestClass()]
    public class UnitTestRepositoryTests
    {
        #region Members
        public Mock<EFDbContext> MockContext { get; set; }

        public Mock<DbSet<UnitTestSolution>> MockUnitTestSolution { get; set; }

        public UnitTestRepository Service { get; set; }
        #endregion

        [TestInitialize]
        public void Init()
        {
            IQueryable<UnitTestSolution> SolutionList = new HashSet<UnitTestSolution>
            {
                new UnitTestSolution() {Id = 1, SolutionId = 1, Solution = new Solution(){ Id = 1, Name = "S1"} },
                new UnitTestSolution() {Id = 2, SolutionId = 2, Solution = new Solution(){ Id = 2, Name = "S2"} },
                new UnitTestSolution() {Id = 3, SolutionId = 3, Solution = new Solution(){ Id = 3, Name = "S3"} },
                new UnitTestSolution() {Id = 4, SolutionId = 4, Solution = new Solution(){ Id = 4, Name = "S4"} },
                new UnitTestSolution() {Id = 5, SolutionId = 5, Solution = new Solution(){ Id = 5, Name = "S5"} }
            }.AsQueryable();

            MockContext = new Mock<EFDbContext>();
            MockUnitTestSolution = GetQueryableMockDbSet(SolutionList);
            MockContext.Setup(x => x.UnitTestSolutions).Returns(MockUnitTestSolution.Object);
            Service = new UnitTestRepository(MockContext.Object);
        }

        private Mock<DbSet<T>> GetQueryableMockDbSet<T>(IQueryable<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            return dbSet;
        }

        [TestMethod]
        public void Can_Get_All_UnitTestSolutions()
        {
            Assert.AreEqual(5, Service.GetSolutions().Count());
        }
    }
}