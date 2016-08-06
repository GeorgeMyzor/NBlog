using System;
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
        public static DalUser ToDalUser(this BllUser bllUser)
        {
            return new DalUser()
            {
                Id = bllUser.Id,
                Name = bllUser.Name,
                CreationDate = bllUser.CreationDate,
                Roles = bllUser.Roles.Select(bllRole => bllRole.ToDalRole()).ToList()
            };
        }

        public static BllUser ToBllUser(this DalUser dalUser)
        {
            return new BllUser()
            {
                Id = dalUser.Id,
                Name = dalUser.Name,
                CreationDate = dalUser.CreationDate,
                Roles = dalUser.Roles.Select(dalRole => dalRole.ToBllRole()).ToList()
            };
        }
    }
}
