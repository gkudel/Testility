using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Testility.WebUI.Services.Abstract;
using Testility.WebUI.Areas.Authorization.Controllers;
using System.Web.Mvc;
using Testility.WebUI.Areas.Authorization.Models;
using System.Threading.Tasks;
using Testility.UnitTests.DbContextMock;
using System.Net;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using Testility.WebUI.Infrastructure;
using System.Web;
using System.Security.Principal;
using System.Security.Claims;
using Testility.UnitTests.Constants;

namespace Testility.UnitTests.Controllers
{
    [TestClass]
    public class AuthControllerTests
    {

        public MockIdentityService serviceMock;
        public AuthController authController;

        [TestInitialize]
        public void Init()
        {

            var context = new Mock<ControllerContext>();
            var mockIdentity = new Mock<IIdentity>();
            context.SetupGet(x => x.HttpContext.User.Identity).Returns(mockIdentity.Object);
            mockIdentity.Setup(x => x.Name).Returns("someValidId");

            serviceMock = new MockIdentityService();
            authController = new AuthController(serviceMock.Repository) { ControllerContext = context.Object };



        AutoMapperConfiguration.Configure();
        }

        [TestMethod]
        public void Can_Login_ReturnView()
        {
             ActionResult result = authController.LogIn();
             Assert.IsInstanceOfType(result, typeof(ViewResult));

        }

        [TestMethod]
        public void Cannot_Login_BadRequest()
        {
            var res = authController.LogIn(null).Result as HttpStatusCodeResult;
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Assert.AreNotEqual(null, res);
            Assert.AreEqual(res.StatusCode, expected.StatusCode);
        }

        [TestMethod]
        public void Cannot_Login_RedirectModelInvalid()
        {
            authController.ModelState.AddModelError("Error", "Error");
            LoginVM model = new LoginVM() { Email = "ss", Password = "as", ReturnUrl = "aa" };
            ActionResult res = authController.LogIn(model).Result;
            var returnedModel = (res as ViewResult).Model as LoginVM;

            Assert.AreNotEqual(null, res);
            Assert.IsInstanceOfType(res, typeof(ViewResult));
            Assert.AreEqual("ss", returnedModel.Email);
            Assert.AreEqual("as", returnedModel.Password);
            Assert.AreEqual("aa", returnedModel.ReturnUrl);
        }

