using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
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
        private readonly IArticleService service;
        private readonly int pageSize;

        public ArticleController(IArticleService service)
        {
            this.service = service;
            pageSize = int.Parse(WebConfigurationManager.AppSettings["PageSize"]);
        }
        
        [AllowAnonymous]
        public ActionResult Index(int? id, string title)
        {
            var article = service.GetArticleEntity(id.Value).ToMvcArticle();

            string urlWithTitle = article.Title.RemoveSpecialCharacters();
            urlWithTitle = Url.Encode(urlWithTitle);
            
            if (!urlWithTitle.Equals(title))
            {
                return RedirectToAction("Index", new { id, title = urlWithTitle });
            }
            
            return View(article);
        }

        [AllowAnonymous]
        public ActionResult All(int page = 1)
        {
            var articles = new ListViewModel<ArticleViewModel>()
            {
                ViewModels = service.GetPagedArticles(page, pageSize).Select(bllArticle => bllArticle.ToMvcArticle()),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = service.GetAllArticleEntities().Count()
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
            articleViewModel.AuthorId = 7022;
            service.CreateArticle(articleViewModel.ToBllArticle());

            if (ModelState.IsValid)
            {
                service.CreateArticle(articleViewModel.ToBllArticle());
                return RedirectToAction("All");
            }

            return View();
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound("NotFound.");

            //TODO check for rigths
            var editingArticle = service.GetArticleEntity(id.Value).ToMvcArticle();

            return View(editingArticle);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult ConfirmEdit(ArticleViewModel editingArticle)
        {
            if (ModelState.IsValid)
            {
                service.UpdateArticle(editingArticle.ToBllArticle());

                int id = editingArticle.Id;
                return RedirectToAction("Index", "Article", new { id });
            }

            return View();
        }
        
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return HttpNotFound("NotFound.");

            //TODO check for rigths
            var deletingArticle = service.GetArticleEntity(id.Value).ToMvcArticle();

            return View(deletingArticle);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(ArticleViewModel editingArticle)
        {
            service.DeleteArticle(editingArticle.ToBllArticle());

            return RedirectToAction("All");
        }
    }
}