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

        public DalArticle GetById(int key)
        {
            throw new NotImplementedException();
        }

        public DalArticle GetByPredicate(Expression<Func<DalArticle, bool>> f)
        {
            throw new NotImplementedException();
        }

        public void Create(DalArticle e)
        {
            throw new NotImplementedException();
        }

        public void Delete(DalArticle e)
        {
            throw new NotImplementedException();
        }

        public void Update(DalArticle entity)
        {
            throw new NotImplementedException();
        }
    }
}
