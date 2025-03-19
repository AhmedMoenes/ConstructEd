using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConstructEd.Models
{
    public enum PaymentStatus : byte
    {
        Success,
        Failed,
        Pending
    }
    public class Payment
    {
        [Key]
        public int Id { get; set; } // Primary Key

        [Required]
        public string CardHolderName { get; set; }

        [Required]
        public string MaskedCardNumber { get; set; } 

        [Required]
        public string ExpiryDate { get; set; } 

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public Guid TransactionID { get; set; } = Guid.NewGuid();

        [Required]
        public PaymentStatus Status { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } 
        public ApplicationUser User { get; set; }

        public ICollection<PaymentCourse>? PaymentCourses { get; set; } = new HashSet<PaymentCourse>();
        public ICollection<PaymentPlugin>? PaymentPlugin { get; set; } = new HashSet<PaymentPlugin>();

    }
}