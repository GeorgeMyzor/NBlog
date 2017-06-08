using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Entities;
using BLL.Interface.Services;
using BLL.Mappers;
using DAL.Interface.Repository;

namespace BLL.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IUnitOfWork uow;
        private readonly IAnswerRepository answerRepository;

        public AnswerService(IUnitOfWork uow, IAnswerRepository repository)
        {
            this.uow = uow;
            this.answerRepository = repository;
        }

        public int GetAnswersCount(string userName)
        {
            return answerRepository.GetCount(userName);
        }

        public BllAnswer GetAnswerEntity(int id)
        {
            return answerRepository.GetById(id).ToBllAnswer();
        }

        public IEnumerable<BllAnswer> GetQuestionAnswers(int questionId)
        {
            return answerRepository.GetQuestionAnswers(questionId).Select(answer => answer.ToBllAnswer());
        }

        public void CreateAnswer(BllAnswer answer)
        {
            answer.PublicationDate = DateTime.Now;

            answerRepository.Create(answer.ToDalAnswer());
            uow.Commit();
        }

        public void DeleteAnswer(BllAnswer answer)
        {
            answerRepository.Delete(answer.ToDalAnswer());
            uow.Commit();
        }

        public void UpdateAnswer(BllAnswer answer)
        {
            answerRepository.Update(answer.ToDalAnswer());
            uow.Commit();
        }
    }
}
