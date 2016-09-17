using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Entities;
using BLL.Interface.Services;
using BLL.Mappers;
using DAL.Interface.DTO;
using DAL.Interface.Repository;

namespace BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork uow;
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;

        public AccountService(IUnitOfWork uow, IUserRepository userRepository, IRoleRepository roleRepository)
        {
            this.uow = uow;
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
        }

        #region Read

        public BllUser GetAccountEntity(string email)
        {
            var user = userRepository.GetByPredicate(dalUser => dalUser.Email == email)?.ToBllUser();

            if (user != null)
            {
                int userActivity = userRepository.GetUserActivity(user.Id);
                user.Rank = RankDistributor.GetRank(userActivity);

                int subCost = user.Roles.Sum(role => roleRepository.GetRoleCost(role.Id));
                user.SubscriptionCost = subCost;
            }

            return user;
        }

        public BllUser GetAccountEntityByName(string name)
        {
            return userRepository.GetByPredicate(dalUser => dalUser.Name == name)?.ToBllUser();
        }

        public IEnumerable<BllUser> GetAllUserEntities()
        {
            return userRepository.GetAll().Select(user => user.ToBllUser());
        }

        #endregion

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

        public void UpdateAccountPicture(BllUser user)
        {
            userRepository.UpdateUserPicture(user.ToDalUser());
            uow.Commit();
        }
    }
}
