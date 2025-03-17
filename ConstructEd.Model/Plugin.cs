using System.ComponentModel.DataAnnotations;

namespace ConstructEd.Models
{
    public class Plugin
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public string? Image {  get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Enrollment>? Enrollments { get; set; } = new HashSet<Enrollment>();
        public ICollection<ShoppingCart>? ShoppingCarts { get; set; } = new HashSet<ShoppingCart>();
        public ICollection<Wishlist>? Wishlists { get; set; }
    }
}
