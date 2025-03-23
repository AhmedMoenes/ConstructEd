using System.ComponentModel.DataAnnotations;

namespace ConstructEd.ViewModels
{
    public class CourseReviewViewModel
    {
        public int Id { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [StringLength(500, ErrorMessage = "Review cannot exceed 500 characters.")]
        public string Comment { get; set; }

        public string? UserName { get; set; } 
        public DateTime CreatedAt { get; set; }
    }
}
