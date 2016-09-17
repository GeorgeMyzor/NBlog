using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        #region Read

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
            var user = userRepository.GetByPredicate(dalUser => dalUser.Name == name)?.ToBllUser();
            if (user != null)
            {
                int userActivity = userRepository.GetUserActivity(user.Id);
                user.Rank = RankDistributor.GetRank(userActivity);
                user.Articles =
                    articleRepository.GetArticlesByPredicate(article => article.AuthorId == user.Id)
                        .Select(dalArticle => dalArticle.ToBllArticle());
            }

            return user;
        }

        public BllUser GetUserEntityByEmail(string email)
        {
            return userRepository.GetByPredicate(dalUser => dalUser.Email == email)?.ToBllUser();
        }

        public IEnumerable<BllUser> GetAllUserEntities()
        {
            return userRepository.GetAll().Select(dalUser => dalUser.ToBllUser());
        }

        public IEnumerable<BllUser> GetPagedUsers(int pageNum, int pageSize)
        {
            return userRepository.GetPagedUsers(pageNum, pageSize).Select(dalUser =>
            {
                var bllUser = dalUser.ToBllUser();
                int userActivity = userRepository.GetUserActivity(dalUser.Id);
                bllUser.Rank = RankDistributor.GetRank(userActivity);
                return bllUser;
            });
        }

        #endregion

        public void CreateUser(BllUser user)
        {
            user.CreationDate = DateTime.Now;
            user.Password = Sha256Hash(user.Password);
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

        public static string Sha256Hash(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}
