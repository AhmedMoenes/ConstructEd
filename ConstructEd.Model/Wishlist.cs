using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConstructEd.Models
{
    public class Wishlist
    {
        [Key]
        public int Id { get; set; }

        // Foreign key for User
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }

        // Foreign key for Product
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course? Course { get; set; }

        public DateTime AddedOn { get; set; } // Optional: To track when the product was added to the wishlist
    }
}
