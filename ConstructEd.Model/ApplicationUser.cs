using System.Globalization;
using Microsoft.AspNetCore.Identity;

namespace ConstructEd.Models
{
    public class ApplicationUser : IdentityUser
    { 
        // Extended Properties
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Role { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
