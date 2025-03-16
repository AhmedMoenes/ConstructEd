  using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConstructEd.Models
{
    public class Enrollment
    {
        [Key]
        public int Id { get; set; }
        public DateTime EnrollmentDate { get; set; } = DateTime.UtcNow;
        public double? Progress { get; set; } = 0.00;
       //removing wish list from from enrollment...
        // Navigation Properties
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public ICollection<PaymentCourse> PaymentCourses { get; set; } = new HashSet<PaymentCourse>();

    }
}
