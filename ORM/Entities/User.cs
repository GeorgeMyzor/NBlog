using System;
using System.Collections.Generic;

namespace ORM.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual List<Role> Roles { get; set; }
    }
}
