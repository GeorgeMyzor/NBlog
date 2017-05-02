using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Entities;

namespace BLL.Interface.Services
{
    public interface IQuestionService
    {
        int GetQuestionsCount();
        int GetQuestionsCount(string userName);
        BllQuestion GetQuestionEntity(int id);
        IEnumerable<BllQuestion> GetAllQuestionEntities();
        IEnumerable<BllQuestion> FindQuestionEntities(string term);
        IEnumerable<BllQuestion> GetPagedQuestions(int pageNum, int pageSize);
        IEnumerable<BllQuestion> GetRecentQuestions();
        IEnumerable<BllQuestion> GetPopularQuestions();
        void CreateQuestion(BllQuestion article);
        void DeleteQuestion(BllQuestion article);
        void UpdateQuestion(BllQuestion article);
    }
}
