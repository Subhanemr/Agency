using Agency.Models.Common;

namespace Agency.Models
{
    public class Category : BaseNameEntity
    {
        public ICollection<Product> Products { get; set; }
    }
}
