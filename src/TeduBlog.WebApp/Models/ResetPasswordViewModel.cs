using System.ComponentModel.DataAnnotations;

namespace TeduBlog.WebApp.Models
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "{0} required")]
        [EmailAddress(ErrorMessage = "Email format is not correct.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [StringLength(100, ErrorMessage = "{0} must contains at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password confirmation")]
        [Compare("Password", ErrorMessage = "Password confirmation is not match")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
