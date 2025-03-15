namespace ConstructEd.Models
{
    public class PaymentCourse
    {
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}