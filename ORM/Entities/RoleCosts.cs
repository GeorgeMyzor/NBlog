using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Entities
{
    public class RoleCosts
    {
        public int Id { get; set; }
        public int Cost { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
    }
}
