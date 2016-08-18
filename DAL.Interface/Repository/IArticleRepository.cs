using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface.DTO;

namespace DAL.Interface.Repository
{
    public interface IArticleRepository : IRepository<DalArticle>
    {
        IEnumerable<DalArticle> GetPagedArticles(int pageNum, int pageSize);
        IEnumerable<DalArticle> GetPagedArticles(int pageNum, int pageSize, int userId);
    }
}
