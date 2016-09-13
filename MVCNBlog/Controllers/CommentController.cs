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
        public ActionResult Create(CommentViewModel commentViewModel)
        {
            int id = commentViewModel.ArticleId;
            if (ModelState.IsValid)
            {
                var currentUser = userService.GetUserEntityByEmail(User.Identity.Name).ToMvcUser();
                commentViewModel.AuthorId = currentUser.Id;

                commentService.CreateComment(commentViewModel.ToBllComment());

                if (Request.IsAjaxRequest())
                {
                    var comments = commentService.GetArticleComments(commentViewModel.ArticleId)
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
            var editingComment = commentService.GetCommentEntity(id ?? 0)?.ToMvcComment();

            if (editingComment == null)
            {
                var outputString = $"{nameof(editingComment)} wasn't found.";
                var httpException = new HttpException(404, outputString);
                throw httpException;
            }

            if (editingComment.Author?.Email == User.Identity.Name || Roles.IsUserInRole("Moderator") ||
                Roles.IsUserInRole("Administrator"))
            {
                return View("Index", editingComment);
            }

            var httpNoPermissionsException = new HttpException(403, $"User with name - {User.Identity.Name} " +
                                                                    $"don't have permissions to edit or delete" +
                                                                    $" comment with id {editingComment.Id}.");
            throw httpNoPermissionsException;
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult ConfirmEdit(CommentViewModel editingComment)
        {
            commentService.UpdateComment(editingComment.ToBllComment());

            int id = editingComment.ArticleId;
            return RedirectToAction("Index","Article", new {id});
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(CommentViewModel deletingComment)
        {
            commentService.DeleteComment(new BllComment()
            {
                Id = deletingComment.Id
            });

            int id = deletingComment.ArticleId;
            return RedirectToAction("Index", "Article", new { id });
        }
    }
}