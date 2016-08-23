using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Interface.DTO;
using DAL.Interface.Repository;
using DAL.Mappers;
using ORM.Entities;

namespace DAL.ConcreteRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext context;

        public UserRepository(DbContext context)
        {
            this.context = context;
            if (context == null)
                throw new ArgumentNullException(nameof(context),"Context is null.");
        }

        public IEnumerable<DalUser> GetAll()
        {
            return context.Set<User>().ToList().Select(ormUser => ormUser.ToDalUser());
        }
        
        public int GetCount(string userName = null)
        {
            return userName == null
                ? context.Set<User>().Count()
                : context.Set<User>().Count(user => user.Name == userName);
        }

        public DalUser GetById(int id)
        {
            ValidateId(id);

            var ormUser = context.Set<User>().FirstOrDefault(user => user.Id == id);

            return ormUser?.ToDalUser();
        }
        
        public DalUser GetByPredicate(Expression<Func<DalUser, bool>> expression)
        {
            var newExpr = Modifier.Convert<DalUser,User>(expression);

            var user = context.Set<User>().FirstOrDefault(newExpr);
            return user?.ToDalUser();
        }

        public int GetUserActivity(int id)
        {
            ValidateId(id);

            int activity = context.Set<Article>().Count(article => article.Author.Id == id);
            activity += context.Set<Comment>().Count(article => article.Author.Id == id);

            return activity;
        }

        public IEnumerable<DalUser> GetPagedUsers(int pageNum, int pageSize)
        {
            ValidatePageParams(pageNum, pageSize);

            var ormUser = context.Set<User>().OrderByDescending(user => user.Name).
                Skip((pageNum - 1) * pageSize).Take(pageSize).ToList();

            return ormUser.Select(user => user.ToDalUser());
        }
        
        public void Create(DalUser dalUser)
        {
            ValidateUser(dalUser);

            var ormUser = dalUser.ToOrmUser();
            var newRoles = new List<Role>();

            foreach (var role in ormUser.Roles)
            {
                var dbRole = context.Set<Role>().Find(role.Id);
                newRoles.Add(dbRole);
            }
            ormUser.Roles = newRoles;
            context.Set<User>().Add(ormUser);
        }

        public void Delete(DalUser dalUser)
        {
            ValidateUser(dalUser);

            var ormUser = dalUser.ToOrmUser();

            ormUser = context.Set<User>().Single(u => u.Id == ormUser.Id);
            context.Set<User>().Remove(ormUser);
        }
        
        public void Update(DalUser dalUser)
        {
            ValidateUser(dalUser);

            var editingUser = dalUser.ToOrmUser();
            var ormUser = context.Set<User>().Single(u => u.Id == dalUser.Id);

            ormUser.Name = editingUser.Name;

            CopyOrmUserPaidRoles(ormUser, editingUser);

            UpdateOrmUserRoles(ormUser, editingUser);
        }
        
        public void UpdatePaidRole(DalUser dalUser)
        {
            ValidateUser(dalUser);

            var editingUser = dalUser.ToOrmUser();
            var ormUser = context.Set<User>().Single(u => u.Id == dalUser.Id);

            CopyOrmUserNotPaidRoles(ormUser, editingUser);

            UpdateOrmUserRoles(ormUser,editingUser);
        }

        #region Private methods

        private void CopyOrmUserPaidRoles(User ormUser, User editingUser)
        {
            var paidRolesId = context.Set<RoleCosts>().Select((costs => costs.RoleId));
            foreach (var paidRoleId in paidRolesId)
            {
                var paidRole = ormUser.Roles.Find((role => role.Id == paidRoleId));

                if (paidRole != null)
                    editingUser.Roles.Add(paidRole);
            }
        }

        private void CopyOrmUserNotPaidRoles(User ormUser, User editingUser)
        {
            var paidRolesId = context.Set<RoleCosts>().Select((costs => costs.RoleId));
            foreach (var paidRoleId in paidRolesId)
            {
                var notPaidRole = ormUser.Roles.Find((role => role.Id != paidRoleId));

                if (notPaidRole != null)
                    editingUser.Roles.Add(notPaidRole);
            }
        }

        private void UpdateOrmUserRoles(User ormUser, User editingUser)
        {
            ormUser.Roles.Clear();
            foreach (var role in editingUser.Roles)
            {
                var dbRole = context.Set<Role>().Find(role.Id);
                ormUser.Roles.Add(dbRole);
            }
        }

        private static void ValidateUser(DalUser dalUser)
        {
            if (dalUser == null)
            {
                //TODO logg
                throw new ArgumentNullException(nameof(dalUser), $"{nameof(dalUser)} is null.");
            }
        }

        private static void ValidateId(int id)
        {
            if (id < 0)
            {
                //TODO logg
                throw new ArgumentNullException(nameof(id), "Id must be positive.");
            }
        }

        private static void ValidatePageParams(int pageNum, int pageSize)
        {
            if (pageNum < 1)
            {
                //TODO logg
                throw new ArgumentOutOfRangeException(nameof(pageNum), $"{nameof(pageNum)} must be greator then 0.");
            }
            else if (pageSize < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageSize), $"{nameof(pageSize)} must be greator then 0.");
            }
        }
        #endregion
    }
}
