using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Interface.Entities;
using DAL.Interface.DTO;

namespace BLL.Mappers
{
    public static class BllRoleMapper
    {
        public static BllRole ToBllRole(this DalRole dalRole)
        {
            return new BllRole
            {
                Id = dalRole.Id,
                Name = dalRole.Name,
            };
        }

        public static DalRole ToDalRole(this BllRole bllRole)
        {
            return new DalRole
            {
                Id = bllRole.Id
            };
        }
    }
}
