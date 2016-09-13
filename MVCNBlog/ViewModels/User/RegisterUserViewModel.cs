using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MVCNBlog.Infrastructure.ValidationAttributes;
using MVCNBlog.ViewModels.Roles;

namespace MVCNBlog.ViewModels.User
{
    public class RegisterUserViewModel
    {
        [CorrectEmail]
        [Remote("ValidateEmail", "User")]
        public string Email { get; set; }
        [CorrectName]
        [Remote("ValidateName", "User")]
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public IRole Role { get; set; }

        [CorrectPassword]
        [DataType(DataType.Password)]
        [Remote("ValidatePassword", "User")]
        public string Password { get; set; }

        [CorrectPassword]
        [Remote("ValidatePassword", "User")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Passwords must match")]
        [DataType(DataType.Password)]
        [DisplayName("Confrim password")]
        public string ConfirmPassword { get; set; }

        public byte[] UserPic { get; set; }
    }
}