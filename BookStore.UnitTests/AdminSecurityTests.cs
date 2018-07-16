﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BookStore.WebUI.Infrastructure.Abstract;
using BookStore.WebUI.Models;
using BookStore.WebUI.Controllers;
using System.Web.Mvc;

namespace BookStore.UnitTests
{
    [TestClass]
    public class AdminSecurityTests
    {
        [TestMethod]
        public void Can_Login_With_Vaild_Credentials()
        {
            //Arrange
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "secret")).Returns(true);
            LoginViewModel model = new LoginViewModel
            {
                Username = "admin",
                Password = "secret"
            };
            AccountController target = new AccountController(mock.Object);

            //Act
            ActionResult result = target.Login(model, "/MyUrl");

            //Assert 
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/MyUrl", ((RedirectResult)result).Url);
        }

        [TestMethod]
        public void Can_Login_With_InVaild_Credentials()
        {
            //Arrange
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("UserX", "PassX")).Returns(false);
            LoginViewModel model = new LoginViewModel
            {
                Username = "UserX",
                Password = "PassX"
            };
            AccountController target = new AccountController(mock.Object);

            //Act
            ActionResult result = target.Login(model, "/MyUrl");

            //Assert 
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
