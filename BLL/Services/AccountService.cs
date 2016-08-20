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
        private readonly IArticleRepository articleRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IRepository<DalComment> commentRepository;

        public AccountService(IUnitOfWork uow, IUserRepository userRepository, IArticleRepository articleRepository,
            IRoleRepository roleRepository, IRepository<DalComment> commentRepository)
        {
            this.uow = uow;
            this.userRepository = userRepository;
            this.articleRepository = articleRepository;
            this.roleRepository = roleRepository;
            this.commentRepository = commentRepository;
        }

        public BllUser GetAccountEntity(int id)
        {
            var user = userRepository.GetById(id)?.ToBllUser();

            if (user != null)
            {
                int userActivity = articleRepository.GetCount(user.Name);
                userActivity += commentRepository.GetCount(user.Name);
                user.Rank = RankDistributor.GetRank(userActivity);

                int subCost = user.Roles.Sum(role => roleRepository.GetRoleCost(role.Id));
                user.SubscriptionCost = subCost;
            }

            return user;
        }

        public IEnumerable<BllUser> GetAllUserEntities()
        {
            return userRepository.GetAll().Select(user => user.ToBllUser());
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
