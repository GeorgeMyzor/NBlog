using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using BLL.Interface.Entities;
using BLL.Interface.Services;
using LoggingModule;
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
        private readonly ILogger logger;
        private readonly int pageSize;

        public ArticleController(IArticleService articleService, IUserService userService, ILogger logger)
        {
            this.logger = logger;
            this.articleService = articleService;
            this.userService = userService;
            pageSize = int.Parse(WebConfigurationManager.AppSettings["PageSize"]);
        }

        [AllowAnonymous]
        public ActionResult Index(int? id, string title)
        {
            var article = articleService.GetArticleEntity(id ?? 0).ToMvcArticle();

            if (article == null)
            {
                var httpException = new HttpException(404, "Not found");
                logger.Warn(httpException, $"Article with id {nameof(article)} wasnt found.");
                throw httpException;
            }

            string urlWithTitle = article.Title.RemoveSpecialCharacters();
            urlWithTitle = Url.Encode(urlWithTitle);

            if (!urlWithTitle.Equals(title))
            {
                return RedirectToAction("Index", new {id, title = urlWithTitle});
            }

            return View(article);
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
            var currentUser = userService.GetUserEntity(User.Identity.Name)?.ToMvcUser();
            if(currentUser == null)
            {
                var httpException = new HttpException(404, "Not found");
                logger.Warn(httpException, $"{nameof(currentUser)} wasnt found. When trying to add new atricle.");
                throw httpException;
            }

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
            {
                var httpException = new HttpException(404, "Not found");
                logger.Warn(httpException, $"{nameof(editingArticle)} wasnt found. When trying to edit atricle.");
                throw httpException;
            }

            if (editingArticle.Author?.Name == User.Identity.Name || Roles.IsUserInRole("Moderator") ||
                Roles.IsUserInRole("Administrator"))
            {
                return View(editingArticle);
            }

            var httpNoPermissionsException = new HttpException(403, "No permissions");
            logger.Warn(httpNoPermissionsException, $"User with name - {User.Identity.Name} " +
                                                    $"don't have permissions to edit article with id {editingArticle.Id}.");
            throw httpNoPermissionsException;
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
            {
                var httpException = new HttpException(404, "Not found");
                logger.Warn(httpException, $"{nameof(deletingArticle)} wasnt found. When trying to delte atricle.");
                throw httpException;
            }

            if (deletingArticle.Author?.Name == User.Identity.Name || Roles.IsUserInRole("Moderator") ||
                Roles.IsUserInRole("Administrator"))
            {
                return View(deletingArticle);
            }

            var httpNoPermissionsException = new HttpException(403, "No permissions");
            logger.Warn(httpNoPermissionsException, $"User with name - {User.Identity.Name} " +
                                                    $"don't have permissions to delete article with id {deletingArticle.Id}.");
            throw httpNoPermissionsException;
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