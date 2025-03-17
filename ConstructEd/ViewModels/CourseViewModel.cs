using System.ComponentModel.DataAnnotations;
using ConstructEd.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConstructEd.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public decimal Duration { get; set; }

        public Category Category { get; set; }

        [Required]
        public int InstructorId { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime EnrolledDate { get; set; } = DateTime.UtcNow;

        public List<CourseContentViewModel> CourseContents { get; set; } = new List<CourseContentViewModel>();
        public bool IsInWishlist { get; set; }

    }
}