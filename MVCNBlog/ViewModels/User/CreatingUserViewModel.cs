using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCNBlog.Infrastructure.ValidationAttributes;
using MVCNBlog.ViewModels.Roles;

namespace MVCNBlog.ViewModels
{
    public class CreatingUserViewModel : AbstractUser
    {
        [Remote("ValidatePassword", "User")]
        [CorrectPassword(ErrorMessage = "Password should be 5 to 15 length, at least one digit.")]
        public string Password { get; set; }

        [Remote("ValidatePassword", "User")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Passwords must match")]
        [CorrectPassword(ErrorMessage = "Password should be 5 to 15 length, at least one digit.")]
        public string ConfirmPassword { get; set; }
    }
}