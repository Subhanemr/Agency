using Agency.Models.Common;

namespace Agency.Models
{
    public class Product : BaseNameEntity
    {
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public string Img { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
