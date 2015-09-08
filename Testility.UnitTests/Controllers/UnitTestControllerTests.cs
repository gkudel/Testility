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
using System.Web.Mvc;
using System.Net;
using Testility.UnitTests.DbContextMock;

namespace Testility.WebUI.Controllers.Tests
{
    [TestClass()]
    public class UnitTestControllerTests
    {
        public UnitTestController unitTestController { get; set; }
        public MockDbRepository MockDbRepository { get; set; }

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

            MockDbRepository = new MockDbRepository();
            MockDbRepository.Mock.Setup(x => x.GetUnitTestSolutions(It.IsAny<bool>())).Returns(SolutionList);

            AutoMapperConfiguration.Configure();
            unitTestController = new UnitTestController(MockDbRepository.Mock.Object);
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

        #region Edit & Create
        #region GET
        [TestMethod]
        public void Can_Create_Redirects()
        {
            var result = unitTestController.Create() as ViewResult;
            Assert.AreEqual("Solution", result.ViewName);
            Assert.AreEqual(null, result.Model);
        }

        [TestMethod]
        public void Cannot_EditWithouId_BadRequest()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            int? x = null;
            var result = unitTestController.Edit(x) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Cannot_EditNonExistsSolution_NotFound()
        {
            MockDbRepository.Mock.Setup(x => x.GetUnitTestSolution(It.IsAny<int>())).Returns((UnitTestSolution)null);
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            var result = unitTestController.Edit(It.IsAny<int>()) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Can_Edit_Solution_Redirect()
        {
            UnitTestSolution singleSolution = new UnitTestSolution() { Id = 1 };
            MockDbRepository.Mock.Setup(x => x.GetUnitTestSolution(It.IsAny<int>())).Returns(singleSolution);
            var result = unitTestController.Edit(1) as ViewResult;
            Assert.AreEqual("Solution", result.ViewName);
        }
        #endregion GET

        #endregion Edit & Create

        #region Delete
        [TestMethod]
        public void Cannot_Delete_WithoutId()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var result = unitTestController.Delete(null) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Cannot_Delete_NonExists_Solutions()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            MockDbRepository.Mock.Setup(x => x.GetUnitTestSolution(It.IsAny<int>())).Returns((UnitTestSolution)null);

            var result = unitTestController.Delete(10) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Can_Delete_Solution_RedirectToIndex()
        {

            UnitTestSolution solution = new UnitTestSolution() { Id = 1, Name = "ok" };
            MockDbRepository.Mock.Setup(x => x.GetUnitTestSolution(It.IsAny<int>())).Returns(solution);

            ViewResult result = unitTestController.Delete(1) as ViewResult;
            var model = (result as ViewResult).Model as SolutionViewModel;
            Assert.AreEqual(1, model.Id);
        }

        [TestMethod]
        public void Cannot_Delete_Solution_WhenException()
        {
            MockDbRepository.Mock.Setup(x => x.DeleteUnitSolution(It.IsAny<int>())).Throws(new Exception());
            var result = unitTestController.DeleteConfirmed(1) as RedirectToRouteResult;
            MockDbRepository.Mock.Verify(x => x.DeleteUnitSolution(It.IsAny<int>()), Times.Once);
            Assert.AreEqual("List", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Can_Delete_Solution()
        {
            var result = unitTestController.DeleteConfirmed(1) as RedirectToRouteResult;
            MockDbRepository.Mock.Verify(x => x.DeleteUnitSolution(1), Times.Once);
            Assert.AreNotEqual(null, unitTestController.TempData["savemessage"]);
            Assert.AreEqual("List", result.RouteValues["action"]);
        }
        #endregion Delete

    }
}