        [TestMethod]
        public void Cannot_Login_EmailNotConfirmed_Redirect()
        {
            LoginVM model = new LoginVM() { Email = "userNotConfirmed", Password = "userNotConfirmed", ReturnUrl = "userNotConfirmed" };
            ActionResult result = authController.LogIn(model).Result;

            serviceMock.Mock.Verify(x => x.GenerateTokenToLogin(It.IsAny<string>()), Times.Never);
            serviceMock.Mock.Verify(x => x.SetTwoFactorAuthCookie(It.IsAny<string>()), Times.Never);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Login_Exception()
        {
            LoginVM model = new LoginVM() { Email = "userConfirmed", Password = "userConfirmed", ReturnUrl = "userConfirmed" };
            serviceMock.Mock.Setup(x => x.GenerateTokenToLogin(It.IsAny<string>())).Throws(new Exception());
            ActionResult result = authController.LogIn(model).Result;

            

            serviceMock.Mock.Verify(x => x.GenerateTokenToLogin(It.IsAny<string>()), Times.Once);
            serviceMock.Mock.Verify(x => x.SetTwoFactorAuthCookie(It.IsAny<string>()), Times.Never);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_Login_RedirectToAction()
        {
            LoginVM model = new LoginVM() { Email = "userConfirmed", Password = "userConfirmed", ReturnUrl = "userConfirmed" };
            var res = authController.LogIn(model).Result as RedirectToRouteResult;

            serviceMock.Mock.Verify(x => x.GenerateTokenToLogin(It.IsAny<string>()), Times.Once);
            serviceMock.Mock.Verify(x => x.SetTwoFactorAuthCookie(It.IsAny<string>()), Times.Once);
            Assert.AreEqual("VerifyCode", res.RouteValues["action"]);
        }

        [TestMethod]
        public void Can_VerifyCode()
        {
            serviceMock.Mock.Setup(x => x.GetTwoFactorUserIdAsync()).Returns(Task.FromResult("someString"));

            ActionResult res = authController.VerifyCode().Result as ViewResult;
            Assert.IsInstanceOfType(res, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_VerifyCode()
        {
            serviceMock.Mock.Setup(x => x.GetTwoFactorUserIdAsync()).Returns(Task.FromResult(""));

            var res = authController.VerifyCode().Result as RedirectToRouteResult;
            Assert.AreEqual("LogIn", res.RouteValues["action"]);
        }

        [TestMethod]
        public void Cannot_VerifyCode_RedirectModelInvalid()
        {
            authController.ModelState.AddModelError("Error", "Error");
            VerifyCodeVM model = new VerifyCodeVM() { token = "someToken"};
            ActionResult res = authController.VerifyCode(model).Result;

            var returnedModel = (res as ViewResult).Model as VerifyCodeVM;

            Assert.AreNotEqual(null, res);
            Assert.IsInstanceOfType(res, typeof(ViewResult));
            Assert.AreEqual("someToken", returnedModel.token);
        }

        [TestMethod]
        public void Cannot_VerifyCode_NullObject_BadRequest()
        {
            var res = authController.VerifyCode(null).Result as HttpStatusCodeResult;
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Assert.AreNotEqual(null, res);
            Assert.AreEqual(res.StatusCode, expected.StatusCode);
        }

        [TestMethod]
        public void Cannot_VerifyCode_WrongVM_NotFound()
        {
            serviceMock.Mock.Setup(x => x.GetTwoFactorUserIdAsync()).Returns(Task.FromResult(""));
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.NotFound);

            VerifyCodeVM model = new VerifyCodeVM() { };

            var result =  authController.VerifyCode(model).Result as HttpStatusCodeResult;
            Assert.AreNotEqual(null, result);
            Assert.AreEqual(result.StatusCode, expected.StatusCode);
        }

        [TestMethod]
        public void Cannot_VerifyCode_UserNull_Redirect()
        {
            serviceMock.Mock.Setup(x => x.GetTwoFactorUserIdAsync()).Returns(Task.FromResult("someId"));
            VerifyCodeVM model = new VerifyCodeVM() { token = "someToken" };
            ActionResult res = authController.VerifyCode(model).Result as ViewResult;

            Assert.IsInstanceOfType(res, typeof(ViewResult));
            serviceMock.Mock.Verify(x => x.CreateIdentityAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Never);
            serviceMock.Mock.Verify(x => x.SignInAsync(It.IsAny<IdentityUser>()), Times.Never);
        }

        [TestMethod]
        public void Cannot_VerifyCode_Exception()
        {
            serviceMock.Mock.Setup(x => x.CreateIdentityAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).Throws(new Exception());
            serviceMock.Mock.Setup(x => x.GetTwoFactorUserIdAsync()).Returns(Task.FromResult("someValidId"));

            VerifyCodeVM model = new VerifyCodeVM() { token = "someToken" };
            ActionResult res = authController.VerifyCode(model).Result as ViewResult;

            Assert.IsInstanceOfType(res, typeof(ViewResult));
            serviceMock.Mock.Verify(x => x.CreateIdentityAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
            serviceMock.Mock.Verify(x => x.SignInAsync(It.IsAny<IdentityUser>()), Times.Never);
        }

        [TestMethod]
        public void Can_VerifyCode_ValidateToSite()
        {
            serviceMock.Mock.Setup(x => x.GetTwoFactorUserIdAsync()).Returns(Task.FromResult("someValidId"));
            VerifyCodeVM model = new VerifyCodeVM() { token = "someToken" };

            var res = authController.VerifyCode(model).Result as RedirectToRouteResult;

            Assert.AreEqual("List", res.RouteValues["action"]);
            serviceMock.Mock.Verify(x => x.CreateIdentityAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
            serviceMock.Mock.Verify(x => x.SignInAsync(It.IsAny<IdentityUser>()), Times.Once);
        }

        [TestMethod]
        public void Can_Register_ReturnView()
        {
            ActionResult result = authController.Register();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_Register_InvalidModel()
        {
            authController.ModelState.AddModelError("Error", "Error");
            RegisterVM model = new RegisterVM() { Email = "xx", Password = "xx"};
            ActionResult res = authController.Register(model).Result;
            var returnedModel = (res as ViewResult).Model as RegisterVM;

            Assert.AreNotEqual(null, res);
            Assert.IsInstanceOfType(res, typeof(ViewResult));
            Assert.AreEqual("xx", returnedModel.Email);
            Assert.AreEqual("xx", returnedModel.Password);
        }

        [TestMethod]
        public void Cannot_Register_ModelNull()
        {
            var res = authController.Register(null).Result as HttpStatusCodeResult;
            HttpStatusCodeResult expected = new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Assert.AreNotEqual(null, res);
            Assert.AreEqual(res.StatusCode, expected.StatusCode);
        }

        [TestMethod]
        public void Cannot_Register_NewUserException()
        {

            RegisterVM model = new RegisterVM() {ConfirmPassword = "xx", Name = "",  Email = "xx", Password = "xx" };
            serviceMock.Mock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).Throws(new Exception());

            var result =  authController.Register(model).Result as ViewResult;

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            serviceMock.Mock.Verify(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
            serviceMock.Mock.Verify(x => x.SetTwoFactorEnabledProtection(It.IsAny<string>()), Times.Never);
            serviceMock.Mock.Verify(x => x.GenerateEmailToken(It.IsAny<string>()), Times.Never);
            serviceMock.Mock.Verify(x => x.SendConfirmationEMail(It.IsAny<string>(), It.IsAny<string>()), Times.Never);      
        }

        [TestMethod]
        public void Can_Register_WithPramsReturnView()
        {

            RegisterVM model = new RegisterVM() { ConfirmPassword = "xx", Name = "", Email = "xx", Password = "xx" };

            var result = authController.Register(model).Result as ViewResult;

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            serviceMock.Mock.Verify(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once);
            serviceMock.Mock.Verify(x => x.SetTwoFactorEnabledProtection(It.IsAny<string>()), Times.Once);
            serviceMock.Mock.Verify(x => x.GenerateEmailToken(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Can_Update_User()
        {
            RegisterVM model = new RegisterVM() { Id="xx",  ConfirmPassword = "xx", Name = "", Email = "xx", Password = "xx" };
            var result = authController.Register(model).Result as RedirectToRouteResult;

            serviceMock.Mock.Verify(x => x.UpdateUserData(It.IsAny<IdentityUser>()), Times.Once);
            Assert.AreEqual("List", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Cannot_Update_UserException()
        {
            RegisterVM model = new RegisterVM() { Id = "xx", ConfirmPassword = "xx", Name = "", Email = "xx", Password = "xx" };
            serviceMock.Mock.Setup(x => x.UpdateUserData(It.IsAny<IdentityUser>())).Throws(new Exception());

            var result = authController.Register(model).Result as ViewResult;

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsNotInstanceOfType(result, typeof(RedirectToRouteResult));
        }

        [TestMethod]
        public void Cannot_ConfirmEmail_InvalidData()
        {
            var result = authController.ConfirmEmail(AuthConstants.InvalidId, AuthConstants.InvalidToken).Result as ViewResult;
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
            Assert.AreEqual("Error", result.ViewName);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Cannot_ConfirmEmail_NullData()
        {
            var result = authController.ConfirmEmail( null, null ).Result as ViewResult;
            Assert.AreEqual("Error", result.ViewName);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_ConfirmEmail_NullData()
        {
            var result = authController.ConfirmEmail(AuthConstants.ValidId, AuthConstants.ValidToken).Result as ViewResult;
            Assert.AreEqual("ConfirmEmail", result.ViewName);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }





    }
}
