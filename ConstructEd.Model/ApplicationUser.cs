using System.Globalization;
using Microsoft.AspNetCore.Identity;

namespace ConstructEd.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Extended Properties
        public string FullName { get; set; }
        public string? ProfilePicture { get; set; }
        public DateTime CreatedAt { get; set; }

        // HashSet: ------
        public virtual ICollection<Enrollment>? Enrollments { get; set; } = new HashSet<Enrollment>();
        public virtual ICollection<Payment>? Payments { get; set; } = new HashSet<Payment>();
        public virtual ICollection<ShoppingCart>? ShoppingCarts { get; set; } = new HashSet<ShoppingCart>();
        public ICollection<Wishlist>? Wishlists { get; set; }

    }
}
