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

            var article = questionService.GetQuestionEntity(id.Value).ToMvcQuestion();

            if (article == null)
                throw new HttpException(404, $"{nameof(article)} with id - {id} wasnt found.");

            string urlWithTitle = article.Title.RemoveSpecialCharacters();
            urlWithTitle = Url.Encode(urlWithTitle);

            if (!urlWithTitle.Equals(title))
            {
                return RedirectToAction("Index", new { id, title = urlWithTitle });
            }

            return View(article);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult All(int page = 1)
        {
            var totalItems = questionService.GetQuestionsCount();
            if (page > (totalItems + pageSize - 1) / pageSize)
                throw new HttpException(404, "");

            var articles = new ListViewModel<QuestionViewModel>()
            {
                ViewModels =
                    questionService.GetPagedQuestions(page, pageSize).Select(bllArticle => bllArticle.ToMvcQuestion()),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = totalItems
                }
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("All", articles);
            }

            return View(articles);
        }
        
        [AllowAnonymous]
        public ActionResult Find(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                var articles = questionService.GetPagedQuestions(1, pageSize).Select(bllArticle => bllArticle.ToMvcQuestion());

                if (Request.IsAjaxRequest())
                {
                    return PartialView("AllArticles", articles);
                }

                return RedirectToAction("All");
            }

            var findedArticles =
                questionService.FindQuestionEntities(term).Select(bllArticle => bllArticle.ToMvcQuestion()).ToList();

            var findedPagedArticles = new ListViewModel<QuestionViewModel>()
            {
                ViewModels = findedArticles,
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = 1,
                    ItemsPerPage = pageSize,
                    TotalItems = questionService.GetQuestionsCount()
                }
            };

            ViewBag.GroupName = !findedArticles.Any() ? "Nothing was found." : "";

            if (Request.IsAjaxRequest())
            {
                return PartialView(findedArticles);
            }

            TempData["isFind"] = true;
            return View("All", findedPagedArticles);
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

            var editingArticle = questionService.GetQuestionEntity(id.Value).ToMvcQuestion();

            if (editingArticle == null)
                throw new HttpException(404, $"{nameof(editingArticle)} wasnt found. When trying to edit atricle.");

            if (editingArticle.Author?.Email == User.Identity.Name || Roles.IsUserInRole("Moderator") ||
                Roles.IsUserInRole("Administrator"))
            {
                return View(editingArticle);
            }

            throw new HttpException(403, "No permissions");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(QuestionViewModel editingArticle)
        {
            if (ModelState.IsValid)
            {
                questionService.UpdateQuestion(editingArticle.ToBllQuestion());

                return RedirectToAction("Index", "Article", new { id = editingArticle.Id });
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

            var deletingArticle = questionService.GetQuestionEntity(id.Value).ToMvcQuestion();

            if (deletingArticle == null)
                throw new HttpException(404, $"{nameof(deletingArticle)} wasnt found. When trying to delete atricle.");

            if (deletingArticle.Author?.Email == User.Identity.Name || Roles.IsUserInRole("Moderator") ||
                Roles.IsUserInRole("Administrator"))
            {
                return View(deletingArticle);
            }

            throw new HttpException(403, $"User with email - {User.Identity.Name} " +
                                                    $"don't have permissions to delete article with id {deletingArticle.Id}.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(QuestionViewModel editingArticle)
        {
            questionService.DeleteQuestion(editingArticle.ToBllQuestion());

            return RedirectToAction("All");
        }

        #endregion
    }
}