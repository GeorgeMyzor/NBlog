using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL.Interface.Entities;
using MVCNBlog.ViewModels;

namespace MVCNBlog.Infrastructure.Mappers
{
    public static class MvcQuestionMapper
    {
        public static QuestionViewModel ToMvcQuestion(this BllQuestion questionEntity)
        {
            return questionEntity == null ? null : new QuestionViewModel()
            {
                Id = questionEntity.Id,
                Title = questionEntity.Title,
                Header = questionEntity.Header,
                Content = questionEntity.Content,
                PublicationDate = questionEntity.PublicationDate,
                Author = questionEntity.Author?.ToMvcUser(),
                AuthorId = questionEntity.AuthorId,
                Answers = questionEntity.Answers.Select(bllComment => bllComment.ToMvcAnswer()).ToList()
            };
        }

        public static BllQuestion ToBllQuestion(this QuestionViewModel mvcQuestion)
        {
            return new BllQuestion()
            {
                Id = mvcQuestion.Id,
                Title = mvcQuestion.Title,
                Header = mvcQuestion.Header,
                Content = mvcQuestion.Content,
                PublicationDate = mvcQuestion.PublicationDate,
                AuthorId = mvcQuestion.AuthorId
            };
        }
    }
}