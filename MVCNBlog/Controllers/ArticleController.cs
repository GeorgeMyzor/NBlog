using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Interface.Entities;
using BLL.Interface.Services;
using MVCNBlog.Infrastructure.Mappers;
using MVCNBlog.ViewModels;
using MVCNBlog.ViewModels.Roles;

namespace MVCNBlog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService service;

        public ArticleController(IArticleService service)
        {
            this.service = service;
        }
        
        public ActionResult Index(int? id)
        {
            var article = service.GetArticleEntity(id.Value).ToMvcArticle();

            return View(article);
        }

        public ActionResult All()
        {
            return View(service.GetAllArticleEntities().Select(article => article.ToMvcArticle()));
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
            articleViewModel.Author = new UserViewModel()
            {
                Id = 7022,
                Role = new AdministratorRole()
                {
                    RoleId = 1
                }

            };
            service.CreateArticle(articleViewModel.ToBllArticle());

            return RedirectToAction("Index");
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
            editingArticle.Author = new UserViewModel()
            {
                Id = 7022,
                Role = new AdministratorRole()
                {
                    RoleId = 1
                }

            };

            service.UpdateArticle(editingArticle.ToBllArticle());

            return RedirectToAction("Index");
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
            service.DeleteArticle(new BllArticle()
            {
                Id = editingArticle.Id
            });

            return RedirectToAction("Index");
        }
    }
}