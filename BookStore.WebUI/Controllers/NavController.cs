using BookStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IBookRepository repository; //93
        public NavController(IBookRepository repo)
        {
            repository = repo;
        }
        public PartialViewResult Menu(string specialization = null/*, bool mobileLayout = false*/ )  //91.94.97.155
        {
            ViewBag.SelectedSpec = specialization; //97
            IEnumerable<string> spec = repository.Books
                                         .Select(b => b.Specialization)
                                         .Distinct();
            //string viewName = mobileLayout ? "MenuHorizontal" : "Menu";
            return PartialView("FlexMenu", spec);
        }
    }
}