using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BLL.Interface.Services;
using MVCNBlog.Infrastructure.Mappers;
using MVCNBlog.ViewModels;

namespace MVCNBlog.Controllers
{
    [Authorize]
    public class AnswerController : Controller
    {
        private readonly IAnswerService answerService;
        private readonly IUserService userService;

        public AnswerController(IAnswerService answerService, IUserService userService)
        {
            this.answerService = answerService;
            this.userService = userService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AnswerViewModel newAnswer)
        {
            int id = newAnswer.QuestionId;
            if (ModelState.IsValid)
            {
                var currentUser = userService.GetUserByEmail(User.Identity.Name);
                newAnswer.AuthorId = currentUser.Id;

                answerService.CreateAnswer(newAnswer.ToBllAnswer());

                if (Request.IsAjaxRequest())
                {
                    var answers = answerService.GetQuestionAnswers(newAnswer.QuestionId)
                        .Select(comment => comment.ToMvcAnswer());

                    return PartialView("~/Views/Question/Answers.cshtml", answers);
                }

                return RedirectToAction("Index", "Question", new { id = id });
            }

            if (Request.IsAjaxRequest())
            {
                var answers = answerService.GetQuestionAnswers(newAnswer.QuestionId)
                    .Select(comment => comment.ToMvcAnswer());

                return PartialView("~/Views/Question/Answers.cshtml", answers);
            }


            TempData["CommentError"] = string.Join(" ", ModelState.Values
                                            .SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage));

            return RedirectToAction("Index", "Question", new { id = id });
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null || id < 0)
                throw new HttpException(404, "Incorrect id.");

            var editingAnswer = answerService.GetAnswerEntity(id.Value)?.ToMvcAnswer();

            if (editingAnswer == null)
                throw new HttpException(404, $"{nameof(editingAnswer)} wasn't found.");

            if (editingAnswer.Author?.Email == User.Identity.Name || Roles.IsUserInRole("Moderator") ||
                Roles.IsUserInRole("Administrator"))
            {
                return View("Index", editingAnswer);
            }

            throw new HttpException(403, $"User with name - {User.Identity.Name} " +
                                                                    $"don't have permissions to edit or delete" +
                                                                    $" comment with id {editingAnswer.Id}.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AnswerViewModel editingAnswer)
        {
            answerService.UpdateAnswer(editingAnswer.ToBllAnswer());

            int id = editingAnswer.QuestionId;
            return RedirectToAction("Index", "Question", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IsAnswer(AnswerViewModel editingAnswer)
        {
            editingAnswer.IsAnswer = true;
            answerService.UpdateAnswer(editingAnswer.ToBllAnswer());

            int id = editingAnswer.QuestionId;
            return RedirectToAction("Index", "Question", new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(AnswerViewModel deletingAnswer)
        {
            answerService.DeleteAnswer(deletingAnswer.ToBllAnswer());

            int id = deletingAnswer.QuestionId;
            return RedirectToAction("Index", "Question", new { id });
        }
    }
}
