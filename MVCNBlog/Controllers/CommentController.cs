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
    [Authorize]
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
            int id = commentViewModel.ArticleId;
            if (ModelState.IsValid)
            {
                //TODO auth user
                commentViewModel.AuthorId = 7022;
                service.CreateComment(commentViewModel.ToBllComment());

                return RedirectToAction("Index", "Article", new { id });
            }
            TempData["CommentError"] = string.Join(" ", ModelState.Values
                                            .SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage));
            return RedirectToAction("Index", "Article", new { id });
        }
        
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return HttpNotFound("NotFound.");


            if (ModelState.IsValid)
            {
                //TODO check for rigths
                var editingArticle = service.GetCommentEntity(id.Value).ToMvcComment();
                return View("Index", editingArticle);
            }

            return (View("Index"));
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult ConfirmEdit(CommentViewModel editingComment)
        {
            editingComment.AuthorId = 7022;

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