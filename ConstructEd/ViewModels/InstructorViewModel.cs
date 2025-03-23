using System.ComponentModel.DataAnnotations;

namespace ConstructEd.ViewModels
{
    public class InstructorViewModel
    {
        public string? Id {  get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmedPassword { get; set; }

        [Display(Name = "Bio")]
        [StringLength(500, ErrorMessage = "Bio cannot exceed 500 characters")]
        public string? Bio { get; set; }
        public bool IsInstructor { get; set; }

        [Display(Name = "Profile Picture")]
        public string? ProfilePicture { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int? Experience { get; set; }
        public List<CourseViewModel> Courses { get; set; } = new List<CourseViewModel>();
    }
}