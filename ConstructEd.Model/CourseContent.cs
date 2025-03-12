namespace ConstructEd.Models
{
    public class CourseContent
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string FileUrl { get; set; }
        public int Order { get; set; }

        // Navigation Properties
        public int CourseId { get; set; }
        public Course? Course { get; set; }
    }
}
