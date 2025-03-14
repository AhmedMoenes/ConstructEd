using ConstructEd.Models;

namespace ConstructEd.Repositories

{
    public interface IEnrollmentRepository : IRepository<Enrollment>
    {
        Task<Enrollment?> GetByUserAndCourseAsync(string userId, int courseId);
        Task<List<Enrollment>> GetByUserIdAsync(string userId);
    }
}
