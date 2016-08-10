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
    public class ArticleRepository : IRepository<DalArticle>
    {
        private readonly DbContext context;

        public ArticleRepository(DbContext uow)
        {
            this.context = uow;
        }

        public IEnumerable<DalArticle> GetAll()
        {
            return context.Set<Article>().ToList().Select(ormArticle => ormArticle.ToDalArticle());
        }

        public DalArticle GetById(int id)
        {
            var ormArticle = context.Set<Article>().FirstOrDefault(article => article.Id == id);

            return ormArticle.ToDalArticle();
        }

        public DalArticle GetByPredicate(Expression<Func<DalArticle, bool>> f)
        {
            throw new NotImplementedException();
        }

        public void Create(DalArticle dalArticle)
        {
            var ormArticle = dalArticle.ToOrmArticle();
            
            CopyTags(dalArticle, ormArticle);

            ormArticle.Author = context.Set<User>().SingleOrDefault((user => user.Id == ormArticle.Author.Id));
            context.Set<Article>().Add(ormArticle);
        }

        public void Delete(DalArticle e)
        {
            throw new NotImplementedException();
        }

        public void Update(DalArticle dalArticle)
        {
            var ormArticle = context.Set<Article>().Single(u => u.Id == dalArticle.Id);

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
