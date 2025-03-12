namespace ConstructEd.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public int Progress { get; set; }

        // Navigation Properties
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Course Course { get; set; }
    }
}
