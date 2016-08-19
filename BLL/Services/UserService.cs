﻿using System;
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

        public UserService(IUnitOfWork uow, IUserRepository repository)
        {
            this.uow = uow;
            this.userRepository = repository;
        }

        public int GetUsersCount()
        {
            return userRepository.GetCount();
        }

        public BllUser GetUserEntity(int id)
        {
            return userRepository.GetById(id)?.ToBllUser();
        }

        public BllUser GetUserEntity(string name)
        {
            return userRepository.GetByPredicate(user => user.Name == name)?.ToBllUser();
        }

        public IEnumerable<BllUser> GetAllUserEntities()
        {
            return userRepository.GetAll().Select(user => user.ToBllUser());
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
    }
}
