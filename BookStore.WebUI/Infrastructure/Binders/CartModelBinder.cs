using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using System.Web.ModelBinding; 125

namespace BookStore.WebUI.Infrastructure.Binders
{
    public class CartModelBinder : IModelBinder //123,125
    {
        private const string sessionKey = "Cart";
        public object BindModel(ControllerContext controllerContext, //125
                      ModelBindingContext bindingContext)
        {
            //get Cart from session
            Cart cart = null;
            if (controllerContext.HttpContext.Session != null)
                cart = (Cart) controllerContext.HttpContext.Session[sessionKey];

            if (cart == null)
            {
                cart = new Cart();
                if (controllerContext.HttpContext.Session != null)
                    controllerContext.HttpContext.Session[sessionKey] = cart; 
            }
            return cart;
        }

    }
}