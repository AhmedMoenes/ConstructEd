using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ConstructEd.Models
{
    public class Instructor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Bio { get; set; }

        public string? ProfilePicture { get; set; } // URL to the instructor's profile picture

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property for courses taught by the instructor
        public ICollection<Course>? Courses { get; set; }

    }
}