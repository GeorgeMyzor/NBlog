﻿using System;
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

        public UserRepository(DbContext uow)
        {
            this.context = uow;
        }

        public IEnumerable<DalUser> GetAll()
        {
            return context.Set<User>().ToList().Select(ormUser => ormUser.ToDalUser());
        }

        public DalUser GetById(int key)
        {
            var ormUser = context.Set<User>().FirstOrDefault(user => user.Id == key);

            int ormUserNotes = context.Set<Article>().Count(article => article.Author.Id == ormUser.Id);
            ormUserNotes += context.Set<Comment>().Count(comment => comment.Author.Id == ormUser.Id);

            var dalUser = ormUser.ToDalUser();
            dalUser.NotesCount = ormUserNotes;
            return dalUser;
        }

        public DalUser GetByPredicate(Expression<Func<DalUser, bool>> f)
        {
            //Expression<Func<DalUser, bool>> -> Expression<Func<User, bool>> (!)
            throw new NotImplementedException();
        }

        public void Create(DalUser dalUser)
        {
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
            var ormUser = dalUser.ToOrmUser();

            ormUser = context.Set<User>().Single(u => u.Id == ormUser.Id);
            context.Set<User>().Remove(ormUser);
        }
        
        public void Update(DalUser dalUser)
        {
            var editingUser = dalUser.ToOrmUser();
            var ormUser = context.Set<User>().Single(u => u.Id == dalUser.Id);

            ormUser.Name = editingUser.Name;

            CopyOrmUserPaidRoles(ormUser, editingUser);

            UpdateOrmUserRoles(ormUser, editingUser);
        }
        
        public void UpdatePaidRole(DalUser dalUser)
        {
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
        
        #endregion
    }
}
