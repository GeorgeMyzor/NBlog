using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL.Interface.Entities;
using MVCNBlog.ViewModels;

namespace MVCNBlog.Infrastructure.Mappers
{
    public static class MvcArticleMapper
    {
        public static ArticleViewModel ToMvcArticle(this BllArticle articleEntity)
        {
            return new ArticleViewModel()
            {
                Id = articleEntity.Id,
                Content = articleEntity.Content,
                PublicationDate = articleEntity.PublicationDate,
                Author = articleEntity.Author?.ToMvcUser(),
                AuthorId = articleEntity.AuthorId,
                Comments = articleEntity.Comments.Select(bllComment => bllComment.ToMvcComment()).ToList(),
                Tags = articleEntity.Tags
            };
        }

        public static BllArticle ToBllArticle(this ArticleViewModel mvcArticle)
        {
            return new BllArticle()
            {
                Id = mvcArticle.Id,
                Content = mvcArticle.Content,
                PublicationDate = mvcArticle.PublicationDate,
                AuthorId = mvcArticle.AuthorId
            };
        }
    }
}