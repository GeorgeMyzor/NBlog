using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly int pageSize;

        public ArticleController(IArticleService articleService, IUserService userService)
        {
            this.articleService = articleService;
            this.userService = userService;
            pageSize = int.Parse(WebConfigurationManager.AppSettings["PageSize"]);
        }
        
        
        #region View

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index(int? id, string title)
        {
            if(id == null || id < 0)
                throw new HttpException(404, "Incorrect id.");

            var article = articleService.GetArticleEntity(id.Value).ToMvcArticle();

            if (article == null)
                throw new HttpException(404, $"{nameof(article)} with id - {id} wasnt found.");

            string urlWithTitle = article.Title.RemoveSpecialCharacters();
            urlWithTitle = Url.Encode(urlWithTitle);

            if (!urlWithTitle.Equals(title))
            {
                return RedirectToAction("Index", new {id, title = urlWithTitle});
            }

            return View(article);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult All(int page = 1)
        {
            var totalItems = articleService.GetArticlesCount();
            if (page > (totalItems + pageSize - 1)/pageSize)
                throw new HttpException(404, "");

            var articles = new ListViewModel<ArticleViewModel>()
            {
                ViewModels =
                    articleService.GetPagedArticles(page, pageSize).Select(bllArticle => bllArticle.ToMvcArticle()),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = totalItems
                }
            };
            
            if (Request.IsAjaxRequest())
            {
                return PartialView("All", articles);
            }

            return View(articles);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Popular()
        {
            var popularArticles = articleService.GetPopularArticles().Select(bllArticle => bllArticle.ToMvcArticle());

            return View("PopularArticles", popularArticles);
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult PopularSide()
        {
            var popularArticles = articleService.GetPopularArticles().Select(bllArticle => bllArticle.ToMvcArticle());

            return PartialView("PopularArticles", popularArticles);
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult RecentSide()
        {
            var recentArticles = articleService.GetRecentArticles().Select(bllArticle => bllArticle.ToMvcArticle());

            return PartialView("RecentArticles", recentArticles);
        }
        
        [AllowAnonymous]
        public ActionResult Find(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                var articles = articleService.GetPagedArticles(1, pageSize).Select(bllArticle => bllArticle.ToMvcArticle());

                return PartialView("AllArticles", articles);
            }

            var findedArticles =
                articleService.FindArticleEntities(term).Select(bllArticle => bllArticle.ToMvcArticle()).ToList();

            var findedPagedArticles = new ListViewModel<ArticleViewModel>()
            {
                ViewModels = findedArticles,
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = 1,
                    ItemsPerPage = pageSize,
                    TotalItems = articleService.GetArticlesCount()
                }
            };

            ViewBag.GroupName = !findedArticles.Any() ? "Nothing was found." : "";

            if (Request.IsAjaxRequest())
            {
                return PartialView(findedArticles);
            }

            TempData["isFind"] = true;
            return View("All", findedPagedArticles);
        }

        #endregion

        #region Create

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ArticleViewModel newArticle)
        {
            var currentUser = userService.GetUserByEmail(User.Identity.Name)?.ToMvcUser();
            if(currentUser == null)
                throw new HttpException(404, $"{nameof(currentUser)} wasnt found. When trying to add new atricle.");

            newArticle.AuthorId = currentUser.Id;
            
            if (ModelState.IsValid)
            {
                newArticle.HeaderPicture = GetHeaderPicture();
                if (newArticle.HeaderPicture == null)
                    return RedirectToAction("Create");

                articleService.CreateArticle(newArticle.ToBllArticle());
                return RedirectToAction("All");
            }

            return View();
        }

        #endregion

        #region Edit

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null || id < 0)
                throw new HttpException(404, "Incorrect id.");

            var editingArticle = articleService.GetArticleEntity(id.Value).ToMvcArticle();

            if (editingArticle == null)
                throw new HttpException(404, $"{nameof(editingArticle)} wasnt found. When trying to edit atricle.");

            if (editingArticle.Author?.Email == User.Identity.Name || Roles.IsUserInRole("Moderator") ||
                Roles.IsUserInRole("Administrator"))
            {
                return View(editingArticle);
            }
            
            throw new HttpException(403, "No permissions");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ArticleViewModel editingArticle)
        {
            if (ModelState.IsValid)
            {
                editingArticle.HeaderPicture = GetHeaderPicture();
                if (editingArticle.HeaderPicture == null)
                    return View();

                articleService.UpdateArticle(editingArticle.ToBllArticle());
                
                return RedirectToAction("Index", "Article", new { id = editingArticle.Id });
            }

            return View();
        }

        #endregion

        #region Delete

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null || id < 0)
                throw new HttpException(404, "Incorrect id.");

            var deletingArticle = articleService.GetArticleEntity(id.Value).ToMvcArticle();

            if (deletingArticle == null)
                throw new HttpException(404, $"{nameof(deletingArticle)} wasnt found. When trying to delete atricle.");

            if (deletingArticle.Author?.Email == User.Identity.Name || Roles.IsUserInRole("Moderator") ||
                Roles.IsUserInRole("Administrator"))
            {
                return View(deletingArticle);
            }

            throw new HttpException(403, $"User with email - {User.Identity.Name} " +
                                                    $"don't have permissions to delete article with id {deletingArticle.Id}.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ArticleViewModel editingArticle)
        {
            articleService.DeleteArticle(editingArticle.ToBllArticle());

            return RedirectToAction("All");
        }

        #endregion
        
        [NonAction]
        private byte[] GetHeaderPicture()
        {
            HttpPostedFileBase uploadImage = Request.Files["uploadImage"];
            if (uploadImage != null && uploadImage.ContentLength / 1024 < 1500 && uploadImage.ContentLength > 0)
            {
                byte[] imageData = null;

                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                return imageData;
            }

            TempData["PicError"] = "Either image not found or image size too big.";
            return null;
        }
    }
}