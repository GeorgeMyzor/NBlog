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
        public static DalComment ToDalComment(this Comment ormComment)
        {
            return ormComment == null ? null : new DalComment
            {
                Id = ormComment.Id,
                Content = ormComment.Content,
                PublicationDate = ormComment.PublicationDate,
                ArticleId = ormComment.ArticleId,
                AuthorId = ormComment.Author?.Id,
                Author = ormComment.Author.ToDalUser()
            };
        }

        public static Comment ToOrmComment(this DalComment dalComment)
        {
            return new Comment
            {
                Id = dalComment.Id,
                Content = dalComment.Content,
                PublicationDate = dalComment.PublicationDate
            };
        }
    }
}
