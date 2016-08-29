using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCNBlog.Infrastructure.ValidationAttributes;
using MVCNBlog.ViewModels.Roles;

namespace MVCNBlog.ViewModels
{
    public class AccountViewModel
    {
        public int Id { get; set; }
        [CorrectEmail]
        [Remote("ValidateEmail", "User")]
        public string Email { get; set; }
        [CorrectName]
        [Remote("ValidateName", "User")]
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public string Rank { get; set; }
        public int SubscriptionCost { get; set; }
        public byte[] UserPic { get; set; }
        public IPayedRole PayedRole { get; set; }
        public IRole Role { get; set; }
    }
}