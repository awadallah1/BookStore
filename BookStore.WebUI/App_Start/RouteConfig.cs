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

            ////OLD
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            AreaRegistration.RegisterAllAreas();
            //New Route To Show new url with pagex


            //Lists the first page of Books from all categories
            //      localhost/
            routes.MapRoute(
                name: null,
                url: "",
                defaults: new { controller = "Book", action = "List", category = (string)null, page = 1 }
            );

            //Lists the specified page (in this case, page 2), showing items from all categories
            //    localhost/Page2
            routes.MapRoute(
                name: null,
                url: "Page{page}",
                defaults: new { controller = "Book", action = "List", category = (string)null, page = @"\d+" }
            );

            //Shows the first page of items from a specific category (in this case, the Sport category)
            //   localhost/Sport

            routes.MapRoute(
                name: null,
                url: "{caty}",
                defaults: new { controller = "Book", action = "List", page = 1 }
            );

            //Shows the specified page (in this case, page 2) of items from the specified category (in this
            //case, IT)
            //    localhost/IT/Page2

            routes.MapRoute(
                name: null,
            url: "{category}/Page{page}",
            defaults: new { controller = "Book", action = "List", page = @"\d+" }
           );

            ////
            routes.MapRoute(
            name: null,
            url: "Page{page}",
            defaults: new { controller = "Book", action = "List", id = UrlParameter.Optional }
           );


            //This is the default Route map it works with all controllers and actions
            routes.MapRoute(null, "{controller}/{action}");

            //Handle Error 404 if any url error found// This route map should be at last of all maps
            routes.MapRoute(
              name: "404-PageNotFound",
              url: "{*url}",
              defaults: new { controller = "StaticContent", action = "PageNotFound" }
          );
            

            ////Default Route

            //routes.MapRoute(
            //    name: null,
            //    url: "{controller}/{action}",
            //    defaults: new { controller = "Book", action = "List" }
            //);

        }
    }
}
