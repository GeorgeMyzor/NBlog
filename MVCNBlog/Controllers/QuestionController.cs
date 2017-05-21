using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using BLL.Interface.Services;
using MVCNBlog.Infrastructure;
using MVCNBlog.Infrastructure.Mappers;
using MVCNBlog.ViewModels;

namespace MVCNBlog.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
        private readonly IQuestionService questionService;
        private readonly IUserService userService;
        private readonly int pageSize;

        public QuestionController(IQuestionService questionService, IUserService userService)
        {
            this.questionService = questionService;
            this.userService = userService;
            pageSize = int.Parse(WebConfigurationManager.AppSettings["PageSize"]);
        }


        #region View

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index(int? id, string title)
        {
            if (id == null || id < 0)
                throw new HttpException(404, "Incorrect id.");

            var question = questionService.GetQuestionEntity(id.Value).ToMvcQuestion();

            if (question == null)
                throw new HttpException(404, $"{nameof(question)} with id - {id} wasnt found.");

            string urlWithTitle = question.Title.RemoveSpecialCharacters();
            urlWithTitle = Url.Encode(urlWithTitle);

            if (!urlWithTitle.Equals(title))
            {
                return RedirectToAction("Index", new { id, title = urlWithTitle });
            }

            return View(question);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult All(int page = 1)
        {
            var totalItems = questionService.GetQuestionsCount();
            if (page > (totalItems + pageSize - 1) / pageSize)
                throw new HttpException(404, "");

            var questions = new ListViewModel<QuestionViewModel>()
            {
                ViewModels =
                    questionService.GetPagedQuestions(page, pageSize).Select(bllQuestion => bllQuestion.ToMvcQuestion()),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = totalItems
                }
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("All", questions);
            }

            return View(questions);
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
        public ActionResult Create(QuestionViewModel newArticle)
        {
            var currentUser = userService.GetUserByEmail(User.Identity.Name)?.ToMvcUser();
            if (currentUser == null)
                throw new HttpException(404, $"{nameof(currentUser)} wasnt found. When trying to add new atricle.");

            newArticle.AuthorId = currentUser.Id;

            if (ModelState.IsValid)
            {
                questionService.CreateQuestion(newArticle.ToBllQuestion());
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

            var editingQuestion = questionService.GetQuestionEntity(id.Value).ToMvcQuestion();

            if (editingQuestion == null)
                throw new HttpException(404, $"{nameof(editingQuestion)} wasnt found. When trying to edit atricle.");

            if (editingQuestion.Author?.Email == User.Identity.Name || Roles.IsUserInRole("Moderator") ||
                Roles.IsUserInRole("Administrator"))
            {
                return View(editingQuestion);
            }

            throw new HttpException(403, "No permissions");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuestionViewModel editingQuestion)
        {
            if (ModelState.IsValid)
            {
                questionService.UpdateQuestion(editingQuestion.ToBllQuestion());

                return RedirectToAction("Index", "Question", new { id = editingQuestion.Id });
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

            var deletingQuestion = questionService.GetQuestionEntity(id.Value).ToMvcQuestion();

            if (deletingQuestion == null)
                throw new HttpException(404, $"{nameof(deletingQuestion)} wasnt found. When trying to delete question.");

            if (deletingQuestion.Author?.Email == User.Identity.Name || Roles.IsUserInRole("Moderator") ||
                Roles.IsUserInRole("Administrator"))
            {
                return View(deletingQuestion);
            }

            throw new HttpException(403, $"User with email - {User.Identity.Name} " +
                                                    $"don't have permissions to delete question with id {deletingQuestion.Id}.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(QuestionViewModel editingQuestion)
        {
            questionService.DeleteQuestion(editingQuestion.ToBllQuestion());

            return RedirectToAction("All");
        }

        #endregion
    }
}