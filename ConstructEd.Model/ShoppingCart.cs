using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConstructEd.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }

        // 🔹 User Foreign Key (Every cart entry belongs to a user)
        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public ApplicationUser? User { get; set; }

        // 🔹 Course Foreign Key (Nullable)
        [ForeignKey(nameof(Course))]
        public int? CourseId { get; set; }
        public Course? Course { get; set; }

        // 🔹 Plugin Foreign Key (Nullable) 
        [ForeignKey(nameof(Plugin))]
        public int? PluginId { get; set; }
        public Plugin? Plugin { get; set; }

        // 🔹 Ensure Only One of Course or Plugin is Chosen
        [NotMapped]
        public bool IsValid => (CourseId.HasValue && !PluginId.HasValue) || (!CourseId.HasValue && PluginId.HasValue);
    }
}