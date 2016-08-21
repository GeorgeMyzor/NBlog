using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MVCNBlog.Infrastructure.ValidationAttributes;
using MVCNBlog.ViewModels.Roles;

namespace MVCNBlog.ViewModels.User
{
    public class RegisterUserViewModel
    {
        [CorrectName]
        [Remote("ValidateName", "User")]
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public IRole Role { get; set; }

        [Remote("ValidatePassword", "User")]
        [CorrectPassword]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Remote("ValidatePassword", "User")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Passwords must match")]
        [CorrectPassword]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}