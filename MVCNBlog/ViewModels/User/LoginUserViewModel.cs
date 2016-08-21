using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MVCNBlog.Infrastructure.ValidationAttributes;
using MVCNBlog.ViewModels.Roles;

namespace MVCNBlog.ViewModels.User
{
    public class LoginUserViewModel
    {
        [CorrectName]
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public IRole Role { get; set; }

        [DataType(DataType.Password)]
        [Remote("ValidatePassword", "User")]
        [CorrectPassword]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}