using System.ComponentModel.DataAnnotations;

namespace Agency.Areas.Admin.ViewModels
{
    public class CreateCategoryVM
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(25, ErrorMessage =("25 characters"))]
        public string Name { get; set; }
    }
}
