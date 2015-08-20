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
using Testility.WebUI.Areas.Setup.Controllers;
using Testility.WebUI.Areas.Setup.Model;
using Testility.WebUI.Services.Abstract;
using Testility.WebUI.Model;
using Testility.WebUI.Infrastructure;

namespace Testility.UnitTests
{
    [TestClass]
    public class SolutionControllerTests
    {
        #region Members
        public Mock<ISetupRepository> ServiceMock { get; set; }
        public Mock<ICompilerService> CompilerMock { get; set; }
        public SolutionController solutionController { get; set; }
        #endregion Members

        #region Init
        [TestInitialize]
        public void Int()
        {          
            IQueryable<Solution> SolutionList = new List<Solution>
            { new Solution() {Id = 1, Name = "ko"},
              new Solution() {Id = 2, Name = "ko"}
            }.AsQueryable();

            ServiceMock = new Mock<ISetupRepository>();
            ServiceMock.Setup(x => x.GetSolutions(It.IsAny<bool>())).Returns(SolutionList);
            //ServiceMock.Setup(x => x.GetSolution(It.IsAny<int>())).Returns(singleSolution);
            ServiceMock.Setup(x => x.DeleteSolution(It.IsAny<int>())).Returns(true);
            ServiceMock.Setup(x => x.IsAlreadyDefined(It.IsAny<string>(), It.IsAny<int>())).Returns(true);
            
            CompilerMock = new Mock<ICompilerService>();

            AutoMapperConfiguration.Configure();
            solutionController = new SolutionController(ServiceMock.Object, CompilerMock.Object);
        }
        #endregion Init

        #region Index
        [TestMethod]
        public void List_Contains_All_Data()
        {
            var result = solutionController.List(null);
            var model = (result as ViewResult).Model as IndexViewModel<SolutionIndexItemViewModel>;
            Assert.AreEqual(2, model.List.Count());
        }
        #endregion Index

        #region Edit & Create

        #region GET

        [TestMethod]
        public void Can_Create_Redirects()
        {
            var result = solutionController.Create() as ViewResult;
            Assert.AreEqual("Solution", result.ViewName);
            Assert.AreNotEqual(null, result.Model);
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
            ServiceMock.Setup(x => x.GetSolution(It.IsAny<int>())).Returns((Solution)null);
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            var result = solutionController.Edit(It.IsAny<int>()) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Can_Edit_Solution_Redirect()
        {
            Solution singleSolution = new Solution() { Id = 1 };
            ServiceMock.Setup(x => x.GetSolution(It.IsAny<int>())).Returns(singleSolution);
            var result = solutionController.Edit(1) as ViewResult;
            Assert.AreEqual("Solution", result.ViewName);
        }
        #endregion GET

        #region POST
        [TestMethod]
        public void Cannot_EditPostNonExistsSolution_NotFound()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            SolutionViewModel solution = new SolutionViewModel() { Id = 11 };
            var result = solutionController.EditPost(solution) as HttpStatusCodeResult;

            Solution singleSolution = new Solution() { Id = 1 };
            ServiceMock.Setup(x => x.GetSolution(It.IsAny<int>())).Returns(singleSolution);

            ServiceMock.Verify(m => m.GetSolution(11), Times.Once);
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Cannot_EditPostNullSolution_BadRequest()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var result = solutionController.EditPost(null) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Cannot_EditPostInvalidSolution_Redirect()
        {
            solutionController.ModelState.AddModelError("Error", "Error");
            SolutionViewModel solution = new SolutionViewModel() { Name = "ok" };
            var actionResult = solutionController.EditPost(solution) as ViewResult;
            Assert.AreEqual("Solution", actionResult.ViewName);
        }

        [TestMethod]
        public void Can_EditSolution_RedirectToAction()
        {

            ServiceMock.Setup(x => x.Save(It.IsAny<Solution>(), It.IsAny<int[]>()));
            CompilerMock.Setup(x => x.Compile(It.IsAny<Solution>(), It.IsAny<int[]>())).Returns(new List<Error>());

            SolutionViewModel solution = new SolutionViewModel() { Name = "ok" };
            var actionResult = solutionController.EditPost(solution) as RedirectToRouteResult;


            ServiceMock.Verify(x => x.Save(It.IsAny<Solution>(), It.IsAny<int[]>()), Times.Once);
            Assert.AreNotEqual(null, solutionController.TempData["savemessage"]);
            Assert.AreEqual("List", actionResult.RouteValues["action"]);
        }

        [TestMethod]
        public void Cannot_EditSolution_CompileError()
        {

            ServiceMock.Setup(x => x.Save(It.IsAny<Solution>(), It.IsAny<int[]>()));
            //CompilerMock.Setup(x => x.Compile(It.IsAny<Solution>())).Returns(new List<Error>() {new Error() });

            SolutionViewModel solution = new SolutionViewModel() { Name = "ok" };
            var actionResult = solutionController.EditPost(solution) as ViewResult;

            ServiceMock.Verify(x => x.Save(It.IsAny<Solution>(), It.IsAny<int[]>()), Times.Never);
            Assert.AreEqual(null, solutionController.TempData["savemessage"]);
            Assert.AreEqual("Solution", actionResult.ViewName);
        }


        [TestMethod]
        public void Cannot_EditSolution_Exception()
        {
            ServiceMock.Setup(x => x.Save(It.IsAny<Solution>(), It.IsAny<int[]>())).Throws(new Exception());
            CompilerMock.Setup(x => x.Compile(It.IsAny<Solution>(), It.IsAny<int[]>())).Returns(new List<Error>());

            SolutionViewModel solution = new SolutionViewModel() { Name = "ok" };
            var actionResult = solutionController.EditPost(solution) as ViewResult;

            ServiceMock.Verify(x => x.Save(It.IsAny<Solution>(), It.IsAny<int[]>()), Times.Once);
            Assert.AreEqual("Solution", actionResult.ViewName);

        }
        #endregion POST

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
            ServiceMock.Setup(x => x.GetSolution(It.IsAny<int>())).Returns((Solution)null);

            var result = solutionController.Delete(10) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Can_Delete_Solution_RedirectToIndex()
        {

            Solution solution = new Solution() { Id = 1, Name = "ok" };
            ServiceMock.Setup(x => x.GetSolution(It.IsAny<int>())).Returns(solution);

            ViewResult result = solutionController.Delete(1) as ViewResult;
            var model = (result as ViewResult).Model as SolutionViewModel;
            Assert.AreEqual(1, model.Id);
        }

        [TestMethod]
        public void Cannot_Delete_Solution_WhenException()
        {
            ServiceMock.Setup(x => x.DeleteSolution(It.IsAny<int>())).Throws(new Exception());
            var result = solutionController.DeleteConfirmed(1) as RedirectToRouteResult;
            ServiceMock.Verify(x => x.DeleteSolution(It.IsAny<int>()), Times.Once);
            Assert.AreEqual("List", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Can_Delete_Solution()
        {
            var result = solutionController.DeleteConfirmed(1) as RedirectToRouteResult;
            ServiceMock.Verify(x=>x.DeleteSolution(1), Times.Once);
            Assert.AreNotEqual(null, solutionController.TempData["savemessage"]);
            Assert.AreEqual("List", result.RouteValues["action"]);
        }
        #endregion Delete
    }
}
