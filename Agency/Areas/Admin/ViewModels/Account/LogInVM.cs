using System.ComponentModel.DataAnnotations;

namespace Agency.Areas.Admin.ViewModels
{
    public class LogInVM
    {
        [Required(ErrorMessage = "Username or Email is required")]
        [MaxLength(25, ErrorMessage = ("25 characters"))]
        public string UserNameOrEmail { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
