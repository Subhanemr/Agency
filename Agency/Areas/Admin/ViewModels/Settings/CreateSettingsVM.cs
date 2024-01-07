using System.ComponentModel.DataAnnotations;

namespace Agency.Areas.Admin.ViewModels
{
    public class CreateSettingsVM
    {
        [Required(ErrorMessage = "Key is reuqired")]
        public string Key { get; set; }
        [Required(ErrorMessage = "Value is reuqired")]
        public string Value { get; set; }
    }
}
