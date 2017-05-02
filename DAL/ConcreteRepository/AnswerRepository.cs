using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface.DTO;
using DAL.Interface.Repository;

namespace DAL.ConcreteRepository
{
    public class AnswerRepository : IAnswerRepository
    {
        public int GetCount(string userName = null)
        {
            throw new NotImplementedException();
        }

        public DalAnswer GetById(int key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalAnswer> GetAll()
        {
            throw new NotImplementedException();
        }

        public DalAnswer GetByPredicate(Expression<Func<DalAnswer, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public void Create(DalAnswer e)
        {
            throw new NotImplementedException();
        }

        public void Delete(DalAnswer e)
        {
            throw new NotImplementedException();
        }

        public void Update(DalAnswer entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DalAnswer> GetQuestionAnswers(int articleId)
        {
            throw new NotImplementedException();
        }
    }
}
