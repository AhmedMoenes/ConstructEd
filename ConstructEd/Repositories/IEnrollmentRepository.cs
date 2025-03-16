using ConstructEd.Data;
using ConstructEd.Models;

namespace ConstructEd.Repositories

{
    public interface IEnrollmentRepository : IRepository<Enrollment>
    {
        Task InsertRangeAsync(IEnumerable<Enrollment> enrollments);
        Task<bool> IsUserEnrolledAsync(string userId, int? courseId, int? pluginId);
        Task<bool> IsUserEnrolledInCourseAsync(string userId, int courseId);

        Task<bool> IsUserEnrolledInPluginAsync(string userId, int pluginId);
        Task<List<Enrollment>> GetAllEnrollmentsByUserIdAsync(string userId);
        Task<List<Course>> GetUserEnrolledCoursesAsync(string userId);
        Task<List<Plugin>> GetUserEnrolledPluginsAsync(string userId);
    }
}
