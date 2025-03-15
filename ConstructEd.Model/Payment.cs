using Microsoft.EntityFrameworkCore;
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
    //[Index(nameof(TransactionId), IsUnique = true)]
    public class Payment
    {
        //[Key]
        //public int Id { get; set; }

        //[Required]
        //public decimal Amount { get; set; }

        //[Required]
        //public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        //[Required]
        //[StringLength(50)]
        //public string TransactionId { get; set; } // guid at VM

        //[StringLength(3)]
        //[Required]
        //public string CurrencyCode { get; set; } = "EGP";

        //// Navigation Properties
        //[Required]
        //[ForeignKey("User")]
        //public string UserId { get; set; }
        //public ApplicationUser? User { get; set; }

        //[Required]
        //[ForeignKey("Course")]
        //public int CourseId { get; set; }
        //public Course? Course { get; set; }
        [Key]
        public int PaymentId { get; set; } // Primary Key

        [Required]
        public string CardHolderName { get; set; }

        [Required]
        public string MaskedCardNumber { get; set; } // Store only the last 4 digits for security

        [Required]
        public string ExpiryDate { get; set; } // MM/YY format

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public PaymentStatus Status { get; set; } // e.g., "Success", "Failed", "Pending"

        public DateTime PaymentDate { get; set; } = DateTime.Now; // Auto-generated timestamp

    }
}