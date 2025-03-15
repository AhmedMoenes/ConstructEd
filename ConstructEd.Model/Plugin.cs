using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructEd.Models
{
    public class Plugin
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Enrollment> Enrollments { get; set; } = new HashSet<Enrollment>();
        public ICollection<ShoppingCart> ShoppingCarts { get; set; } = new HashSet<ShoppingCart>();
        public ICollection<Wishlist> Wishlists { get; set; }
    }
}
