using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface.DTO;
using ORM.Entities;

namespace DAL.Mappers
{
    public static class DalArticleMapper
    {
        public static DalArticle ToDalArticle(this Article ormArticle)
        {
            
            return ormArticle == null ? null : new DalArticle
            {
                Id = ormArticle.Id,
                Title = ormArticle.Title,
                Header = ormArticle.Header,
                Content = ormArticle.Content,
                PublicationDate = ormArticle.PublicationDate,
                AuthorId = ormArticle.Author?.Id,
                Author = ormArticle.Author?.ToDalUser(),
                Comments = ormArticle.Comments.Select(ormComment => ormComment.ToDalComment()),
                Tags = ormArticle.Tags.Select(ormTag => ormTag.Name)
            };
        }

        public static Article ToOrmArticle(this DalArticle dalArticle)
        {
            return new Article
            {
                Id = dalArticle.Id,
                Title = dalArticle.Title,
                Header = dalArticle.Header,
                Content = dalArticle.Content,
                PublicationDate = dalArticle.PublicationDate,
                Tags = new List<Tag>()
            };
        }
    }
}
