using System.ComponentModel.DataAnnotations;

namespace ConstructEd.ViewModels
{
    public class InstructorViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Bio { get; set; }

        public string? ProfilePicture { get; set; } // URL to the instructor's profile picture

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public List<CourseViewModel> Courses { get; set; } = new List<CourseViewModel>();
    }
}