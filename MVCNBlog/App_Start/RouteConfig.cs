using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVCNBlog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: null,
              url: "articles/Page{page}",
              defaults: new { Controller = "Article", action = "All" }
            );

            routes.MapRoute(
             name: null,
             url: "users/Page{page}",
             defaults: new { Controller = "User", action = "Index" }
           );

            routes.MapRoute(
               name: "AllArticle",
               url: "articles",
               defaults: new { controller = "Article", action = "All" }
           );
            
            routes.MapRoute(
                name: "Edit",
                url: "articles/{id}",
                defaults: new { controller = "Article", action = "Index", id = UrlParameter.Optional }
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
