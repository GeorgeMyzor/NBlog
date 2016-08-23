using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Entities;
using DAL.Interface.DTO;

namespace BLL.Mappers
{
    public static class BllCommentMapper
    {
        public static BllComment ToBllComment(this DalComment dalComment)
        {
            return dalComment == null ? null : new BllComment()
            {
                Id = dalComment.Id,
                Content = dalComment.Content,
                ArticleId = dalComment.ArticleId,
                PublicationDate = dalComment.PublicationDate,
                AuthorId = dalComment.AuthorId,
                Author = dalComment.Author?.ToBllUser()
            };
        }

        public static DalComment ToDalComment(this BllComment commentEntity)
        {
            return new DalComment()
            {
                Id = commentEntity.Id,
                Content = commentEntity.Content,
                ArticleId = commentEntity.ArticleId,
                PublicationDate = commentEntity.PublicationDate,
                AuthorId = commentEntity.AuthorId
            };
        }
    }
}
