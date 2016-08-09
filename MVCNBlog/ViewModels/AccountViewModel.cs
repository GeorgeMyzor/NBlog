using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCNBlog.ViewModels.Roles;

namespace MVCNBlog.ViewModels
{
    public class AccountViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public int SubscriptionCost { get; set; }
        public string Rank { get; set; }
        public IPayedRole PayedRole { get; set; }
        public IRole Role { get; set; }
    }
}