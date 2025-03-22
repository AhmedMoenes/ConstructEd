using ConstructEd.Data;
using ConstructEd.Models;
using ConstructEd.Repositories;
using ConstructEd.ViewModels;
using Microsoft.EntityFrameworkCore;

public class StatisticsRepository : IStatisticsRepository
{
    private readonly DataContext _context;

    public StatisticsRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<StatisticsViewModel> GetStatisticsAsync()
    {
        var statistics = new StatisticsViewModel
        {
            TotalCourses = await _context.Courses.CountAsync(),
            ActiveCourses = await _context.Courses.CountAsync(c => c.Enrollments.Any()),
            MostPopularCourse = await _context.Courses
                .OrderByDescending(c => c.Enrollments.Count)
                .Select(c => c.Title)
                .FirstOrDefaultAsync(),
            TotalPlugins = await _context.Plugins.CountAsync(),
            ActivePlugins = await _context.Plugins.CountAsync(p => p.Enrollments.Any()),
            MostPopularPlugin = await _context.Plugins
                .OrderByDescending(p => p.Enrollments.Count)
                .Select(p => p.Title)
                .FirstOrDefaultAsync(),
            TotalEnrollments = await _context.Enrollments.CountAsync(),
            CourseEnrollments = await _context.Enrollments.CountAsync(e => e.CourseId.HasValue),
            PluginEnrollments = await _context.Enrollments.CountAsync(e => e.PluginId.HasValue),
            TotalUsers = await _context.Users.CountAsync(),
            ActiveUsers = await _context.Users.CountAsync(u => u.Enrollments.Any()),
            TotalInstructors = await _context.Users.CountAsync(u => u.IsInstructor),
            TotalPayments = await _context.Payments.CountAsync(),
            TotalRevenue = await _context.Payments
                .Where(p => p.Status == PaymentStatus.Success)
                .SumAsync(p => p.Amount) 
        };

        return statistics;
    }
}