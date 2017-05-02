using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface.DTO;
using ORM.Entities;

namespace DAL.Mappers
{
    public static class DalAnswerMapper
    {
        public static DalAnswer ToDalAnswer(this Answer ormAnswer)
        {
            return ormAnswer == null ? null : new DalAnswer
            {
                Id = ormAnswer.Id,
                Content = ormAnswer.Content,
                PublicationDate = ormAnswer.PublicationDate,
                QuestionId = ormAnswer.QuestionId,
                AuthorId = ormAnswer.Author?.Id,
                IsAnswer = ormAnswer.IsAnswer,
                Author = ormAnswer.Author.ToDalUser()
            };
        }

        public static Answer ToOrmAnswer(this DalAnswer dalAnswer)
        {
            return new Answer
            {
                Id = dalAnswer.Id,
                Content = dalAnswer.Content,
                IsAnswer = dalAnswer.IsAnswer ?? false,
                PublicationDate = dalAnswer.PublicationDate
            };
        }
    }
}
