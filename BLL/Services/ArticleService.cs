﻿using System;
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

        public int GetArticleCount()
        {
            return articleRepository.GetCount();
        }

        public BllArticle GetArticleEntity(int id)
        {
            return articleRepository.GetById(id).ToBllArticle();
        }

        public IEnumerable<BllArticle> GetAllArticleEntities()
        {
            return articleRepository.GetAll().Select(article => article.ToBllArticle());
        }

        public IEnumerable<BllArticle> GetPagedArticles(int pageNum, int pageSize)
        {
            return articleRepository.GetPagedArticles(pageNum, pageSize).Select(dalArticle => dalArticle.ToBllArticle());
        }

        public IEnumerable<BllArticle> GetPagedArticles(int pageNum, int pageSize, int userId)
        {
            return articleRepository.GetPagedArticles(pageNum, pageSize, userId).Select(dalArticle => dalArticle.ToBllArticle());
        }

        public void CreateArticle(BllArticle article)
        {
            article.PublicationDate = DateTime.Today;
            
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
