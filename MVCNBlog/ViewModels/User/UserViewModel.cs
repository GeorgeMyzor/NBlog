using System;
using System.Web.Mvc;
using MVCNBlog.Infrastructure.ValidationAttributes;
using MVCNBlog.ViewModels.Roles;

namespace MVCNBlog.ViewModels.User
{
    public class UserViewModel
    {
        public int Id { get; set; }
        [CorrectName]
        [Remote("ValidateName", "User")]
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public string Rank { get; set; }
        public IRole Role { get; set; }
        public IPayedRole PayedRole { get; set; }
    }
}