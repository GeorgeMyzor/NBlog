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

        public ArticleRepository(DbContext uow)
        {
            this.context = uow;
        }
        
        public int GetCount(string userName = null)
        {
            return userName == null
                ? context.Set<Article>().Count()
                : context.Set<Article>().Count(article => article.Author.Name == userName);
        }

        public DalArticle GetById(int id)
        {
            var ormArticle = context.Set<Article>().FirstOrDefault(article => article.Id == id);

            return ormArticle.ToDalArticle();
        }
        
        public IEnumerable<DalArticle> GetAll()
        {
            return context.Set<Article>().ToList().Select(ormArticle => ormArticle.ToDalArticle());
        }

        public IEnumerable<DalArticle> GetPagedArticles(int pageNum, int pageSize)
        {
            var ormArticles = context.Set<Article>().OrderByDescending(article => article.PublicationDate).
                Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();

            return ormArticles.Select(article => article.ToDalArticle());
        }

        public IEnumerable<DalArticle> GetPagedArticles(int pageNum, int pageSize, int userId)
        {
            var ormArticles = context.Set<Article>().OrderByDescending(article => article.PublicationDate).
                Where(x => x.Author.Id == userId).Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();

            return ormArticles.Select(article => article.ToDalArticle());
        }

        public DalArticle GetByPredicate(Expression<Func<DalArticle, bool>> f)
        {
            var newExpr = Modifier.Convert<DalArticle, Article>(f);

            var article = context.Set<Article>().FirstOrDefault(newExpr);
            return article?.ToDalArticle();
        }

        public void Create(DalArticle dalArticle)
        {
            var ormArticle = dalArticle.ToOrmArticle();
            
            CopyTags(dalArticle, ormArticle);

            ormArticle.Author = context.Set<User>().SingleOrDefault((user => user.Id == dalArticle.AuthorId));
            context.Set<Article>().Add(ormArticle);
        }

        public void Delete(DalArticle dalArticle)
        {
            var ormArticle = context.Set<Article>().Single(u => u.Id == dalArticle.Id);
            context.Set<Article>().Remove(ormArticle);
        }

        public void Update(DalArticle dalArticle)
        {
            var ormArticle = context.Set<Article>().Single(u => u.Id == dalArticle.Id);

            ormArticle.Title = dalArticle.Title;
            ormArticle.Content = dalArticle.Content;
            ormArticle.Tags.Clear();

            CopyTags(dalArticle, ormArticle);
        }

        #region Private methods

        private void CopyTags(DalArticle dalArticle, Article ormArticle)
        {
            foreach (var tag in dalArticle.Tags)
            {
                var existingTag = context.Set<Tag>().SingleOrDefault((ormTag => ormTag.Name == tag));
                if (existingTag != null)
                    ormArticle.Tags.Add(existingTag);
                else
                    ormArticle.Tags.Add(new Tag()
                    {
                        Name = tag,
                    });
            }
        }

        #endregion
    }
}
