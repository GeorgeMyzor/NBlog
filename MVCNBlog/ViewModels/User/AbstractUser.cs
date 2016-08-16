using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCNBlog.Infrastructure.ValidationAttributes;
using MVCNBlog.ViewModels.Roles;

namespace MVCNBlog.ViewModels
{
    public abstract class AbstractUser
    {
        public int Id { get; set; }
        [Remote("ValidateName", "User")]
        [CorrectName(ErrorMessage = "Name should be 3 to 10 length, only letters.")]
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public IRole Role { get; set; }
    }
}