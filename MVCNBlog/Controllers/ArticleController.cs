using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using BLL.Interface.Entities;
using BLL.Interface.Services;
using MVCNBlog.Infrastructure;
using MVCNBlog.Infrastructure.Mappers;
using MVCNBlog.ViewModels;
using MVCNBlog.ViewModels.Roles;

namespace MVCNBlog.Controllers
{
    [Authorize]
    public class ArticleController : Controller
    {
        private readonly IArticleService articleService;
        private readonly IUserService userService;
        private readonly int pageSize;

        public ArticleController(IArticleService articleService, IUserService userService)
        {
            this.articleService = articleService;
            this.userService = userService;
            pageSize = int.Parse(WebConfigurationManager.AppSettings["PageSize"]);
        }
        
        [AllowAnonymous]
        public ActionResult Index(int? id, string title)
        {
            if (id != null)
            {
                var article = articleService.GetArticleEntity(id.Value).ToMvcArticle();

                string urlWithTitle = article.Title.RemoveSpecialCharacters();
                urlWithTitle = Url.Encode(urlWithTitle);
            
                if (!urlWithTitle.Equals(title))
                {
                    return RedirectToAction("Index", new { id, title = urlWithTitle });
                }
            
                return View(article);
            }
            return HttpNotFound("Not found.");
        }

        [AllowAnonymous]
        public ActionResult All(int page = 1)
        {
            var articles = new ListViewModel<ArticleViewModel>()
            {
                ViewModels = articleService.GetPagedArticles(page, pageSize).Select(bllArticle => bllArticle.ToMvcArticle()),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = articleService.GetArticlesCount()
                }
            };

            return View(articles);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ArticleViewModel articleViewModel)
        {
            var currentUser = userService.GetUserEntity(User.Identity.Name).ToMvcUser();
            articleViewModel.AuthorId = currentUser.Id;
            
            if (ModelState.IsValid)
            {
                articleService.CreateArticle(articleViewModel.ToBllArticle());
                return RedirectToAction("All");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            var editingArticle = articleService.GetArticleEntity(id ?? 0).ToMvcArticle();
            
            if (editingArticle == null)
                throw new HttpException(404, "Not found");

            if (editingArticle.Author?.Name == User.Identity.Name || Roles.IsUserInRole("Moderator") ||
                Roles.IsUserInRole("Administrator"))
            {
                return View(editingArticle);
            }

            throw new HttpException(403, "No permissions");
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult ConfirmEdit(ArticleViewModel editingArticle)
        {
            if (ModelState.IsValid)
            {
                articleService.UpdateArticle(editingArticle.ToBllArticle());

                int id = editingArticle.Id;
                return RedirectToAction("Index", "Article", new { id });
            }

            return View();
        }
        
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var deletingArticle = articleService.GetArticleEntity(id ?? 0).ToMvcArticle();

            if (deletingArticle == null)
                throw new HttpException(404, "Not found");

            if (deletingArticle.Author?.Name == User.Identity.Name || Roles.IsUserInRole("Moderator") ||
                Roles.IsUserInRole("Administrator"))
            {
                return View(deletingArticle);
            }

            throw new HttpException(403, "No permissions");
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(ArticleViewModel editingArticle)
        {
            articleService.DeleteArticle(editingArticle.ToBllArticle());

            return RedirectToAction("All");
        }
    }
}