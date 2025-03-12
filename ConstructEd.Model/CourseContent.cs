using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConstructEd.Models
{
    public enum ContentType
    {
        Video,
        Document,
        Quiz,
        Assignment
    }
    public class CourseContent
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public ContentType Type { get; set; }
        public string FileUrl { get; set; }
        public int Order { get; set; }

        // Navigation Properties
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course? Course { get; set; }
    }
}
