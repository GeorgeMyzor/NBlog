using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL.Interface.Entities;
using MVCNBlog.ViewModels;

namespace MVCNBlog.Infrastructure.Mappers
{
    public static class MvcCommentMapper
    {
        public static CommentViewModel ToMvcComment(this BllComment commentEntity)
        {
            return new CommentViewModel()
            {
                Id = commentEntity.Id,
                Content = commentEntity.Content,
                ArticleId = commentEntity.ArticleId,
                PublicationDate = commentEntity.PublicationDate,
                Author = commentEntity.Author?.ToMvcUser()
            };
        }

        public static BllComment ToBllComment(this CommentViewModel mvcComment)
        {
            return new BllComment()
            {
                Id = mvcComment.Id,
                Content = mvcComment.Content,
                AuthorId = mvcComment.AuthorId,
                ArticleId = mvcComment.ArticleId
            };
        }
    }
}