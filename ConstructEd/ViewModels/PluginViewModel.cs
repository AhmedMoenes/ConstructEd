using System.ComponentModel.DataAnnotations;
using ConstructEd.Models;

namespace ConstructEd.ViewModels
{
    public class PluginViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 1000, ErrorMessage = "Price must be between $0.01 and $1000")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Category is required")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Category Category { get; set; }
    }
}