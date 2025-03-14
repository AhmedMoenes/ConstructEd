using ConstructEd.Data;
using ConstructEd.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConstructEd.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly DataContext dataContext;

        public CourseRepository(DataContext dataContext)
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

        public async Task<ICollection<Course>> GetAllAsync()
        {
            return await dataContext.Courses.ToListAsync();
        }

        public async Task<Course> GetByIdAsync(int id)
        {
            return await dataContext.Courses.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task InsertAsync(Course obj)
        {
            await dataContext.AddAsync(obj);
        }

        public async Task<int> SaveAsync()
        {
            return await dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Course obj)
        {
            dataContext.Update(obj); 
            await Task.CompletedTask; 
        }
    }
}