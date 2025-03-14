using System.Globalization;
using Microsoft.AspNetCore.Identity;

namespace ConstructEd.Models
{
    public class ApplicationUser : IdentityUser
    { 
        // Extended Properties
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }= new HashSet<Enrollment>();
        public virtual IEnumerable<Payment> Payments { get; set; } = new HashSet<Payment>();
    }
}
