using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;
using Testility.Engine.Abstract;
using Testility.Engine.Model;
using Testility.WebUI.Areas.Setup.Model;
using Testility.WebUI.Services.Abstract;
using Testility.WebUI.Model;
using Testility.WebUI.Infrastructure;
using Testility.UnitTests.DbContextMock;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;

namespace Testility.WebUI.Areas.WebApi.Controllers
{   
    [TestClass()]
    public class SolutionControllerTests
    {
        #region Members
        public MockSetupRepository MockSetupRepository { get; set; }
        public Mock<ICompilerService> CompilerMock { get; set; }
        public SolutionController solutionController { get; set; }

        #endregion Members
        [TestInitialize]
        public void Int()
        {
            MockSetupRepository = new MockSetupRepository();
            CompilerMock = new Mock<ICompilerService>();
            solutionController = new SolutionController(MockSetupRepository.Repository, CompilerMock.Object);
            var config = new HttpConfiguration();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/Solutions");
            var route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "products" } });

            solutionController.ControllerContext = new HttpControllerContext(config, routeData, request);
            solutionController.Request = request;
            solutionController.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            AutoMapperConfiguration.Configure();
        }

        #region Get
        [TestMethod()]
        public void Get_Contains_All_Solutions()
        {
            HttpResponseMessage message = solutionController.Get();
            Assert.AreNotEqual(null, message);
            Assert.AreEqual(HttpStatusCode.OK, message.StatusCode);            
        }

        [TestMethod()]
        public void Cannot_Get_Invalid_NotFound()
        {
            HttpResponseMessage message = solutionController.Get(11);
            Assert.AreNotEqual(null, message);
            Assert.AreEqual(HttpStatusCode.NotFound, message.StatusCode);
        }

        [TestMethod()]
        public void Cannot_Get_Valid_NotFound()
        {
            SetupSolution singleSolution = new SetupSolution() { Id = 1 };
            MockSetupRepository.Mock.Setup(x => x.GetSetupSolution(It.IsAny<int>())).Returns(singleSolution);
            HttpResponseMessage message = solutionController.Get(1);
            Assert.AreNotEqual(null, message);
            Assert.AreEqual(HttpStatusCode.OK, message.StatusCode);
            MockSetupRepository.Mock.Verify(m => m.GetSetupSolution(1), Times.Once);
            MockSetupRepository.Mock.Verify(m => m.SaveSetupSolution(It.IsAny<SetupSolution>(), It.IsAny<int[]>()), Times.Never);
        }
        #endregion Get

        #region POST
        [TestMethod]
        public void Cannot_EditPostNonExistsSolution_NotFound()
        {
            SolutionViewModel solution = new SolutionViewModel() { Id = 11 };
            var result = solutionController.Post(solution) as HttpResponseMessage;

            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            MockSetupRepository.Mock.Verify(m => m.GetSetupSolution(11), Times.Once);
            MockSetupRepository.Mock.Verify(m => m.SaveSetupSolution(It.IsAny<SetupSolution>(), It.IsAny<int[]>()), Times.Never);
        }

        [TestMethod]
        public void Cannot_EditPostNullSolution_BadRequest()
        {
            var result = solutionController.Post(null) as HttpResponseMessage;
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
            MockSetupRepository.Mock.Verify(m => m.GetSetupSolution(11), Times.Never);
            MockSetupRepository.Mock.Verify(m => m.SaveSetupSolution(It.IsAny<SetupSolution>(), It.IsAny<int[]>()), Times.Never);        
        }

        [TestMethod]
        public void Cannot_EditPostInvalidSolution_Redirect()
        {
            solutionController.ModelState.AddModelError("Error", "Error");
            SolutionViewModel solution = new SolutionViewModel() { Name = "ok" };
            var actionResult = solutionController.Post(solution) as HttpResponseMessage;
            Assert.AreEqual(HttpStatusCode.BadRequest, actionResult.StatusCode);
            MockSetupRepository.Mock.Verify(m => m.GetSetupSolution(11), Times.Never);
            MockSetupRepository.Mock.Verify(m => m.SaveSetupSolution(It.IsAny<SetupSolution>(), It.IsAny<int[]>()), Times.Never);
        }

        [TestMethod]
        public void Can_EditSolution_RedirectToAction()
        {
            MockSetupRepository.Mock.Setup(x => x.SaveSetupSolution(It.IsAny<SetupSolution>(), It.IsAny<int[]>()));
            CompilerMock.Setup(x => x.Compile(It.IsAny<Solution>(), It.IsAny<int[]>())).Returns(new List<Error>());
            MockSetupRepository.Mock.Setup(x => x.GetSetupSolution(1)).Returns(new SetupSolution());
            SolutionViewModel solution = new SolutionViewModel() { Id = 1, Name = "ok" };

            var actionResult = solutionController.Post(solution) as HttpResponseMessage;

            MockSetupRepository.Mock.Verify(m => m.GetSetupSolution(1), Times.Once);
            MockSetupRepository.Mock.Verify(x => x.SaveSetupSolution(It.IsAny<SetupSolution>(), It.IsAny<int[]>()), Times.Once);
            Assert.AreEqual(HttpStatusCode.OK, actionResult.StatusCode);
        }

        [TestMethod]
        public void Can_EditSolution_CompileError()
        {
            MockSetupRepository.Mock.Setup(x => x.SaveSetupSolution(It.IsAny<SetupSolution>(), It.IsAny<int[]>()));
            MockSetupRepository.Mock.Setup(x => x.GetSetupSolution(1)).Returns(new SetupSolution());
            CompilerMock.Setup(x => x.Compile(It.IsAny<Solution>(), It.IsAny<int[]>())).Returns(new List<Error>() { new Error()});
            SolutionViewModel solution = new SolutionViewModel() { Id = 1, Name = "ok" };

            var actionResult = solutionController.Post(solution) as HttpResponseMessage;

            MockSetupRepository.Mock.Verify(m => m.GetSetupSolution(1), Times.Once);
            MockSetupRepository.Mock.Verify(x => x.SaveSetupSolution(It.IsAny<SetupSolution>(), It.IsAny<int[]>()), Times.Once);
            Assert.AreEqual(HttpStatusCode.OK, actionResult.StatusCode); ;
        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Cannot_EditSolution_Exception()
        {
            MockSetupRepository.Mock.Setup(x => x.SaveSetupSolution(It.IsAny<SetupSolution>(), It.IsAny<int[]>())).Throws(new Exception());
            CompilerMock.Setup(x => x.Compile(It.IsAny<Solution>(), It.IsAny<int[]>())).Returns(new List<Error>());
            MockSetupRepository.Mock.Setup(x => x.GetSetupSolution(1)).Returns(new SetupSolution());
            SolutionViewModel solution = new SolutionViewModel() { Id = 1, Name = "ok" };

            var actionResult = solutionController.Post(solution) as HttpResponseMessage;

            MockSetupRepository.Mock.Verify(m => m.GetSetupSolution(1), Times.Once);
            MockSetupRepository.Mock.Verify(x => x.SaveSetupSolution(It.IsAny<SetupSolution>(), It.IsAny<int[]>()), Times.Once);
        }
        #endregion POST
    }
}

