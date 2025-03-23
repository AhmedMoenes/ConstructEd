using ConstructEd.Data;
using ConstructEd.Models;
using ConstructEd.Repositories;
using Microsoft.EntityFrameworkCore;

public class CourseReviewRepository : ICourseReviewRepository
{
    private readonly DataContext _dataContext;

    public CourseReviewRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IEnumerable<CourseReview>> GetReviewsByCourseIdAsync(int courseId)
    {
        return await _dataContext.CourseReview
            .Include(r => r.User)
            .Where(r => r.CourseId == courseId)
            .ToListAsync();
    }

    public async Task AddReviewAsync(CourseReview review)
    {
        _dataContext.CourseReview.Add(review);
        await _dataContext.SaveChangesAsync();
    }
}