using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface.DTO;
using ORM.Entities;

namespace DAL.Mappers
{
    public static class DalCommentMapper
    {
        public static DalComment ToDalComment(this Comment ormArticle)
        {
            return new DalComment
            {
                Id = ormArticle.Id,
                Content = ormArticle.Content,
                PublicationDate = ormArticle.PublicationDate,
                ArticleId = ormArticle.ArticleId,
                Author = ormArticle.Author.ToDalUser()
            };
        }

        public static Comment ToOrmComment(this DalComment dalArticle)
        {
            return new Comment
            {
                Id = dalArticle.Id,
                Content = dalArticle.Content,
                PublicationDate = dalArticle.PublicationDate,
                Author = dalArticle.Author.ToOrmUser()
            };
        }
    }
}
