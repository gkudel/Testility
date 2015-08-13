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
using Testility.WebUI.Mappings.Infrastructure;
using Testility.WebUI.Areas.Setup.Models;

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
            ServiceMock.Setup(x => x.GetAllSourceCodes(true)).Returns(sourceList);
            ServiceMock.Setup(x => x.GetSourceCode(1, It.IsAny<bool>())).Returns(singleSource);
            ServiceMock.Setup(x => x.Delete(It.IsAny<int>())).Returns(true);
          
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
        public void Cannot_EditPostNonExistsSourceCodes_NotFound()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            EditViewModel model = new EditViewModel()
            {
                SourceCode = new SourceCode() { Id = 11 },
                UploadedFile = File.Object
            };
            var result = sourceCodesController.EditPost(model) as HttpStatusCodeResult;
            ServiceMock.Verify(m => m.GetSourceCode(11, false), Times.Once);
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Cannot_EditPostNullSourceCodes_BadRequest()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            EditViewModel model = new EditViewModel()
            {
                SourceCode = null,
                UploadedFile = File.Object
            };
            var result = sourceCodesController.EditPost(model) as HttpStatusCodeResult;
            Assert.AreEqual(expected.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void Cannot_EditPostInvalidSourceCodes_Redirect()
        {
            sourceCodesController.ModelState.AddModelError("Error", "Error");
            EditViewModel model = new EditViewModel()
            {
                SourceCode = new SourceCode() { Id = 1 },
                UploadedFile = File.Object
            };
            var actionResult = sourceCodesController.EditPost(model) as ViewResult;
            ServiceMock.Verify(m => m.GetSourceCode(1, false), Times.Once);
            ServiceMock.Verify(m => m.Save(It.IsAny<SourceCode>()), Times.Never);
            Assert.AreEqual("CreateAndEdit", actionResult.ViewName);
        }

        [TestMethod]
        public void Cannot_EditPostSourceCodes_Redirect()
        {
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.NotFound);
            CompilerMock.Setup(x => x.Compile(It.IsAny<Input>())).Returns(new Result() { Errors = new List<Error>(){new Error(){Message = "blbl"}}});
            EditViewModel model = new EditViewModel()
            {
                SourceCode = new SourceCode(),
                UploadedFile = File.Object
            };
            ServiceMock.Verify(m => m.Save(It.IsAny<SourceCode>()), Times.Never);
            var actionResult = sourceCodesController.EditPost(model) as ViewResult;
            Assert.AreEqual("CreateAndEdit", actionResult.ViewName);
        }

        [TestMethod]
        public void Can_EditPostSourceCodes_Redirect()
        {
            AutoMapperConfigurationWebUI.Configure();
            CompilerMock.Setup(x => x.Compile(It.IsAny<Input>())).Returns(new Result()
            {
                Errors = new List<Error>(),
                TestedClasses = new List<Engine.Model.TestedClass>()
                {
                    new Engine.Model.TestedClass()
                    {
                        Methods = new List<Engine.Model.TestedMethod>()
                        {
                            new Engine.Model.TestedMethod()
                            {
                                Tests = new List<Engine.Model.Test>()
                            }
                        }
                    }
                }
            });
            EditViewModel model = new EditViewModel()
            {
                SourceCode = new SourceCode(),
                UploadedFile = File.Object
            };
            var result = sourceCodesController.EditPost(model) as RedirectToRouteResult;
            ServiceMock.Verify(m => m.Save(It.IsAny <SourceCode>()), Times.Once);
            Assert.AreEqual("Index", result.RouteValues["action"]);
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
            ServiceMock.Verify(x=>x.GetSourceCode(It.IsAny<int>(), It.IsAny<bool>()),Times.Once);  
        }

        [TestMethod]
        public void Cannot_Delete_SourceCodes_WhenException()
        {
            ServiceMock.Setup(x => x.Delete(It.IsAny<int>())).Throws(new Exception());
            var result = sourceCodesController.DeleteConfirmed(1) as RedirectToRouteResult;
            ServiceMock.Verify(x=>x.Delete(It.IsAny<int>()), Times.Once);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Can_Delete_SourceCodes()
        {
            var result = sourceCodesController.DeleteConfirmed(1) as RedirectToRouteResult;
            ServiceMock.Verify(x=>x.Delete(1), Times.Once);
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
