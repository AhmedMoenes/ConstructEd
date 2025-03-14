using ConstructEd.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConstructEd.Data;

namespace ConstructEd.Repositories
{
    public class CourseContentRepository : ICourseContentRepository
    {
        private readonly DataContext dataContext;

        public CourseContentRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task DeleteAsync(int id)
        {
            var emp = await GetByIdAsync(id);
            if (emp != null)
            {
                dataContext.Remove(emp);
            }
        }

        public async Task<ICollection<CourseContent>> GetAllAsync()
        {
            return await dataContext.CourseContents
                .Include(m => m.Course)
                .ToListAsync();
        }

        public async Task<CourseContent> GetByIdAsync(int id)
        {
            return await dataContext.CourseContents
                .Include(m => m.Course)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task InsertAsync(CourseContent obj)
        {
            await dataContext.AddAsync(obj);
        }

        public async Task<int> SaveAsync()
        {
            return await dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(CourseContent obj)
        {
            dataContext.Update(obj); 
            await Task.CompletedTask;
        }
    }
}