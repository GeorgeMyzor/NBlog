using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Entities;
using DAL.Interface.DTO;

namespace BLL.Mappers
{
    public static class BllAnswerMapper
    {
        public static BllAnswer ToBllAnswer(this DalAnswer dalAnswer)
        {
            return dalAnswer == null ? null : new BllAnswer()
            {
                Id = dalAnswer.Id,
                Content = dalAnswer.Content,
                QuestionId = dalAnswer.QuestionId,
                IsAnswer = dalAnswer.IsAnswer,
                PublicationDate = dalAnswer.PublicationDate,
                AuthorId = dalAnswer.AuthorId,
                Author = dalAnswer.Author?.ToBllUser()
            };
        }

        public static DalAnswer ToDalAnswer(this BllAnswer answerEntity)
        {
            return new DalAnswer()
            {
                Id = answerEntity.Id,
                Content = answerEntity.Content,
                QuestionId = answerEntity.QuestionId,
                IsAnswer = answerEntity.IsAnswer,
                PublicationDate = answerEntity.PublicationDate,
                AuthorId = answerEntity.AuthorId
            };
        }
    }
}
