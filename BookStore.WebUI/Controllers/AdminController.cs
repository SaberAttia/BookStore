using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    //[Authorize]
    public class AdminController : Controller   //160
    {
        private IBookRepository repository;
        public AdminController(IBookRepository repo)
        {
            repository = repo;
        }
        public ViewResult Index()
        {
            return View(repository.Books);
        }
        public ViewResult Edit(int ISBN) //169
        {
            Book book = repository.Books.FirstOrDefault(b => b.ISBN == ISBN);
            return View(book);
        }
        [HttpPost]  //173
        public ActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                repository.SaveBook(book);
                TempData["message"] = book.Title + " has been saved";
                return RedirectToAction("Index");
            }
            else
            {
                //not completed
                return View(book);
            }
        }

        public ViewResult Create()   //181
        {
            return View("Edit", new Book());
        }

        [HttpGet]  //185
        public ActionResult Delete(int ISBN)
        {
            Book deletedBook = repository.DeleteBook(ISBN);
            if (deletedBook != null)
            {
                TempData["message"] = deletedBook.Title + " was deleted";
            }
            return RedirectToAction("Index");
        }
    }
}