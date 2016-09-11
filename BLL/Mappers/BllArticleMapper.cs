using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Entities;
using DAL.Interface.DTO;

namespace BLL.Mappers
{
    public static class BllArticleMapper
    {
        public static BllArticle ToBllArticle(this DalArticle dalArticle)
        {
            return dalArticle == null ? null : new BllArticle()
            {
                Id = dalArticle.Id,
                Title = dalArticle.Title,
                Header = dalArticle.Header,
                Content = dalArticle.Content,
                PublicationDate = dalArticle.PublicationDate,
                AuthorId = dalArticle.AuthorId,
                Author = dalArticle.Author?.ToBllUser(),
                Comments = dalArticle.Comments.Select(dalComment => dalComment.ToBllComment()),
                Tags = dalArticle.Tags
            };
        }

        public static DalArticle ToDalArticle(this BllArticle articleEntity)
        {
            return new DalArticle()
            {
                Id = articleEntity.Id,
                Title = articleEntity.Title,
                Header = articleEntity.Header,
                Content = articleEntity.Content,
                PublicationDate = articleEntity.PublicationDate,
                AuthorId = articleEntity.AuthorId,
                Tags = articleEntity.Tags
            };
        }
    }
}
