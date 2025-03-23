using ConstructEd.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConstructEd.Repositories

{
    public interface ICourseContentRepository : IRepository<CourseContent>
    {
        Task<Dictionary<int, string>> GetCourseNamesAsync();
        Task<string> GetCourseNameByIdAsync(int courseId);
        Task<ICollection<CourseContent>> GetCourseContent(int id);
        ICollection<string> GetCategories();
        Task<int?> GetCourseIdByContentIdAsync(int courseContentId);

    }
}
