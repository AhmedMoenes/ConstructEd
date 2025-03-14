using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConstructEd.Models
{

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
        public string TransactionId { get; set; } // guid at VM

        [StringLength(3)]
        [Required]
        public string CurrencyCode { get; set; } = "EGP";

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