namespace Testility.WebUI.Areas.Setup.Controllers
{
    [TestClass]
    public class SolutionControllerTests
    {
        #region Members
        public MockSetupRepository MockSetupRepository { get; set; }
        public SolutionController solutionController { get; set; }
        #endregion Members

        #region Init
        [TestInitialize]
        public void Int()
        {
            MockSetupRepository = new MockSetupRepository();

            AutoMapperConfiguration.Configure();
            solutionController = new SolutionController(MockSetupRepository.Repository);
        }
        #endregion Init

        #region Index
        [TestMethod]
        public void List_Contains_All_Data()
        {
            solutionController.PageSize = 10;
            var result = solutionController.List(null);
            var model = (result as ViewResult).Model as IndexViewModel<SolutionIndexItemViewModel>;
            Assert.AreEqual(4, model.List.Count());
        }

        [TestMethod()]
        public void Can_GetFirstPage_Solutions()
        {
            solutionController.PageSize = 3;

            SolutionIndexItemViewModel[] result = ((IndexViewModel<SolutionIndexItemViewModel>)solutionController.List(null, 1).Model).List.ToArray();

            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual(2, result[1].Id);
            Assert.AreEqual(3, result[2].Id);
        }

        [TestMethod()]
        public void Can_GetSecondPage_UnitTestSolutions()
        {
            solutionController.PageSize = 3;

            SolutionIndexItemViewModel[] result = ((IndexViewModel<SolutionIndexItemViewModel>)solutionController.List(null, 2).Model).List.ToArray();

            Assert.AreEqual(result.Length, 1);
            Assert.AreEqual(4, result[0].Id);
        }
        #endregion Index

