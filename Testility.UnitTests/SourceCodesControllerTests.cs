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
using Testility.WebUI.Mappings;
using Testility.WebUI.Services;


namespace Testility.UnitTests
{
    [TestClass]
    public class SourceCodesControllerTests
    {

        public Mock<ISetupRepository> ServiceMock { get; set; }
        public Mock<ICreateInputClassFromFile> CreateInputClassMock { get; set; }

        public Mock<ICompiler> CompilerMock { get; set; }
        public SourceCodesController sourceCodesController { get; set; }
        public Mock<HttpPostedFileBase> File {get;set;}

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
          
            CompilerMock = new Mock<ICompiler>();
            CompilerMock.Setup(x => x.Compile(It.IsAny<Input>())).Returns(new Result(){});

            File = new Mock<HttpPostedFileBase>();
            File.Setup(d => d.FileName).Returns("test1.txt");

            CreateInputClassMock = new Mock<ICreateInputClassFromFile>();
            CreateInputClassMock.Setup(x => x.CreateInputClass(It.IsAny<SourceCode>(), File.Object)).Returns(new Input() {Code = "xx", Language = "ss", Name = "test", ReferencedAssemblies = "System.dll"});

            sourceCodesController = new SourceCodesController(ServiceMock.Object, CreateInputClassMock.Object, CompilerMock.Object);
        }


        [TestMethod]
        public void Index_Contains_All_Data()
        {
            var result = sourceCodesController.Index();
            var model = (result as ViewResult).Model as IQueryable<SourceCode>;
            Assert.AreEqual(2, model.Count());
        }

        [TestMethod]
        public void Canot_Details_WithouId()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var result = sourceCodesController.Details(null) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Cannot_Details_NonExists_SourceCodes()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            var result = sourceCodesController.Details(10) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);

        }

        [TestMethod]
        public void Can_Display_Details_Of_SourceCodes()
        {
            ViewResult result = sourceCodesController.Details(1) as ViewResult;
            var model = (result as ViewResult).Model as SourceCode;
            ServiceMock.Verify(x=>x.GetSourceCode(1), Times.Once);
            Assert.AreEqual(1, model.Id);
        }
  
        [TestMethod]
        public void Cannot_EditWithouId_BadRequest()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            int? x = null;
            var result = sourceCodesController.Edit(x) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Cannot_EditNonExistsSourceCodes_NotFound()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            var result = sourceCodesController.Edit(10) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }


        [TestMethod]
        public void Can_Edit_SourceCodes_Redirect()
        {
            var result = sourceCodesController.Edit(1) as ViewResult;
            Assert.AreEqual("CreateAndEdit", result.ViewName);
        }

        [TestMethod]
        public void Cannot_EditSourceCodes_Redirect()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            CompilerMock.Setup(x => x.Compile(It.IsAny<Input>())).Returns(new Result() { Errors = new List<Error>(){new Error(){Message = "blbl"}}});
            SourceCode sourceCode = new SourceCode();
            var actionResult = sourceCodesController.EditPost(sourceCode, File.Object) as ViewResult;
            Assert.AreEqual("CreateAndEdit", actionResult.ViewName);
        }

        [TestMethod]
        public void Cannot_EditSourceCodes_NotFound()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            var result = sourceCodesController.EditPost(null, File.Object) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Cannot_Delete_WithoutId()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var result = sourceCodesController.Delete(null) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Cannot_Delete_NonExists_SourceCodes()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            var result = sourceCodesController.Delete(10) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Can_Delete_SourceCodes_RedirectToIndex()
        {
            ViewResult result = sourceCodesController.Delete(1) as ViewResult;
            var model = (result as ViewResult).Model as SourceCode;
            Assert.AreEqual(1, model.Id);
            ServiceMock.Verify(x=>x.GetSourceCode(It.IsAny<int>()),Times.Once);  
        }

        [TestMethod]
        public void Cannot_Delete_SourceCodes_WhenException()
        {
            ServiceMock.Setup(x => x.DeleteSourceCode(It.IsAny<int>())).Throws(new Exception());
            var result = sourceCodesController.DeleteConfirmed(1) as RedirectToRouteResult;
            ServiceMock.Verify(x=>x.DeleteSourceCode(It.IsAny<int>()), Times.Once);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Can_Delete_SourceCodes()
        {
            var result = sourceCodesController.DeleteConfirmed(1) as RedirectToRouteResult;
            ServiceMock.Verify(x=>x.DeleteSourceCode(1), Times.Once);
            Assert.AreNotEqual(null, sourceCodesController.TempData["savemessage"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }


        //[TestMethod]
        //public void Cannot_Edit_WithouId()
        //{
        //}

        //[TestMethod]
        //public void Cannot_AddSourceCodes()
        //{
           

        //[TestMethod]
        //public void Cannot_Edit_Invalid_SourCodes()
        //{
         
        //}

        //[TestMethod]
        //public void Can_Edit_SourceCodes()
        //{
        //}



    }
}
