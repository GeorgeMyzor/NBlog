using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MVCNBlog.Infrastructure.ValidationAttributes;

namespace MVCNBlog.ViewModels.User
{
    public class LoginUserViewModel : AbstractUser
    {
        [DataType(DataType.Password)]
        [Remote("ValidatePassword", "User")]
        [CorrectPassword]
        public string Password { get; set; }
        //TODO remember me?
    }
}