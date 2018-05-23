using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BookStore.Domain.Concrete;
using BookStore.Domain.Entities;
using BookStore.WebUI.Controllers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace BookStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_All_Books()
        {
            //Arrange
            Mock<IBooksRepository> mock = new Mock<IBooksRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>{
                new Book { ISBN=1,Title="book1"},
                new Book { ISBN=2,Title="book2"},
                new Book { ISBN=3,Title="book3"}
            });

            AdminController target = new AdminController(mock.Object);

            //Act
            Book[] result = ((IEnumerable<Book>)target.Index().ViewData.Model).ToArray();

            //Assert
            Assert.AreEqual(3, result.Length);
            Assert.AreEqual("book3", result[2].Title);

        }

        [TestMethod]
        public void Can_Edit_Exist_Book()
        {
            //Arrange
            Mock<IBooksRepository> mock = new Mock<IBooksRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book {ISBN=1,Title="Web" },
                new Book {ISBN=2,Title="DB" },
                new Book {ISBN=3,Title="Network" }

            });

            //Act
            AdminController target = new AdminController(mock.Object);
            Book b1 = target.Edit(1).ViewData.Model as Book;
            Book b2 = target.Edit(2).ViewData.Model as Book;
            Book b3 = target.Edit(3).ViewData.Model as Book;

            //Assert
            Assert.AreEqual(1, b1.ISBN);
            Assert.AreEqual("Network", b3.Title);

        }

        [TestMethod]
        public void Cannot_Edit_nonExists_Book()
        {
            //Arrange
            Mock<IBooksRepository> mock = new Mock<IBooksRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book {ISBN=1,Title="Web" },
                new Book {ISBN=2,Title="DB" },
                new Book {ISBN=3,Title="Network" }

            });

            //Act
            AdminController target = new AdminController(mock.Object);

            Book b4 = target.Edit(4).ViewData.Model as Book;

            //Assert
            Assert.IsNull(b4);
        }

       
        [TestMethod]
        public void Cannot_Save_InValidChanges()
        {
            //Arrange
            Mock<IBooksRepository> mock = new Mock<IBooksRepository>();
            AdminController target = new AdminController(mock.Object);
            Book book = new Book { Title = "Test" };
            target.ModelState.AddModelError("error", "error");

            //Act try to save changes
            ActionResult result = target.Edit(book);

            //Assert check repository that was called
            mock.Verify(m => m.SaveBook(It.IsAny<Book>()), Times.Never());
            //Assert check result type
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Can_delete()
        {
            //Arrange
            Mock<IBooksRepository> mock = new Mock<IBooksRepository>();
            Book book = new Book { ISBN = 3, Title = "Book3" };
            mock.Setup(m => m.Books).Returns(new Book[] {
                new Book {ISBN=1,Title="Book1" },
                book,
                new Book {ISBN=1,Title="Book2" }
            });
            AdminController target = new AdminController(mock.Object);


            //Act
            target.Delete(book.ISBN,null);

            //Assert
            mock.Verify(m => m.DeleteBook(book.ISBN));
        }

    }
}
