using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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
                var currentUser = userService.GetUserEntity(User.Identity.Name).ToMvcUser();
                commentViewModel.AuthorId = currentUser.Id;

                commentService.CreateComment(commentViewModel.ToBllComment());

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
                var editingComment= commentService.GetCommentEntity(id.Value).ToMvcComment();

                if (editingComment.Author.Name == User.Identity.Name || Roles.IsUserInRole("Moderator") || Roles.IsUserInRole("Administrator"))
                {
                    return View("Index", editingComment);
                }
                throw new HttpException(403, "No permission ");
            }

            return (View("Index"));
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