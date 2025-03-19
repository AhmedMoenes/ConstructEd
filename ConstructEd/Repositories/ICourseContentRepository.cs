using ConstructEd.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConstructEd.Repositories

{
    public interface ICourseContentRepository : IRepository<CourseContent>
    {
        IEnumerable<SelectListItem> GetContentTypesAsSelectList();
        Task<Dictionary<int, string>> GetCourseNamesAsync();
        Task<string> GetCourseNameByIdAsync(int courseId);
        Task<ICollection<CourseContent>> GetCourseContent(int id);
    }
}
