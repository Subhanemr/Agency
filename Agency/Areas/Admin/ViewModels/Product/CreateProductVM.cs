using a = Agency.Models;
using System.ComponentModel.DataAnnotations;

namespace Agency.Areas.Admin.ViewModels
{
    public class CreateProductVM
    {
        [Required(ErrorMessage = "Is required")]
        [MaxLength(25, ErrorMessage = ("25 characters"))]
        public string Name { get; set; }
        [Required(ErrorMessage = "Is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Greater than 0")]
        public decimal Price { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Greater than 0")]
        public int CategoryId { get; set; }
        public ICollection<a.Category>? Categories { get; set; }

        [Required(ErrorMessage = "Is required")]
        public IFormFile Photo { get; set; }
    }
}
