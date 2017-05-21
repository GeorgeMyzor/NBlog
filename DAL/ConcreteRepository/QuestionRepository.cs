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
    public class QuestionRepository : IQuestionRepository
    {
        private readonly DbContext context;

        public QuestionRepository(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context), "Context is null.");
            this.context = context;
        }

        public int GetCount(string userName = null)
        {
            return userName == null
                  ? context.Set<Question>().Count()
                  : context.Set<Question>().Count(article => article.Author.Name == userName);

        }

        public DalQuestion GetById(int id)
        {
            ValidateParams(id: id);

            var ormArticle = context.Set<Question>().FirstOrDefault(article => article.Id == id);
            if (ormArticle != null)
            {
                ormArticle.Answers = new List<Answer>(ormArticle.Answers.OrderByDescending(comment => comment.PublicationDate));
            }

            return ormArticle.ToDalQuestion();
        }

        public IEnumerable<DalQuestion> GetAll()
        {
            return context.Set<Question>().ToList().Select(article => article.ToDalQuestion());
        }

        public DalQuestion GetByPredicate(Expression<Func<DalQuestion, bool>> expression)
        {
            var newExpr = Modifier.Convert<DalQuestion, Question>(expression);

            var article = context.Set<Question>().FirstOrDefault(newExpr);
            return article?.ToDalQuestion();
        }

        public void Create(DalQuestion dalQuestion)
        {
            ValidateArticle(dalQuestion);

            var ormArticle = dalQuestion.ToOrmQuestion();
            
            ormArticle.Author = context.Set<User>().SingleOrDefault((user => user.Id == dalQuestion.AuthorId));
            context.Set<Question>().Add(ormArticle);
        }

        public void Delete(DalQuestion dalQuestion)
        {
            ValidateArticle(dalQuestion);

            var ormArticle = context.Set<Question>().Single(u => u.Id == dalQuestion.Id);
            context.Set<Question>().Remove(ormArticle);
        }

        public void Update(DalQuestion dalQuestion)
        {
            ValidateArticle(dalQuestion);

            var ormArticle = context.Set<Question>().Single(u => u.Id == dalQuestion.Id);

            ormArticle.Title = dalQuestion.Title;
            ormArticle.Header = dalQuestion.Header;
            ormArticle.Content = dalQuestion.Content;
        }

        public IEnumerable<DalQuestion> GetPagedArticles(int pageNum, int pageSize)
        {
            ValidateParams(pageNum, pageSize);

            var ormArticles = context.Set<Question>().OrderByDescending(article => article.PublicationDate).
                Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();

            return ormArticles.Select(article => article.ToDalQuestion());
        }

        public IEnumerable<DalQuestion> GetArticlesByPredicate(Expression<Func<DalQuestion, bool>> expression)
        {
            var articles = context.Set<Question>().OrderByDescending(article => article.PublicationDate)
                .ToList().Select(ormArticle => ormArticle.ToDalQuestion());

            var dalArticles = articles as IList<DalQuestion> ?? articles.ToList();
            var findedArticles = dalArticles.Where(expression.Compile());

            return findedArticles;
        }
        private static void ValidateArticle(DalQuestion dalArticle)
        {
            if (dalArticle == null)
            {
                throw new ArgumentNullException(nameof(dalArticle), $"{nameof(dalArticle)} is null.");
            }
        }

        private static void ValidateParams(int pageNum = 1, int pageSize = 1, int id = 0)
        {
            if (pageNum < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageNum), $"{nameof(pageNum)} must be greator then 0.");
            }
            else if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), $"{nameof(pageSize)} must be greator then 0.");
            }
            else if (id < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), $"{nameof(pageSize)} must be positive.");
            }
        }

    }
}
