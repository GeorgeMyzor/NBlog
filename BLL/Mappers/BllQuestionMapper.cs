using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Entities;
using DAL.Interface.DTO;

namespace BLL.Mappers
{
    public static class BllQuestionMapper
    {
        public static BllQuestion ToBllQuestion(this DalQuestion dalArticle)
        {
            return dalArticle == null ? null : new BllQuestion()
            {
                Id = dalArticle.Id,
                Title = dalArticle.Title,
                Header = dalArticle.Header,
                Content = dalArticle.Content,
                PublicationDate = dalArticle.PublicationDate,
                AuthorId = dalArticle.AuthorId,
                Author = dalArticle.Author?.ToBllUser(),
                Answers = dalArticle.Answers.Select(dalComment => dalComment.ToBllAnswer())
            };
        }

        public static DalQuestion ToDalQuestion(this BllQuestion articleEntity)
        {
            return new DalQuestion()
            {
                Id = articleEntity.Id,
                Title = articleEntity.Title,
                Header = articleEntity.Header,
                Content = articleEntity.Content,
                PublicationDate = articleEntity.PublicationDate,
                AuthorId = articleEntity.AuthorId
            };
        }
    }
}
