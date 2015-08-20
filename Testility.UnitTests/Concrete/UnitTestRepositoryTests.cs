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
using Testility.UnitTests.DbContextMock;

namespace Testility.Domain.Concrete.Tests
{
    [TestClass()]
    public class UnitTestRepositoryTests
    {
        #region Members
        public Mock<EFDbContext> MockContext { get; set; }

        public UnitTestRepository Service { get; set; }
        #endregion

        [TestInitialize]
        public void Init()
        {
            ICollection<UnitTestSolution> unitTestList = new HashSet<UnitTestSolution>
            {
                new UnitTestSolution() {Id = 1, SolutionId = 1, Solution = new Solution(){ Id = 1, Name = "S1"} },
                new UnitTestSolution() {Id = 2, SolutionId = 2, Solution = new Solution(){ Id = 2, Name = "S2"} },
                new UnitTestSolution() {Id = 3, SolutionId = 3, Solution = new Solution(){ Id = 3, Name = "S3"} },
                new UnitTestSolution() {Id = 4, SolutionId = 4, Solution = new Solution(){ Id = 4, Name = "S4"} },
                new UnitTestSolution() {Id = 5, SolutionId = 5, Solution = new Solution(){ Id = 5, Name = "S5"} }
            };

            MockContext = EntityFrameworkMockHelper.GetMockContext<EFDbContext>();
            MockContext.Object.UnitTestSolutions.AddRange(unitTestList);
            Service = new UnitTestRepository(MockContext.Object);
        }

        [TestMethod]
        public void Can_Get_All_UnitTestSolutions()
        {
            Assert.AreEqual(5, Service.GetSolutions().Count());
        }
    }
}