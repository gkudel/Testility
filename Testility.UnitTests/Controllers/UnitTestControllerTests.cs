using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testility.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.Domain.Abstract;
using Moq;
using Testility.Domain.Entities;
using Testility.WebUI.Infrastructure;
using Testility.WebUI.Model;

namespace Testility.WebUI.Controllers.Tests
{
    [TestClass()]
    public class UnitTestControllerTests
    {
        public Mock<IDbRepository> ServiceMock { get; set; }
        public UnitTestController unitTestController { get; set; }

        [TestInitialize]
        public void Int()
        {
            IQueryable<UnitTestSolution> SolutionList = new HashSet<UnitTestSolution>
            {
                new UnitTestSolution() {Id = 1, SetupSolutionId = 1, SetupSolution = new SetupSolution(){ Id = 1, Name = "S1"} },
                new UnitTestSolution() {Id = 2, SetupSolutionId = 2, SetupSolution = new SetupSolution(){ Id = 2, Name = "S2"} },
                new UnitTestSolution() {Id = 3, SetupSolutionId = 3, SetupSolution = new SetupSolution(){ Id = 3, Name = "S3"} },
                new UnitTestSolution() {Id = 4, SetupSolutionId = 4, SetupSolution = new SetupSolution(){ Id = 4, Name = "S4"} },
                new UnitTestSolution() {Id = 5, SetupSolutionId = 5, SetupSolution = new SetupSolution(){ Id = 5, Name = "S5"} }
            }.AsQueryable();

            ServiceMock = new Mock<IDbRepository>();
            ServiceMock.Setup(x => x.GetUnitTestSolutions(It.IsAny<bool>())).Returns(SolutionList);

            AutoMapperConfiguration.Configure();
            unitTestController = new UnitTestController(ServiceMock.Object);
        }

        [TestMethod()]
        public void Can_GetAll_UnitTestSolutions()
        {
            unitTestController.PageSize = 3;

            UnitTestIndexItemViewModel[] result = ((IndexViewModel<UnitTestIndexItemViewModel>)unitTestController.List().Model).List.ToArray();

            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual("S1", result[0].SetupName);
            Assert.AreEqual(2, result[1].Id);
            Assert.AreEqual("S2", result[1].SetupName);
            Assert.AreEqual(3, result[2].Id);
            Assert.AreEqual("S3", result[2].SetupName);
        }

        [TestMethod()]
        public void Can_GetFirstPage_UnitTestSolutions()
        {
            unitTestController.PageSize = 3;

            UnitTestIndexItemViewModel[] result = ((IndexViewModel<UnitTestIndexItemViewModel>)unitTestController.List(1).Model).List.ToArray();

            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual(2, result[1].Id);
            Assert.AreEqual(3, result[2].Id);
        }

        [TestMethod()]
        public void Can_GetSecondPage_UnitTestSolutions()
        {
            unitTestController.PageSize = 3;

            UnitTestIndexItemViewModel[] result = ((IndexViewModel<UnitTestIndexItemViewModel>)unitTestController.List(2).Model).List.ToArray();

            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(4, result[0].Id);
            Assert.AreEqual(5, result[1].Id);
        }
    }
}