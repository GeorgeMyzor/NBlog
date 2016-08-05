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
                Author = commentEntity.Author.ToMvcUser()
            };
        }

        public static BllComment ToBllComment(this CommentViewModel dalComment)
        {
            return new BllComment()
            {
                Id = dalComment.Id,
                Content = dalComment.Content,
                Author = dalComment.Author.ToBllUser()
            };
        }
    }
}