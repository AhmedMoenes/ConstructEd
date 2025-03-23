using ConstructEd.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ConstructEd.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        // DbSet properties for your entities
        public DbSet<Course> Courses { get; set; }
        public DbSet<Plugin> Plugins { get; set; }
        public DbSet<CourseContent> CourseContents { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<ContactForm> ContactForms { get; set; }
        public DbSet<CourseReview> CourseReview { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Call the base method for Identity configurations

            // Configure cascade delete for Course -> ShoppingCarts relationship
            modelBuilder.Entity<Course>()
                .HasMany(c => c.ShoppingCarts)
                .WithOne(sc => sc.Course)
                .HasForeignKey(sc => sc.CourseId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete shopping carts when a course is deleted

            // Configure cascade delete for Course -> Wishlists relationship
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Wishlists)
                .WithOne(w => w.Course)
                .HasForeignKey(w => w.CourseId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete wishlists when a course is deleted

            // Configure cascade delete for Course -> Enrollments relationship
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Enrollments)
                .WithOne(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete enrollments when a course is deleted

            // Configure cascade delete for Course -> CourseContents relationship
            modelBuilder.Entity<Course>()
                .HasMany(c => c.CourseContents)
                .WithOne(cc => cc.Course)
                .HasForeignKey(cc => cc.CourseId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete course contents when a course is deleted

            // ✅ Configure cascade delete for Course -> PaymentCourse relationship
            modelBuilder.Entity<Course>()
                .HasMany(c => c.PaymentCourses) // Assuming PaymentCourses is a navigation property
                .WithOne(pc => pc.Course)
                .HasForeignKey(pc => pc.CourseId)
                .OnDelete(DeleteBehavior.Cascade); // Cascade delete payment courses when a course is deleted
        }

    }
}