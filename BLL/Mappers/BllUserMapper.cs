﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Entities;
using DAL.Interface.DTO;

namespace BLL.Mappers
{
    public static class BllUserMapper
    {
        public static BllUser ToBllUser(this DalUser dalUser)
        {
            return dalUser == null ? null : new BllUser()
            {
                Id = dalUser.Id,
                Email = dalUser.Email,
                Name = dalUser.Name,
                CreationDate = dalUser.CreationDate,
                Password = dalUser.Password,
                UserPic = dalUser.UserPic,
                Roles = dalUser.Roles.Select(dalRole => dalRole.ToBllRole())
            };
        }

        public static DalUser ToDalUser(this BllUser bllUser)
        {
            return new DalUser()
            {
                Id = bllUser.Id,
                Email = bllUser.Email,
                Name = bllUser.Name,
                CreationDate = bllUser.CreationDate,
                Password = bllUser.Password,
                UserPic = bllUser.UserPic,
                Roles = bllUser.Roles.Select(bllRole => bllRole.ToDalRole())
            };
        }
    }
}
