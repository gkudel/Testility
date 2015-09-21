using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Testility.WebUI.Areas.Setup.Controllers;
using Testility.Domain.Abstract;
using System.Linq;
using Testility.Domain.Entities;
using System.Collections.Generic;
using Testility.WebUI.Infrastructure;
using System.Web.Mvc;
using Testility.WebUI.Areas.Setup.Model;
using System.Net;
using Testility.WebUI.Model;
using Testility.WebUI.Services.Abstract;
using System.Web;
using System.Threading.Tasks;

namespace Testility.WebUI.Areas.Setup.Controllers
{
    [TestClass]
    public class ReferenceControllerTests
    {
        #region Members
        public Mock<IDbRepository> ServiceMock { get; set; }
        public Mock<IFileService> FileServiceMock { get; set; }
        private Func<bool> uploaded = () => true;
        public ReferencesController referencesController { get; set; }
        #endregion Members

        #region Init
        [TestInitialize]
        public void Int()
        {
            IQueryable<Reference> RefList = new List<Reference>
            { new Reference() {Id = 1, Name = "ko"},
              new Reference() {Id = 2, Name = "ko"}
            }.AsQueryable();

            Reference singleRef = new Reference() { Id = 3, Name = "okoko" };

            ServiceMock = new Mock<IDbRepository>();
            ServiceMock.Setup(x => x.GetReferences()).Returns(RefList);
            ServiceMock.Setup(x => x.GetReference(3)).Returns(singleRef);
            ServiceMock.Setup(x => x.DeleteReference(It.IsAny<int>())).Returns(true);
            ServiceMock.Setup(x => x.Save(It.IsAny<Reference>()));

            FileServiceMock = new Mock<IFileService>();
            FileServiceMock.Setup(x => x.UploadReferenceAsync(It.IsAny<Reference>(), It.IsAny<string>())).Returns(Task.FromResult("path"));

            AutoMapperConfiguration.Configure();
            referencesController = new ReferencesController (ServiceMock.Object, FileServiceMock.Object);
        }
        #endregion Init

        #region Index
        [TestMethod]
        public void List_ContainsAllReferences()
        {
            var result = referencesController.List();
            var model = (result as ViewResult).Model as IndexViewModel<ReferencesViewModel>;
            Assert.AreEqual(2, model.List.Count());
        }
        #endregion Index

        #region Edit & Create
        #region GET
        [TestMethod]
        public void Can_CreateReferences_Redirects()
        {
            var result = referencesController.Create() as ViewResult;
            Assert.AreEqual("Reference", result.ViewName);
        }

        [TestMethod]
        public void Cannot_EditReferenceWithouId_BadRequest()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            int? x = null;
            var result = referencesController.Edit(x) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Cannot_EditNonExistsReferences_NotFound()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            var result = referencesController.Edit(10) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Can_Edit_References_Redirect()
        {
            var result = referencesController.Edit(3) as ViewResult;
            var model = (result as ViewResult).Model as ReferencesViewModel;
            Assert.AreEqual("Reference", result.ViewName);
            Assert.AreEqual(3, model.Id);
        }
        #endregion GET

        #region POST
        [TestMethod]
        public void Cannot_EditInvalidReference_Redirect()
        {
            referencesController.ModelState.AddModelError("Error", "Error");
            ReferencesViewModel reference = new ReferencesViewModel() { Name = "ok" };
            var actionResult = referencesController.Edit(reference).Result as ViewResult;
            Assert.AreEqual("Reference", actionResult.ViewName);
        }

        [TestMethod]
        public void Can_EditSolution_RedirectToAction()
        {
            ReferencesViewModel reference = new ReferencesViewModel() { Name = "ok" };
            var actionResult = referencesController.Edit(reference).Result as RedirectToRouteResult;

            ServiceMock.Verify(x => x.Save(It.IsAny<Reference>()), Times.Once);
            Assert.AreNotEqual(null, referencesController.TempData["savemessage"]);
            Assert.AreEqual("List", actionResult.RouteValues["action"]);
        }

        [TestMethod]
        public void Cannot_EditSolution_SaveException()
        {

            ServiceMock.Setup(x => x.Save(It.IsAny<Reference>())).Throws(new Exception());
            ReferencesViewModel reference = new ReferencesViewModel() { Name = "ok" };
            var actionResult = referencesController.Edit(reference).Result as ViewResult;
            var model = (actionResult as ViewResult).Model as ReferencesViewModel;


            ServiceMock.Verify(x => x.Save(It.IsAny<Reference>()), Times.Once);
            Assert.AreEqual(null, referencesController.TempData["savemessage"]);
            Assert.AreEqual("Reference", actionResult.ViewName);
            Assert.AreEqual("ok", model.Name);
        }

        public void Cannot_EditNotUploaded_Redirect()
        {
            ReferencesViewModel reference = new ReferencesViewModel() { Name = "ok" };
            var actionResult = referencesController.Edit(reference).Result as ViewResult;
            Assert.AreEqual("Reference", actionResult.ViewName);
        }
        #endregion POST

        #endregion Edit & Create


        #region Delete
        [TestMethod]
        public void Cannot_DeleteReferenceWithoutId()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var result = referencesController.Delete(null) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Cannot_Delete_NonExists_Reference()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            ServiceMock.Setup(x => x.GetSetupSolution(It.IsAny<int>())).Returns((SetupSolution)null);

            var result = referencesController.Delete(10) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Can_Delete_Reference_RedirectToIndex()
        {

            ViewResult result = referencesController.Delete(3) as ViewResult;
            var model = (result as ViewResult).Model as ReferencesViewModel;
            Assert.AreEqual(3, model.Id);
        }

        [TestMethod]
        public void Cannot_Delete_Reference_WhenException()
        {
            ServiceMock.Setup(x => x.DeleteReference(It.IsAny<int>())).Throws(new Exception());

            ReferencesViewModel reference = new ReferencesViewModel() { Id = 3, Name = "ok" };
            var result = referencesController.DeleteConfirmed(reference) as RedirectToRouteResult;
            ServiceMock.Verify(x => x.DeleteReference(It.IsAny<int>()), Times.Once);
            Assert.AreEqual("List", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Can_Delete_References()
        {
            ReferencesViewModel reference = new ReferencesViewModel() { Id = 3, Name = "ok" };
            var result = referencesController.DeleteConfirmed(reference) as RedirectToRouteResult;
            ServiceMock.Verify(x => x.DeleteReference(It.IsAny<int>()), Times.Once);
            Assert.AreNotEqual(null, referencesController.TempData["savemessage"]);
            Assert.AreEqual("List", result.RouteValues["action"]);
        }
        #endregion Delete

    }
}
