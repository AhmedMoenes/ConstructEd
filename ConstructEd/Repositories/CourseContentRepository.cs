using ConstructEd.Models;
using Microsoft.EntityFrameworkCore;
using ConstructEd.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ConstructEd.Repositories
{
    public class CourseContentRepository : ICourseContentRepository
    {
        private readonly DataContext _dataContext;

        public CourseContentRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dataContext.CourseContents.Remove(entity);
                await SaveAsync();
            }
        }

        public async Task<ICollection<CourseContent>> GetAllAsync()
        {
            return await _dataContext.CourseContents
                .Include(m => m.Course)
                .ToListAsync();
        }

        public async Task<CourseContent> GetByIdAsync(int id)
        {
            return await _dataContext.CourseContents
                .Include(m => m.Course)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<ICollection<CourseContent>> GetCourseContent(int id)
        {
            return await _dataContext.CourseContents
                .Include(m => m.Course).Where(m => m.CourseId == id).ToListAsync();
        }

        public ICollection<string> GetCategories()
        {
            var categories = Enum.GetNames(typeof(ContentType));
            return categories;
        }

        public async Task InsertAsync(CourseContent obj)
        {
            await _dataContext.CourseContents.AddAsync(obj);
            await SaveAsync();
        }
        public async Task SaveAsync()
        {
            await _dataContext.SaveChangesAsync();
        }
        public async Task UpdateAsync(CourseContent obj)
        {
            _dataContext.CourseContents.Update(obj);
            await SaveAsync();
        }

        public async Task<Dictionary<int, string>> GetCourseNamesAsync()
        {
            return await _dataContext.Courses
                .ToDictionaryAsync(c => c.Id, c => c.Title);
        }
        public async Task<string> GetCourseNameByIdAsync(int courseId)
        {
            var course = await _dataContext.Courses
                .FirstOrDefaultAsync(c => c.Id == courseId);
            return course?.Title;
        }
    }
}