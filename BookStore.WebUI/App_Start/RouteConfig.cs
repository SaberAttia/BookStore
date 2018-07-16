using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BookStore.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(null,                //87 url/
                "",new
                {
                    controller = "Book",
                    Action = "List",
                    specialization = (string)null,
                    pageno =1
                }
                );

            routes.MapRoute(null,                 //89 url/BookListPage2
               "BookListPage{pageno}", new
               {
                   controller = "Book",
                   Action = "List",
                   specialization = (string)null,
               }
               );

            routes.MapRoute(null,                 //88 url/Information System
               "{specialization}", new
               {
                   controller = "Book",
                   Action = "List",
                   pageno = 1
               }
               );

            routes.MapRoute(null,                 //90 url/Information System/Page2
               "{specialization}/Page{pageno}", new
               {
                   controller = "Book",
                   Action = "List",
               },
               new { pageno = @"\d+" }  //92
               );

           // routes.MapRoute(
           //    name: null,
           //    url: "BookListPage{pageno}",
           //    defaults: new { controller = "Book", action = "List" } //78
           //);


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {  id = UrlParameter.Optional } //15
            );
        }
    }
}
