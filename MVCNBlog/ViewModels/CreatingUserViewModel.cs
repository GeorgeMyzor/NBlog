using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCNBlog.Infrastructure.ValidationAttributes;

namespace MVCNBlog.ViewModels
{
    public class CreatingUserViewModel : UserViewModel
    {
        [Remote("ValidatePassword", "User")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Passwords must match")]
        [CorrectPassword(ErrorMessage = "Password should be 5 to 15 length, at least one digit.")]
        public string RePassword { get; set; }
    }
}