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
                name: "FindArticle",
                url: "articles/find/{term}",
                defaults: new { controller = "Article", action = "Find" }
                );

            routes.MapRoute(
                name: "ArticleAction",
                url: "articles/{id}/{title}/{action}",
                defaults: new {controller = "Article", action = "Index", title = UrlParameter.Optional},
                constraints: new {action = @"Delete|Edit|Index", id = @"\d+"}
                );

            routes.MapRoute(
                name: "AllArticles",
                url: "articles/{action}",
                defaults: new { controller = "Article", action = "All" },
                constraints: new { action = @"Create|All|Find|Popular" }
                );
            
            routes.MapRoute(
                name: "PagedArticles",
                url: "articles/page{page}",
                defaults: new { Controller = "Article", action = "All" }
                );

            routes.MapRoute(
                name: "ChildActions",
                url: "article/{action}",
                defaults: new { Controller = "Article"},
                constraints: new { action = @"PopularSide|RecentSide"}
                );

            #endregion

            #region User routes

            routes.MapRoute(
               name: "EditUser",
               url: "users/{id}/UpdatePicture",
               defaults: new { controller = "User", action = "UpdatePicture" }
               );

            routes.MapRoute(
                name: "UserAction",
                url: "users/{id}/{name}/{action}",
                defaults: new { controller = "User", action = "Index", name = UrlParameter.Optional },
                constraints: new { action = @"Delete|Edit|Index", id = @"\d+" }
                );

            routes.MapRoute(
                name: "AllUsers",
                url: "users/{action}",
                defaults: new { controller = "User", action = "All" },
                constraints: new { action = @"Create|All" }
                );

            routes.MapRoute(
                name: "PagedUsers",
                url: "users/page{page}",
                defaults: new { Controller = "User", action = "All" }
                );

            routes.MapRoute(
               name: "UserValidation",
               url: "user/{action}",
               defaults: new { controller = "User" },
                constraints: new { action = @"ValidateName|ValidatePassword|ValidateEmail|ValidateLoginEmail|ValidateLoginName" }
               );

            #endregion

            #region Comment routes

            routes.MapRoute(
               name: "CommentAction",
               url: "comment/{id}/{action}",
               defaults: new { controller = "Comment", action = "Edit" },
               constraints: new { action = "Edit|Delete", id = @"\d+" }
               );

            routes.MapRoute(
                name: "CommentCreate",
                url: "comment/create",
                defaults: new { controller = "Comment", action = "Create" }
                );

            #endregion

            #region Account routes

            routes.MapRoute(
               name: "AuthAction",
               url: "{action}",
               defaults: new { controller = "Account" },
               constraints: new { action = @"Login|Register|Logoff" }
               );

            routes.MapRoute(
               name: "ViewAccount",
               url: "account",
               defaults: new { controller = "Account", action = "Index" }
               );

            routes.MapRoute(
                name: "AccountAction",
                url: "account/{action}",
                defaults: new {controller = "Account", action = "Index"},
                constraints: new {action = @"Edit|Delete|Index"}
                );

            routes.MapRoute(
               name: "EditAccount",
               url: "account/{action}",
               defaults: new { controller = "Account", action = "Edit" },
               constraints: new {action = @"UpdatePicture|Edit|EditVipStatus" }
               );

            #endregion
            
            routes.MapRoute(
                name: "Main",
                url: "",
                defaults: new {controller = "Article", action = "All"}
                );

            routes.MapRoute(
                "NotFound",
                "{*url}",
                new { controller = "Error", action = "Error" }
            );
        }
    }
    /*  URL schema:
            * 1.  NBlog.com/                                 Главная страница; содержит список статей и навигацией.
            * 
            * Article:
            * 2.  NBlog.com/articles                         Страница со списком статей.
            * 3.  NBlog.com/articles/create                  Страница с полями для создания статьи.
            * 4.  NBlog.com/articles/<article code>          Страница с информацией о статье с указанным номером,
            *                                                   там можно найти изменение удаление(для уполнамоченных), 
            *                                                   комментарии и комментирование.
            * 5.  NBlog.com/articles/<article code>/edit     Изменяет выбранную статью (отправка формы).     
            * 6.  NBlog.com/articles/<article code>/delete   Удаляет выбранную статью (отправка формы).   
            * 7.  NBlog.com/articles/page<page num>          Страница со статьями по указанному номеру. 
            * 8.  NBlog.com/articles/find/<term>             Страница со статьями найдеными по тексту или по тэгама.
            * 9.  NBlog.com/articles/popular                 Страница с наиболее популярными статьями.
            *              
            * Comment:
            * 10.  NBlog.com/comment/<comment code>/edit     Страница редактирования комментария.
            * 11.  NBlog.com/comment/<comment code>/delete   Страница подтверждения удаления комментария.
            *                                                  
            * User:
            * 12.  NBlog.com/users                            Показывает страницу со списком пользователей, там же находится
            *                                                    изменение и удаления. (Админ, модератор)
            * 13.
            * 14.  NBlog.com/users/<user code>/edit           Изменяет выбранного пользователя (отправка формы). (Админ, модератор)
            * 15.  NBlog.com/users/<user code>/delete         Удаляет выбранного пользователя (отправка формы). (Админ, модератор)
            * 16.  NBlog.com/users/page<page num>             Выводит страницу с пользователями по указанному номеру. (Админ, модератор)
            * 
            * Account:          
            * 17. NBlog.com/login                            Страница логина.
            * 18. NBlog.com/register                         Страница регистрации.
            * 19. NBlog.com/logoff                           Страница выхода из аккаунта.
            * 20. NBlog.com/account                          Страница аккаунта.
            * 21. NBlog.com/account/edit                     Изменение аккаунта.
            */
}