        #region Edit & Create

        #region GET

        [TestMethod]
        public void Can_Create_Redirects()
        {
            var result = solutionController.Create() as ViewResult;
            Assert.AreEqual("Solution", result.ViewName);
            Assert.AreEqual(null, result.Model);
        }

        [TestMethod]
        public void Cannot_EditWithouId_BadRequest()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            int? x = null;
            var result = solutionController.Edit(x) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Cannot_EditNonExistsSolution_NotFound()
        {
            MockSetupRepository.Mock.Setup(x => x.GetSetupSolution(It.IsAny<int>())).Returns((SetupSolution)null);
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            var result = solutionController.Edit(It.IsAny<int>()) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Can_Edit_Solution_Redirect()
        {
            SetupSolution singleSolution = new SetupSolution() { Id = 1 };
            MockSetupRepository.Mock.Setup(x => x.GetSetupSolution(It.IsAny<int>())).Returns(singleSolution);
            var result = solutionController.Edit(1) as ViewResult;
            Assert.AreEqual("Solution", result.ViewName);
        }
        #endregion GET

        #endregion Edit & Create

        #region Delete
        [TestMethod]
        public void Cannot_Delete_WithoutId()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var result = solutionController.Delete(null) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Cannot_Delete_NonExists_Solutions()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            MockSetupRepository.Mock.Setup(x => x.GetSetupSolution(It.IsAny<int>())).Returns((SetupSolution)null);

            var result = solutionController.Delete(10) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Can_Delete_Solution_RedirectToIndex()
        {

            SetupSolution solution = new SetupSolution() { Id = 1, Name = "ok" };
            MockSetupRepository.Mock.Setup(x => x.GetSetupSolution(It.IsAny<int>())).Returns(solution);

            ViewResult result = solutionController.Delete(1) as ViewResult;
            var model = (result as ViewResult).Model as SolutionViewModel;
            Assert.AreEqual(1, model.Id);
        }

        [TestMethod]
        public void Cannot_Delete_Solution_WhenException()
        {
            MockSetupRepository.Mock.Setup(x => x.DeleteSolution(It.IsAny<int>())).Throws(new Exception());
            var result = solutionController.DeleteConfirmed(1) as RedirectToRouteResult;
            MockSetupRepository.Mock.Verify(x => x.DeleteSolution(It.IsAny<int>()), Times.Once);
            Assert.AreEqual("List", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Can_Delete_Solution()
        {
            var result = solutionController.DeleteConfirmed(1) as RedirectToRouteResult;
            MockSetupRepository.Mock.Verify(x=>x.DeleteSolution(1), Times.Once);
            Assert.AreNotEqual(null, solutionController.TempData["savemessage"]);
            Assert.AreEqual("List", result.RouteValues["action"]);
        }
        #endregion Delete
    }
}
