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
        private readonly IRepository<DalArticle> articleRepository;

        public ArticleService(IUnitOfWork uow, IRepository<DalArticle> repository)
        {
            this.uow = uow;
            this.articleRepository = repository;
        }

        public BllArticle GetArticleEntity(int id)
        {
            return articleRepository.GetById(id).ToBllArticle();
        }

        public IEnumerable<BllArticle> GetAllArticleEntities()
        {
            return articleRepository.GetAll().Select(article => article.ToBllArticle());
        }

        public void CreateArticle(BllArticle article)
        {
            article.PublicationDate = DateTime.Today;
            article.Tags = TagParser.GetTags(article.Content);

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
            article.Tags = TagParser.GetTags(article.Content);
            articleRepository.Update(article.ToDalArticle());
            uow.Commit();
        }
    }
}
