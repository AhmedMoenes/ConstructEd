using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConstructEd.Models
{
    public enum PaymentStatus
    {
        Pending,
        Processing,
        Completed,
        Failed,
        Refunded,
        PartiallyRefunded
    }

    public enum PaymentMethod
    {
        CreditCard,
        DebitCard,
        PayPal,
        BankTransfer,
        ApplePay,
        GooglePay,
        Other
    }

    [Index(nameof(TransactionId), IsUnique = true)]
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(50)]
        public string TransactionId { get; set; }

        [StringLength(100)]
        public string? ReceiptNumber { get; set; }

        [Required]
        public PaymentStatus Status { get; set; }

        [Required]
        public PaymentMethod Method { get; set; }

        [StringLength(3)]
        [Required]
        public string CurrencyCode { get; set; } = "EGP";

        [StringLength(50)]
        public string? PaymentProvider { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal RefundedAmount { get; set; } = 0;

        public DateTime? RefundDate { get; set; }

        [StringLength(50)]
        public string? RefundTransactionId { get; set; }

        // Audit fields
        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [StringLength(450)]
        public string? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        [StringLength(450)]
        public string? ModifiedBy { get; set; }

        // Navigation Properties
        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }

        [Required]
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course? Course { get; set; }
    }
}