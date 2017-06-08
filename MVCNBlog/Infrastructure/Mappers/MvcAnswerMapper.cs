using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL.Interface.Entities;
using MVCNBlog.ViewModels;

namespace MVCNBlog.Infrastructure.Mappers
{
    public static class MvcAnswerMapper
    {
        public static AnswerViewModel ToMvcAnswer(this BllAnswer answerEntity)
        {
            return answerEntity == null ? null : new AnswerViewModel()
            {
                Id = answerEntity.Id,
                Content = answerEntity.Content,
                QuestionId = answerEntity.QuestionId,
                IsAnswer = answerEntity.IsAnswer,
                PublicationDate = answerEntity.PublicationDate,
                Author = answerEntity.Author?.ToMvcUser()
            };
        }

        public static BllAnswer ToBllAnswer(this AnswerViewModel mvcAnswer)
        {
            return new BllAnswer()
            {
                Id = mvcAnswer.Id,
                Content = mvcAnswer.Content,
                AuthorId = mvcAnswer.AuthorId,
                IsAnswer = mvcAnswer.IsAnswer ?? false,
                QuestionId = mvcAnswer.QuestionId
            };
        }
    }
}