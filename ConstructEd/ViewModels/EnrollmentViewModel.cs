namespace ConstructEd.ViewModels
{
    public class EnrollmentViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string EnrollmentDate { get; set; }
        public int Progress { get; set; }
    }
}