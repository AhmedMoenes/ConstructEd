using System.ComponentModel.DataAnnotations;

namespace ConstructEd.ViewModels
{
    public class ProfileViewModel
    {
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Profile Picture")]
        public string? ProfilePicture { get; set; }

        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Experience Years")]
        public int? Experience { get; set; }

        [Display(Name = "Bio")]
        public string? Bio { get; set; }
        public string Email { get; set; }
        public List<EnrollmentViewModel>? Enrollments { get; set; }
        public List<CourseViewModel>? Courses { get; set; } 

    }
}
