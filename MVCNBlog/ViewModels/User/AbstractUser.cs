using System;
using MVCNBlog.Infrastructure.ValidationAttributes;
using MVCNBlog.ViewModels.Roles;

namespace MVCNBlog.ViewModels.User
{
    //TODO refactor
    public abstract class AbstractUser
    {
        public int Id { get; set; }
        [CorrectName]
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public IRole Role { get; set; }
    }
}