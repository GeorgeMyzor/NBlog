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
    public class CommentController : Controller
    {
        private readonly ICommentService service;

        public CommentController(ICommentService service)
        {
            this.service = service;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(CommentViewModel commentViewModel)
        {
            //TODO auth user
            commentViewModel.Author = new UserViewModel()
            {
                Id = 7022,
                Role = new AdministratorRole()
                {
                    RoleId = 1
                }

            };
            service.CreateComment(commentViewModel.ToBllComment());

            int id = commentViewModel.ArticleId;
            return RedirectToAction("Index", "Article", new { id });
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound("NotFound.");

            //TODO check for rigths
            var editingArticle = service.GetCommentEntity(id.Value).ToMvcComment();

            return View("Index", editingArticle);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult ConfirmEdit(CommentViewModel editingComment)
        {
            editingComment.Author = new UserViewModel()
            {
                Id = 7022,
                Role = new AdministratorRole()
                {
                    RoleId = 1
                }

            };

            service.UpdateComment(editingComment.ToBllComment());

            int id = editingComment.ArticleId;
            return RedirectToAction("Index","Article", new {id});
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(CommentViewModel deletingComment)
        {
            service.DeleteComment(new BllComment()
            {
                Id = deletingComment.Id
            });

            int id = deletingComment.ArticleId;
            return RedirectToAction("Index", "Article", new { id });
        }
    }
}