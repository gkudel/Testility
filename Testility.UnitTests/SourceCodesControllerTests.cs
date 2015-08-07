using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Testility.Domain.Abstract;
using Testility.Domain.Entities;
using Testility.WebUI.Areas.Setup.Controllers;


namespace Testility.UnitTests
{
    [TestClass]
    public class SourceCodesControllerTests
    {

        public Mock<ISetupRepository> ServiceMock { get; set; }
        public SourceCodesController SourceCodesController { get; set; }

        [TestInitialize]
        public void Int()
        {
            SourceCode singleSource = new SourceCode(){Id = 1};

            IQueryable<SourceCode> sourceList = new List<SourceCode>
            { new SourceCode() {Id = 1, Name = "ko", Code = "okok"},
              new SourceCode() {Id = 2, Name = "ko", Code = "okok"}
            }.AsQueryable();

            ServiceMock = new Mock<ISetupRepository>();
            ServiceMock.Setup(x => x.SourceCodes).Returns(sourceList);
            ServiceMock.Setup(x => x.GetSourceCode(1)).Returns(singleSource);
            ServiceMock.Setup(x => x.DeleteSourceCode(It.IsAny<int>())).Returns(true);

            SourceCodesController = new SourceCodesController(ServiceMock.Object);
        }


        [TestMethod]
        public void Index_Contains_All_Data()
        {
            var result = SourceCodesController.Index();
            var model = (result as ViewResult).Model as IQueryable<SourceCode>;
            Assert.AreEqual(2, model.Count());
        }

        [TestMethod]
        public void Canot_Details_WithouId()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var result = SourceCodesController.Details(null) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]

        public void Cannot_Details_NonExists_SourceCodes()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            var result = SourceCodesController.Details(10) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);

        }

       [TestMethod]
        public void Cannot_Create_SourCodes()
        {
            SourceCode sourceCode = new SourceCode(){Id = 1};
            SourceCodesController.ModelState.AddModelError("key", "error");

            ViewResult result = SourceCodesController.Create(sourceCode) as ViewResult;
            var model = (result as ViewResult).Model as SourceCode;

            Assert.AreEqual(1, model.Id);
        }

        [TestMethod]
        public void Cannot_Edit_WithouId()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            int? x = null;
            var result = SourceCodesController.Edit(x) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Cannot_Edit_NonExists_SourceCodes()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            var result = SourceCodesController.Edit(10) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Cannot_Edit_Invalid_SourCodes()
        {
            SourceCode sourceCode = new SourceCode() { Id = 1 };
            SourceCodesController.ModelState.AddModelError("key", "error");

            ViewResult result = SourceCodesController.Create(sourceCode) as ViewResult;
            var model = (result as ViewResult).Model as SourceCode;

            Assert.AreEqual(1, model.Id);
        }

        [TestMethod]
        public void Can_Edit_SourceCodes()
        {
            var result = SourceCodesController.Edit(1) as ViewResult;
            var model = (result as ViewResult).Model as SourceCode;
            Assert.AreEqual(1, model.Id);
        }

        [TestMethod]
        public void Cannot_Delete_WithoutId()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var result = SourceCodesController.Delete(null) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Cannot_Delete_NonExists_SourceCodes()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            var result = SourceCodesController.Delete(10) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }
    }
}
