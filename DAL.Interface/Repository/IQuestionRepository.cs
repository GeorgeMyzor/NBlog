using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DAL.Interface.DTO;

namespace DAL.Interface.Repository
{
    public interface IQuestionRepository : IRepository<DalQuestion>
    {
        IEnumerable<DalQuestion> GetPagedArticles(int pageNum, int pageSize);
        IEnumerable<DalQuestion> GetArticlesByPredicate(Expression<Func<DalQuestion, bool>> expression);
    }
}
