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
    public class AccountService : IAccountService
    {
        //TODO payment etc..

        private readonly IUnitOfWork uow;
        private readonly IUserRepository userRepository;

        public AccountService(IUnitOfWork uow, IUserRepository repository)
        {
            this.uow = uow;
            this.userRepository = repository;
        }

        public BllUser GetAccountEntity(int id)
        {
            return userRepository.GetById(id).ToBllUser();
        }
        public IEnumerable<BllUser> GetAllUserEntities()
        {
            return userRepository.GetAll().Select(user => user.ToBllUser());
        }

        public void CreateAccount(BllUser account)
        {
            account.CreationDate = DateTime.Today;
            userRepository.Create(account.ToDalUser());
            uow.Commit();
        }

        public void DeleteAccount(BllUser account)
        {
            userRepository.Delete(account.ToDalUser());
            uow.Commit();
        }

        public void UpdateAccount(BllUser account)
        {
            userRepository.Update(account.ToDalUser());
            uow.Commit();
        }
        
        public void UpdateAccountPaidRole(BllUser user)
        {
            userRepository.UpdatePaidRole(user.ToDalUser());
            uow.Commit();
        }
    }
}
