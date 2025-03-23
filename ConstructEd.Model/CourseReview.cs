using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConstructEd.Models
{
    public class CourseReview
    {
        public int Id { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [StringLength(500, ErrorMessage = "Review cannot exceed 500 characters.")]
        public string Comment { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public int CourseId { get; set; }
        public Course Course { get; set; }
        [Required]
        public string UserId { get; set; } // Assuming you’re using ASP.NET Identity for users
        public ApplicationUser User { get; set; } // Assuming ApplicationUser is your user class
    }
}