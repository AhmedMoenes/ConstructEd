using System.ComponentModel.DataAnnotations;

namespace ConstructEd.ViewModels
{
    public class PaymentViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string TransactionId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Payment Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        public string PaymentDate { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        [Required]
        [StringLength(3)]
        [Display(Name = "Currency")]
        public string CurrencyCode { get; set; } = "EGP";

        [Display(Name = "Payment Provider")]
        public string PaymentProvider { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Refunded Amount")]
        public decimal RefundedAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [Display(Name = "Refund Date")]
        public string RefundDate { get; set; }

        [Display(Name = "Refund Transaction ID")]
        public string RefundTransactionId { get; set; }

        // User information
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "User ID")]
        public string UserId { get; set; }

        // Course information
        [Required]
        [Display(Name = "Course Title")]
        public string CourseTitle { get; set; }

        [Required]
        [Display(Name = "Course ID")]
        public int CourseId { get; set; }

        // Added for UI display
        [Display(Name = "Receipt Number")]
        public string ReceiptNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [Display(Name = "Created Date")]
        public string CreatedAt { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
        [Display(Name = "Modified Date")]
        public string ModifiedAt { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }
    }

    // For creating new payments
    public class CreatePaymentViewModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Transaction ID")]
        public string TransactionId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        [Required]
        [StringLength(3)]
        [Display(Name = "Currency")]
        public string CurrencyCode { get; set; } = "EGP";

        [Display(Name = "Payment Provider")]
        public string PaymentProvider { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "User ID")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Course ID")]
        public int CourseId { get; set; }
    }

    // For processing refunds
    public class RefundViewModel
    {
        [Required]
        public int PaymentId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Refund amount must be greater than zero")]
        [Display(Name = "Refund Amount")]
        public decimal RefundAmount { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Refund Transaction ID")]
        public string RefundTransactionId { get; set; }
    }
}