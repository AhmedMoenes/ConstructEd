using System.ComponentModel.DataAnnotations;

namespace ConstructEd.Models
{
    public enum Category
    {
        Construction,
        Engineering,
        Architecture,
        ProjectManagement,
        Safety,
        Sustainability,
        Technology,
        Other
    }
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal Duration { get; set; }
        public bool IsActive { get; set; } 
        public Category Category { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Enrollment>Enrollments { get; set; } = new HashSet<Enrollment>();

        public ICollection<CourseContent> CourseContents { get; set; }=new HashSet<CourseContent>(); 
    }
}
