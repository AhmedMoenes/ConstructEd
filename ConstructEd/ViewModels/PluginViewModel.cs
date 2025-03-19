using System.ComponentModel.DataAnnotations;
using ConstructEd.Models;

namespace ConstructEd.ViewModels
{
    public class PluginViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public IFormFile ImageFile { get; set; } 
        public string? Image { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime EnrolledDate { get; set; } = DateTime.UtcNow;
        public bool IsInWishlist { get; set; }
        public bool IsInCart { get; set; }
        public bool IsEnrolled { get; set; }
    }
}