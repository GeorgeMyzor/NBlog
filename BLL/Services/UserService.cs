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
    public class UserService : IUserService
    {
        private readonly IUnitOfWork uow;
        private readonly IUserRepository userRepository;
        private readonly IArticleRepository articleRepository;

        public UserService(IUnitOfWork uow, IUserRepository userRepository, IArticleRepository articleRepository)
        {
            this.uow = uow;
            this.articleRepository = articleRepository;
            this.userRepository = userRepository;
        }

        public int GetUsersCount()
        {
            return userRepository.GetCount();
        }

        public BllUser GetUserEntity(int id)
        {
            var user = userRepository.GetById(id)?.ToBllUser();

            if (user != null)
            {
                int userActivity = userRepository.GetUserActivity(id);
                user.Rank = RankDistributor.GetRank(userActivity);
                user.Articles =
                    articleRepository.GetArticlesByPredicate(article => article.AuthorId == id)
                        .Select(dalArticle => dalArticle.ToBllArticle());
            }

            return user;
        }

        public BllUser GetUserEntity(string name)
        {
            return userRepository.GetByPredicate(user => user.Name == name)?.ToBllUser();
        }

        public BllUser GetUserEntityByEmail(string email)
        {
            return userRepository.GetByPredicate(user => user.Email == email)?.ToBllUser();
        }

        public IEnumerable<BllUser> GetAllUserEntities()
        {
            return userRepository.GetAll().Select(user => user.ToBllUser());
        }

        public IEnumerable<BllUser> GetPagedUsers(int pageNum, int pageSize)
        {
            return userRepository.GetPagedUsers(pageNum, pageSize).Select(dalUser => dalUser.ToBllUser());
        }

        public void CreateUser(BllUser user)
        {
            userRepository.Create(user.ToDalUser());
            uow.Commit();
        }

        public void DeleteUser(BllUser user)
        {
            userRepository.Delete(user.ToDalUser());
            uow.Commit();
        }

        public void UpdateUser(BllUser user)
        {
            userRepository.Update(user.ToDalUser());
            uow.Commit();
        }

        public void UpdateUserPicture(BllUser user)
        {
            userRepository.UpdateUserPicture(user.ToDalUser());
            uow.Commit();
        }
    }
}
