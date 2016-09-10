using System;
using System.Collections.Generic;
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

        public BllArticle GetArticleEntity(int id)
        {
            var bllArticle = articleRepository.GetById(id).ToBllArticle();

            if (bllArticle != null)
            {
                bllArticle.Comments = new List<BllComment>(bllArticle.Comments.OrderByDescending(comment => comment.PublicationDate));
            }

            return bllArticle;
        }

        public IEnumerable<BllArticle> GetAllArticleEntities()
        {
            return articleRepository.GetAll().Select(article => article.ToBllArticle());
        }

        public IEnumerable<BllArticle> FindArticleEntities(string findString)
        {
            IEnumerable<DalArticle> allArticles;
            if (findString.StartsWith("#"))
            {
                allArticles = articleRepository.GetArticlesByPredicate(article => article.Tags.Any(tag => tag.StartsWith(findString)));
            }
            else
            {
                allArticles = articleRepository.GetArticlesByPredicate(article => article.Content.Contains(findString));
            }

            return allArticles.Select(article => article.ToBllArticle());
        }

        public IEnumerable<BllArticle> GetPagedArticles(int pageNum, int pageSize)
        {
            return articleRepository.GetPagedArticles(pageNum, pageSize).Select(dalArticle => dalArticle.ToBllArticle());
        }

        public IEnumerable<BllArticle> GetPagedArticles(int pageNum, int pageSize, int userId)
        {
            return articleRepository.GetPagedArticles(pageNum, pageSize, userId).Select(dalArticle => dalArticle.ToBllArticle());
        }

        public IEnumerable<BllArticle> GetRecentArticles()
        {
            var articles = articleRepository.GetAll();

            articles = articles.OrderByDescending(article => article.PublicationDate);
            articles = articles.Skip(4).Any() ? articles.Take(5) : articles;

            return articles.Select(article => article.ToBllArticle());
        }

        public IEnumerable<BllArticle> GetPopularArticles()
        {
            var articles = articleRepository.GetAll();

            articles = articles.OrderByDescending(article => article.Comments.Count());
            articles = articles.Skip(4).Any() ? articles.Take(5) : articles;

            return articles.Select(article => article.ToBllArticle());
        }

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

        private static void SetTags(BllArticle article)
        {
            var tags = TagParser.GetTags(article.Title).ToList();
            tags.AddRange(TagParser.GetTags(article.Content));
            article.Tags = tags;
        }

    }
}
