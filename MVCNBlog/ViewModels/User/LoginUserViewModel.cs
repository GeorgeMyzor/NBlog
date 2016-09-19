using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MVCNBlog.Infrastructure.ValidationAttributes;
using MVCNBlog.ViewModels.Roles;

namespace MVCNBlog.ViewModels.User
{
    public sealed class LoginUserViewModel
    {
        [CorrectEmail]
        [Remote("ValidateLoginEmail", "User")]
        public string Email { get; set; }
        public DateTime CreationDate { get; set; }
        public IRole Role { get; set; }

        [CorrectPassword]
        [DataType(DataType.Password)]
        [Remote("ValidatePassword", "User")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}