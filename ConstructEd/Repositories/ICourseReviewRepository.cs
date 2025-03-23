using ConstructEd.Models;

namespace ConstructEd.Repositories
{
    public interface ICourseReviewRepository
    {
        Task<IEnumerable<CourseReview>> GetReviewsByCourseIdAsync(int courseId);
        Task AddReviewAsync(CourseReview review);
    }
}
