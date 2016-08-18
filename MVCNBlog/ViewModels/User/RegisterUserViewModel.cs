using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MVCNBlog.Infrastructure.ValidationAttributes;

namespace MVCNBlog.ViewModels.User
{
    public class RegisterUserViewModel : AbstractUser
    {
        [Remote("ValidateName", "User")]
        public new string Name { get; set; }

        [Remote("ValidatePassword", "User")]
        [CorrectPassword]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Remote("ValidatePassword", "User")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Passwords must match")]
        [CorrectPassword]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}