using MVCNBlog.ViewModels.Roles;

namespace MVCNBlog.ViewModels.User
{
    public class UserViewModel : AbstractUser
    {
        public IPayedRole PayedRole { get; set; }
    }
}