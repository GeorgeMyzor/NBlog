using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Entities;
using BLL.Interface.Services;
using BLL.Mappers;
using DAL.Interface.DTO;
using DAL.Interface.Repository;

namespace BLL.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork uow;
        private readonly IQuestionRepository articleRepository;
        private const int SideBarSize = 5;

        public QuestionService(IUnitOfWork uow, IQuestionRepository repository)
        {
            this.uow = uow;
            this.articleRepository = repository;
        }

        public int GetQuestionsCount()
        {
            return articleRepository.GetCount();
        }

        public int GetQuestionsCount(string userName)
        {
            return articleRepository.GetCount(userName);
        }

        public BllQuestion GetQuestionEntity(int id)
        {
            var bllArticle = articleRepository.GetById(id).ToBllQuestion();

            return bllArticle;
        }

        public IEnumerable<BllQuestion> GetAllQuestionEntities()
        {
            return articleRepository.GetAll().Select(article => article.ToBllQuestion());
        }

        public IEnumerable<BllQuestion> FindQuestionEntities(string term)
        {
            var findedArticles = articleRepository.GetArticlesByPredicate(
                article => article.Content.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0);
            
            return findedArticles.Select(article => article.ToBllQuestion());
        }

        public IEnumerable<BllQuestion> GetPagedQuestions(int pageNum, int pageSize)
        {
            return articleRepository.GetPagedArticles(pageNum, pageSize).Select(article => article.ToBllQuestion());
        }

        public IEnumerable<BllQuestion> GetRecentQuestions()
        {
            var articles = articleRepository.GetAll();

            articles = articles.OrderByDescending(article => article.PublicationDate);
            var dalArticles = articles as IList<DalQuestion> ?? articles.ToList();
            articles = dalArticles.Skip(SideBarSize - 1).Any() ? dalArticles.Take(SideBarSize) : dalArticles;

            return articles.Select(article => article.ToBllQuestion());
        }

        public IEnumerable<BllQuestion> GetPopularQuestions()
        {
            var articles = articleRepository.GetAll();

            articles = articles.OrderByDescending(article => article.Answers.Count());
            var dalArticles = articles as IList<DalQuestion> ?? articles.ToList();
            articles = dalArticles.Skip(SideBarSize - 1).Any() ? dalArticles.Take(SideBarSize) : dalArticles;

            return articles.Select(article => article.ToBllQuestion());
        }

        public void CreateQuestion(BllQuestion article)
        {
            article.PublicationDate = DateTime.Now;
            
            articleRepository.Create(article.ToDalQuestion());
            uow.Commit();
        }

        public void DeleteQuestion(BllQuestion article)
        {
            articleRepository.Delete(article.ToDalQuestion());
            uow.Commit();
        }

        public void UpdateQuestion(BllQuestion article)
        {
            articleRepository.Update(article.ToDalQuestion());
            uow.Commit();
        }
    }
}
