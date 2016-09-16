using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Entities;

namespace BLL.Interface.Services
{
    public interface IArticleService
    {
        int GetArticlesCount();
        int GetArticlesCount(string userName);
        BllArticle GetArticleEntity(int id);
        IEnumerable<BllArticle> GetAllArticleEntities();
        IEnumerable<BllArticle> FindArticleEntities(string findString);
        IEnumerable<BllArticle> GetPagedArticles(int pageNum, int pageSize);
        IEnumerable<BllArticle> GetRecentArticles();
        IEnumerable<BllArticle> GetPopularArticles();
        void CreateArticle(BllArticle article);
        void DeleteArticle(BllArticle article);
        void UpdateArticle(BllArticle article);
    }
}
