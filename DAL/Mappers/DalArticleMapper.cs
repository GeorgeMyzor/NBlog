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
            return new DalArticle
            {
                Id = ormArticle.Id,
                Content = ormArticle.Content,
                PublicationDate = ormArticle.PublicationDate,
                Author = ormArticle.Author.ToDalUser(),
                Comments = ormArticle.Comments.Select(ormComment => ormComment.ToDalComment()).ToList()
            };
        }

        public static Article ToOrmArticle(this DalArticle dalArticle)
        {
            return new Article
            {
                Id = dalArticle.Id,
                Content = dalArticle.Content,
                PublicationDate = dalArticle.PublicationDate,
                Author = dalArticle.Author.ToOrmUser(),
                Comments = dalArticle.Comments.Select(ormComment => ormComment.ToOrmComment()).ToList()
            };
        }
    }
}
