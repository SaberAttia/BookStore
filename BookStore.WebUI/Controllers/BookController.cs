using BookStore.Domain.Abstract;
using BookStore.WebUI.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    public class BookController : Controller //11
    {
        private IBookRepository repository; //12
        public int PageSize = 4; //26

        public BookController(IBookRepository bookRep) //13 "Constructor"
        {
            repository = bookRep;
        }
        public ViewResult List(string specialization, int pageno = 1) //26,71,84
        {
            BookListViewModel model =
                  new BookListViewModel { Books = repository.Books //71
                                                                    .Where (b => specialization == null || b.Specialization == specialization) //85
                                                                    .OrderBy(b => b.ISBN)
                                                                    .Skip((pageno - 1) * PageSize)
                                                                    .Take(PageSize),
                                                                    pagingInfo = new Models.PagingInfo
                                                                    {
                                                                        CurrentPage = pageno, 
                                                                        ItemsPerPage = PageSize,
                                                                        //TotalItems = repository.Books.Count()
                                                                        TotalItems = specialization == null ? //82
                                                                                repository.Books.Count() :
                                                                                repository.Books.Where(b => b.Specialization == specialization).Count()

                                                                    },
                                                                    CurrentSpecialization = specialization 
                                                                     };   
              return View(model);


            //return View(repository.Books //26
            //    .OrderBy(b => b.ISBN)
            //    .Skip((pageno-1)*PageSize)
            //    .Take(PageSize)
            //    return View();
        }

        public ViewResult ListAll() //14
        {
            return View(repository.Books);
        }
    }
}