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
    public class ArticleService : IArticleService
    {
        private readonly IUnitOfWork uow;
        private readonly IArticleRepository articleRepository;
        private const int SideBarSize = 5;

        public ArticleService(IUnitOfWork uow, IArticleRepository repository)
        {
            this.uow = uow;
            this.articleRepository = repository;
        }

        public int GetArticlesCount()
        {
            return articleRepository.GetCount();
        }

        public int GetArticlesCount(string userName)
        {
            return articleRepository.GetCount(userName);
        }

        #region Read

        public BllArticle GetArticleEntity(int id)
        {
            var bllArticle = articleRepository.GetById(id).ToBllArticle();

            return bllArticle;
        }

        public IEnumerable<BllArticle> GetAllArticleEntities()
        {
            return articleRepository.GetAll().Select(article => article.ToBllArticle());
        }

        public IEnumerable<BllArticle> FindArticleEntities(string findString)
        {
            IEnumerable<DalArticle> allArticles;
            if (findString.StartsWith("-hashtag-"))
            {
                findString = findString.Replace("-hashtag-", "#");
                allArticles =
                    articleRepository.GetArticlesByPredicate(
                        article =>
                            article.Tags.Any(tag => tag.StartsWith(findString, true, CultureInfo.InvariantCulture)));
            }
            else
            {
                allArticles =
                    articleRepository.GetArticlesByPredicate(
                        article => article.Content.IndexOf(findString, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            return allArticles.Select(article => article.ToBllArticle());
        }

        public IEnumerable<BllArticle> GetPagedArticles(int pageNum, int pageSize)
        {
            return articleRepository.GetPagedArticles(pageNum, pageSize).Select(article => article.ToBllArticle());
        }

        public IEnumerable<BllArticle> GetRecentArticles()
        {
            var articles = articleRepository.GetAll();

            articles = articles.OrderByDescending(article => article.PublicationDate);
            var dalArticles = articles as IList<DalArticle> ?? articles.ToList();
            articles = dalArticles.Skip(SideBarSize - 1).Any() ? dalArticles.Take(SideBarSize) : dalArticles;

            return articles.Select(article => article.ToBllArticle());
        }

        public IEnumerable<BllArticle> GetPopularArticles()
        {
            var articles = articleRepository.GetAll();

            articles = articles.OrderByDescending(article => article.Comments.Count());
            var dalArticles = articles as IList<DalArticle> ?? articles.ToList();
            articles = dalArticles.Skip(SideBarSize - 1).Any() ? dalArticles.Take(SideBarSize) : dalArticles;

            return articles.Select(article => article.ToBllArticle());
        }

        #endregion

        public void CreateArticle(BllArticle article)
        {
            article.PublicationDate = DateTime.Now;

            SetTags(article);

            articleRepository.Create(article.ToDalArticle());
            uow.Commit();
        }

        public void DeleteArticle(BllArticle article)
        {
            articleRepository.Delete(article.ToDalArticle());
            uow.Commit();
        }

        public void UpdateArticle(BllArticle article)
        {
            SetTags(article);

            articleRepository.Update(article.ToDalArticle());
            uow.Commit();
        }

        #region Private methods

        private static void SetTags(BllArticle article)
        {
            var tags = TagParser.GetTags(article.Title).ToList();
            tags.AddRange(TagParser.GetTags(article.Header));
            tags.AddRange(TagParser.GetTags(article.Content));
            article.Tags = tags;
        }

        #endregion
    }
}
