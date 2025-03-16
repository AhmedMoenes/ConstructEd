using ConstructEd.Models;
using Microsoft.EntityFrameworkCore;
using ConstructEd.Data;

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
    }
}