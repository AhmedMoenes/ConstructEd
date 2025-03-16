using System.ComponentModel.DataAnnotations;

namespace ConstructEd.ViewModels
{
    public class PaymentViewModel
    {
        [Required]
        [CreditCard]
        [Display(Name = "Card Number")]
        public string CardNumber { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        [Display(Name = "Card Holder Name")]
        public string CardHolderName { get; set; } // Input only (not stored)
        public string MaskedCardNumber { get; set; } // For display (masked)

        [Display(Name = "Transaction ID")]
        public Guid TransactionID { get; set; }

        public string UserId { get; set; }

        [Required]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/?([0-9]{2})$", ErrorMessage = "Expiry date must be in MM/YY format.")]
        [Display(Name = "Expiry Date (MM/YY)")]
        public string ExpiryDate { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "CVV must be 3 digits.")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "CVV must be numeric and 3 digits.")]
        [Display(Name = "CVV")]
        public string CVV { get; set; }

        [Required]
        [Range(1, 100000, ErrorMessage = "Amount must be greater than zero.")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Courses")]
        public List<int> CourseIds { get; set; } = new List<int>();
        public List<int> PluginIds { get; set; } = new List<int>(); 

    }
}
