using ConstructEd.Models;
using Microsoft.AspNetCore.Identity;
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
        public DbSet<ContactForm> ContactForm { get; set; }
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

            // User -> Roles
            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> Claims
            modelBuilder.Entity<IdentityUserClaim<string>>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(uc => uc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> Logins
            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(ul => ul.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> Tokens
            modelBuilder.Entity<IdentityUserToken<string>>()
                .HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Configure cascade delete for your application-specific tables

            // User -> Enrollments
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.User)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> Payments
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> ShoppingCart
            modelBuilder.Entity<ShoppingCart>()
                .HasOne(sc => sc.User)
                .WithMany(u => u.ShoppingCarts)
                .HasForeignKey(sc => sc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User -> Wishlist
            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.User)
                .WithMany(u => u.Wishlists)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ Enable Cascade Delete for Enrollments when a Course is deleted
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade); // Automatically delete enrollments when a course is deleted

            // ✅ Enable Cascade Delete for Enrollments when a Plugin is deleted
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Plugin)
                .WithMany(p => p.Enrollments)
                .HasForeignKey(e => e.PluginId)
                .OnDelete(DeleteBehavior.Cascade); // Automatically delete enrollments when a plugin is deleted

            #region Delete Plugins

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Plugin)
                .WithMany()
                .HasForeignKey(e => e.PluginId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ShoppingCart>()
                .HasOne(sc => sc.Plugin)
                .WithMany()
                .HasForeignKey(sc => sc.PluginId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.Plugin)
                .WithMany()
                .HasForeignKey(w => w.PluginId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion
        }
    }
}