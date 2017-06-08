using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface.DTO;
using DAL.Interface.Repository;
using DAL.Mappers;
using ORM.Entities;

namespace DAL.ConcreteRepository
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly DbContext context;

        public AnswerRepository(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context), "Context is null.");
            this.context = context;
        }

        public int GetCount(string userName = null)
        {
            return userName == null
                ? context.Set<Answer>().Count()
                : context.Set<Answer>().Count(comment => comment.Author.Name == userName);
        }

        public DalAnswer GetById(int id)
        {
            ValidateId(id);

            var ormComment = context.Set<Answer>().FirstOrDefault(answer => answer.Id == id);

            return ormComment.ToDalAnswer();
        }

        public IEnumerable<DalAnswer> GetAll()
        {
            return
                context.Set<Answer>()
                    .OrderByDescending(answer => answer.PublicationDate)
                    .Select(answer => answer.ToDalAnswer());
        }

        public DalAnswer GetByPredicate(Expression<Func<DalAnswer, bool>> expression)
        {
            var newExpr = Modifier.Convert<DalAnswer, Answer>(expression);

            var comment = context.Set<Answer>().FirstOrDefault(newExpr);
            return comment?.ToDalAnswer();
        }

        public void Create(DalAnswer dalAnswer)
        {
            ValidateComment(dalAnswer);

            var ormAnswer = dalAnswer.ToOrmAnswer();

            ormAnswer.Author = context.Set<User>().SingleOrDefault((user => user.Id == dalAnswer.AuthorId));
            ormAnswer.Question = context.Set<Question>().Single((article => article.Id == dalAnswer.QuestionId));

            context.Set<Answer>().Add(ormAnswer);
            context.SaveChanges();
        }

        public void Delete(DalAnswer dalAnswer)
        {
            ValidateComment(dalAnswer);

            var ormAnswer = context.Set<Answer>().Single(u => u.Id == dalAnswer.Id);
            context.Set<Answer>().Remove(ormAnswer);
        }

        public void Update(DalAnswer dalAnswer)
        {
            ValidateComment(dalAnswer);

            var ormComment = context.Set<Answer>().Single(u => u.Id == dalAnswer.Id);

            ormComment.Content = dalAnswer.Content;
            ormComment.IsAnswer = dalAnswer.IsAnswer;
        }

        public IEnumerable<DalAnswer> GetQuestionAnswers(int articleId)
        {
            var comments = context.Set<Answer>().Where(answer => answer.QuestionId == articleId).
                OrderByDescending(answer => answer.PublicationDate).ToList();

            return comments.Select(ormAnswer => ormAnswer.ToDalAnswer());
        }

        #region Private methods

        private static void ValidateComment(DalAnswer dalAnswer)
        {
            if (dalAnswer == null)
            {
                throw new ArgumentNullException(nameof(dalAnswer), $"{nameof(dalAnswer)} is null.");
            }
        }

        private static void ValidateId(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id), $"{nameof(id)} must be positive.");
            }
        }
        #endregion
    }
}
