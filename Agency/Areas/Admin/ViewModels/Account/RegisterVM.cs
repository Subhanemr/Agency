using System.ComponentModel.DataAnnotations;

namespace Agency.Areas.Admin.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "UserName is required")]
        [MaxLength(25, ErrorMessage = ("25 characters"))]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(25, ErrorMessage = ("25 characters"))]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [MaxLength(25, ErrorMessage = ("25 characters"))]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage ="Passowrd be same")]
        public string ConfirmPassword { get; set; }
    }
}
