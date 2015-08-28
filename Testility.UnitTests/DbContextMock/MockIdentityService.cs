using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Testility.UnitTests.Constants;
using Testility.WebUI.Services.Abstract;

namespace Testility.UnitTests.DbContextMock
{
    public class MockIdentityService
    {

        #region Members
        private Mock<IIdentityServices> _mock;

        public IIdentityServices Repository
        {
            get
            {
                if (_mock == null) Init();
                return _mock.Object;
            }

           
        }

        public Mock<IIdentityServices> Mock
        {
            get
            {
                if (_mock == null) Init();
                return _mock;
            }
        }
        #endregion


        private void Init()
        {
            IdentityUser userWithEmailConfirmed = new IdentityUser() { Id = "userConfirmed" };
            IdentityUser userWithNoEmailConfirmed = new IdentityUser() { Id = "userNotConfirmed" };
            IdentityUser userNull = null;

            _mock = new Mock<IIdentityServices>();

            _mock.Setup(x => x.GetUserAsync("userConfirmed", "userConfirmed")).Returns(Task.FromResult(userWithEmailConfirmed));
            _mock.Setup(x => x.GetUserAsync("userNotConfirmed", "userNotConfirmed")).Returns(Task.FromResult(userWithNoEmailConfirmed));

            _mock.Setup(x => x.GetUserName("userConfirmed")).Returns("userConfirmed");
            _mock.Setup(x => x.GetUserName("userNotConfirmed")).Returns("userNotConfirmed");


            _mock.Setup(x => x.IsEmailConfirmed("userConfirmed")).Returns(Task.FromResult(true));
            _mock.Setup(x => x.IsEmailConfirmed("userNotConfirmed")).Returns(Task.FromResult(false));


            _mock.Setup(x => x.GenerateTokenToLogin(It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));
            _mock.Setup(x => x.SetTwoFactorAuthCookie(It.IsAny<string>()));


            _mock.Setup(x => x.GetUserById("someId")).Returns(Task.FromResult(userNull));
            _mock.Setup(x => x.GetUserById("someValidId")).Returns(Task.FromResult(userWithEmailConfirmed));

            _mock.Setup(x => x.VerifyTokenToLogin("userConfirmed", It.IsAny<string>())).Returns(Task.FromResult(true));


            _mock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Success));
            _mock.Setup(x => x.GenerateEmailToken(It.IsAny<string>())).Returns(Task.FromResult("someToken"));

            _mock.Setup(x => x.SendConfirmationEMail(It.IsAny<string>(), It.IsAny<string>()));

            _mock.Setup(x => x.GetUser(It.IsAny<string>())).Returns(userWithEmailConfirmed);

            _mock.Setup(x => x.confirmEmail(AuthConstants.InvalidId, AuthConstants.InvalidToken)).Returns(Task.FromResult(IdentityResult.Failed(new string[] { AuthConstants.Error })));
            _mock.Setup(x => x.confirmEmail(AuthConstants.ValidId, AuthConstants.ValidToken)).Returns(Task.FromResult(IdentityResult.Success));

        }
    }
}
