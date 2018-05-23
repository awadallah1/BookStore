using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using BookStore.Domain.Concrete;
using System.Collections.Generic;
using BookStore.Domain.Entities;
using BookStore.WebUI.Controllers;
using System.Web.Mvc;
using System.Collections;
using BookStore.WebUI.Models;
using BookStore.WebUI.HtmlHelpers;

namespace BookStore.UnitTests
{
    [TestClass]
    public class CartTests

    {

        //Test that cart can add new item
        [TestMethod]
        public void can_add_newItem()
        {
            //Arrange

            Book book1 = new Book { ISBN = 1, Title = "Book1" };
            Book book2 = new Book { ISBN = 2, Title = "Book2" };
            Cart target = new Cart();

            //Action
            target.AddItem(book1);
            target.AddItem(book2);
            CartLine[] result = target.Lines.ToArray();

            //Assert
            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Book, book1);
            Assert.AreEqual(result[1].Book, book2);
        }

        //Test that cart can exceed quantity of same book
        [TestMethod]
        public void can_add_quantity_to_existing_item()
        {
            //Arrange
            Book book1 = new Book { ISBN = 1, Title = "Book1" };
            Book book2 = new Book { ISBN = 2, Title = "Book2" };
            Cart target = new Cart();

            //Action
            target.AddItem(book1, 1);
            target.AddItem(book1, 2);
            CartLine[] result = target.Lines.OrderBy(cl => cl.Book.ISBN).ToArray();

            //Assert
            Assert.AreEqual(result[0].Quantity, 3);
            Assert.AreEqual(result.Length, 1);

        }

        //Test that cart can remove cartline
        [TestMethod]
        public void can_remove_line()
        {
            //Arrange
            Book book1 = new Book { ISBN = 1, Title = "Book1" };
            Book book2 = new Book { ISBN = 2, Title = "Book2" };
            Cart target = new Cart();
            target.AddItem(book1, 1);
            target.AddItem(book2, 2);
            target.AddItem(book1, 10);

            //Action
            target.RemoveItem(book2);

            //Assert
            Assert.AreEqual(target.Lines.Where(cl => cl.Book == book2).Count(), 0);
            Assert.AreEqual(target.Lines.FirstOrDefault(cl => cl.Book == book1).Quantity, 11);
        }

        //Test the method of calulate total price of shopping cart
        [TestMethod]
        public void calculate_cart_total()
        {
            //Arrange
            Book book1 = new Book { ISBN = 1, Title = "Book1", Price = 5M };
            Book book2 = new Book { ISBN = 2, Title = "Book2", Price = 10M };
            Cart target = new Cart();
            target.AddItem(book1, 5);
            target.AddItem(book2, 2);
            target.AddItem(book1, 3);

            //Action
            decimal CartTotal = target.ComputeTotalValue();

            //Assert
            Assert.AreEqual(CartTotal, 60M);
        }

        //Test the method of reset cart to be empty and ready to now purchasing process
        [TestMethod]
        public void can_clear_content()
        {
            //Arrange
            Book book1 = new Book { ISBN = 1, Title = "Book1" };
            Book book2 = new Book { ISBN = 2, Title = "Book2" };
            Cart target = new Cart();
            target.AddItem(book1, 1);
            target.AddItem(book2, 2);
            target.AddItem(book1, 10);

            //Action
            target.clear();

            //Assert
            Assert.AreEqual(target.Lines.Count(), 0);
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            //Arrange
            Mock<IBooksRepository> mock = new Mock<IBooksRepository>();
            mock.Setup(r => r.Books).Returns(new Book[] { new Book { ISBN = 1, Title = "Asp.Net Mvc" } }.AsQueryable());
            //Act
            CartController target = new CartController(mock.Object, null);
            Cart cart = new Cart();
            target.AddToCart(cart, 1, "MyUrl");
            //Assert
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Book.ISBN, 1);

        }

        [TestMethod]
        public void can_redirect_to_routeUrl()
        {
            //Arrange
            Mock<IBooksRepository> mock = new Mock<IBooksRepository>();
            mock.Setup(r => r.Books).Returns(new Book[] { new Book { ISBN = 1, Title = "Asp.Net Mvc" } }.AsQueryable());
            //Act
            CartController target = new CartController(mock.Object, null);
            Cart cart = new Cart();
            //Action

            RedirectToRouteResult result = target.AddToCart(cart, 1, "MyUrl");

            //Assert
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "MyUrl");
        }

        [TestMethod]
        public void can_view_cart_content()
        {
            //Arrange
            var target = new CartController(null, null);
            Cart cart = new Cart();
            //Action
            ViewResult result = target.Index(cart, "MyUrl");
            CartIndexViewModel cartmodel = (CartIndexViewModel)result.ViewData.Model;
            //Assert
            Assert.AreEqual(cartmodel.Returnurl, "MyUrl");
            Assert.AreSame(cartmodel.Cart, cart);

        }

        [TestMethod]
        public void cannot_checkout_empty_cart()
        {
            //Arrange
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            // mock.Setup(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()));
            CartController target = new CartController(null, mock.Object);
            Cart cart = new Cart();
            ShippingDetails shippingDetails = new ShippingDetails();
            //Action
            ViewResult result = target.Checkout(cart, shippingDetails);
            //Assert
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

            Assert.AreEqual("", result.ViewName);
            Assert.IsFalse(result.ViewData.ModelState.IsValid);

        }

        [TestMethod]
        public void cannot_checkout_invalid_shippingDetails()
        {
            //Arrang
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Book(), 1);
            ShippingDetails shippingDetails = new ShippingDetails();
            
            CartController target = new CartController(null, mock.Object);
            target.ModelState.AddModelError("error", "error");
            //Act
            ViewResult result = target.Checkout(cart, shippingDetails);
           
            //Assert
            Assert.AreEqual("", result.ViewName);
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

        }


    }
}
