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
using Testility.Domain.Abstract;

namespace Testility.Domain.Concrete.Tests
{
    [TestClass()]
    public class UnitTestRepositoryTests
    {
        #region Members
        public Mock<IEFDbContext> MockContext { get; set; }

        public UnitTestRepository Service { get; set; }
        #endregion

        [TestInitialize]
        public void Init()
        {
            ICollection<UnitTestSolution> unitTestList = new HashSet<UnitTestSolution>
            {
                new UnitTestSolution() {Id = 1, SetupSolutionId = 1, SetupSolution = new SetupSolution(){ Id = 1, Name = "S1"} },
                new UnitTestSolution() {Id = 2, SetupSolutionId = 2, SetupSolution = new SetupSolution(){ Id = 2, Name = "S2"} },
                new UnitTestSolution() {Id = 3, SetupSolutionId = 3, SetupSolution = new SetupSolution(){ Id = 3, Name = "S3"} },
                new UnitTestSolution() {Id = 4, SetupSolutionId = 4, SetupSolution = new SetupSolution(){ Id = 4, Name = "S4"} },
                new UnitTestSolution() {Id = 5, SetupSolutionId = 5, SetupSolution = new SetupSolution(){ Id = 5, Name = "S5"} }
            };

            MockContext = EntityFrameworkMockHelper.GetMockContext<IEFDbContext>();
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