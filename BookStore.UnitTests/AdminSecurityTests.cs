using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStore.WebUI.Infrastructur.Abstract;
using Moq;
using BookStore.WebUI.Controllers;
using BookStore.WebUI.Models;
using System.Web.Mvc;

namespace BookStore.UnitTests
{
    [TestClass]
    public class AdminSecurityTests
    {
        [TestMethod]
        public void Can_Login_with_Valid_Credentials()
        {
            //Arrange
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("Amir", "secret")).Returns(true);
            AccountController target = new AccountController(mock.Object);
            LoginViewModel model = new LoginViewModel { UserName = "Amir", Password = "secret" };

            //Act
            ActionResult result = target.Login(model, "/MyUrl");

            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/MyUrl", ((RedirectResult)result).Url);
        }
        [TestMethod]
        public void Can_Login_with_InValid_Credentials()
        {
            //Arrange
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("UserX", "PassX")).Returns(false);
            AccountController target = new AccountController(mock.Object);
            LoginViewModel model = new LoginViewModel { UserName = "UserX", Password = "PassX" };

            //Act
            ActionResult result = target.Login(model, "/MyUrl");

            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
