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
        BllArticle GetArticleEntity(int id);
        IEnumerable<BllArticle> GetAllArticleEntities();
        void CreateArticle(BllArticle article);
        void DeleteArticle(BllArticle article);
    }
}
