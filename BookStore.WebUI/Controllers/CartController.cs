using BookStore.Domain.Abstract;
using BookStore.Domain.Entities;
using BookStore.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.WebUI.Controllers
{
    public class CartController : Controller //116
    {
        private IBookRepository repository;
        private IOrderProcessor orderProcessor; //146

        public CartController(IBookRepository repo, IOrderProcessor proc)  //116,146
        {
            repository = repo; //116
            orderProcessor = proc; //146
        }

        public ViewResult Index(Cart cart, string returnUrl) //121,126
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,   //GetCart() to cart
                ReturnUrl = returnUrl
            });
        }
        public RedirectToRouteResult AddToCart(Cart cart , int isbn, string returnUrl) //117,127 
        {
            Book book = repository.Books.
                        FirstOrDefault(b => b.ISBN == isbn);
            if (book != null)
            {
                cart.AddItem(book);  //GetCart() to cart
            }
            return RedirectToAction("Index", new { returnUrl }); //Index
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int isbn, string returnUrl) //118,128
        {
            Book book = repository.Books.
                        FirstOrDefault(b => b.ISBN == isbn);
            if (book != null)
            {
                cart.RemoveLine(book); //GetCart() to cart
            }
            return RedirectToAction("Index", new {returnUrl}); //Index
        }
        //private Cart GetCart()
        //{
        //    Cart cart =(Cart) Session["Cart"]; //119
        //    if(cart == null)
        //    {
        //        cart = new Cart();
        //        Session["Cart"] = cart;
        //    }
        //    return cart;
        //}

        public PartialViewResult Summary (Cart cart) //132
        {
            return PartialView(cart);
        }

        public ViewResult Checkout() //136
        {
            return View(new ShippingDetails());
        }
        [HttpPost]  //148
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails) //147 Overloading
        {
            if (cart.Lines.Count() == 0)
                ModelState.AddModelError("", "Sorry, your cart is empty");
            if(ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }else
                return View(shippingDetails);

        }
    }
}