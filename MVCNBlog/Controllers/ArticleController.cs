using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Interface.Services;
using MVCNBlog.Infrastructure.Mappers;

namespace MVCNBlog.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService service;

        public ArticleController(IArticleService service)
        {
            this.service = service;
        }
        
        public ActionResult Index()
        {
            return View(service.GetAllArticleEntities().Select(article => article.ToMvcArticle()));
        }
    }
}