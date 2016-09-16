using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BLL.Interface.Entities;
using BLL.Interface.Services;
using LoggingModule;
using MVCNBlog.Infrastructure.Mappers;
using MVCNBlog.ViewModels;
using MVCNBlog.ViewModels.Roles;

namespace MVCNBlog.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService commentService;
        private readonly IUserService userService;

        public CommentController(ICommentService commentService, IUserService userService)
        {
            this.commentService = commentService;
            this.userService = userService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CommentViewModel newComment)
        {
            int id = newComment.ArticleId;
            if (ModelState.IsValid)
            {
                var currentUser = userService.GetUserEntityByEmail(User.Identity.Name);
                newComment.AuthorId = currentUser.Id;

                commentService.CreateComment(newComment.ToBllComment());

                if (Request.IsAjaxRequest())
                {
                    var comments = commentService.GetArticleComments(newComment.ArticleId)
                        .Select(comment => comment.ToMvcComment());
                    
                    return PartialView("~/Views/Article/Comments.cshtml", comments);
                }

                return RedirectToAction("Index", "Article", new { id });
            }

            if (Request.IsAjaxRequest())
            {
                return Json("ModelError", JsonRequestBehavior.AllowGet);
            }

            TempData["CommentError"] = string.Join(" ", ModelState.Values
                                            .SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage));

            return RedirectToAction("Index", "Article", new { id });
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null || id < 0)
                throw new HttpException(404, "Incorrect id.");

            var editingComment = commentService.GetCommentEntity(id.Value)?.ToMvcComment();

            if (editingComment == null)
                throw new HttpException(404, $"{nameof(editingComment)} wasn't found.");

            if (editingComment.Author?.Email == User.Identity.Name || Roles.IsUserInRole("Moderator") ||
                Roles.IsUserInRole("Administrator"))
            {
                return View("Index", editingComment);
            }

            throw new HttpException(403, $"User with name - {User.Identity.Name} " +
                                                                    $"don't have permissions to edit or delete" +
                                                                    $" comment with id {editingComment.Id}.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CommentViewModel editingComment)
        {
            commentService.UpdateComment(editingComment.ToBllComment());

            int id = editingComment.ArticleId;
            return RedirectToAction("Index","Article", new {id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(CommentViewModel deletingComment)
        {
            commentService.DeleteComment(deletingComment.ToBllComment());

            int id = deletingComment.ArticleId;
            return RedirectToAction("Index", "Article", new { id });
        }
    }
}