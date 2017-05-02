using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Entities;
using BLL.Interface.Services;

namespace BLL.Services
{
    public class AnswerService : IAnswerService
    {
        public int GetAnswersCount(string userName)
        {
            throw new NotImplementedException();
        }

        public BllAnswer GetAnswerEntity(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BllAnswer> GetQuestionAnswers(int questionId)
        {
            throw new NotImplementedException();
        }

        public void CreateAnswer(BllAnswer comment)
        {
            throw new NotImplementedException();
        }

        public void DeleteAnswer(BllAnswer comment)
        {
            throw new NotImplementedException();
        }

        public void UpdateAnswer(BllAnswer comment)
        {
            throw new NotImplementedException();
        }
    }
}
