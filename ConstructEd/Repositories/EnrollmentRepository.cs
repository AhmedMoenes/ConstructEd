using ConstructEd.Data;
using ConstructEd.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConstructEd.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly DataContext dataContext;

        public EnrollmentRepository(DataContext dataContext)
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

        public async Task<ICollection<Enrollment>> GetAllAsync()
        {
            return await dataContext.Enrollments
                .Include(n => n.Course)
                .Include(b => b.User)
                .ToListAsync();
        }

        public async Task<Enrollment> GetByIdAsync(int id)
        {
            return await dataContext.Enrollments
                .Include(n => n.Course)
                .Include(b => b.User)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public Task<Enrollment?> GetByUserAndCourseAsync(string userId, int courseId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Enrollment>> GetByUserIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task InsertAsync(Enrollment obj)
        {
            await dataContext.AddAsync(obj);
        }

        public async Task<int> SaveAsync()
        {
            return await dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Enrollment obj)
        {
            dataContext.Update(obj); 
            await Task.CompletedTask;
        }
    }
}