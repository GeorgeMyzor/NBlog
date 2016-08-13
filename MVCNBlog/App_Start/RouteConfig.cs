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

            #region Article routes

            routes.MapRoute(
                name: null,
                url: "articles/Page{page}",
                defaults: new {Controller = "Article", action = "All"}
                );

            routes.MapRoute(
                name: "ArticleAction",
                url: "articles/{id}/{action}",
                defaults: new {controller = "Article", action = "Index"},
                constraints: new {action = @"Index|Delete|Edit"}
                );

            routes.MapRoute(
                name: "AllArticles",
                url: "articles/{action}",
                defaults: new {controller = "Article", action = "All"}
                );

            #endregion

            routes.MapRoute(
                name: null,
                url: "users/Page{page}",
                defaults: new {Controller = "User", action = "Index"}
                );

            routes.MapRoute(
                name: "AllUsers",
                url: "users/{action}",
                defaults: new {controller = "User", action = "Index"}
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );

        }
    }
}
