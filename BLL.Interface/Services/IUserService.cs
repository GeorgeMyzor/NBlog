﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Entities;

namespace BLL.Interface.Services
{
    public interface IUserService
    {
        BllUser GetUserEntity(int id);
        IEnumerable<BllUser> GetAllUserEntities();
        void CreateUser(BllUser user);
        void DeleteUser(BllUser user);
    }
}
