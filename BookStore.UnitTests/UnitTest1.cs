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
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            //arrange
            Mock<IBooksRepository> mock = new Mock<IBooksRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book> {
                new Book {ISBN=1,Title="Book1" },
                new Book {ISBN=2,Title="Book2"},
                new Book {ISBN=3,Title="Book3" },
                new Book {ISBN=4,Title="Book4" },
                new Book {ISBN=5,Title="Book5"}
                });
            BookController Controller = new BookController(mock.Object);
            Controller.PageSize = 3;
            //Act
            BooksListViewModel result = (BooksListViewModel)Controller.List(null,2).Model;
            Book[] BooksArray = result.Books.ToArray();
            //Assert
            Assert.IsTrue(BooksArray.Length == 2);
            Assert.AreEqual(BooksArray[0].Title, "Book4");
            Assert.AreEqual(BooksArray[1].Title, "Book5");
            

        }


        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //Arrange
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new PagingInfo
            {
                ItemsPerPage = 10,
                CurrentPage = 2,
                TotalItems = 28
            };

            Func<int, string> pageurl = i => "Page" + i;
            
            //Act
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageurl);

            //Asserts
            string expected = "<a class=\"btn btn-default\" href=\"Page1\">1</a>"
                            + "<a class=\"btn btn-default btn-primary selected\" href=\"Page2\">2</a>"
                            + "<a class=\"btn btn-default\" href=\"Page3\">3</a>";
            Assert.AreEqual(expected, result.ToString());
            //Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>" 
            //                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>" +
            //                @"<a class=""btn btn-default"" href=""Page3"">3</a>", result.ToString());
        }

        [TestMethod]
        public void Can_Send_View_Model()
        {
            //Arrange
            Mock<IBooksRepository> mock = new Mock<IBooksRepository>();
            mock.Setup(m => m.Books).Returns(new Book[]
            {   new Book{ISBN =1,Title="b1"},
                new Book{ISBN =2,Title="b2"},
                new Book{ISBN =3,Title="b3"},
                new Book{ISBN =4,Title="b4"},
                new Book{ISBN =5,Title="b5"},
                new Book{ISBN =6,Title="b6"},
            });
            BookController controller = new BookController(mock.Object);

            //Act
            BooksListViewModel result = (BooksListViewModel)controller.List(null,2).Model;
            int ItemPerPage = result.Books.Count();
            int currentPage = result.pagingInfo.CurrentPage;
            //Assert
            Assert.AreEqual(2, ItemPerPage);
            Assert.AreEqual(2, currentPage);

        }

        [TestMethod]
        public void can_filter_Books()
        {
            //Arrange
            Mock<IBooksRepository> mock = new Mock<IBooksRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book> {
                new Book {ISBN=1,Title="Book1",Category="IT" },
                new Book {ISBN=2,Title="Book2",Category="IT"},
                new Book {ISBN=3,Title="Book3",Category="OS" },
                new Book {ISBN=4,Title="Book4",Category="IT" },
                new Book {ISBN=5,Title="Book5",Category="IT"}
                });
            BookController Controller = new BookController(mock.Object);
            Controller.PageSize = 3;

            //act
            Book[] result = ((BooksListViewModel)Controller.List("IT", 2).Model).Books.ToArray();

            //assert
            Assert.AreEqual(result.Count(), 1);
            Assert.IsTrue(result[0].Title == "Book5"&&result[0].Category=="IT");
            
        }
        [TestMethod]
        public void can_create_category()
        {
            //Arrange
            Mock<IBooksRepository> mock = new Mock<IBooksRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book> {
                new Book {ISBN=1,Title="Book1",Category="IT" },
                new Book {ISBN=2,Title="Book2",Category="IT"},
                new Book {ISBN=3,Title="Book3",Category="OS" },
                new Book {ISBN=4,Title="Book4",Category="IT" },
                new Book {ISBN=5,Title="Book5",Category="IT"}
                });
            NavController Controller = new NavController(mock.Object);


            //act
            Dictionary<string, int> result = ((Dictionary<string, int>)Controller.Menu().ViewData.Model);

            //assert
            Assert.AreEqual(2,result.Keys.Count);
            Assert.IsTrue(result.Keys.First() == "IT" && result.Values.First() == 4);
            

        }
        [TestMethod]
        public void can_generate_specific_category_books_count()
        {
            //Arrange
            Mock<IBooksRepository> mock = new Mock<IBooksRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book> {
                new Book {ISBN=1,Title="Book1",Category="IT" },
                new Book {ISBN=2,Title="Book2",Category="IT"},
                new Book {ISBN=3,Title="Book3",Category="OS" },
                new Book {ISBN=4,Title="Book4",Category="OS" },
                new Book {ISBN=5,Title="Book5",Category="IT"}
                });
            BookController Controller = new BookController(mock.Object);


            //Action
            int result1 = ((BooksListViewModel)Controller.List("IT").Model).pagingInfo.TotalItems;
            int result2 = ((BooksListViewModel)Controller.List("OS").Model).pagingInfo.TotalItems;
            int result3 = ((BooksListViewModel)Controller.List(null).Model).pagingInfo.TotalItems;

            //Assert
            Assert.AreEqual(result1, 3);
            Assert.AreEqual(result2, 2);
            Assert.AreEqual(result3, 5);

        }

        [TestMethod]
        public void can_indicate_specific_category()
        {
            //Arrange
            Mock<IBooksRepository> mock = new Mock<IBooksRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book> {
                new Book {ISBN=1,Title="Book1",Category="IT" },
                new Book {ISBN=2,Title="Book2",Category="IT"},
                new Book {ISBN=3,Title="Book3",Category="OS" },
                new Book {ISBN=4,Title="Book4",Category="OS" },
                new Book {ISBN=5,Title="Book5",Category="IT"}
                });
            NavController Controller = new NavController(mock.Object);

            //Action
            string result = Controller.Menu("IT").ViewBag.selectedCategory;

            //Assert
            Assert.AreEqual(result, "IT");
           

        }
    }

    }

