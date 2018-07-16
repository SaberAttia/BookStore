using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using BookStore.WebUI.HTMLHelper; //61
using System.Web.Mvc; //62
using BookStore.WebUI.Models; //63
using BookStore.WebUI.Infrastructure.Abstract;

namespace BookStore.UnitTests
{
    [TestClass]
    public class ProductCatalog
    {
        [TestMethod]
        public void Can_Paginate() //27
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>(); //28
            mock.Setup(b => b.Books).Returns(new Book[]            //29
            {
                new Book {ISBN = 1, Title = "Book1"},
                new Book {ISBN = 2, Title = "Book2"},
                new Book {ISBN = 3, Title = "Book3"},
                new Book {ISBN = 4, Title = "Book4"},
                new Book {ISBN = 5, Title = "Book5"}
            });
            BookController controller = new BookController(mock.Object);//30
            controller.PageSize = 3;

            //Act
            // IEnumerable<Book> result = (IEnumerable<Book>) controller.List(2).Model;
            BookListViewModel result = (BookListViewModel)controller.List(null,1).Model; //73


            //Assert //31
            //Book[] bookArray = result.ToArray();
            Book[] bookArray = result.Books.ToArray(); //74
            Assert.IsTrue(bookArray.Length == 3);
            Assert.AreEqual(bookArray[0].Title, "Book1");
            Assert.AreEqual(bookArray[1].Title, "Book2");
            Assert.AreEqual(bookArray[2].Title, "Book3");
        }

        [TestMethod]
        public void Can_Generate_Page_Links() //60
        {
            //Arrange
            HtmlHelper myHelper = null; //61,62
            PagingInfo pagingInfo = new PagingInfo {      //63
                CurrentPage = 2,
                TotalItems = 14,
                ItemsPerPage = 5
            };
            Func<int, string> PageUrlDelegate = i => "Page" + i;
            String expectedResult = "<a class=\"btn btn-default\" href=\"Page1\">1</a>"  //66
                                                        + "<a class=\"btn btn-default btn-primary selected\" href=\"Page2\">2</a>"
                                                        + "<a class=\"btn btn-default\" href=\"Page3\">3</a>"; 
            //Act
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, PageUrlDelegate); //64
           
            //Assert //65
            Assert.AreEqual(expectedResult, result.ToString());

        }

        [TestMethod]
        public void Can_Send_Pagination_View_model() //72
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>(); 
            mock.Setup(b => b.Books).Returns(new Book[]       
           {
                new Book {ISBN = 1, Title = "Operating System"},
                new Book {ISBN = 2, Title = "Web Application Asp.Net"},
                new Book {ISBN = 3, Title = "Android Mobile Applications"},
                new Book {ISBN = 4, Title = "Database Systems"},
                new Book {ISBN = 5, Title = "MIS"}
           });
            BookController controller = new BookController(mock.Object); 
            controller.PageSize = 3;

            //Act
            BookListViewModel result = (BookListViewModel)controller.List(null,2).Model;

            //Assert 
            PagingInfo pageInfo = result.pagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);



        }

        [TestMethod]
        public void Can_Filter_Books() //85
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new Book[]
           {
                new Book {ISBN = 1, Title = "Operating System", Specialization = "CS"},
                new Book {ISBN = 2, Title = "Web Application Asp.Net", Specialization = "IS"},
                new Book {ISBN = 3, Title = "Android Mobile Applications", Specialization = "IS"},
                new Book {ISBN = 4, Title = "Database Systems", Specialization = "IS"},
                new Book {ISBN = 5, Title = "MIS", Specialization = "IS"}
           });
            BookController controller = new BookController(mock.Object);
            controller.PageSize = 3;

            //Act
            Book[] result = ((BookListViewModel)
                                                controller.List("IS", 2).Model).Books.ToArray();

            //Assert 
            Assert.AreEqual(result.Length,1);
            Assert.IsTrue(result[0].Title == "MIS" && result[0].Specialization == "IS");
        }

        [TestMethod]
        public void Can_Create_Specialization() //94
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new Book[]
           {
                new Book {ISBN = 1, Title = "Operating System", Specialization = "CS"},
                new Book {ISBN = 2, Title = "Web Application Asp.Net", Specialization = "IS"},
                new Book {ISBN = 3, Title = "Android Mobile Applications", Specialization = "IS"},
                new Book {ISBN = 4, Title = "Database Systems", Specialization = "IS"},
                new Book {ISBN = 5, Title = "MIS", Specialization = "IS"}
           });
            NavController controller = new NavController(mock.Object);

            //Act
            string[] result = ((IEnumerable<string>)
                                                controller.Menu().Model).ToArray();

            //Assert 
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0]== "CS" && result[1] == "IS");
        }

        [TestMethod]
        public void Indicates_Selected_Specialization() //98
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new Book[]
           {
                new Book {ISBN = 1, Title = "Operating System", Specialization = "CS"},
                new Book {ISBN = 2, Title = "Web Application Asp.Net", Specialization = "IS"},
                new Book {ISBN = 3, Title = "Android Mobile Applications", Specialization = "IS"},
                new Book {ISBN = 4, Title = "Database Systems", Specialization = "IS"},
                new Book {ISBN = 5, Title = "MIS", Specialization = "IS"}
           });
            NavController controller = new NavController(mock.Object);

            //Act
            string result = controller.Menu("IS").ViewBag.SelectedSpec;

            //Assert 
            Assert.AreEqual("IS", result);
        }

        [TestMethod]
        public void Generate_Specialization_Specific_Book_Count() //99
        {
            //Arrange
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(b => b.Books).Returns(new Book[]
           {
                new Book {ISBN = 1, Title = "Operating System", Specialization = "CS"},
                new Book {ISBN = 2, Title = "Web Application Asp.Net", Specialization = "IS"},
                new Book {ISBN = 3, Title = "Android Mobile Applications", Specialization = "IS"},
                new Book {ISBN = 4, Title = "Database Systems", Specialization = "CS"},
                new Book {ISBN = 5, Title = "MIS", Specialization = "IS"}
           });
            BookController controller = new BookController(mock.Object);

            //Act
            int result1 = ((BookListViewModel)controller.List("IS").Model).pagingInfo.TotalItems;
            int result2 = ((BookListViewModel)controller.List("CS").Model).pagingInfo.TotalItems;

            //Assert 
            Assert.AreEqual(result1, 3);
            Assert.AreEqual(result2, 2);
        }
    }
}
