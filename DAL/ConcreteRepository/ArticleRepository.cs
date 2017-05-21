using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface.DTO;
using DAL.Interface.Repository;
using DAL.Mappers;
using ORM.Entities;

namespace DAL.ConcreteRepository
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly DbContext context;

        public ArticleRepository(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context), "Context is null.");
            this.context = context;
        }
        
        /// <summary>
        /// Returns articles count, if <param name="userName"></param> not null, returns user's articles count.
        /// </summary>
        /// <param name="userName">Name of user by which articles count will be searched.</param>
        /// <returns>Articles count.</returns>
        public int GetCount(string userName = null)
        {
            return userName == null
                ? context.Set<Article>().Count()
                : context.Set<Article>().Count(article => article.Author.Name == userName);
        }

        #region Read

        public DalArticle GetById(int id)
        {
            ValidateParams(id: id);

            var ormArticle = context.Set<Article>().FirstOrDefault(article => article.Id == id);
            if (ormArticle != null)
            {
                ormArticle.Comments = new List<Comment>(ormArticle.Comments.OrderByDescending(comment => comment.PublicationDate));
            }

            return ormArticle.ToDalArticle();
        }
        
        public IEnumerable<DalArticle> GetAll()
        {
            return context.Set<Article>().ToList().Select(article => article.ToDalArticle());
        }

        /// <summary>
        /// Returns number of articles which is specified in <param name="pageSize"></param>, 
        /// starting from <param name="pageNum"></param>. Articles are sorted by publication date.
        /// </summary>
        /// <param name="pageNum">Offset in list of articles.</param>
        /// <param name="pageSize">Amount of articles to be returned.</param>
        /// <returns>Articles.</returns>
        public IEnumerable<DalArticle> GetPagedArticles(int pageNum, int pageSize)
        {
            ValidateParams(pageNum, pageSize);

            var ormArticles = context.Set<Article>().OrderByDescending(article => article.PublicationDate).
                Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();

            return ormArticles.Select(article => article.ToDalArticle());
        }
        
        /// <summary>
        /// Return article by predicate <param name="expression"></param>
        /// </summary>
        /// <param name="expression">Rule according to which will find out a article.</param>
        /// <returns>Article.</returns>
        public DalArticle GetByPredicate(Expression<Func<DalArticle, bool>> expression)
        {
            var newExpr = Modifier.Convert<DalArticle, Article>(expression);

            var article = context.Set<Article>().FirstOrDefault(newExpr);
            return article?.ToDalArticle();
        }

        /// <summary>
        /// Return articles by predicate <param name="expression"></param>.
        /// </summary>
        /// <param name="expression">Rule according to which will find out a articles.</param>
        /// <returns>Articles.</returns>
        public IEnumerable<DalArticle> GetArticlesByPredicate(Expression<Func<DalArticle, bool>> expression)
        {
            var articles = context.Set<Article>().OrderByDescending(article => article.PublicationDate)
                .ToList().Select(ormArticle => ormArticle.ToDalArticle());

            var dalArticles = articles as IList<DalArticle> ?? articles.ToList();
            var findedArticles = dalArticles.Where(expression.Compile());

            return findedArticles;
        }

        #endregion

        public void Create(DalArticle dalArticle)
        {
            ValidateArticle(dalArticle);

            var ormArticle = dalArticle.ToOrmArticle();
            
            CopyTags(dalArticle, ormArticle);

            ormArticle.Author = context.Set<User>().SingleOrDefault((user => user.Id == dalArticle.AuthorId));
            context.Set<Article>().Add(ormArticle);
        }

        public void Delete(DalArticle dalArticle)
        {
            ValidateArticle(dalArticle);

            var ormArticle = context.Set<Article>().Single(u => u.Id == dalArticle.Id);
            context.Set<Article>().Remove(ormArticle);
        }

        /// <summary>
        /// Updating article by the following properties: Title, Header, Content, HeaderPicture, Tags.
        /// </summary>
        /// <param name="dalArticle">Article to be updated.</param>
        public void Update(DalArticle dalArticle)
        {
            ValidateArticle(dalArticle);

            var ormArticle = context.Set<Article>().Single(u => u.Id == dalArticle.Id);

            ormArticle.Title = dalArticle.Title;
            ormArticle.Header = dalArticle.Header;
            ormArticle.Content = dalArticle.Content;
            ormArticle.HeaderPicture = dalArticle.HeaderPicture ?? ormArticle.HeaderPicture;
            ormArticle.Tags.Clear();

            CopyTags(dalArticle, ormArticle);
        }

        #region Private methods

        private void CopyTags(DalArticle dalArticle, Article ormArticle)
        {
            foreach (var tag in dalArticle.Tags)
            {
                var existingTag = context.Set<Tag>().FirstOrDefault((ormTag => ormTag.Name == tag));
                if (existingTag != null)
                    ormArticle.Tags.Add(existingTag);
                else
                    ormArticle.Tags.Add(new Tag()
                    {
                        Name = tag,
                    });
            }
        }

        private static void ValidateArticle(DalArticle dalArticle)
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

        #endregion
    }
}
