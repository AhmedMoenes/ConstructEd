using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using ConstructEd.Models;

namespace ConstructEd.ViewModels
{
    public class CourseContentViewModel
    {
        public int? Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public ContentType Type { get; set; }

        public string? FileUrl { get; set; }

        public int? Order { get; set; }

        [Required]
        public int CourseId { get; set; }

        // Additional properties for the view
        public IEnumerable<SelectListItem>? Courses { get; set; }
        public IEnumerable<SelectListItem>? ContentTypes { get; set; }
        //new
        public bool IsEnrolled { get; set; }
    }
}