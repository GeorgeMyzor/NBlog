using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Entities;

namespace BLL.Interface.Services
{
    public interface IAnswerService
    {
        int GetAnswersCount(string userName);
        BllAnswer GetAnswerEntity(int id);
        IEnumerable<BllAnswer> GetQuestionAnswers(int questionId);
        void CreateAnswer(BllAnswer comment);
        void DeleteAnswer(BllAnswer comment);
        void UpdateAnswer(BllAnswer comment);
    }
}
