using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface.DTO;

namespace DAL.Interface.Repository
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        int GetCount(string userName = null);
        TEntity GetById(int key);
        IEnumerable<TEntity> GetAll();
        TEntity GetByPredicate(Expression<Func<TEntity, bool>> f);
        void Create(TEntity e);
        void Delete(TEntity e);
        void Update(TEntity entity);
    }
}
