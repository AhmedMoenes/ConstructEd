using System.ComponentModel.DataAnnotations;
using ConstructEd.Models;

namespace ConstructEd.ViewModels
{
    public class CourseDetailsViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public decimal Duration { get; set; }
        public string Category { get; set; }

        public Instructor? Instructor { get; set; } 
        public List<CourseContentViewModel> CourseContents { get; set; } = new List<CourseContentViewModel>();
        
        public bool IsEnrolled { get; set; }
        public bool IsInWishlist { get; set; }
        public bool IsInCart { get; set; }
    }
}
