using System.ComponentModel.DataAnnotations;

namespace ModelBinderTesting.Controllers.ViewModels
{
    public class ExampleViewModel
    {
        [Required]
        public int? TestInteger { get; set; } 
    }
}