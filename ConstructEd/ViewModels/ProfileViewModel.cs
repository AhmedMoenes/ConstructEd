namespace ConstructEd.ViewModels
{
    public class ProfileViewModel
    {
        public string FullName { get; set; }
        public string? ProfilePicture { get; set; }
        public string Email { get; set; }
        public List<EnrollmentViewModel>? Enrollments { get; set; }
    }
}
