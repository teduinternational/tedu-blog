using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TeduBlog.WebApp.Models
{
    public class ChangeProfileViewModel
    {
        [Required(ErrorMessage = "First name is required")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [DisplayName("Last Name")]

        public string LastName { get; set; }
    }
}
