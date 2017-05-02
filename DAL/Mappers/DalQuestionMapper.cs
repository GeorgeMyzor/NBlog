using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface.DTO;
using ORM.Entities;

namespace DAL.Mappers
{
    public static class DalQuestionMapper
    {
        public static DalQuestion ToDalQuestion(this Question ormQuestion)
        {

            return ormQuestion == null ? null : new DalQuestion
            {
                Id = ormQuestion.Id,
                Title = ormQuestion.Title,
                Header = ormQuestion.Header,
                Content = ormQuestion.Content,
                PublicationDate = ormQuestion.PublicationDate,
                AuthorId = ormQuestion.Author?.Id,
                Author = ormQuestion.Author?.ToDalUser(),
                Answers = ormQuestion.Answers.Select(ormAnswer => ormAnswer.ToDalAnswer()),
            };
        }

        public static Question ToOrmQuestion(this DalQuestion dalQuestion)
        {
            return new Question
            {
                Id = dalQuestion.Id,
                Title = dalQuestion.Title,
                Header = dalQuestion.Header,
                Content = dalQuestion.Content,
                PublicationDate = dalQuestion.PublicationDate
            };
        }
    }
}
