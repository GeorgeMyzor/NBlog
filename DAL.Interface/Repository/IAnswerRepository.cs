using System.Collections.Generic;
using DAL.Interface.DTO;

namespace DAL.Interface.Repository
{
    public interface IAnswerRepository : IRepository<DalAnswer>
    {
        IEnumerable<DalAnswer> GetQuestionAnswers(int articleId);
    }
}
