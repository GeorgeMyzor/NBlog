﻿using System;
using System.Collections.Generic;

namespace ORM.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public DateTime CreationDate { get; set; }
        public byte[] UserPic { get; set; }

        public virtual List<Role> Roles { get; set; }
    }
}
