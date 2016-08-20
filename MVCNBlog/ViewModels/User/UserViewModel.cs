using MVCNBlog.ViewModels.Roles;

namespace MVCNBlog.ViewModels.User
{
    public class UserViewModel : AbstractUser
    {
        public string Rank { get; set; }
        public IPayedRole PayedRole { get; set; }
    }
}