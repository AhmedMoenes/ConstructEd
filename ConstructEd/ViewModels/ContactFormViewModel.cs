using System.ComponentModel.DataAnnotations;

namespace ConstructEd.ViewModels
{
    public class ContactFormViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [Display(Name = "Your Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Your Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Subject is required.")]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Message is required.")]
        [Display(Name = "Your Message")]
        public string Message { get; set; }
    }
}
