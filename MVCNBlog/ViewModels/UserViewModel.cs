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
    public class UserViewModel
    {
        public int Id { get; set; }
        [Remote("ValidateName", "User")]
        [CorrectName(ErrorMessage = "Name should be 3 to 10 length, only letters.")]
        public string Name { get; set; }
        [Remote("ValidatePassword", "User")]
        public string Password { get; set; }
        public DateTime CreationDate { get; set; }
        public string Rank { get; set; }
        public IPayedRole PayedRole { get; set; }
        public IRole Role { get; set; }
    }
}