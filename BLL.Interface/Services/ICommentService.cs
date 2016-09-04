using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Entities;

namespace BLL.Interface.Services
{
    public interface ICommentService
    {
        int GetCommentsCount(string userName);
        BllComment GetCommentEntity(int id);
        IEnumerable<BllComment> GetArticleComments(int articleId);
        void CreateComment(BllComment comment);
        void DeleteComment(BllComment comment);
        void UpdateComment(BllComment comment);
    }
}
