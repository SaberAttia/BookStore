﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BookStore.Domain.Entities;
using System.Linq;
using Moq;
using BookStore.Domain.Abstract;
using BookStore.WebUI.Controllers;
using System.Web.Mvc;
using BookStore.WebUI.Models;

namespace BookStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_AddNew_Lines() //107
        {
            //Arrange //108
            Book b1 = new Book { ISBN = 1, Title = "ASP.NET" };
            Book b2 = new Book { ISBN = 2, Title = "Oracle" };

            //Act //109
            Cart target = new Cart();
            target.AddItem(b1);
            target.AddItem(b2, 3);
            CartLine[] result = target.Lines.ToArray();

            //Assert /110
            Assert.AreEqual(result[0].Book, b1);
            Assert.AreEqual(result[1].Book, b2);
        }

        [TestMethod]
        public void Can_Add_Qty_For_Existing_Lines() //111
        {
            //Arrange //108
            Book b1 = new Book { ISBN = 1, Title = "ASP.NET" };
            Book b2 = new Book { ISBN = 2, Title = "Oracle" };

            //Act //109
            Cart target = new Cart();
            target.AddItem(b1);
            target.AddItem(b2, 3);
            target.AddItem(b1, 5);
            CartLine[] result = target.Lines.OrderBy(
                            cl => cl.Book.ISBN).ToArray();

            //Assert /110
            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Quantity, 6);
            Assert.AreEqual(result[1].Quantity, 3);
        }

        [TestMethod]
        public void Can_Remove_Line() //112
        {
            //Arrange //108
            Book b1 = new Book { ISBN = 1, Title = "ASP.NET" };
            Book b2 = new Book { ISBN = 2, Title = "Oracle" };
            Book b3 = new Book { ISBN = 3, Title = "C#" };

            //Act //109
            Cart target = new Cart();
            target.AddItem(b1);
            target.AddItem(b2, 3);
            target.AddItem(b3, 5);
            target.AddItem(b2, 1);
            target.RemoveLine(b2);

            //Assert /110
            Assert.AreEqual(target.Lines.Where(cl => cl.Book == b2).Count(), 0);
            Assert.AreEqual(target.Lines.Count(), 2);
        }

        [TestMethod]
        public void Calculte_Cart_Total() //113
        {
            //Arrange //108
            Book b1 = new Book { ISBN = 1, Title = "ASP.NET", Price = 100M };
            Book b2 = new Book { ISBN = 2, Title = "Oracle", Price = 50M };
            Book b3 = new Book { ISBN = 3, Title = "C#", Price = 70M };

            //Act //109
            Cart target = new Cart();
            target.AddItem(b1, 1);
            target.AddItem(b2, 2);
            target.AddItem(b3);
            decimal result = target.ComputeTotalValue();

            //Assert /110
            Assert.AreEqual(result, 270);
        }

        [TestMethod]
        public void Can_Clear() //114
        {
            //Arrange //108
            Book b1 = new Book { ISBN = 1, Title = "ASP.NET", Price = 100M };
            Book b2 = new Book { ISBN = 2, Title = "Oracle", Price = 50M };
            Book b3 = new Book { ISBN = 3, Title = "C#", Price = 70M };

            //Act //109
            Cart target = new Cart();
            target.AddItem(b1, 1);
            target.AddItem(b2, 2);
            target.AddItem(b3, 5);
            target.Clear();

            //Assert /110
            Assert.AreEqual(target.Lines.Count(), 0);
        }

        [TestMethod]
        public void Can_Add_To_Cart() //129
        {
            //Arrange 
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(
                new Book[]
                {
                     new Book { ISBN = 1, Title = "ASP.NET MVC", Specialization = "Programming" },
                }.AsQueryable()
                );
            Cart cart = new Cart();
            CartController target = new CartController(mock.Object, null);

            //Act
            target.AddToCart(cart, 1, null);

            //Assert 
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Book.Title, "ASP.NET MVC");
        }

        [TestMethod]
        public void Adding_Book_To_Cart_Goes_To_Cart_Screen() //130
        {
            //Arrange 
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(
                new Book[]
                {
                     new Book { ISBN = 1, Title = "ASP.NET MVC", Specialization = "Programming" },
                }.AsQueryable()
                );
            Cart cart = new Cart();
            CartController target = new CartController(mock.Object, null);

            //Act
            RedirectToRouteResult result = target.AddToCart(cart, 2, "myUrl");

            //Assert 
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Contant() //130
        {
            //Arrange 
            Cart cart = new Cart();
            CartController target = new CartController(null, null);
            //Act
            CartIndexViewModel result = (CartIndexViewModel) target.Index(cart, "myUrl").ViewData.Model;

            //Assert 
            Assert.AreEqual(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        [TestMethod]
        public void Cannot_Checkout_Empty_Cart() //149
        {
            //Arrange 
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController target = new CartController(null, mock.Object);

            //Act
            ViewResult result = target.Checkout(cart, shippingDetails);

            //Assert 
            //mock.Verify(m => m.)
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails() //150
        {
            //Arrange 
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Book(), 1);
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController target = new CartController(null, mock.Object);
            target.ModelState.AddModelError("error", "error");

            //Act
            ViewResult result = target.Checkout(cart, shippingDetails);

            //Assert 
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), 
                                       It.IsAny<ShippingDetails>()), Times.Never());
            Assert.AreEqual("", result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_And_Submit_Order() //150
        {
            //Arrange 
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Book(), 1);
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController target = new CartController(null, mock.Object);

            //Act
            ViewResult result = target.Checkout(cart, shippingDetails);

            //Assert 
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(),
                                       It.IsAny<ShippingDetails>()), Times.Once());
            Assert.AreEqual("Completed", result.ViewName);
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
