using System.ComponentModel.DataAnnotations;
using TeduBlog.WebApp.Extensions;

namespace TeduBlog.WebApp.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Old password is required")]
        public required string OldPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        public required string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm new password required")]
        [PasswordMatch("NewPassword", ErrorMessage = "Confirm password is not matched")]
        public required string ConfirmPassword { get; set; }
    }
}
