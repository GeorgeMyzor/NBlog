using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCNBlog.Infrastructure.ValidationAttributes;

namespace MVCNBlog.ViewModels
{
    public class LoginUserViewModel : AbstractUser
    {
        [Remote("ValidatePassword", "User")]
        [CorrectPassword(ErrorMessage = "Password should be 5 to 15 length, at least one digit.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        //TODO remember me?
    }
}