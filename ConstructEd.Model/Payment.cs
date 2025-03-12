using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConstructEd.Models
{
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed
    }
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }= DateTime.UtcNow;
        public string TransactionId { get; set; }
        public PaymentStatus Status { get; set; }

        // Navigation Properties
        [ForeignKey("User")]
        public string UserId { get; set; }

        public ApplicationUser? User { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course? Course { get; set; }

    }
}
