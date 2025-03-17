using ConstructEd.Data;
using ConstructEd.Models;
using Microsoft.EntityFrameworkCore;

namespace ConstructEd.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly DataContext _dataContext;

        public EnrollmentRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dataContext.Remove(entity);
            }
        }

        public async Task<ICollection<Enrollment>> GetAllAsync()
        {
            return await _dataContext.Enrollments
                .Include(n => n.Course).Include(b => b.Plugin)
                .Include(b => b.User)
                .ToListAsync();
        }

        public async Task<Enrollment> GetByIdAsync(int id)
        {
            return await _dataContext.Enrollments
                .Include(n => n.Course)
                .Include(b => b.User)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task InsertAsync(Enrollment obj)
        {
            await _dataContext.AddAsync(obj);
        }

        public async Task SaveAsync()
        {
            await _dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Enrollment obj)
        {
            _dataContext.Update(obj); 
            await Task.CompletedTask;
        }

        public async Task InsertRangeAsync(IEnumerable<Enrollment> enrollments)
        {
            await _dataContext.Enrollments.AddRangeAsync(enrollments);
        }

        public async Task<bool> IsUserEnrolledAsync(string userId, int? courseId, int? pluginId)
        {
            return await _dataContext.Enrollments
                .AnyAsync(e => e.UserId == userId &&
                              ((courseId != null && e.CourseId == courseId) ||
                                (pluginId != null && e.PluginId == pluginId)));
        }

        public async Task<bool> IsUserEnrolledInCourseAsync(string userId, int courseId)
        {
            return await _dataContext.Enrollments
                .AnyAsync(e => e.UserId == userId && e.CourseId == courseId);
        }

        public async Task<bool> IsUserEnrolledInPluginAsync(string userId, int pluginId)
        {
            return await _dataContext.Enrollments
                .AnyAsync(e => e.UserId == userId && e.PluginId == pluginId);
        }
        public async Task<List<Enrollment>> GetAllEnrollmentsByUserIdAsync(string userId)
        {
            return await _dataContext.Enrollments
                .Include(e => e.Course)  // Include Course details if available
                .Include(e => e.Plugin)  // Include Plugin details if available
                .Where(e => e.UserId == userId)
                .ToListAsync();
        }

        public async Task<List<Course>> GetUserEnrolledCoursesAsync(string userId)
        {
            return await _dataContext.Enrollments
                .Where(e => e.UserId == userId && e.CourseId != null) // Ensure it's a course enrollment
                .Select(e => e.Course) // Select the course object
                .ToListAsync();
        }

        public async Task<List<Plugin>> GetUserEnrolledPluginsAsync(string userId)
        {
            return await _dataContext.Enrollments
                .Where(e => e.UserId == userId && e.PluginId != null) // Ensure it's a plugin enrollment
                .Select(e => e.Plugin) // Select the plugin object
                .ToListAsync();
        }

        
    }
}